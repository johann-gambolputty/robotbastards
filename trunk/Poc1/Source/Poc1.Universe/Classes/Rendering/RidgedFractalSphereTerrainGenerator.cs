using Rb.Core.Maths;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Generates sphere planet terrain from a ridged multi-fractal
	/// </summary>
	public class RidgedFractalSphereTerrainGenerator : SphereTerrainGenerator
	{
		/// <summary>
		/// Sets up this terrain generator
		/// </summary>
		public RidgedFractalSphereTerrainGenerator( )
		{
			m_Noise = new Noise( ).GetNoise;
		}

		/// <summary>
		/// Sets up this terrain generator
		/// </summary>
		/// <param name="seed">Noise seed</param>
		public RidgedFractalSphereTerrainGenerator( int seed )
		{
			m_Noise = new Noise( seed ).GetNoise;
		}

		/// <summary>
		/// Gets the height of a point on the surface of the surface
		/// </summary>
		public override float GetHeight( float x, float y, float z )
		{
			float fVal = Fractals.SimpleFractal( x, y, z, 0.9f, 8, 1.0f, m_Noise ) * 0.8f;
			fVal += ( Fractals.RidgedFractal( x, y, z, 2.1f, 8, 2.0f, m_Noise ) - 0.5f ) * 0.2f;
			fVal = Utils.Clamp( fVal, 0, 1 );

			return fVal;
		}

		private readonly Fractals.Basis3dFunction m_Noise;
	}
}
