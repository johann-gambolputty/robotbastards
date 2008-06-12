
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Tools.Atmosphere
{
	public class Atmosphere
	{
		public float InnerRadius
		{
			get { return m_InnerRadius; }
			set { m_InnerRadius = value; }
		}

		public float OuterRadius
		{
			get { return m_OuterRadius; }
			set { m_OuterRadius = value; }
		}

		public ITexture BuildLookupTexture( int viewAngleSamples, int sunAngleSamples, int heightSamples, float innerRadius, float outerRadius )
		{
			float viewAngle = 0;
			float viewAngleInc = Constants.TwoPi / ( viewAngleSamples - 1 );
			float sunAngleInc = Constants.TwoPi / ( sunAngleSamples - 1 );
			float heightInc = ( outerRadius - innerRadius ) / ( heightSamples - 1 );
			for ( int viewAngleSample = 0; viewAngleSample < viewAngleSamples; ++viewAngleSample, viewAngle += viewAngleInc )
			{
				Vector3 viewDir = new Vector3( Functions.Sin( viewAngle ), 0, Functions.Cos( viewAngle ) );

				float sunAngle = 0;
				for ( int sunAngleSample = 0; sunAngleSample < sunAngleSamples; ++sunAngleSample, sunAngle += sunAngleInc )
				{
					float height = innerRadius;
					for ( int heightSample = 0; heightSample < heightSamples; ++heightSample, height += heightInc )
					{
						ComputeScattering( viewDir, sunAngle, height );
					}
				}
			}
			return null;
		}

		private float m_InnerRadius = 0;
		private float m_OuterRadius = 10000;

		private static void ComputeScattering( Vector3 viewDir, float sunAngle, float height )
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
			//			H0 = 7994m
			//	k = number of samples to take along view vector
			//
		}
	}
}
