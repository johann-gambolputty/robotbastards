#pragma once
#pragma managed( push, off )

#include "SseTerrainDisplacer.h"

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			///	\brief	Base class for plane geometry displacers. Provides functions required by SseTerrainGenerator
			class SsePlaneTerrainDisplacer : public SseTerrainDisplacer
			{
				public :

					inline void GetUpVector( __m128& xxxx, __m128& yyyy, __m128& zzzz )
					{
						xxxx = Constants::Fc_0;
						yyyy = Constants::Fc_1;
						zzzz = Constants::Fc_0;
					}

					inline void MapToDisplacementSpace( __m128& xxxx, __m128& yyyy, __m128& zzzz )
					{
					//	SetLength( xxxx, yyyy, zzzz, GetFunctionScale( ) );
					}
			};

			///	\brief	Displaces heights to the minimum height, for planar geometry
			class SseFlatPlaneTerrainDisplacer : public SsePlaneTerrainDisplacer
			{
				public :

					///	\brief	Maps 4 (x,y,z) vectors onto the minimum distance of this displacer.
					inline __m128 Displace( __m128& xxxx, __m128& yyyy, __m128& zzzz ) const
					{
						yyyy = _mm_mul_ps( yyyy, m_MinHeight );
						return _mm_set1_ps( 0 );
					}
			}; //SseFlatPlaneTerrainDisplacer

		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed( pop )