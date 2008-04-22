#pragma once
#pragma managed(push, off)

#include "SseUtils.h"


namespace Poc1
{
	namespace Fast
	{
		///	\brief	Fast noise implementation using SSE SIMD instructions
		class _CRT_ALIGN(16) SseNoise
		{
			public :

				///	\brief	Initializes this noise object with a seed value of zero
				SseNoise( );

				///	\brief	Initializes this noise object with a supplied seed value
				SseNoise( unsigned int seed );

				///	\brief	Fills a bitmap with noise values
				void GenerateRgbBitmap( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow ) const;

				//	TODO: AP: Add SSE detection

				///	\brief	Generates 4 noise values from 4 input vectors
				///
				///	Note that performance isn't fantastic due to requirement of loading unaligned addresses into SSE2 registers
				///	Still about twice as fast as the non-SIMD version.
				///
				void Noise( const float* pVec0, const float* pVec1, const float* pVec2, const float* pVec3, float* pResults ) const;

				///	\brief	Generates 4 noise values from 4 input vectors
				__m128 Noise( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const;

			private :
				
				int m_Perms[ 512 ];

				///	\brief	Initializes permutation table, using a given seed value
				void InitializePerms( const unsigned int seed );

				///	\brief	Generates a permutation of an input vector
				__m128i Perm( __m128i vec ) const;

		};

		inline __m128i Poc1::Fast::SseNoise::Perm( __m128i vec ) const
		{
			//	Originally I thought to get permutation values from a PRN generator (to avoid a potentially expensive
			//	load), like this:
		//	uint n = ( uint )( x );
		//	n = ( n << 13 ) ^ n;
		//	return ( ( n * ( n * n * 15731 + 789221 ) + 1376312589 ) & 0xff;
			//	SSE2 version:
		//	vec = _mm_xor_si128( _mm_slli_epi32( vec, 13 ), vec );
		//	vec = _mm_add_epi32( Mul( _mm_add_epi32( Mul( vec, Mul( vec, m_Prime0 ) ), m_Prime1 ), vec ), m_Prime2 );
		//	return _mm_and_si128( vec, Constants::Ic_FF );

			//	But it produced odd patterns in the noise, so I switched back to the standard improved noise
			//	method of using a pre-computed permutation table as a temporary measure...
			//	I don't know why, but doing it like this has not slowed things down at all...
			return _mm_set_epi32( m_Perms[vec.m128i_i32[3]], m_Perms[vec.m128i_i32[2]], m_Perms[vec.m128i_i32[1]], m_Perms[vec.m128i_i32[0]]);
		}

		inline __m128 Poc1::Fast::SseNoise::Noise( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const
		{
			__m128i ixxxx = RoundToInt( xxxx );
			__m128i iyyyy = RoundToInt( yyyy );
			__m128i izzzz = RoundToInt( zzzz );

			xxxx = _mm_sub_ps( xxxx, _mm_cvtepi32_ps( ixxxx ) );
			yyyy = _mm_sub_ps( yyyy, _mm_cvtepi32_ps( iyyyy ) );
			zzzz = _mm_sub_ps( zzzz, _mm_cvtepi32_ps( izzzz ) );
			
			ixxxx = _mm_and_si128( ixxxx, Constants::Ic_FF );
			iyyyy = _mm_and_si128( iyyyy, Constants::Ic_FF );
			izzzz = _mm_and_si128( izzzz, Constants::Ic_FF );

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

			__m128i AA1 = Perm( _mm_add_epi32( AA, Constants::Ic_1 ) );
			__m128i BA1 = Perm( _mm_add_epi32( BA, Constants::Ic_1 ) );
			__m128i AB1 = Perm( _mm_add_epi32( AB, Constants::Ic_1 ) );
			__m128i BB1 = Perm( _mm_add_epi32( BB, Constants::Ic_1 ) );
			AA = Perm( AA );
			BA = Perm( BA );
			AB = Perm( AB );
			BB = Perm( BB );


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
						Lerp( fade0, Grad( AA1, xxxx, yyyy, lzzzz ), Grad( BA1, lxxxx, yyyy, lzzzz ) ),
						Lerp( fade0, Grad( AB1, xxxx, lyyyy, lzzzz ), Grad( BB1, lxxxx, lyyyy, lzzzz ) )
					)
				); 
			res = _mm_div_ps( res, _mm_set1_ps( 0.9f ) );

			return res;
		}
	};
};

#pragma managed(pop)
