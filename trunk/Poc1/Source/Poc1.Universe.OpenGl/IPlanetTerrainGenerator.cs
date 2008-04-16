
using Rb.Core.Maths;
using System.Drawing.Imaging;

namespace Poc1.Universe.OpenGl
{
	internal enum PlanetMapFace
	{
		NegX,
		PosX,
		NegY,
		PosY,
		NegZ,
		PosZ
	}

	internal class PlanetMap
	{
		public static void UvToXyz( float u, float v, PlanetMapFace face, out float x, out float y, out float z )
		{
			switch ( face )
			{
				default:
				case PlanetMapFace.NegX:
					x = -1;
					y = v;
					z = u;
					break;
				case PlanetMapFace.PosX:
					x = 1;
					y = v;
					z = -u;
					break;
				case PlanetMapFace.NegY:
					x = -u;
					y = -1;
					z = -v;
					break;
				case PlanetMapFace.PosY:
					x = -u;
					y = 1;
					z = v;
					break;
				case PlanetMapFace.NegZ:
					x = -u;
					y = v;
					z = -1;
					break;
				case PlanetMapFace.PosZ:
					x = u;
					y = v;
					z = 1;
					break;
			}

		}
	
		public static void UvToSt( float u, float v, PlanetMapFace face, out float s, out float t )
		{
			//
			//	x, y, z
			//	r=length
			//	s=invTan(y/x)
			//	t=invCos(z/r)
			//

			float x, y, z;
			UvToXyz( u, v, face, out x, out y, out z );
			float invLength = 1.0f / Functions.Sqrt( u * u + v * v + 1 );
			s = Functions.Atan2( x, y );
			t = Functions.Acos( z * invLength );
		}

	}

	internal unsafe interface IPlanetTerrainGenerator
	{
		PixelFormat CubeMapFormat
		{
			get;
		}

		void GenerateSide( PlanetMapFace face, byte* pixels, int width, int height, int stride );
	}
}
