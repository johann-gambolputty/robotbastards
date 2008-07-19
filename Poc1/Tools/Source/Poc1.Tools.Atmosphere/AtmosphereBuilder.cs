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
		//	Realtime rendering of atmospheric scattering for flight simulators:
		//		http://www2.imm.dtu.dk/pubdb/views/edoc_download.php/2554/pdf/imm2554.pdf
		//
		//	Display of The Earth Taking into Account Atmospheric Scattering: (Nishita)
		//		http://nis-lab.is.s.u-tokyo.ac.jp/~nis/abs_sig.html#sig93
		//
		//	Implementation of Nishita's paper:
		//		http://www.gamedev.net/reference/articles/article2093.asp
		//
		//	Discussion about atmospheric shaders:
		//		http://www.gamedev.net/community/forums/topic.asp?topic_id=335023
		//
		//	Paper that this model is based on:
		//		http://www.vis.uni-stuttgart.de/~schafhts/HomePage/pubs/wscg07-schafhitzel.pdf
		//

		/// <summary>
		/// Gets/sets the number of samples to take when calculating optical depth integral
		/// </summary>
		public int AttenuationSamples
		{
			get { return m_AttenuationSamples; }
			set { m_AttenuationSamples = value; }
		}

		/// <summary>
		/// Gets/sets the inner radius of the planet
		/// </summary>
		public float InnerRadius
		{
			get { return m_InnerRadius; }
			set { m_InnerRadius = value; }
		}

		/// <summary>
		/// Gets/sets the outer radius of the planet
		/// </summary>
		public float OuterRadius
		{
			get { return m_OuterRadius; }
			set { m_OuterRadius = value; }
		}

		/// <summary>
		/// Molecular density scale
		/// </summary>
		public float KScale
		{
			get { return m_KScale; }
			set { m_KScale = value; }
		}

		/// <summary>
		/// Builds a 3D lookup texture
		/// </summary>
		/// <param name="viewAngleSamples">Width of the 3d texture. Corresponds to viewing angle</param>
		/// <param name="sunAngleSamples">Height of the 3d texture. Corresponds to angle to the sun</param>
		/// <param name="heightSamples">Depth of the 3d texture. Corresponds to the number of height samples to take betweem the inner to outer radius</param>
		/// <param name="progress">Optional progress object</param>
		/// <returns>Returns a 3d texture data. Returns null if cancel was flagged in the progress object</returns>
		public unsafe Texture3dData BuildLookupTexture( int viewAngleSamples, int sunAngleSamples, int heightSamples, AtmosphereBuildProgress progress )
		{
			Texture3dData data = new Texture3dData( );
			data.Create( viewAngleSamples, sunAngleSamples, heightSamples, TextureFormat.R8G8B8 );
			fixed ( byte* voxels = data.Bytes )
			{
				if ( !BuildLookupTexture( viewAngleSamples, sunAngleSamples, heightSamples, voxels, progress ?? new AtmosphereBuildProgress( ) ) )
				{
					return null;
				}
			}
			return data;
		}

		#region Private Members

		private float m_InnerRadius = 0;
		private float m_OuterRadius = 10000;

		/// <summary>
		/// Builds a 3D lookup texture containing the in-scattering values for a parameterization of
		/// the view frame and sun position.
		/// </summary>
		/// <param name="viewAngleSamples">Width of the 3d texture. Corresponds to viewing angle</param>
		/// <param name="sunAngleSamples">Height of the 3d texture. Corresponds to angle to the sun</param>
		/// <param name="heightSamples">Depth of the 3d texture. Corresponds to the number of height samples to take betweem the inner to outer radius</param>
		/// <param name="progress">Progress object</param>
		/// <param name="voxels">3d texture voxels</param>
		/// <returns>true if build completed (wasn't cancelled)</returns>
		private unsafe bool BuildLookupTexture( int viewAngleSamples, int sunAngleSamples, int heightSamples, byte* voxels, AtmosphereBuildProgress progress )
		{
			m_H0 = InnerRadius + ( OuterRadius - InnerRadius ) * 0.75f;
			m_InvH0 = -1.0f / m_H0;
			float innerRadius = InnerRadius;
			float outerRadius = OuterRadius;
			float viewAngle = 0;
			float viewAngleInc = Constants.Pi / ( viewAngleSamples - 1 );
			float heightInc = ( outerRadius - innerRadius ) / ( heightSamples - 1 );
			float sunAngleInc = ( Constants.Pi ) / ( sunAngleSamples - 1 );

			for ( int viewAngleSample = 0; viewAngleSample < viewAngleSamples; ++viewAngleSample, viewAngle += viewAngleInc )
			{
				Vector3 viewDir = new Vector3( Functions.Sin( viewAngle ), Functions.Cos( viewAngle ), 0 );

				float sunAngle = 0;
				for ( int sunAngleSample = 0; sunAngleSample < sunAngleSamples; ++sunAngleSample, sunAngle += sunAngleInc )
				{
					Vector3 sunDir = new Vector3( Functions.Sin( sunAngle ), Functions.Cos( sunAngle ), 0 );
					float height = innerRadius;
					for ( int heightSample = 0; heightSample < heightSamples; ++heightSample, height += heightInc )
					{
						ComputeScattering( viewDir, sunAngle, sunDir, height, voxels );
					//	voxels[ 0 ] = ( byte )Utils.Clamp( ( height - innerRadius ) * 255.0f / ( outerRadius - innerRadius ), 0, 255 );
					//	voxels[ 1 ] = ( byte )Utils.Clamp( viewAngleSample * 255.0f / viewAngleSamples, 0, 255 );
					//	voxels[ 2 ] = ( byte )Utils.Clamp( sunAngleSample * 255.0f / sunAngleSamples, 0, 255 );
						voxels += 3;
					}
				}

				if ( progress != null )
				{
					progress.OnSliceCompleted( viewAngleSample / ( float )( viewAngleSamples - 1 ) );

					if ( progress.Cancel )
					{
						return false;
					}
				}
			}
			return true;
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
			Sphere3 atmoSphere = new Sphere3( Point3.Origin, OuterRadius ); // ahaha
			Point3 viewPos = new Point3( 0, height, 0 );
			Line3Intersection viewRayIntersection = Intersections3.GetRayIntersection( new Ray3( viewPos, viewDir ), atmoSphere );	//	TODO: AP: Use optimised intersection
			Vector3 sampleStep = ( viewRayIntersection.IntersectionPosition - viewPos ) / ( AttenuationSamples - 1 );
			
			float mul = 1.0f / sampleStep.Length;

			float[] components = new float[ 3 ];
			for ( int componentIndex = 0; componentIndex < 3; ++componentIndex )
			{
				Point3 samplePos = viewPos;

				float invWv4 = m_InvWv4[ componentIndex ];
				float accum = 0;
				for ( int sampleCount = 0; sampleCount < AttenuationSamples; ++sampleCount )
				{
					float sampleHeight = samplePos.DistanceTo( Point3.Origin ) - InnerRadius;
					float pCoeff = Functions.Exp( sampleHeight * m_InvH0 );

					Line3Intersection sunRayIntersection = Intersections3.GetRayIntersection( new Ray3( samplePos, sunDir ), atmoSphere );
					if ( sunRayIntersection == null )
					{
						break;
					}

					float outScatterToSun = ComputeOpticalDepth( samplePos, ( sunRayIntersection.IntersectionPosition - samplePos ) / AttenuationSamples, invWv4 );
					float outScatterToViewer = ComputeOpticalDepth( samplePos, ( samplePos - viewPos ) / AttenuationSamples, invWv4 );

					accum += pCoeff * Functions.Exp( -outScatterToSun - outScatterToViewer ) * mul;

					samplePos += sampleStep;
				}
				float cosSunAngle = Functions.Cos( sunAngle );
				float phase = 0.75f * ( 1 + cosSunAngle * cosSunAngle );	//	TODO: AP: Rayleigh phase function - move to HG Mie
				float scatter = ( KScale * phase ) * invWv4;
				float res = m_SunColour[ componentIndex ] * scatter * accum;
				components[ componentIndex ] = res;
			}

			voxel[ 0 ] = ( byte )Utils.Clamp( components[ 0 ] * 255.0f, 0, 255 );
			voxel[ 1 ] = ( byte )Utils.Clamp( components[ 1 ] * 255.0f, 0, 255 );
			voxel[ 2 ] = ( byte )Utils.Clamp( components[ 2 ] * 255.0f, 0, 255 );
		}

		/// <summary>
		/// Calculates the average atmospheric density along a ray
		/// </summary>
		private float ComputeOpticalDepth( Point3 pt, Vector3 step, float invWv4 )
		{
			float phase = ( KScale * 4 * Constants.Pi ) * invWv4;
			float accum = 0;
			float mul = 1.0f / step.Length;
			for ( int sampleCount = 0; sampleCount < AttenuationSamples; ++sampleCount )
			{
				float ptHeight = pt.DistanceTo( Point3.Origin ) - InnerRadius;
				float pCoeff = Functions.Exp( ptHeight * m_InvH0 );
				accum += pCoeff * mul;
				pt += step;
			}

			return phase * accum;
		}

		private readonly static float[] m_Wavelengths = new float[ 3 ] { 0.650f, 0.610f, 0.475f };
		private readonly static float[] m_InvWv4 = new float[ 3 ];

		private const float BMul = 3 / ( 16 * Constants.Pi );

		private float m_H0;
		private float m_InvH0;
		private float m_KScale = 0.67f;
		private int m_AttenuationSamples = 10;
		private float[] m_SunColour = new float[ 3 ]{ 1, 1, 1 };

		/// <summary>
		/// Initializes static values
		/// </summary>
		static AtmosphereBuilder( )
		{
			for ( int componentIndex = 0; componentIndex < 3; ++componentIndex )
			{
				float wv = m_Wavelengths[ componentIndex ];
				m_InvWv4[ componentIndex ] = 1.0f / ( wv * wv * wv * wv );
			}
		}

		/// <summary>
		/// Builds a lookup table that stores the coefficient in the phase function that is based on the viewer's angle to the sun
		/// </summary>
		/// <remarks>
		/// Stores the result of K.Fr(om)
		/// </remarks>
		private void BuildAttenuationCoefficientLookupTable( float[] table )
		{
			float sunAngleInc = Constants.TwoPi / ( table.Length - 1 );
			float sunAngle = 0;
			for ( int sunAngleSample = 0; sunAngleSample < table.Length; ++sunAngleSample, sunAngle += sunAngleInc )
			{
				float cosSunAngle = Functions.Cos( sunAngle );
				float bCoeff = KScale * BMul * ( 2 + 0.5f * ( cosSunAngle * cosSunAngle ) );
				table[ sunAngleSample ] = bCoeff;
			}
		}

		#endregion
	}
}
