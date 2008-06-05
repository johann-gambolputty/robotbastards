#include "StdAfx.h"
#include "Mem.h"
#include "TerrainGenerator.h"
#include "UTerrainGenerator.h"
#include "Sse/SseTerrainDisplacer.h"

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
		
			//	---------------------------------------------------------- TerrainGenerator Methods

			TerrainGenerator::TerrainGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction )
			{
				/*
				switch ( generatorType )
				{
					case TerrainGeneratorType::Ridged	:
						{
							if ( enableGroundDisplacement )
							{
								const float groundDisplacementInfluence = 0.3f;
								typedef SseFull3dDisplacer< SseSimpleFractal, SseRidgedFractal > DisplacerType;
								SseSphereTerrainGeneratorT< DisplacerType >* impl;
								impl = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< DisplacerType >( );

								impl->GetDisplacer( ).GetBaseDisplacer( ).GetFunction( ).GetNoise( ).SetNewSeed( seed );
								impl->GetDisplacer( ).GetBaseDisplacer( ).GetFunction( ).Setup( 4.1f, 0.8f, 12 );
								impl->GetDisplacer( ).GetFunction( ).GetNoise( ).SetNewSeed( seed );
								impl->GetDisplacer( ).GetFunction( ).Setup( 2.1f, 1.1f, 4 );
								impl->GetDisplacer( ).SetInfluence( groundDisplacementInfluence );
								m_pImpl = impl;
							}
							else
							{
								typedef SseFunction3dDisplacer< SseRidgedFractal > DisplacerType;
								SseSphereTerrainGeneratorT< DisplacerType >* impl = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< DisplacerType >( );
								impl->GetDisplacer( ).GetFunction( ).GetNoise( ).SetNewSeed( seed );
								impl->GetDisplacer( ).GetFunction( ).Setup( 3.1f, 1.1f, 10 );
								m_pImpl = impl;
							}
							break;
						}
					case TerrainGeneratorType::Flat		:
					case TerrainGeneratorType::Simple	:
					case TerrainGeneratorType::Voronoi	:
					default								:
						m_pImpl = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< SseFlatTerrainDisplacer >( );
						break;
				}
				*/
				m_pImpl = TerrainFunction::CreateGenerator( geometry, heightFunction );
			}

			TerrainGenerator::TerrainGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction, TerrainFunction^ groundFunction )
			{
				m_pImpl = TerrainFunction::CreateGenerator( geometry, heightFunction, groundFunction );
			}

			TerrainGenerator::!TerrainGenerator( )
			{
				AlignedDelete( m_pImpl );
			}

			TerrainGenerator::~TerrainGenerator( )
			{
				AlignedDelete( m_pImpl );
			}

			void TerrainGenerator::SetSmallestStepSize( const float x, const float z )
			{
				m_pImpl->SetSmallestStepSize( x, z );
			}

			void TerrainGenerator::Setup( const float patchScale, const float minHeight, const float maxHeight, const float terrainFunctionRadius )
			{
				m_pImpl->GetBaseDisplacer( ).Setup( patchScale, minHeight, ( minHeight + maxHeight ) / 2, maxHeight, terrainFunctionRadius );
			}

			void TerrainGenerator::GenerateTerrainPropertyCubeMapFace( const CubeMapFace face, const int width, const int height, const int stride, unsigned char* pixels )
			{
				//	TODO: AP: reimplement for sphere geometries
			//	m_pImpl->GenerateTerrainPropertyCubeMapFace( GetUCubeMapFace( face ), width,  height, stride, pixels );
			}

			void TerrainGenerator::GenerateVertices( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, float uvRes, void* vertices )
			{
				float originArr[] = { origin->X, origin->Y, origin->Z };
				float xStepArr[] = { xStep->X, xStep->Y, xStep->Z };
				float zStepArr[] = { zStep->X, zStep->Y, zStep->Z };

				m_pImpl->GenerateVertices( originArr, xStepArr, zStepArr, width, height, uvRes, ( UTerrainVertex* )vertices );
			}

			void TerrainGenerator::GenerateVertices( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, float uvRes, void* vertices, [System::Runtime::InteropServices::Out]float% error )
			{
				float originArr[] = { origin->X, origin->Y, origin->Z };
				float xStepArr[] = { xStep->X, xStep->Y, xStep->Z };
				float zStepArr[] = { zStep->X, zStep->Y, zStep->Z };

				float err;
				m_pImpl->GenerateVertices( originArr, xStepArr, zStepArr, width, height, uvRes, ( UTerrainVertex* )vertices, err );
				error = err;
			}
			
			//	-----------------------------------------------------------------------------------
		}; //Fast
	}; //Terrain
}; //Poc1
