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
		public unsafe AtmosphereBuildOutputs Build( AtmosphereBuildModel model, AtmosphereBuildParameters parameters, AtmosphereBuildProgress progress )
		{
			Texture2dData opticalDepthTexture = new Texture2dData( );
			Texture3dData scatteringTexture = new Texture3dData( );
			scatteringTexture.Create( parameters.ViewAngleSamples, parameters.SunAngleSamples, parameters.HeightSamples, TextureFormat.R8G8B8A8 );

			SetupModelAndParameters( parameters, model );
			progress = progress ?? new AtmosphereBuildProgress( );

			fixed ( byte* voxels = scatteringTexture.Bytes )
			{
				if ( !BuildScatteringTexture( parameters, voxels, progress ) )
				{
					return null;
				}
			}
			if ( progress.Cancel )
			{
				return null;
			}

			opticalDepthTexture.Create( parameters.OpticalDepthResolution, parameters.OpticalDepthResolution, TextureFormat.R8G8B8 );
			fixed ( byte* pixels = opticalDepthTexture.Bytes )
			{
				if ( !BuildOpticalDepthTexture( parameters, pixels, progress ) )
				{
					return null;
				}				
			}

			return new AtmosphereBuildOutputs( scatteringTexture, opticalDepthTexture );
		}

		#region Private Members

		/// <summary>
		/// Builds a 2D lookup texture containing optical depth for a paramerization of the view frame
		/// </summary>
		/// <param name="buildParams">Atmosphere build parameters</param>
		/// <param name="pixels">Atmosphere texture pixels</param>
		/// <param name="progress">Progress indicator</param>
		/// <returns>Returns true if the build completed (wasn't cancelled)</returns>
		private unsafe bool BuildOpticalDepthTexture( AtmosphereBuildParameters buildParams, byte* pixels, AtmosphereBuildProgress progress )
		{
			//	TODO: AP: Because we know that the view to pos angle range will never be > pi, can optimise this later
			int viewSamples = buildParams.OpticalDepthResolution;
			int heightSamples = buildParams.OpticalDepthResolution;
			float viewAngleInc = Constants.Pi / ( viewSamples - 1 );
			float heightRange = ( m_OuterRadius - m_InnerRadius );
			float heightInc = ( heightRange - heightRange * 0.05f ) / ( heightSamples - 1 );	//	Push height range in slightly to allow simplification of sphere intersections

			float height = m_InnerRadius;
			for ( int heightSample = 0; heightSample < heightSamples; ++heightSample, height += heightInc )
			{
				Point3 pos = new Point3( 0, height, 0 );
				//	Start the view angle at pi, and count down to 0. This is because it is quickest to address
				//	the 2D texture using the dot of the view vector and the view position, saving a (1-th) operation
				float viewAngle = Constants.Pi;
				Point3 lastAtmInt = pos;
				for ( int viewSample = 0; viewSample < viewSamples; ++viewSample, viewAngle -= viewAngleInc )
				{
					Vector3 viewDir = new Vector3( Functions.Sin( viewAngle ), Functions.Cos( viewAngle ), 0 );

					//	NOTE: If ray intersection fails, the previous intersection position is used...
					Point3 atmInt;
					if ( !GetRayPlanetAndAtmosphereIntersection( pos, viewDir, out atmInt ) )
					{
						atmInt = lastAtmInt;
					}
					else
					{
						lastAtmInt = atmInt;
					}

					Vector3 step = ( atmInt - pos ) / m_AttenuationSamples;
					float oR = ComputeOpticalDepth( pos, step, m_RayleighCoefficients[ 0 ], m_MieCoefficients[ 0 ] );
					float oG = ComputeOpticalDepth( pos, step, m_RayleighCoefficients[ 1 ], m_MieCoefficients[ 1 ] );
					float oB = ComputeOpticalDepth( pos, step, m_RayleighCoefficients[ 2 ], m_MieCoefficients[ 2 ] );

					float attR = Functions.Exp( -oR );
					float attG = Functions.Exp( -oG );
					float attB = Functions.Exp( -oB );

					pixels[ 2 ] = ( byte )( Utils.Clamp( attR, 0, 1 ) * 255.0f );
					pixels[ 1 ] = ( byte )( Utils.Clamp( attG, 0, 1 ) * 255.0f );
					pixels[ 0 ] = ( byte )( Utils.Clamp( attB, 0, 1 ) * 255.0f );
					pixels += 3;
				}

				if ( progress != null )
				{
					progress.OnSliceCompleted( heightSample / ( float )( heightSamples - 1 ) );
					if ( progress.Cancel )
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Builds a 3D lookup texture containing the scattering values for a parameterization of
		/// the view frame and sun position.
		/// </summary>
		/// <param name="parameters">Parameters for the build process</param>
		/// <param name="progress">Progress object</param>
		/// <param name="voxels">3d texture voxels</param>
		/// <returns>true if build completed (wasn't cancelled)</returns>
		private unsafe bool BuildScatteringTexture( AtmosphereBuildParameters parameters, byte* voxels, AtmosphereBuildProgress progress )
		{
			float viewAngle = Constants.Pi;
			float viewAngleInc = Constants.Pi / ( parameters.ViewAngleSamples - 1 );
			float heightRange = ( m_OuterRadius - m_InnerRadius );
			float heightInc = ( heightRange - heightRange * 0.05f ) / ( parameters.HeightSamples - 1 );	//	Push height range in slightly to allow simplification of sphere intersections
			float sunAngleInc = ( Constants.Pi ) / ( parameters.SunAngleSamples - 1 );

			for ( int viewAngleSample = 0; viewAngleSample < parameters.ViewAngleSamples; ++viewAngleSample, viewAngle -= viewAngleInc )
			{
				Vector3 viewDir = new Vector3( Functions.Sin( viewAngle ), Functions.Cos( viewAngle ), 0 );

				float sunAngle = 0;
				for ( int sunAngleSample = 0; sunAngleSample < parameters.SunAngleSamples; ++sunAngleSample, sunAngle += sunAngleInc )
				{
					Vector3 sunDir = new Vector3( Functions.Sin( sunAngle ), Functions.Cos( sunAngle ), 0 );
					float height = m_InnerRadius;
					for ( int heightSample = 0; heightSample < parameters.HeightSamples; ++heightSample, height += heightInc )
					{
						ComputeScattering( viewDir, sunDir, height, voxels );
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

		private bool GetRayAtmosphereIntersection( Point3 origin, Vector3 dir, out Point3 intPt )
		{
			Ray3 ray = new Ray3( origin, dir );
			Line3Intersection outerIntersection = Intersections3.GetRayIntersection( ray, m_OuterSphere );
			if ( outerIntersection == null )
			{
				intPt = origin;
				return false;
			}
			intPt = outerIntersection.IntersectionPosition;
			return true;	
		}

		private bool GetRayPlanetAndAtmosphereIntersection( Point3 origin, Vector3 dir, out Point3 intPt )
		{
			//	TODO: AP: Optimise ray intersection code (origin is always outside inner sphere, inside outer sphere)
			Ray3 ray = new Ray3( origin, dir );
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
		}
		
		/// <summary>
		/// Computes the in-scattering term for a given sun angle, viewer orientation, and height. Stores
		/// the result in a 3-component voxel
		/// </summary>
		private unsafe void ComputeScattering( Vector3 viewDir, Vector3 sunDir, float height, byte* voxel )
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
			GetRayPlanetAndAtmosphereIntersection( viewPos, viewDir, out viewEnd );

			//	The summation of attenuation is a Reimann sum approximation of the integral
			//	of atmospheric density .
		//	float viewRayLength = ( viewEnd - viewPos ).Length;
		//	Vector3 viewVec = ( viewEnd - viewPos ).MakeNormal( );
			Vector3 sampleStep = ( viewEnd - viewPos ) / m_AttenuationSamples;
		//	float mul = ( InscatterDistanceFudgeFactor * viewRayLength ) / m_OuterRadiusMetres;
			float mul = ( m_InscatterDistanceFudgeFactor * sampleStep.Length );
			Point3 samplePos = viewPos + sampleStep;
			float[] mAccum = new float[ 3 ];
			float[] rAccum = new float[ 3 ];
			for ( int sampleCount = 1; sampleCount < m_AttenuationSamples; ++sampleCount )
			{
				//	Cast a ray to the sun. Don't intersect the planet. This is a cheap way of providing dark nights
				Point3 sunPt;
			//	if ( !GetRayAtmosphereIntersection( samplePos, sunDir, out sunPt ) )
				if ( !GetRayPlanetAndAtmosphereIntersection( samplePos, sunDir, out sunPt ) )
				{
					break;
				}

				float sampleHeight = samplePos.DistanceTo( Point3.Origin ) - m_InnerRadius;
				sampleHeight = Utils.Max( sampleHeight, 0 );
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
				ptHeight = Utils.Max( ptHeight, 0 );
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
			SetComponentScales( );

			m_InnerRadius = model.InnerRadiusMetres;
			m_OuterRadius = model.InnerRadiusMetres + model.AtmosphereThicknessMetres;
			m_InnerSphere = new Sphere3( Point3.Origin, m_InnerRadius * 0.01f );	//	NOTE: AP: See remarks
			m_OuterSphere = new Sphere3( Point3.Origin, m_OuterRadius );
			m_AttenuationSamples = parameters.AttenuationSamples;
			float heightRange = ( m_OuterRadius - 3 );
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
