#pragma once
#pragma managed( push, off )

#include "SseUtils.h"

namespace Poc1
{
	namespace Fast
	{
		class SseVector
		{

			public :

				SseVector( )
				{
					m_Vec = _mm_set1_ps( 0 );
				}
				
				SseVector( const SseVector& src )
				{
					m_Vec = src.m_Vec;
				}

				SseVector( const float* src )
				{
					m_Vec = _mm_load_ps( src );
				}

				SseVector( const float x, const float y, const float z )
				{
					m_Vec = _mm_set_ps( x, y, z, 0 );
				}

				void Normalise( )
				{
					__m128 m = _mm_mul_ps( m_Vec, m_Vec );	//	x.x, y.y, z.z, 0
															//	x.x + y.y + z.z, 
				}

				static const SseVector Cross( const SseVector& lhs, const SseVector& rhs )
				{
					return SseVector( );
					//vector1 = _mm_load_ps(v1);
					//vector2 = _mm_load_ps(v2);

					//vector3 = _mm_shuffle_ps(vector2, vector1, _MM_SHUFFLE(3, 0, 2, 2));
					//vector4 = _mm_shuffle_ps(vector1, vector2, _MM_SHUFFLE(3, 1, 0, 1));

					//vector5 = _mm_mul_ps(vector3, vector4);

					//vector3 = _mm_shuffle_ps(vector1, vector2, _MM_SHUFFLE(3, 0, 2, 2));
					//vector4 = _mm_shuffle_ps(vector2, vector1, _MM_SHUFFLE(3, 1, 0, 1));

					//vector3 = _mm_mul_ps(vector3, vector4);
					//vector3 = _mm_sub_ps(vector5, vector3);

					//_mm_store_ps(out, vector3);

					//out[1] *= -1;
				}

			private :

				__m128 m_Vec;
		};
	};
};

#pragma managed( pop )
