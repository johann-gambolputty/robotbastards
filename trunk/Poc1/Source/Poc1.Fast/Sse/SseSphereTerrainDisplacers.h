#pragma once
#pragma managed(push, off)

#include "SseRidgedFractal.h"

namespace Poc1
{
	namespace Fast
	{
		//	--------------------------------------------------------------------------------- Types

		///	\brief	Base class for terrain displacers
		class SseSphereTerrainDisplacer
		{
			public :

				///	\brief	Sets up reasonable defaults
				SseSphereTerrainDisplacer( )
				{
					Setup( 1.0f, 2.0f, 3.0f );
				}

				///	\brief	Sets up this displacer
				void Setup( float minHeight, float seaLevel, float maxHeight )
				{
					m_MinHeight = _mm_set1_ps( minHeight );
					m_SeaLevel = _mm_set1_ps( seaLevel );
					m_MaxHeight = _mm_set1_ps( maxHeight );
					m_HeightRange = _mm_sub_ps( m_MaxHeight, m_MinHeight );
				}

			protected :

				__m128 m_MinHeight;
				__m128 m_SeaLevel;
				__m128 m_MaxHeight;
				__m128 m_HeightRange;

				///	\brief	Maps 4 normalized height values into the height range of this displacer
				__m128 MapToHeightRange( const __m128 heights ) const
				{
					return _mm_add_ps( m_MinHeight, _mm_mul_ps( heights, m_HeightRange ) );
				}
		};

		///	\brief	SseSphereTerrainGenerator Displacer type. Does not perturb input positions
		class SseFlatSphereTerrainDisplacer : public SseSphereTerrainDisplacer
		{
			public :

				///	\brief	Maps 4 (x,y,z) vectors onto the minimum distance of this displacer.
				inline __m128 Displace( __m128& xxxx, __m128& yyyy, __m128& zzzz ) const
				{
					xxxx = _mm_mul_ps( xxxx, m_MinHeight );
					yyyy = _mm_mul_ps( yyyy, m_MinHeight );
					zzzz = _mm_mul_ps( zzzz, m_MinHeight );
					return _mm_set1_ps( 1 );
				}
		};

		///	\brief	SseSphereTerrainGenerator Displacer type. Uses a ridged fractal to generate input positions
		class SseRidgedFractalDisplacer : public SseSphereTerrainDisplacer
		{
			public :

				SseRidgedFractalDisplacer( )
				{
				}

				///	\brief	Gets the fractal object
				SseRidgedFractal& GetFractal( )
				{
					return m_Fractal;
				}
				
				///	\brief	Gets the fractal object
				const SseRidgedFractal& GetFractal( ) const
				{
					return m_Fractal;
				}

				///	\brief	Maps 4 (x,y,z) vectors onto the minimum distance of this displacer.
				inline __m128 Displace( __m128& xxxx, __m128& yyyy, __m128& zzzz ) const
				{
					__m128 heights = m_Fractal.GetValue( xxxx, yyyy, zzzz );
					const __m128 actualHeights = MapToHeightRange( heights );
					xxxx = _mm_mul_ps( xxxx, actualHeights );
					yyyy = _mm_mul_ps( yyyy, actualHeights );
					zzzz = _mm_mul_ps( zzzz, actualHeights );
					return heights;
				}

			private :

				SseRidgedFractal m_Fractal;
		};
	}; //Fast
}; //Poc1

#pragma managed(pop)
