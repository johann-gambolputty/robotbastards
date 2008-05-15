#include "StdAfx.h"
#include "Mem.h"
#include "SphereTerrainGenerator.h"
#include "Sse\SseSphereTerrainGenerator.h"

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
		
			//	---------------------------------------------------- SphereTerrainGenerator Methods

			SphereTerrainGenerator::SphereTerrainGenerator( const TerrainGeneratorType generatorType, const unsigned int seed )
			{
				switch ( generatorType )
				{
					case TerrainGeneratorType::Ridged	:
						{
							SseSphereTerrainGeneratorT< SseRidgedFractalDisplacer >* impl = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< SseRidgedFractalDisplacer >( );
							impl->GetDisplacer( ).GetFractal( ).GetNoise( ).SetNewSeed( seed );
							impl->GetDisplacer( ).GetFractal( ).Setup( 3.1f, 1.1f, 10 );
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

			void SphereTerrainGenerator::SetSmallestStepSize( const float x, const float z )
			{
				m_pImpl->SetSmallestStepSize( x, z );
			}

			void SphereTerrainGenerator::SetHeightRange( const float minHeight, const float maxHeight )
			{
				m_pImpl->GetBaseDisplacer( ).Setup( minHeight, ( minHeight + maxHeight ) / 2, maxHeight, 6.0f );
			}

			void SphereTerrainGenerator::GenerateTerrainPropertyCubeMapFace( const CubeMapFace face, const int width, const int height, const int stride, unsigned char* pixels )
			{
				m_pImpl->GenerateTerrainPropertyCubeMapFace( GetUCubeMapFace( face ), width,  height, stride, pixels );
			}

			void SphereTerrainGenerator::GenerateVertices( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, float uvRes, void* vertices )
			{
				float originArr[] = { origin->X, origin->Y, origin->Z };
				float xStepArr[] = { xStep->X, xStep->Y, xStep->Z };
				float zStepArr[] = { zStep->X, zStep->Y, zStep->Z };

				m_pImpl->GenerateVertices( originArr, xStepArr, zStepArr, width, height, uvRes, ( UTerrainVertex* )vertices );
			}

			void SphereTerrainGenerator::GenerateVertices( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, float uvRes, void* vertices, [System::Runtime::InteropServices::Out]float% error )
			{
				float originArr[] = { origin->X, origin->Y, origin->Z };
				float xStepArr[] = { xStep->X, xStep->Y, xStep->Z };
				float zStepArr[] = { zStep->X, zStep->Y, zStep->Z };

				float err;
				m_pImpl->GenerateVertices( originArr, xStepArr, zStepArr, width, height, uvRes, ( UTerrainVertex* )vertices, err );
				error = err;
			}

			float SphereTerrainGenerator::GetMaximumPatchError( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, const int subdivisions )
			{
				float originArr[] = { origin->X, origin->Y, origin->Z };
				float xStepArr[] = { xStep->X, xStep->Y, xStep->Z };
				float zStepArr[] = { zStep->X, zStep->Y, zStep->Z };

				return m_pImpl->GetMaximumPatchError( originArr, xStepArr, zStepArr, width, height, subdivisions );
			}
			
			//	-----------------------------------------------------------------------------------
		};
	};
};