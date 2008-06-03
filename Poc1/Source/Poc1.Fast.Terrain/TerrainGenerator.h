#pragma once

#include "UTerrainVertex.h"
#include "TerrainFunction.h"

#pragma managed( push, off )

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			class SseTerrainDisplacer;

			class TerrainGenerator
			{
				public :

					TerrainGenerator( );

					///	\brief	Gets the displacer
					virtual SseTerrainDisplacer& GetBaseDisplacer( ) = 0;

					///	\brief	Gets the displacer
					virtual const SseTerrainDisplacer& GetBaseDisplacer( ) const = 0;

					///	\brief	Sets the smallest possible step size (finest LOD)
					void SetSmallestStepSize( const float x, const float z );

					///	\brief	Generates terrain vertex points and normals
					virtual void GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, float uvRes, UTerrainVertex* vertices ) = 0;

					///	\brief	Generates terrain vertex points and normals. Gets maximum patch error
					virtual void GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, float uvRes, UTerrainVertex* vertices, float& maxError ) = 0;

				protected :

					float m_SmallestX;
					float m_SmallestZ;

			}; //TerrainGenerator

			//
			//	C#: SphereTerrain
			//                  |
			//	Managed C++: SphereTerrainGenerator
			//					|
			//	Unmanaged: TerrainGenerator->SseSphereTerrainGenerator=>SseTerrainDisplacer
			//
			//	C#: TerrainMesh
			//					|
			//	Managed C++: FlatTerrainGenerator
			//
			//	Unmanaged: TerrainGenerator->SseFlatTerrainGenerator=>SseTerrainDisplacer
			//
			
			//	--------------------------------------------------- TerrainGenerator Inline Methods

			inline TerrainGenerator::TerrainGenerator( ) :
				m_SmallestX( 0 ),
				m_SmallestZ( 0 )
			{
			}

			inline void TerrainGenerator::SetSmallestStepSize( const float x, const float z )
			{
				m_SmallestX = x;
				m_SmallestZ = z;
			}

			//	-----------------------------------------------------------------------------------

		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed( pop )