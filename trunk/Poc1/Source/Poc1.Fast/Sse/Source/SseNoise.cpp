#include "stdafx.h"
#include "Sse/SseNoise.h"

#include <stdlib.h>
#include <emmintrin.h>

#pragma managed(off)

extern "C" bool __stdcall DllMain( void*, int, void* )
{
	Poc1::Fast::Constants::InitializeConstants( );
	return true;
}

namespace Poc1
{
	namespace Fast
	{

		///	\brief	Generates noise in the range 0..256
		inline static __m128 Noise256( const SseNoise& noise, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz )
		{
			return _mm_mul_ps( _mm_add_ps( noise.Noise( xxxx, yyyy, zzzz ), _mm_set1_ps( 1.0f ) ), _mm_set1_ps( 128.0f ) );
		}

		SseNoise::SseNoise( )
		{
			Constants::InitializeConstants( );
			InitializePerms( 0 );
			_MM_SET_ROUNDING_MODE( _MM_ROUND_NEAREST );
		}

		SseNoise::SseNoise( unsigned int seed )
		{
			Constants::InitializeConstants( );
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
					__m128 noiseVal = Noise256( *this, xxxx, yyyy, zzzz );
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

		void Poc1::Fast::SseNoise::GenerateTiledBitmap( const int width, const int height, const int stride, unsigned char* pixels, const float startX, const float startY, const float noiseWidth, const float noiseHeight ) const
		{
			float incX = noiseWidth / ( float )width;
			float incY = noiseHeight / ( float )height;

			__m128 originXxxx = _mm_set_ps( startX, startX + incX, startX + incX * 2, startX + incX * 3 );
			__m128 incXxxx = _mm_set_ps( incX * 4, incX * 4, incX * 4, incX * 4 );
			const __m128 zzzz = _mm_set1_ps( 2 );

			__m128 normalize = _mm_div_ps( _mm_set1_ps( 128 ), _mm_set1_ps( noiseWidth * noiseHeight ) );
			__m128 wwww = _mm_set1_ps( noiseWidth );
			__m128 hhhh = _mm_set1_ps( noiseHeight );
			_CRT_ALIGN(16) float tileNArr[ 4 ];

			unsigned char* curPixel = pixels;
			float y = startY;
			int w4 = width / 4;
			for ( int row = 0; row < height; ++row, y += incY )
			{
				/*
				float wrapY = y - m_Height;
				float fY = y - m_StartY;
				float invY = m_Height - fY;
				*/
				const __m128 wrapYyyy = _mm_set1_ps( y - noiseHeight );
				const __m128 fYyyy = _mm_set1_ps( y - startY );
				const __m128 invYyyy = _mm_sub_ps( hhhh, fYyyy );
				const __m128 yyyy = _mm_set1_ps( y );
				__m128 xxxx = originXxxx;
				for ( int col = 0; col < w4; ++col )
				{
					__m128 wrapXxxx = _mm_sub_ps( xxxx, wwww );
					__m128 fXxxx = _mm_sub_ps( xxxx, _mm_set1_ps( startX ) );
					__m128 invXxxx= _mm_sub_ps( wwww, fXxxx );
					/*
					float wrapX = x - m_Width;
					float fX = x - m_StartX;
					float invX = m_Width - fX;

					float v0 = basis( x, y ) * invX * invY;
					float v1 = basis( wrapX, y ) * fX * invY;
					float v2 = basis( wrapX, wrapY ) * fX * fY;
					float v3 = basis( x, wrapY ) * invX * fY;

					return ( v0 + v1 + v2 + v3 ) * m_InvWH;
					*/

					__m128 n0 = Noise( xxxx, yyyy, zzzz );
					__m128 n1 = Noise( wrapXxxx, yyyy, zzzz );
					__m128 n2 = Noise( wrapXxxx, wrapYyyy, zzzz );
					__m128 n3 = Noise( xxxx, wrapYyyy, zzzz );

					n0 = _mm_mul_ps( n0, _mm_mul_ps( invXxxx, invYyyy ) );
					n1 = _mm_mul_ps( n1, _mm_mul_ps( fXxxx, invYyyy ) );
					n2 = _mm_mul_ps( n2, _mm_mul_ps( fXxxx, fYyyy ) );
					n3 = _mm_mul_ps( n3, _mm_mul_ps( invXxxx, fYyyy ) );

					const __m128 sumN =_mm_add_ps( _mm_add_ps( n0, n1 ), _mm_add_ps( n2, n3 ) );
					__m128 tileN = _mm_add_ps( _mm_div_ps( sumN, normalize ), _mm_set1_ps( 128 ) );
					tileN = Clamp( tileN, _mm_set1_ps( 0 ), _mm_set1_ps( 256 ) );

					_mm_store_ps( tileNArr, tileN );

					*curPixel = ( unsigned char )( tileNArr[ 3 ] ); curPixel += stride;
					*curPixel = ( unsigned char )( tileNArr[ 2 ] ); curPixel += stride;
					*curPixel = ( unsigned char )( tileNArr[ 1 ] ); curPixel += stride;
					*curPixel = ( unsigned char )( tileNArr[ 0 ] ); curPixel += stride;

					xxxx = _mm_add_ps( xxxx, incXxxx );
				}
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