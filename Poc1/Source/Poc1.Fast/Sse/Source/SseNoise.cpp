#include "stdafx.h"
#include "Sse/SseNoise.h"

#include <stdlib.h>
#include <emmintrin.h>

#pragma managed(off)

namespace Poc1
{
	namespace Fast
	{
		SseNoise::SseNoise( )
		{
			InitializePerms( 0 );
			_MM_SET_ROUNDING_MODE( _MM_ROUND_NEAREST );
		}

		SseNoise::SseNoise( unsigned int seed )
		{
			InitializePerms( seed );
			_MM_SET_ROUNDING_MODE( _MM_ROUND_NEAREST );
		}

		void SseNoise::SetNewSeed( const unsigned int seed )
		{
			InitializePerms( seed );
		}

		void Poc1::Fast::SseNoise::GenerateRgbBitmap( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow ) const
		{
			const int w4 = width / 4;

			__m128 rowxxxx = _mm_set_ps( origin[ 0 ], origin[ 0 ] + incCol[ 0 ], origin[ 0 ] + 2 * incCol[ 0 ], origin[ 0 ] + 3 * incCol[ 0 ] );
			__m128 rowyyyy = _mm_set_ps( origin[ 1 ], origin[ 1 ] + incCol[ 1 ], origin[ 1 ] + 2 * incCol[ 1 ], origin[ 1 ] + 3 * incCol[ 1 ] );
			__m128 rowzzzz = _mm_set_ps( origin[ 2 ], origin[ 2 ] + incCol[ 2 ], origin[ 2 ] + 2 * incCol[ 2 ], origin[ 2 ] + 3 * incCol[ 2 ] );

			__m128 incColxxxx = _mm_set_ps( incCol[ 0 ] * 4, incCol[ 0 ] * 4, incCol[ 0 ] * 4, incCol[ 0 ] * 4 );
			__m128 incColyyyy = _mm_set_ps( incCol[ 1 ] * 4, incCol[ 1 ] * 4, incCol[ 1 ] * 4, incCol[ 1 ] * 4 );
			__m128 incColzzzz = _mm_set_ps( incCol[ 2 ] * 4, incCol[ 2 ] * 4, incCol[ 2 ] * 4, incCol[ 2 ] * 4 );

			__m128 incRowxxxx = _mm_set_ps( incRow[ 0 ], incRow[ 0 ], incRow[ 0 ], incRow[ 0 ] );
			__m128 incRowyyyy = _mm_set_ps( incRow[ 1 ], incRow[ 1 ], incRow[ 1 ], incRow[ 1 ] );
			__m128 incRowzzzz = _mm_set_ps( incRow[ 2 ], incRow[ 2 ], incRow[ 2 ], incRow[ 2 ] );

			_CRT_ALIGN(16) float res[ 4 ];

			unsigned char* firstRowPixel = pixels;
			int stride = width * 3;
			for ( int row = 0; row < height; ++row, firstRowPixel += stride )
			{
				__m128 xxxx = rowxxxx;
				__m128 yyyy = rowyyyy;
				__m128 zzzz = rowzzzz;
				unsigned char* curPixel = firstRowPixel;
				for ( int col = 0; col < w4; ++col )
				{
					__m128 noiseVal = _mm_add_ps( Noise( xxxx, yyyy, zzzz ), Constants::Fc_1 );
					noiseVal = _mm_mul_ps( noiseVal, Constants::Fc_128 );
					_mm_store_ps( res, noiseVal );
					unsigned char b0 = ( unsigned char )( res[ 3 ] );
					unsigned char b1 = ( unsigned char )( res[ 2 ] );
					unsigned char b2 = ( unsigned char )( res[ 1 ] );
					unsigned char b3 = ( unsigned char )( res[ 0 ] );

					curPixel[ 0 ] = curPixel[ 1 ] = curPixel[ 2 ] = b0;
					curPixel[ 3 ] = curPixel[ 4 ] = curPixel[ 5 ] = b1;
					curPixel[ 6 ] = curPixel[ 7 ] = curPixel[ 8 ] = b2;
					curPixel[ 9 ] = curPixel[ 10 ] = curPixel[ 11 ] = b3;

					curPixel += 12;
					xxxx = _mm_add_ps( xxxx, incColxxxx );
					yyyy = _mm_add_ps( yyyy, incColyyyy );
					zzzz = _mm_add_ps( zzzz, incColzzzz );
				}
				rowxxxx = _mm_add_ps( rowxxxx, incRowxxxx );
				rowyyyy = _mm_add_ps( rowyyyy, incRowyyyy );
				rowzzzz = _mm_add_ps( rowzzzz, incRowzzzz );
			}
		}

		void Poc1::Fast::SseNoise::Noise( const float* pVec0, const float* pVec1, const float* pVec2, const float* pVec3, float* pResults ) const
		{
			__m128 xxxx = _mm_set_ps( pVec0[ 0 ], pVec1[ 0 ], pVec2[ 0 ], pVec3[ 0 ] );
			__m128 yyyy = _mm_set_ps( pVec0[ 1 ], pVec1[ 1 ], pVec2[ 1 ], pVec3[ 1 ] );
			__m128 zzzz = _mm_set_ps( pVec0[ 2 ], pVec1[ 2 ], pVec2[ 2 ], pVec3[ 2 ] );

			__m128 res = Noise( xxxx, yyyy, zzzz );
			pResults[ 0 ] = res.m128_f32[ 3 ];
			pResults[ 1 ] = res.m128_f32[ 2 ];
			pResults[ 2 ] = res.m128_f32[ 1 ];
			pResults[ 3 ] = res.m128_f32[ 0 ];
		}

		void Poc1::Fast::SseNoise::InitializePerms( const unsigned int seed )
		{
			int perms[ 256 ];

			for ( int i = 0; i < 256; ++i )
			{
				perms[ i ] = i;
			}
			srand( seed );
			for ( int i = 0; i < 256; ++i )
			{
				int index0 = rand( ) & 0xff;
				int index1 = rand( ) & 0xff;

				int tmp = perms[ index0 ];
				perms[ index0 ] = perms[ index1 ];
				perms[ index1 ] = tmp;
			}

			for ( int i = 0; i < 256; ++i )
			{
				m_Perms[ i ] = m_Perms[ i + 256 ] = perms[ i ];
			}
		}

	}; //Fast
}; //Poc1