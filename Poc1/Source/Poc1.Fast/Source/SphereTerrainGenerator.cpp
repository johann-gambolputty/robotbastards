#include "StdAfx.h"
#include "Mem.h"
#include "SphereTerrainGenerator.h"
#include "Sse\SseSphereTerrainGenerator.h"

namespace Poc1
{
	namespace Fast
	{		
		SphereTerrainGenerator::SphereTerrainGenerator( const TerrainGeneratorType generatorType, const unsigned int seed )
		{
			switch ( generatorType )
			{
				case TerrainGeneratorType::Ridged	:
					{
						SseSphereTerrainGeneratorT< SseRidgedFractalDisplacer >* impl = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< SseRidgedFractalDisplacer >( );
						impl->GetDisplacer( ).GetFractal( ).GetNoise( ).SetNewSeed( seed );
						impl->GetDisplacer( ).GetFractal( ).Setup( 2.0f, 0.9f, 12 );
						m_pImpl = impl;
						break;
					}
				case TerrainGeneratorType::Flat		:
				case TerrainGeneratorType::Simple	:
				case TerrainGeneratorType::Voronoi	:
				default								:
					m_pImpl = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< SseFlatSphereTerrainDisplacer >( );
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

		void SphereTerrainGenerator::SetHeightRange( const float minHeight, const float maxHeight )
		{
			m_pImpl->GetBaseDisplacer( ).Setup( minHeight, ( minHeight + maxHeight ) / 2, maxHeight );
		}

		void SphereTerrainGenerator::GenerateTexture( const CubeMapFace face, const PixelFormat format, const int width, const int height, const int stride, unsigned char* pixels )
		{
			m_pImpl->GenerateTexture( GetUCubeMapFace( face ), GetUPixelFormat( format ), width,  height, stride, pixels );
		}

		void SphereTerrainGenerator::GenerateVertices( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, void* vertices, const int stride, const int positionOffset, const int normalOffset )
		{
			float originArr[] = { origin->X, origin->Y, origin->Z };
			float xStepArr[] = { xStep->X, xStep->Y, xStep->Z };
			float zStepArr[] = { zStep->X, zStep->Y, zStep->Z };

			m_pImpl->GenerateVertices( originArr, xStepArr, zStepArr, width, height, vertices, stride, positionOffset, normalOffset );
		}

	};
};