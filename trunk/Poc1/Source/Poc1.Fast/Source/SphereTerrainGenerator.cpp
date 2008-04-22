#include "StdAfx.h"
#include "Mem.h"
#include "SphereTerrainGenerator.h"
#include "Sse\SseSphereTerrainGenerator.h"

namespace Poc1
{
	namespace Fast
	{		
		SphereTerrainGenerator::SphereTerrainGenerator( const TerrainType terrain, const unsigned int seed )
		{
			switch ( terrain )
			{
				case TerrainType_Simple		:
				case TerrainType_Ridged		:
				case TerrainType_Voronoi	:
				case TerrainType_Flat		:
				default						:
					m_pImpl = new ( Aligned( 16 ) ) SseSphereTerrainGenerator( );
					break;
			}
		}

		SphereTerrainGenerator::!SphereTerrainGenerator( )
		{
			AlignedDelete( m_pImpl );
		}

		SphereTerrainGenerator::~SphereTerrainGenerator( )
		{
			AlignedDelete( m_pImpl );
		}

		void SphereTerrainGenerator::GenerateTexture( const CubeMapFace face, const PixelFormat format, const int width, const int height, const int stride, unsigned char* pixels )
		{
			m_pImpl->GenerateTexture( GetUCubeMapFace( face ), GetUPixelFormat( format ), width,  height, stride, pixels );
		}

		void SphereTerrainGenerator::GenerateVertices( Point3 origin, Vector3 xAxis, Vector3 zAxis, void* vertices, const int stride, const int positionOffset, const int normalOffset )
		{
			float originArr[] = { origin->X, origin->Y, origin->Z };
			float xAxisArr[] = { xAxis->X, xAxis->Y, xAxis->Z };
			float zAxisArr[] = { zAxis->X, zAxis->Y, zAxis->Z };

			m_pImpl->GenerateVertices( originArr, xAxisArr, zAxisArr, vertices, stride, positionOffset, normalOffset );
		}

	};
};