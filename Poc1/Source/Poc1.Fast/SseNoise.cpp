#include "stdafx.h"
#include "SseNoise.h"

#include <stdlib.h>
#include <emmintrin.h>

#pragma managed(off)

#pragma unmanaged
struct Constants
{
	static __m128i Ic_FF;
	static __m128i Ic_15;
	static __m128i Ic_2;
	static __m128i Ic_1;

	static __m128 Fc_Sign;

	static __m128 Fc_255;
	static __m128 Fc_128;
	static __m128 Fc_15;
	static __m128 Fc_10;
	static __m128 Fc_8;
	static __m128 Fc_6;
	static __m128 Fc_4;
	static __m128 Fc_2;
	static __m128 Fc_1;
	static __m128 Fc_0;

	static void Initialize( )
	{
		Ic_FF = _mm_set1_epi32( 0xff );
		Ic_15 = _mm_set1_epi32( 15 );
		Ic_2 = _mm_set1_epi32( 2 );
		Ic_1 = _mm_set1_epi32( 1 );

		__m128i sgn = _mm_set1_epi32( 0x7fffffff );
		Fc_Sign = *( __m128* )&sgn;
		Fc_255 = _mm_set1_ps( 255 );
		Fc_128 = _mm_set1_ps( 128 );
		Fc_15 = _mm_set1_ps( 15 );
		Fc_10 = _mm_set1_ps( 10 );
		Fc_8 = _mm_set1_ps( 8 );
		Fc_6 = _mm_set1_ps( 6 );
		Fc_4 = _mm_set1_ps( 4 );
		Fc_2 = _mm_set1_ps( 2 );
		Fc_1 = _mm_set1_ps( 1 );
		Fc_0 = _mm_set1_ps( 0 );
	}
};

__m128i Constants::Ic_FF;
__m128i Constants::Ic_15;
__m128i Constants::Ic_2;
__m128i Constants::Ic_1;
__m128 Constants::Fc_Sign;
__m128 Constants::Fc_255;
__m128 Constants::Fc_128;
__m128 Constants::Fc_15;
__m128 Constants::Fc_10;
__m128 Constants::Fc_8;
__m128 Constants::Fc_6;
__m128 Constants::Fc_4;
__m128 Constants::Fc_2;
__m128 Constants::Fc_1;
__m128 Constants::Fc_0;


inline static __m128 Round( __m128 v )
{
	return _mm_cvtepi32_ps( _mm_cvttps_epi32( v ) );
}

inline static __m128 Fade( __m128 v )
{
	//	6v5-15v4+10v3
	//	= (v.(6v - 15)).v3
	__m128 v3 = _mm_mul_ps( v, _mm_mul_ps( v, v ) );
	__m128 res = _mm_sub_ps( _mm_mul_ps( v, Constants::Fc_6 ), Constants::Fc_15 );
	res = _mm_add_ps( _mm_mul_ps( res, v ), Constants::Fc_10 );
	return _mm_mul_ps( res, v3 );
}

inline __m128i Mul( const __m128i& val0, const __m128i& val1 )
{
	//	There's an intrinsic for multiplying two __m128i values in SSE4 (_mm_mul_epi32), but not in SSE2...
	__m128i tmp = _mm_slli_epi32( _mm_mulhi_epi16( val0, val1 ), 16 );
	tmp = _mm_or_si128( tmp, _mm_mullo_epi16( val0, val1 ) );
	return tmp;
}

inline __m128i Add( const __m128i& val0, const int val1 )
{
	return _mm_add_epi32( val0, _mm_set1_epi32( val1 ) );
}

inline __m128 Lerp( const __m128& t, const __m128 a, const __m128 b )
{
	return _mm_add_ps( a, _mm_mul_ps( _mm_sub_ps( b, a ), t ) );
}

/*
inline __m128 FAnd( const __m128i& lhs, const __m128i rhs )
{
	__m128i res = _mm_and_si128( lhs, rhs );
	return *( __m128* )&res;
}
*/

inline __m128 Neg( const __m128 val )
{
	return _mm_sub_ps( Constants::Fc_0, val );
}

inline __m128 Grad( __m128i h, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz )
{
//	int h = hash & 15;                      // CONVERT LO 4 BITS OF HASH CODE
//	float u = h < 8 ? x : y,                 // INTO 12 GRADIENT DIRECTIONS.
//		   v = h < 4 ? y : h == 12 || h == 14 ? x : z;
//	return ( ( h & 1 ) == 0 ? u : -u ) + ( ( h & 2 ) == 0 ? v : -v );
	h = _mm_and_si128( h, Constants::Ic_15 );
	const __m128 hF = _mm_cvtepi32_ps( h );

	const __m128 uMask = _mm_cmplt_ps( hF, Constants::Fc_8 );
	const __m128 vMask = _mm_cmplt_ps( hF, Constants::Fc_4 );

	__m128 uuuu = _mm_and_ps( uMask, xxxx );
	uuuu = _mm_or_ps( uuuu, _mm_andnot_ps( uMask, yyyy ) );

	__m128 vvvv = _mm_and_ps( vMask, yyyy );
	vvvv = _mm_or_ps( vvvv, _mm_andnot_ps( vMask, zzzz ) );	//	TODO: AP: Add (h==12||h==14) condition

	const __m128 hAndOne = _mm_cvtepi32_ps( _mm_and_si128( h, Constants::Ic_2 ) );
	const __m128 hAndTwo = _mm_cvtepi32_ps( _mm_and_si128( h, Constants::Ic_1 ) );
	
	const __m128 val0Mask = _mm_cmpeq_ps( hAndOne, Constants::Fc_0 );
	const __m128 val0 = _mm_or_ps( _mm_and_ps( val0Mask, uuuu ), _mm_andnot_ps( val0Mask, Neg( uuuu ) ) );
	
	const __m128 val1Mask = _mm_cmpeq_ps( hAndTwo, Constants::Fc_0 );
	const __m128 val1 = _mm_or_ps( _mm_and_ps( val1Mask, vvvv ), _mm_andnot_ps( val1Mask, Neg( vvvv ) ) );

	return _mm_add_ps( val0, val1 );
}

inline __m128 Poc1::Fast::SseNoise::Noise( __m128 xxxx, __m128 yyyy, __m128 zzzz )
{
	__m128i ixxxx = _mm_cvttps_epi32( xxxx );
	__m128i iyyyy = _mm_cvttps_epi32( yyyy );
	__m128i izzzz = _mm_cvttps_epi32( zzzz );

	xxxx = _mm_sub_ps( xxxx, _mm_cvtepi32_ps( ixxxx ) );
	yyyy = _mm_sub_ps( yyyy, _mm_cvtepi32_ps( iyyyy ) );
	zzzz = _mm_sub_ps( zzzz, _mm_cvtepi32_ps( izzzz ) );

	__m128 fade0 = Fade( xxxx );
	__m128 fade1 = Fade( yyyy );
	__m128 fade2 = Fade( zzzz );

	//	Determine corner hash values
	__m128i A = _mm_add_epi32( Perm( ixxxx ), iyyyy );
	__m128i AA = _mm_add_epi32( Perm( A ), izzzz );
	__m128i AB = _mm_add_epi32( Perm( _mm_add_epi32( A, Constants::Ic_1 ) ), izzzz );
	__m128i B = _mm_add_epi32( Perm( _mm_add_epi32( ixxxx, Constants::Ic_1 ) ), iyyyy );
	__m128i BA = _mm_add_epi32( Perm( B ), izzzz );
	__m128i BB = _mm_add_epi32( Perm( _mm_add_epi32( B, Constants::Ic_1 ) ), izzzz );

	__m128 lxxxx = _mm_sub_ps( xxxx, Constants::Fc_1 );
	__m128 lyyyy = _mm_sub_ps( yyyy, Constants::Fc_1 );
	__m128 lzzzz = _mm_sub_ps( zzzz, Constants::Fc_1 );

	__m128 res =
		Lerp
		(
			fade2,
			Lerp
			(
				fade1,
				Lerp( fade0, Grad( AA, xxxx, yyyy, zzzz ), Grad( BA, lxxxx, yyyy, zzzz ) ),
				Lerp( fade0, Grad( AB, xxxx, lyyyy, zzzz ), Grad( BB, lxxxx, lyyyy, zzzz ) )
			),
			Lerp
			(
				fade1,
				Lerp( fade0, Grad( AA, xxxx, yyyy, lzzzz ), Grad( BA, lxxxx, yyyy, lzzzz ) ),
				Lerp( fade0, Grad( AB, xxxx, lyyyy, lzzzz ), Grad( BB, lxxxx, lyyyy, lzzzz ) )
			)
		); 
	res = _mm_div_ps( res, _mm_set1_ps( 0.9f ) );

	return res;
}

inline __m128 Poc1::Fast::SseNoise::SimpleFractal( __m128 xxxx, __m128 yyyy, __m128 zzzz )
{
	__m128 total = _mm_set1_ps( 0 );
	__m128 amp = _mm_set1_ps( 1 );
	__m128 freq = _mm_set1_ps( 1.2f );
	__m128 persistence = _mm_set1_ps( 0.6f );
	__m128 max = total;
	int numOctaves = 8;

	for ( int octave = 0; octave < numOctaves; ++octave )
	{
		total = _mm_add_ps( total, _mm_mul_ps( Noise( xxxx, yyyy, zzzz ), amp ) );
		max = _mm_add_ps( max, amp );
		amp = _mm_mul_ps( amp, persistence );
		xxxx = _mm_mul_ps( xxxx, freq );
		yyyy = _mm_mul_ps( yyyy, freq );
		zzzz = _mm_mul_ps( zzzz, freq );
	}

	return _mm_div_ps( _mm_add_ps( total, max ), _mm_mul_ps( max, Constants::Fc_2 ) );
}

inline __m128 Abs( const __m128& val )
{
	return _mm_and_ps( val, Constants::Fc_Sign );
}

inline __m128 Poc1::Fast::SseNoise::RidgedFractal( __m128 xxxx, __m128 yyyy, __m128 zzzz )
{
	__m128 test = Abs( _mm_set1_ps( -1 ) );

	__m128 offset = _mm_set1_ps( 1 );
	__m128 signal = _mm_sub_ps( offset, Abs( Noise( xxxx, yyyy, zzzz ) ) );
	signal = _mm_mul_ps( signal, signal );
	__m128 result = signal;

	__m128 freq = _mm_set1_ps( 1.2f );
	__m128 gain = _mm_set1_ps( 1.8f );
	__m128 exp = _mm_set1_ps( 1 );
	__m128 max = _mm_set1_ps( 1 );

	int numOctaves = 8;

	for ( int octave = 1; octave < numOctaves; ++octave )
	{
		xxxx = _mm_mul_ps( xxxx, freq );
		yyyy = _mm_mul_ps( yyyy, freq );
		zzzz = _mm_mul_ps( zzzz, freq );

		__m128 weight = _mm_mul_ps( signal, gain );
		weight = _mm_and_ps( weight, _mm_cmpgt_ps( weight, Constants::Fc_0 ) );

		__m128 weightMask = _mm_cmple_ps( weight, Constants::Fc_1 );
		weight = _mm_or_ps( _mm_and_ps( weightMask, weight ), _mm_andnot_ps( weightMask, Constants::Fc_1 ) );

		signal = _mm_sub_ps( offset, Abs( Noise( xxxx, yyyy, zzzz ) ) );
		signal = _mm_mul_ps( signal, signal );
		signal = _mm_mul_ps( signal, weight );
		max = _mm_add_ps( max, _mm_div_ps( Constants::Fc_1, exp ) );
		result = _mm_add_ps( result, _mm_div_ps( signal, exp ) );
		exp = _mm_mul_ps( exp, freq );
	}

	return _mm_div_ps( result, max );
}

Poc1::Fast::SseNoise::SseNoise( )
{
	InitializePerms( 0 );
	Constants::Initialize( );
	m_Prime0 = _mm_set1_epi32( 15731 );
	m_Prime1 = _mm_set1_epi32( 789221 );
	m_Prime2 = _mm_set1_epi32( 1376312589 );
	_MM_SET_ROUNDING_MODE( _MM_ROUND_UP );
}

Poc1::Fast::SseNoise::SseNoise( unsigned int seed )
{
	InitializePerms( seed );
	Constants::Initialize( );
	m_Prime0 = _mm_set1_epi32( 15731 );
	m_Prime1 = _mm_set1_epi32( 789221 );
	m_Prime2 = _mm_set1_epi32( 1376312589 );
	_MM_SET_ROUNDING_MODE( _MM_ROUND_UP );
}

inline __m128i Poc1::Fast::SseNoise::Perm( __m128i vec )
{
//	uint n = ( uint )( x );
//	n = ( n << 13 ) ^ n;
//	return ( 1.0f - ( ( n * ( n * n * 15731 + 789221 ) + 1376312589 ) & 0x7fffffff ) / 1073741824.0f );

//	n = ( n << 13 ) ^ n;

//	vec = _mm_xor_si128( _mm_slli_epi32( vec, 13 ), vec );
//	vec = _mm_add_epi32( Mul( _mm_add_epi32( Mul( vec, Mul( vec, m_Prime0 ) ), m_Prime1 ), vec ), m_Prime2 );
//	return _mm_and_si128( vec, Constants::Ic_FF );

	//	I don't know why, but doing it like this has not slowed things down at all...
	return _mm_set_epi32( m_Perms[vec.m128i_i32[0]], m_Perms[vec.m128i_i32[1]], m_Perms[vec.m128i_i32[2]], m_Perms[vec.m128i_i32[3]]);
}

void Poc1::Fast::SseNoise::GenerateRgbBitmap( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow )
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

void Poc1::Fast::SseNoise::GenerateRgbSimpleFractal( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow )
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

	_CRT_ALIGN(16) float res[ 4 ] = { 0, 0, 0, 0 };

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
			__m128 val = _mm_mul_ps( SimpleFractal( xxxx, yyyy, zzzz ), Constants::Fc_255 );
			_mm_store_ps( res, val );
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

void Poc1::Fast::SseNoise::GenerateRgbRidgedFractal( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow )
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

	_CRT_ALIGN(16) float res[ 4 ] = { 0, 0, 0, 0 };

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
			__m128 val = _mm_mul_ps( RidgedFractal( xxxx, yyyy, zzzz ), Constants::Fc_255 );
			_mm_store_ps( res, val );
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

void Poc1::Fast::SseNoise::Noise( const float* pVec0, const float* pVec1, const float* pVec2, const float* pVec3, float* pResults )
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
