#include "StdAfx.h"
#include "Mem.h"
#include "SphereTerrainGenerator.h"
#include "USphereTerrainTypeSelector.h"
#include "Sse\SseSphereTerrainGenerator.h"

namespace Poc1
{
	namespace Fast
	{
		
		//	-------------------------------------------------------- SphereTerrainGenerator Methods

		SphereTerrainGenerator::SphereTerrainGenerator( const TerrainGeneratorType generatorType, const unsigned int seed )
		{
			switch ( generatorType )
			{
				case TerrainGeneratorType::Ridged	:
					{
						SseSphereTerrainGeneratorT< SseRidgedFractalDisplacer >* impl = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< SseRidgedFractalDisplacer >( );
						impl->GetDisplacer( ).GetFractal( ).GetNoise( ).SetNewSeed( seed );
						impl->GetDisplacer( ).GetFractal( ).Setup( 2.1f, 5.0f, 12 );
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

			m_Selector = new USphereTerrainTypeSelector( );
			const int sand		= m_Selector->AddType( "sand", UColour( 255, 255, 192 ) );
			const int dirt		= m_Selector->AddType( "dirt", UColour( 128, 64, 0 ) );
			const int grass		= m_Selector->AddType( "grass", UColour( 0, 255, 0 ) );
			const int forest	= m_Selector->AddType( "forest", UColour( 0, 100, 0 ) );
			const int snow		= m_Selector->AddType( "snow", UColour( 255, 255, 255 ) );
			const int rock		= m_Selector->AddType( "rock", UColour( 80, 80, 80 ) );

			UTerrainDistributionBuilder defaultSlopes;
			defaultSlopes.Add( 0, 1 ).Add( 1, 1 );

			/*
			USphereTerrainTypeSelector::LatitudeData& arctic = m_Selector->AddLatitudeData( );
			arctic.m_Distributions[ snow ].Build
				(
					UTerrainDistributionBuilder( ).Add( 0, 0 ).Add( 0.5f, 1.0f ).Add( 1, 1 ),
					defaultSlopes
				);
			arctic.m_Distributions[ rock ].Build
				(
					UTerrainDistributionBuilder( ).Add( 0, 1 ).Add( 0.5f, 0.0f ),
					defaultSlopes
				);
			*/

			USphereTerrainTypeSelector::LatitudeData& temperate = m_Selector->AddLatitudeData( );

			temperate.m_Distributions[ sand ].Build
				(
					UTerrainDistributionBuilder( ).Add( 0, 1 ).AddSharp( 0.10f, 0.0f ),
					defaultSlopes
				);

			temperate.m_Distributions[ dirt ].Build
				(
					UTerrainDistributionBuilder( ).Add( 0, 0 ).AddSharp( 0.05f, 1.0f ).AddSharp( 0.3f, 0 ),
					defaultSlopes
				);

			temperate.m_Distributions[ grass ].Build
				(
					UTerrainDistributionBuilder( ).Add( 0, 0 ).AddSharp( 0.25f, 1.0f ).AddSharp( 0.5f, 0 ),
					defaultSlopes
				);

			temperate.m_Distributions[ forest ].Build
				(
					UTerrainDistributionBuilder( ).Add( 0, 0 ).AddSharp( 0.45f, 1.0f ).AddSharp( 0.87f, 0 ),
					defaultSlopes
				);

			temperate.m_Distributions[ rock ].Build
				(
					UTerrainDistributionBuilder( ).Add( 0, 0 ).AddSharp( 0.80f, 1.0f ).AddSharp( 0.90f, 0 ),
					defaultSlopes
				);

			temperate.m_Distributions[ snow ].Build
				(
					UTerrainDistributionBuilder( ).Add( 0, 0 ).Add( 0.85f, 1.0f ).Add( 1, 1 ),
					defaultSlopes
				);
		}

		SphereTerrainGenerator::!SphereTerrainGenerator( )
		{
			AlignedDelete( m_pImpl );
			delete m_Selector;
		}

		SphereTerrainGenerator::~SphereTerrainGenerator( )
		{
			AlignedDelete( m_pImpl );
			delete m_Selector;
		}

		void SphereTerrainGenerator::SetHeightRange( const float minHeight, const float maxHeight )
		{
			m_pImpl->GetBaseDisplacer( ).Setup( minHeight, ( minHeight + maxHeight ) / 2, maxHeight );
		}

		void SphereTerrainGenerator::GenerateTexture( const CubeMapFace face, const PixelFormat format, const int width, const int height, const int stride, unsigned char* pixels )
		{
			m_pImpl->GenerateTexture( *m_Selector, GetUCubeMapFace( face ), GetUPixelFormat( format ), width,  height, stride, pixels );
		}

		void SphereTerrainGenerator::GenerateVertices( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, float uvRes, void* vertices )
		{
			float originArr[] = { origin->X, origin->Y, origin->Z };
			float xStepArr[] = { xStep->X, xStep->Y, xStep->Z };
			float zStepArr[] = { zStep->X, zStep->Y, zStep->Z };

			m_pImpl->GenerateVertices( originArr, xStepArr, zStepArr, width, height, uvRes, ( UTerrainVertex* )vertices );
		}

		void SphereTerrainGenerator::GenerateVertices( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, void* vertices, [System::Runtime::InteropServices::Out]float% error )
		{
			float originArr[] = { origin->X, origin->Y, origin->Z };
			float xStepArr[] = { xStep->X, xStep->Y, xStep->Z };
			float zStepArr[] = { zStep->X, zStep->Y, zStep->Z };

			float err;
			m_pImpl->GenerateVertices( originArr, xStepArr, zStepArr, width, height, ( UTerrainVertex* )vertices, err );
			error = err;
		}

		float SphereTerrainGenerator::GetMaximumPatchError( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, const int subdivisions )
		{
			float originArr[] = { origin->X, origin->Y, origin->Z };
			float xStepArr[] = { xStep->X, xStep->Y, xStep->Z };
			float zStepArr[] = { zStep->X, zStep->Y, zStep->Z };

			return m_pImpl->GetMaximumPatchError( originArr, xStepArr, zStepArr, width, height, subdivisions );
		}
		
		//	---------------------------------------------------------------------------------------
	};
};