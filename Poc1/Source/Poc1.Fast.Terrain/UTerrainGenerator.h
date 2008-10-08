#pragma once

#include "UTerrainVertex.h"
#include "UEnums.h"
#include "TerrainFunction.h"

#pragma managed( push, off )

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			class SseTerrainDisplacer;

			class UTerrainGenerator
			{
				public :

					UTerrainGenerator( );

					///	\brief	Gets the displacer
					virtual SseTerrainDisplacer& GetBaseDisplacer( ) = 0;

					///	\brief	Gets the displacer
					virtual const SseTerrainDisplacer& GetBaseDisplacer( ) const = 0;

					///	\brief	Sets the smallest possible step size (finest LOD)
					void SetSmallestStepSize( const float x, const float z );

					///	\brief	Generates a cube map face bitmap
					virtual void GenerateTerrainPropertyCubeMapFace( const UCubeMapFace face, const int width, const int height, const int stride, unsigned char* pixels ) = 0;

					///	\brief	Generates terrain vertex points and normals
					virtual void GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, const float* uv, float uvRes, UTerrainVertex* vertices ) = 0;

					///	\brief	Generates terrain vertex points and normals. Gets maximum patch error
					virtual void GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, const float* uv, float uvRes, UTerrainVertex* vertices, float& maxError ) = 0;

				protected :

					float m_SmallestX;
					float m_SmallestZ;

			}; //UTerrainGenerator
			
			//	--------------------------------------------------- TerrainGenerator Inline Methods

			inline UTerrainGenerator::UTerrainGenerator( ) :
				m_SmallestX( 0 ),
				m_SmallestZ( 0 )
			{
			}

			inline void UTerrainGenerator::SetSmallestStepSize( const float x, const float z )
			{
				m_SmallestX = x;
				m_SmallestZ = z;
			}

			//	-----------------------------------------------------------------------------------

		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed( pop )