#include "stdafx.h"
#include "SseNoise.h"

#include <emmintrin.h>

#pragma managed(off)

inline static __m128 Round( __m128 v )
{
	return _mm_cvtepi32_ps( _mm_cvttps_epi32( v ) );
}

inline static __m128 Fade( __m128 v )
{
	//	6v5-15v4+10v3
	//	= (v.(6v - 15)).v3
	__m128 v3 = _mm_mul_ps( v, _mm_mul_ps( v, v ) );
	__m128 res = _mm_sub_ps( _mm_mul_ps( v, _mm_set1_ps( 6 ) ), _mm_set1_ps( 15 ) );
	res = _mm_add_ps( _mm_mul_ps( res, v ), _mm_set1_ps( 10 ) );
	return _mm_mul_ps( res, v3 );
}


void Poc1::Fast::SseNoise::Noise( const float* pVec0, const float* pVec1, const float* pVec2, const float* pVec3, float* results )
{
	__m128 vec0 = _mm_set_ps( pVec0[ 0 ], pVec1[ 0 ], pVec2[ 0 ], pVec3[ 0 ] );
	__m128 vec1 = _mm_set_ps( pVec0[ 1 ], pVec1[ 1 ], pVec2[ 1 ], pVec3[ 1 ] );
	__m128 vec2 = _mm_set_ps( pVec0[ 2 ], pVec1[ 2 ], pVec2[ 2 ], pVec3[ 2 ] );

	__m128i iVec0 = _mm_cvttps_epi32( vec0 );
	__m128i iVec1 = _mm_cvttps_epi32( vec1 );
	__m128i iVec2 = _mm_cvttps_epi32( vec2 );
	
//	uint n = ( uint )( x );
//	n = ( n << 13 ) ^ n;
//	return ( 1.0f - ( ( n * ( n * n * 15731 + 789221 ) + 1376312589 ) & 0x7fffffff ) / 1073741824.0f );

	vec0 = _mm_sub_ps( vec0, _mm_cvtepi32_ps( iVec0 ) );
	vec1 = _mm_sub_ps( vec1, _mm_cvtepi32_ps( iVec1 ) );
	vec2 = _mm_sub_ps( vec2, _mm_cvtepi32_ps( iVec2 ) );

	__m128 fade0 = Fade( vec0 );
	__m128 fade1 = Fade( vec1 );
	__m128 fade2 = Fade( vec2 );

	//	- Get gradients from 8 corners of the cubes
	//	- Interpolate fade curves using gradients

}