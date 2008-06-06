#pragma once

#include "SseConstants.h"
#include "UEnums.h"

#pragma managed(push, off)

namespace Poc1
{
	namespace Fast
	{
		///	\brief	Returns the negative of a floating point vector
		inline __m128 Neg( const __m128 val )
		{
			return _mm_sub_ps( Constants::Fc_0, val );
		}

		///	\brief	Clamps 4 values to 4 ranges
		inline __m128 Clamp( const __m128& val, const __m128& min, const __m128& max )
		{
			return _mm_max_ps( _mm_min_ps( val, max ), min );
		}

		///	\brief	Gets the position on a cube map face
		inline void CubeFacePosition( const UCubeMapFace face, const __m128& uuuu, const __m128& vvvv, __m128& xxxx, __m128& yyyy, __m128& zzzz )
		{
			switch ( face )
			{
				default:
				case NegativeX:
				//	x = -1; y = v; z = u;
					xxxx = Constants::Fc_Neg1;
					yyyy = vvvv;
					zzzz = uuuu;
					break;
				case PositiveX:
				//	x = 1; y = v; z = -u;
					xxxx = Constants::Fc_1;
					yyyy = vvvv;
					zzzz = Neg( uuuu );
					break;
				case NegativeY:
					//x = -u; y = -1; z = -v;
					xxxx = Neg( uuuu );
					yyyy = Constants::Fc_Neg1;
					zzzz = Neg( vvvv );
					break;
				case PositiveY:
				//	x = -u; y = 1; z = v;
					xxxx = Neg( uuuu );
					yyyy = Constants::Fc_1;
					zzzz = vvvv;
					break;
				case NegativeZ:
				//	x = -u; y = v; z = -1;
					xxxx = Neg( uuuu );
					yyyy = vvvv;
					zzzz = Constants::Fc_Neg1;
					break;
				case PositiveZ:
					//x = u; y = v; z = 1;
					xxxx = uuuu;
					yyyy = vvvv;
					zzzz = Constants::Fc_1;
					break;
			};

		}

		///	\brief	Rounds 4 floating point values to integers
		inline __m128i RoundToInt( __m128 v )
		{
			return _mm_cvtps_epi32( _mm_sub_ps( v, _mm_set1_ps( 0.5f ) ) );
		}

		///	\brief	Sets the length of 4 vectors
		inline void SetLength( __m128& xxxx, __m128& yyyy, __m128& zzzz, const __m128& len )
		{
			__m128 xxxx2 = _mm_mul_ps( xxxx, xxxx );
			__m128 yyyy2 = _mm_mul_ps( yyyy, yyyy );
			__m128 zzzz2 = _mm_mul_ps( zzzz, zzzz );

			//	NOTE: AP: Don't use reciprocal sqrt - has dubious precision
			__m128 sqr = _mm_sqrt_ps( _mm_add_ps( xxxx2, _mm_add_ps( yyyy2, zzzz2 ) ) );
			sqr = _mm_div_ps( len, sqr );
			xxxx = _mm_mul_ps( xxxx, sqr );
			yyyy = _mm_mul_ps( yyyy, sqr );
			zzzz = _mm_mul_ps( zzzz, sqr );
		}

		///	\brief	Sets the length of 4 vectors
		inline void SetLength( __m128& xxxx, __m128& yyyy, __m128& zzzz, const float len )
		{
			__m128 llll = _mm_set1_ps( len );
			SetLength( xxxx, yyyy, zzzz, llll );
		}

		inline __m128 GetLengths( const __m128& xxxx, const __m128& yyyy, const __m128& zzzz )
		{
			__m128 xxxx2 = _mm_mul_ps( xxxx, xxxx );
			__m128 yyyy2 = _mm_mul_ps( yyyy, yyyy );
			__m128 zzzz2 = _mm_mul_ps( zzzz, zzzz );
			
			//	NOTE: AP: Don't use reciprocal sqrt - has dubious precision
			return _mm_sqrt_ps( _mm_add_ps( xxxx2, _mm_add_ps( yyyy2, zzzz2 ) ) );
		}
		
		///	\brief	Normalizes 4 vectors, stored component-wise in 4 SSE values
		inline void Normalize( __m128& xxxx, __m128& yyyy, __m128& zzzz )
		{
			__m128 xxxx2 = _mm_mul_ps( xxxx, xxxx );
			__m128 yyyy2 = _mm_mul_ps( yyyy, yyyy );
			__m128 zzzz2 = _mm_mul_ps( zzzz, zzzz );
			
			__m128 rsqrt = _mm_rsqrt_ps( _mm_add_ps( xxxx2, _mm_add_ps( yyyy2, zzzz2 ) ) );
			xxxx = _mm_mul_ps( xxxx, rsqrt );
			yyyy = _mm_mul_ps( yyyy, rsqrt );
			zzzz = _mm_mul_ps( zzzz, rsqrt );
		}

		///	\brief	Calculates the dot product of 2 sets of 4 vectors
		inline __m128 Dot( const __m128& xxxx0, const __m128& yyyy0, const __m128& zzzz0, const __m128& xxxx1, const __m128& yyyy1, const __m128& zzzz1 )
		{
			return
				_mm_add_ps
				(
					_mm_mul_ps( xxxx0, xxxx1 ),
					_mm_add_ps
					(
						_mm_mul_ps( yyyy0, yyyy1 ),
						_mm_mul_ps( zzzz0, zzzz1 )
					)
				);
		}

		///	\brief	Gets the cross products of two sets of 4 vectors
		inline void GetCrossProducts( __m128& cpXxxx, __m128& cpYyyy, __m128& cpZzzz, const __m128& xxxx0, const __m128& yyyy0, const __m128& zzzz0, const __m128& xxxx1, const __m128& yyyy1, const __m128& zzzz1 )
		{
			cpXxxx = _mm_sub_ps( _mm_mul_ps( yyyy0, zzzz1 ), _mm_mul_ps( zzzz0, yyyy1 ) );
			cpYyyy = _mm_sub_ps( _mm_mul_ps( zzzz0, xxxx1 ), _mm_mul_ps( xxxx0, zzzz1 ) );
			cpZzzz = _mm_sub_ps( _mm_mul_ps( xxxx0, yyyy1 ), _mm_mul_ps( yyyy0, xxxx1 ) );
		}
		
		///	\brief	Gets the cross products of two sets of 4 vectors, and adds them to cp---- vectors
		inline void AccumulateCrossProducts( __m128& cpXxxx, __m128& cpYyyy, __m128& cpZzzz, const __m128& xxxx0, const __m128& yyyy0, const __m128& zzzz0, const __m128& xxxx1, const __m128& yyyy1, const __m128& zzzz1 )
		{
			cpXxxx = _mm_add_ps( cpXxxx, _mm_sub_ps( _mm_mul_ps( yyyy0, zzzz1 ), _mm_mul_ps( zzzz0, yyyy1 ) ) );
			cpYyyy = _mm_add_ps( cpYyyy, _mm_sub_ps( _mm_mul_ps( zzzz0, xxxx1 ), _mm_mul_ps( xxxx0, zzzz1 ) ) );
			cpZzzz = _mm_add_ps( cpZzzz, _mm_sub_ps( _mm_mul_ps( xxxx0, yyyy1 ), _mm_mul_ps( yyyy0, xxxx1 ) ) );
		}


		///	\brief	Returns the absolute value of a floating point vector
		inline __m128 Abs( const __m128& val )
		{
			return _mm_and_ps( val, Constants::Fc_Sign );
		}

		///	\brief	Fades floating point values
		inline static __m128 Fade( const __m128& v )
		{
			//	6v5-15v4+10v3
			//	= (v.(6v - 15)).v3
			__m128 v3 = _mm_mul_ps( v, _mm_mul_ps( v, v ) );
			__m128 res = _mm_sub_ps( _mm_mul_ps( v, Constants::Fc_6 ), Constants::Fc_15 );
			res = _mm_add_ps( _mm_mul_ps( res, v ), Constants::Fc_10 );
			return _mm_mul_ps( res, v3 );
		}

		///	\brief	Multiplies two 4x32bit integer vectors
		///
		///	There's an intrinsic for multiplying two __m128i values in SSE4 (_mm_mul_epi32), but not in SSE2...
		///
		inline __m128i Mul( const __m128i& val0, const __m128i& val1 )
		{
			__m128i tmp = _mm_slli_epi32( _mm_mulhi_epi16( val0, val1 ), 16 );
			tmp = _mm_or_si128( tmp, _mm_mullo_epi16( val0, val1 ) );
			return tmp;
		}

		///	\brief	Adds a duplicated integer value to an integer vector
		inline __m128i Add( const __m128i& val0, const int val1 )
		{
			return _mm_add_epi32( val0, _mm_set1_epi32( val1 ) );
		}

		///	\brief	Linearly interpolates between 2 floating point vectors
		inline __m128 Lerp( const __m128& t, const __m128 a, const __m128 b )
		{
			return _mm_add_ps( a, _mm_mul_ps( _mm_sub_ps( b, a ), t ) );
		}

		///	\brief	Noise utility function: Returns the gradient for a given hash value
		///
		///	Adapted to SSE2 from Ken Perlin's Improved Noise function:
		///	http://mrl.nyu.edu/~perlin/noise/
		///
		inline __m128 Grad( __m128i h, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz )
		{
			//	Original Perlin code:
		//	int h = hash & 15;
		//	float u = h < 8 ? x : y,
		//		   v = h < 4 ? y : h == 12 || h == 14 ? x : z;
		//	return ( ( h & 1 ) == 0 ? u : -u ) + ( ( h & 2 ) == 0 ? v : -v );
			h = _mm_and_si128( h, Constants::Ic_15 );
			const __m128 hF = _mm_cvtepi32_ps( h );

			const __m128 uMask = _mm_cmplt_ps( hF, Constants::Fc_8 );
			const __m128 vMask = _mm_cmplt_ps( hF, Constants::Fc_4 );

			__m128 uuuu = _mm_and_ps( uMask, xxxx );
			uuuu = _mm_or_ps( uuuu, _mm_andnot_ps( uMask, yyyy ) );

			__m128 vvvv = _mm_and_ps( vMask, yyyy );

			__m128 hMask = _mm_or_ps( _mm_cmpeq_ps( hF, _mm_set1_ps( 12 ) ), _mm_cmpeq_ps( hF, _mm_set1_ps( 14 ) ) );
			__m128 xOrZ = _mm_or_ps( _mm_and_ps( hMask, xxxx ), _mm_andnot_ps( hMask, zzzz ) );
			vvvv = _mm_or_ps( vvvv, _mm_andnot_ps( vMask, xOrZ ) );

			const __m128 hAndOne = _mm_cvtepi32_ps( _mm_and_si128( h, Constants::Ic_2 ) );
			const __m128 hAndTwo = _mm_cvtepi32_ps( _mm_and_si128( h, Constants::Ic_1 ) );
			
			const __m128 val0Mask = _mm_cmpeq_ps( hAndOne, Constants::Fc_0 );
			const __m128 val0 = _mm_or_ps( _mm_and_ps( val0Mask, uuuu ), _mm_andnot_ps( val0Mask, Neg( uuuu ) ) );
			
			const __m128 val1Mask = _mm_cmpeq_ps( hAndTwo, Constants::Fc_0 );
			const __m128 val1 = _mm_or_ps( _mm_and_ps( val1Mask, vvvv ), _mm_andnot_ps( val1Mask, Neg( vvvv ) ) );

			return _mm_add_ps( val0, val1 );
		}

	};
};

#pragma managed(pop)