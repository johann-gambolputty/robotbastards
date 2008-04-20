using Poc1.Fast;
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
			FastNoise n = new FastNoise();
			Point3 test0 = Point3.Origin;
			Point3 test1 = new Point3( 1.1f, 2.2f, 3.3f );
			Point3 test2 = new Point3( 4.1f, 5.2f, 6.3f );
			Point3 test3 = new Point3( -4.1f, -5.2f, 6.3f );
			FastNoiseResult result = n.Noise( test0, test1, test2, test3 );

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
			float fVal = Fractals.RidgedFractal( x, y, z, 1.5f, 8, 0.8f, m_Noise );
			return fVal;
		}

		private readonly Fractals.Basis3dFunction m_Noise;
	}
}
