using System;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Tools.Atmosphere
{
	/// <summary>
	/// Atmosphere generator
	/// </summary>
	public class AtmosphereBuilder
	{
		//
		//	Discrepancies in atmospheric modelling papers:
		//		- HG: 
		//			- Neilsen: HG(t,g) = (1-g2)/(4pi(1 + g2 - 2cos(t))^1.5)
		//			- Wolfram: HG(t,g) = (1-g2)/(1 + g2 - 2.g.cos(t))^1.5
		//			- O'Neil (adapted HG): HG(t,g) = 3(1-g2)/2(2+g2) . (1+t2)/(1+g2 - 2gt)^1.5
		//			- O'Neil and Wolfram work well. Neilsen doesn't (denominator can evalute to < 0 before raising to 3/2 power)
		//	- Scattering coefficients: (# is wavelength)
		//			- Neilsen Rayleigh coefficient: 8pi^2(n2-1)^2/3N#^4
		//			- Neilsen Mie coefficient: 0.434c.pi.4pi^2/#^2K
		//				- K=non-wavelength dependent fudge factor for Bm: ~0.67
		//				- c=concentration factor(0.6544T-0.6510).10e-16 (T=turbidity. ~555nm)
		//
		//
		//

		//
		//	Realtime rendering of atmospheric scattering for flight simulators:
		//		http://www2.imm.dtu.dk/pubdb/views/edoc_download.php/2554/pdf/imm2554.pdf
		//
		//	Display of The Earth Taking into Account Atmospheric Scattering: (Nishita)
		//		http://nis-lab.is.s.u-tokyo.ac.jp/~nis/abs_sig.html#sig93
		//
		//	Implementation of Nishita's paper:
		//		http://www.gamedev.net/reference/articles/article2093.asp
		//
		//	A practical analytical model of daylight:
		//		http://www.cs.utah.edu/vissim/papers/sunsky/sunsky.pdf
		//
		//	Discussion about atmospheric shaders:
		//		http://www.gamedev.net/community/forums/topic.asp?topic_id=335023
		//
		//	Paper that this model is based on:
		//		http://www.vis.uni-stuttgart.de/~schafhts/HomePage/pubs/wscg07-schafhitzel.pdf
		//
		//	Heyney-Greenstein on Mathworld:
		//		http://scienceworld.wolfram.com/physics/Heyney-GreensteinPhaseFunction.html
		//

		/// <summary>
		/// Builds a 3D lookup texture
		/// </summary>
		/// <param name="model">The atmosphere model</param>
		/// <param name="parameters">Parameters for the build process</param>
		/// <param name="progress">Optional progress object</param>
		/// <returns>Returns a 3d texture data. Returns null if cancel was flagged in the progress object</returns>
		public unsafe Texture3dData BuildLookupTexture( AtmosphereBuildModel model, AtmosphereBuildParameters parameters, AtmosphereBuildProgress progress )
		{
			Texture3dData data = new Texture3dData( );
			data.Create( parameters.ViewAngleSamples, parameters.SunAngleSamples, parameters.HeightSamples, TextureFormat.R8G8B8A8 );
			fixed ( byte* voxels = data.Bytes )
			{
				if ( !BuildLookupTexture( model, parameters, voxels, progress ?? new AtmosphereBuildProgress( ) ) )
				{
					return null;
				}
			}
			return data;
		}

		#region Private Members

		/// <summary>
		/// Builds a 3D lookup texture containing the in-scattering values for a parameterization of
		/// the view frame and sun position.
		/// </summary>
		/// <param name="model">The atmosphere model</param>
		/// <param name="parameters">Parameters for the build process</param>
		/// <param name="progress">Progress object</param>
		/// <param name="voxels">3d texture voxels</param>
		/// <returns>true if build completed (wasn't cancelled)</returns>
		private unsafe bool BuildLookupTexture( AtmosphereBuildModel model, AtmosphereBuildParameters parameters, byte* voxels, AtmosphereBuildProgress progress )
		{
			SetupModelAndParameters( parameters, model );
			float viewAngle = 0;
			float viewAngleInc = Constants.Pi / ( parameters.ViewAngleSamples - 1 );
			float heightRange = ( m_OuterRadius - m_InnerRadius );
			float heightInc = ( heightRange - heightRange * 0.05f ) / ( parameters.HeightSamples - 1 );	//	Push height range in slightly to allow simplification of sphere intersections
			float sunAngleInc = ( Constants.Pi ) / ( parameters.SunAngleSamples - 1 );

			SetComponentScales( );

			for ( int viewAngleSample = 0; viewAngleSample < parameters.ViewAngleSamples; ++viewAngleSample, viewAngle += viewAngleInc )
			{
				Vector3 viewDir = new Vector3( Functions.Sin( viewAngle ), Functions.Cos( viewAngle ), 0 );

				float sunAngle = 0;
				for ( int sunAngleSample = 0; sunAngleSample < parameters.SunAngleSamples; ++sunAngleSample, sunAngle += sunAngleInc )
				{
					Vector3 sunDir = new Vector3( Functions.Sin( sunAngle ), Functions.Cos( sunAngle ), 0 );
					float height = m_InnerRadius;
					for ( int heightSample = 0; heightSample < parameters.HeightSamples; ++heightSample, height += heightInc )
					{
						ComputeScattering( viewDir, sunAngle, sunDir, height, voxels );
						voxels += 4;
					}
				}

				if ( progress != null )
				{
					progress.OnSliceCompleted( viewAngleSample / ( float )( parameters.ViewAngleSamples - 1 ) );

					if ( progress.Cancel )
					{
						return false;
					}
				}
			}
			return true;
		}

		public static float HgPhase( float cosAngle, float g )
		{
			float g2 = g * g;
			float t0 = ( 3 * ( 1 - g2 ) ) / 2 * ( 2 + g2 );
			float tden0 = Functions.Pow( 1 + g2 - 2 * g * cosAngle, 3.0f / 2.0f );
			float t1 = ( 1 + cosAngle * cosAngle ) / tden0;
			float tRes = t0 * t1;
			return tRes;
		}

		public static float RayleighPhase( float cosAngle )
		{
			return 0.75f * ( 1 + cosAngle * cosAngle );
		}

		
		/// <summary>
		/// Gets an intersection point between a ray (origin within inner/outer sphere boundary) and the inner and outer spheres
		/// </summary>
		private bool GetRayIntersection( Point3 origin, Vector3 dir, out Point3 intPt )
		{
			Ray3 ray = new Ray3( origin, dir );
			/*
			Line3Intersection outerIntersection = Intersections3.GetRayIntersection( ray, m_OuterSphere );
			if ( outerIntersection == null )
			{
				intPt = origin;
				return false;
			}
			intPt = outerIntersection.IntersectionPosition;
			return true;
			/*/
			//	TODO: AP: Optimise ray intersection code (origin is always outside inner sphere, inside outer sphere)
			Line3Intersection innerIntersection = Intersections3.GetRayIntersection( ray, m_InnerSphere );
			Line3Intersection outerIntersection = Intersections3.GetRayIntersection( ray, m_OuterSphere );
			if ( innerIntersection == null )
			{
				if ( outerIntersection == null )
				{
					//	This should not happen, but there may be an edge condition
					intPt = Point3.Origin;
					return false;
				}
				intPt = outerIntersection.IntersectionPosition;
				return true;
			}
			if ( ( outerIntersection == null ) || ( innerIntersection.Distance < outerIntersection.Distance ) )
			{
				intPt = innerIntersection.IntersectionPosition;
				return true;
			}
			intPt = outerIntersection.IntersectionPosition;
			return true;
			//*/
		}

		/// <summary>
		/// Computes the in-scattering term for a given sun angle, viewer orientation, and height. Stores
		/// the result in a 3-component voxel
		/// </summary>
		private unsafe void ComputeScattering( Vector3 viewDir, float sunAngle, Vector3 sunDir, float height, byte* voxel )
		{
			//	Iv(t) = Is(t) (K.Fr(s)/t^4) E(i=0..k) p.exp(-Tl-Tv)

			//	Where:
			//	t = wavelength of incident light at viewpoint
			//	s = scattering angle between viewer and incident light
			//	Iv = Intensity of light of given wavelength at viewer
			//	Is = Intensity of incident light of given wavelength
			//	K = Constant molecular density at sea level:
			//		2pi^2(n^2 - 1)^2/3.Ns
			//		Where:
			//			n = Index of refraction of the air
			//			Ns = Molecular number density of the standard atmosphere
			//	Fr(s) = Scattering phase function
			//		3/4(1 + cos2(s))
			//	p = Density ratio:
			//		exp(-h/H0)
			//		Where:
			//			h = height
			//			H0 = 7994m (thickness of atmosphere if density is uniform)
			//	k = number of samples to take along view vector
			//


			//
			//	Alternative phase function (Henyey-Greenstein function):
			//		F(th, g) = 3(1-g^2)/(2(2+g^2)) . (1+cos^2(th))/(1+g^2 - 2.g.cos(th))^3/2
			//
			//	g is asymmetry factor:
			//		g = 5/9u - (4/3 - 25/81u^2)x^-1/3 + x^1/3
			//
			//		x = 5/u + 125/729u^3 + (64/27 - 325/243u^2 + 1250/2187u^4)^1/2
			//		u = haze factor and wavelength (0.7-0.85)
		//	float cosSunAngle = Functions.Cos( sunAngle );
			Point3 viewPos = new Point3( 0, height, 0 );

			Point3 viewEnd;
			GetRayIntersection( viewPos, viewDir, out viewEnd );

			//	The summation of attenuation is a Reimann sum approximation of the integral
			//	of atmospheric density .
			float viewRayLength = ( viewEnd - viewPos ).Length;
			Vector3 viewVec = ( viewEnd - viewPos ).MakeNormal( );
			Vector3 sampleStep = ( viewEnd - viewPos ) / m_AttenuationSamples;
		//	float mul = ( InscatterDistanceFudgeFactor * viewRayLength ) / m_OuterRadius;
			float mul = ( m_InscatterDistanceFudgeFactor * sampleStep.Length );
			Point3 samplePos = viewPos + sampleStep;
			float[] mAccum = new float[ 3 ];
			float[] rAccum = new float[ 3 ];
			for ( int sampleCount = 1; sampleCount < m_AttenuationSamples; ++sampleCount )
			{
				//	Cast a ray to the sun
				Point3 sunPt;
				if ( !GetRayIntersection( samplePos, sunDir, out sunPt ) )
				{
					break;
				}

				float sampleHeight = samplePos.DistanceTo( Point3.Origin ) - m_InnerRadius;
				float pRCoeff = Functions.Exp( sampleHeight * m_InvRH0 ) * mul;
				float pMCoeff = Functions.Exp( sampleHeight * m_InvMH0 ) * mul;

				//	Calculate (wavelength-dependent) out-scatter terms
				Vector3 viewStep = ( viewPos - samplePos ) / m_AttenuationSamples;
				Vector3 sunStep = ( sunPt - samplePos ) / m_AttenuationSamples; 
				for ( int component = 0; component < 3; ++component )
				{
					//	TODO: AP: This should calculate the rayleigh + mie out scatter terms separately, then sum them
					float bR = m_RayleighCoefficients[ component ];
					float bM = m_MieCoefficients[ component ];
					float sunOutScatter = ComputeOpticalDepth( samplePos, sunStep, bR, bM );
					float viewOutScatter = ComputeOpticalDepth( samplePos, viewStep, bR, bM );
					float outScatter = Functions.Exp( ( -sunOutScatter - viewOutScatter ) * m_OutscatterFudge );
					mAccum[ component ] += pMCoeff * outScatter;
					rAccum[ component ] += pRCoeff * outScatter;
				}

				samplePos += sampleStep;
			}

			float[] accum = new float[ 3 ];
			float avgMie = 0;
			for ( int component = 0; component < 3; ++component )
			{
				float rComponent = rAccum[ component ] * m_RayleighCoefficients[ component ] * m_RayleighFudge;
				float mComponent = mAccum[ component ] * m_MieCoefficients[ component ] * m_MieFudge;

				avgMie += mComponent;
				accum[ component ] = rComponent;
			}

			//	TODO: AP: Fix stupid backwards textures :( 
			voxel[ 3 ] = ( byte )Utils.Clamp( accum[ 0 ] * 255.0f, 0, 255 );
			voxel[ 2 ] = ( byte )Utils.Clamp( accum[ 1 ] * 255.0f, 0, 255 );
			voxel[ 1 ] = ( byte )Utils.Clamp( accum[ 2 ] * 255.0f, 0, 255 );
			voxel[ 0 ] = ( byte )Utils.Clamp( avgMie * 255.0f / 3.0f, 0, 255 );
		}
		
		/// <summary>
		/// Calculates the average atmospheric density along a ray
		/// </summary>
		private float ComputeOpticalDepth( Point3 pt, Vector3 step, float bR, float bM )
		{
			float pmAccum = 0;
			float prAccum = 0;
			float mul = m_OutscatterDistanceFudgeFactor * step.Length;
			for ( int sampleCount = 0; sampleCount < m_AttenuationSamples; ++sampleCount )
			{
				float ptHeight = pt.DistanceTo( Point3.Origin ) - m_InnerRadius;
				float pmCoeff = Functions.Exp( ptHeight * m_InvMH0 );
				float prCoeff = Functions.Exp( ptHeight * m_InvRH0 );
				pmAccum += pmCoeff * mul;
				prAccum += prCoeff * mul;
				pt += step;
			}

			return bR * prAccum + bM * pmAccum;
		}

		private readonly static double[] m_Wavelengths = new double[ 3 ] { 650e-9, 610e-9, 475e-9 };

		private const double AirIndexOfRefraction = 1.0003;
		private static readonly double AirNumberDensity = 2.504e25;
		private float m_InnerRadius;
		private float m_OuterRadius;
		private Sphere3 m_InnerSphere;
		private Sphere3 m_OuterSphere;
		private float m_InvMH0;
		private float m_InvRH0;
		private readonly float[] m_RayleighCoefficients = new float[ 3 ];
		private readonly float[] m_MieCoefficients = new float[ 3 ];
		private float m_InscatterDistanceFudgeFactor;
		private float m_OutscatterDistanceFudgeFactor;
		private float m_MieFudge;
		private float m_RayleighFudge;
		private float m_OutscatterFudge;
		private int m_AttenuationSamples = 10;

		private void SetComponentScales( )
		{
			//
			//	BetaM = 0.434 * c * PI * ( 4 * PI * PI ) * K / ( Lambda * Lambda )
			//	with
			//	c = ( 0.6544 * T - 0.6510 ) * 10e-16
			//	and T is the turbidity factor
			//	and K is almost equal to 0.67
			//
			double turbidity = 1;
			double c = ( 0.6544 * turbidity - 0.6510 ) * 10e-16;
			double k = 0.67;
			double iR2 = ( AirIndexOfRefraction * AirIndexOfRefraction ) - 1;
			double mieNum = c  * 4 * Math.PI * Math.PI * Math.PI * k;
			double rayleighNum = ( 8 * Math.PI * Math.PI * Math.PI * iR2 * iR2 ) / ( 3 * AirNumberDensity );

			for ( int component = 0; component < 3; ++component )
			{
				double wv = m_Wavelengths[ component ];
				double wv2 = wv * wv;
				double wv4 = wv2 * wv2;

				m_RayleighCoefficients[ component ] = ( float )( rayleighNum / wv4 );
				m_MieCoefficients[ component ] = ( float )( mieNum / wv2 );
			}

			//	Values taken from http://patarnott.com/atms749/pdf/RayleighScatteringForAtmos.pdf
		//	m_RayleighCoefficients[ 0 ] = 0.03539f;
		//	m_RayleighCoefficients[ 1 ] = 0.06613f;
		//	m_RayleighCoefficients[ 2 ] = 0.23522f;
		//	m_RayleighCoefficients[ 0 ] = 0.0000004f;
		//	m_RayleighCoefficients[ 1 ] = 0.0000006f;
		//	m_RayleighCoefficients[ 2 ] = 0.00000243f;
		}

		/// <summary>
		/// Sets up stored and derived parameters from an atmosphere model
		/// </summary>
		/// <remarks>
		/// The inner radius is diminished by a constant amount, because otherwise, the quantised
		/// view directions hit the inner sphere and cause a blue band to appear when the actual
		/// view direction approaches it.
		/// </remarks>
		private void SetupModelAndParameters( AtmosphereBuildParameters parameters, AtmosphereBuildModel model )
		{
			m_InnerRadius = model.InnerRadius;
			m_OuterRadius = model.OuterRadius;
			m_InnerSphere = new Sphere3( Point3.Origin, m_InnerRadius * 0.9f );	//	NOTE: AP: See remarks
			m_OuterSphere = new Sphere3( Point3.Origin, m_OuterRadius );
			m_AttenuationSamples = parameters.AttenuationSamples;
			float heightRange = ( model.OuterRadius - model.InnerRadius );
			m_InvRH0 = -1.0f / ( heightRange * model.RayleighDensityScaleHeightFraction );
			m_InvMH0 = -1.0f / ( heightRange * model.MieDensityScaleHeightFraction );
			m_InscatterDistanceFudgeFactor = model.InscatterDistanceFudgeFactor;
			m_OutscatterDistanceFudgeFactor = model.OutscatterDistanceFudgeFactor;
			m_MieFudge = model.MieFudgeFactor;
			m_RayleighFudge = model.RayleighFudgeFactor;
			m_OutscatterFudge = model.OutscatterFudgeFactor;
		}

		#endregion
	}
}
