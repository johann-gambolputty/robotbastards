using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Tools.Atmosphere
{
	/// <summary>
	/// Atmosphere generator
	/// </summary>
	public class Atmosphere
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
		/// Builds a 3D lookup texture
		/// </summary>
		/// <param name="viewAngleSamples">Width of the 3d texture. Corresponds to viewing angle</param>
		/// <param name="sunAngleSamples">Height of the 3d texture. Corresponds to angle to the sun</param>
		/// <param name="heightSamples">Depth of the 3d texture. Corresponds to the number of height samples to take betweem the inner to outer radius</param>
		/// <returns>Returns a 3d texture data</returns>
		public unsafe Texture3dData BuildLookupTexture( int viewAngleSamples, int sunAngleSamples, int heightSamples )
		{
			Texture3dData data = new Texture3dData( );
			data.Create( viewAngleSamples, sunAngleSamples, heightSamples, TextureFormat.R8G8B8 );
			fixed ( byte* voxels = data.Bytes )
			{
				BuildLookupTexture( viewAngleSamples, sunAngleSamples, heightSamples, voxels );
			}
			return data;
		}

		#region Private Members

		private float m_InnerRadius = 0;
		private float m_OuterRadius = 10000;

		/// <summary>
		/// Builds a 3D lookup texture
		/// </summary>
		/// <param name="viewAngleSamples">Width of the 3d texture. Corresponds to viewing angle</param>
		/// <param name="sunAngleSamples">Height of the 3d texture. Corresponds to angle to the sun</param>
		/// <param name="heightSamples">Depth of the 3d texture. Corresponds to the number of height samples to take betweem the inner to outer radius</param>
		/// <param name="voxels">3d texture voxels</param>
		private unsafe void BuildLookupTexture( int viewAngleSamples, int sunAngleSamples, int heightSamples, byte* voxels )
		{
			float innerRadius = InnerRadius;
			float outerRadius = OuterRadius;
			float viewAngle = 0;
			float viewAngleInc = Constants.TwoPi / ( viewAngleSamples - 1 );
			float heightInc = ( outerRadius - innerRadius ) / ( heightSamples - 1 );

			float[] bCoeffLut = new float[ sunAngleSamples ];
			BuildAttenuationCoefficientLookupTable( sunAngleLut );

			for ( int viewAngleSample = 0; viewAngleSample < viewAngleSamples; ++viewAngleSample, viewAngle += viewAngleInc )
			{
				Vector3 viewDir = new Vector3( Functions.Sin( viewAngle ), 0, Functions.Cos( viewAngle ) );

				for ( int sunAngleSample = 0; sunAngleSample < sunAngleSamples; ++sunAngleSample )
				{
					float bCoeff = bCoeffLut[ sunAngleSample ];
					float height = innerRadius;
					for ( int heightSample = 0; heightSample < heightSamples; ++heightSample, height += heightInc )
					{
						ComputeScattering( viewDir, bCoeff, height, voxels );
						voxels += 3;
					}
				}
			}
		}

		/// <summary>
		/// Computes the scattering term for a given sun angle, viewer orientation, and height. Stores
		/// the result in a 3-component voxel
		/// </summary>
		private unsafe	static void ComputeScattering( Vector3 viewDir, float bCoeff, float height, byte* voxel )
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

			float bR = ComputeScattering( RWavelength, viewDir, bCoeff * InvRWavelength4, height );
			float bG = ComputeScattering( GWavelength, viewDir, bCoeff * InvGWavelength4, height );
			float bB = ComputeScattering( BWavelength, viewDir, bCoeff * InvBWavelength4, height );

			voxel[ 0 ] = ( byte )Utils.Clamp( bR * 255.0f, 0, 255 );
			voxel[ 1 ] = ( byte )Utils.Clamp( bG * 255.0f, 0, 255 );
			voxel[ 2 ] = ( byte )Utils.Clamp( bB * 255.0f, 0, 255 );
		}

		/// <summary>
		/// Computes the scattering term for a given sun angle, viewer orientation, height and wavelength.
		/// </summary>
		private static float ComputeScattering( float wavelength, Vector3 viewDir, float phaseCoeff, float height )
		{
			float Fr = phaseCoeff * wavelength;

			for ( int i = 0; i < AttenuationSamples; ++i )
			{
			}

			return K * Fr;
		}

		private const int AttenuationSamples = 100;

		private const float InvH0 = 1.0f / 7994.0f;
		private const float K = 0;
		private const float RWavelength = 650;
		private const float GWavelength = 610;
		private const float BWavelength = 475;
		private const float InvRWavelength4 = 1.0f / ( RWavelength * RWavelength * RWavelength * RWavelength );
		private const float InvGWavelength4 = 1.0f / ( GWavelength * GWavelength * GWavelength * GWavelength );
		private const float InvBWavelength4 = 1.0f / ( BWavelength * BWavelength * BWavelength * BWavelength );
		private const float BMul = 3 / ( 16 * Constants.Pi );

		/// <summary>
		/// Builds a lookup table that stores the coefficient in the phase function that is based on the viewer's angle to the sun
		/// </summary>
		private static void BuildAttenuationCoefficientLookupTable( float[] table )
		{
			float sunAngleInc = Constants.TwoPi / ( table.Length - 1 );
			float sunAngle = 0;
			for ( int sunAngleSample = 0; sunAngleSample < table.Length; ++sunAngleSample, sunAngle += sunAngleInc )
			{
				float cosSunAngle = Functions.Cos( sunAngle );
				float bCoeff = BMul * ( 2 + 0.5f * ( cosSunAngle * cosSunAngle ) );
				table[ sunAngleSample ] = bCoeff;
			}
		}

		#endregion
	}
}
