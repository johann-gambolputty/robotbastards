#pragma once
#pragma managed(push, off)

#include <emmintrin.h>


namespace Poc1
{
	namespace Fast
	{
		///	\brief	Handy packed integer/floating point constants
		struct Constants
		{
			//@{
			///	\name	Packed 32-bit integer values

			static __m128i Ic_FF;
			static __m128i Ic_15;
			static __m128i Ic_2;
			static __m128i Ic_1;

			//@}

			//@{
			///	\name	32-bit floating point values

			static __m128 Fc_Sign;	///<	Floating point sign bit - used for fp abs function
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
			static __m128 Fc_Neg1;

			//@}

			///	\brief	Constants constructor
			///
			///	Constants instance constructor initializes the static constant values - must be done in this
			///	odd way because initalizer code containing intrinsics is not allowed in managed C++ assemblies
			///
			///	Trying to initialize directly causes the following warning, followed by error:
			///	warning C4793: 'aligned data types not supported in managed code' : causes native code generation for function '`dynamic initializer for 'Poc1::Fast::Constants::Ic_FF'''
			///
			Constants( );
		};


		//	Fast noise implementation using SSE SIMD instructions
		class _CRT_ALIGN(16) SseNoise
		{
			public :

				///	\brief	Initializes this noise object with a seed value of zero
				SseNoise( );

				///	\brief	Initializes this noise object with a supplied seed value
				SseNoise( unsigned int seed );

				//	TODO: AP: Add SSE detection

				void GenerateRgbBitmap( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow );
				void GenerateRgbSimpleFractal( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow );
				void GenerateRgbRidgedFractal( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow );

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

				void InitializePerms( const unsigned int seed );

				__m128i Perm( __m128i vec ) const;
				__m128 SimpleFractal( __m128 xxxx, __m128 yyyy, __m128 zzzz );
				__m128 RidgedFractal( __m128 xxxx, __m128 yyyy, __m128 zzzz );
		};

		///	\brief	Returns the absolute value of a floating point vector
		inline __m128 Abs( const __m128& val )
		{
			return _mm_and_ps( val, Constants::Fc_Sign );
		}

		///	\brief	Rounds floating point values
		inline static __m128 Round( __m128 v )
		{
			return _mm_cvtepi32_ps( _mm_cvttps_epi32( v ) );
		}

		///	\brief	Fades floating point values
		inline static __m128 Fade( __m128 v )
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

		///	\brief	Returns the negative of a floating point vector
		inline __m128 Neg( const __m128 val )
		{
			return _mm_sub_ps( Constants::Fc_0, val );
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
			vvvv = _mm_or_ps( vvvv, _mm_andnot_ps( vMask, zzzz ) );	//	TODO: AP: Add (h==12||h==14) condition

			const __m128 hAndOne = _mm_cvtepi32_ps( _mm_and_si128( h, Constants::Ic_2 ) );
			const __m128 hAndTwo = _mm_cvtepi32_ps( _mm_and_si128( h, Constants::Ic_1 ) );
			
			const __m128 val0Mask = _mm_cmpeq_ps( hAndOne, Constants::Fc_0 );
			const __m128 val0 = _mm_or_ps( _mm_and_ps( val0Mask, uuuu ), _mm_andnot_ps( val0Mask, Neg( uuuu ) ) );
			
			const __m128 val1Mask = _mm_cmpeq_ps( hAndTwo, Constants::Fc_0 );
			const __m128 val1 = _mm_or_ps( _mm_and_ps( val1Mask, vvvv ), _mm_andnot_ps( val1Mask, Neg( vvvv ) ) );

			return _mm_add_ps( val0, val1 );
		}

		inline __m128i Poc1::Fast::SseNoise::Perm( __m128i vec ) const
		{
			//	Originally I thought to get permutation values from a PRN generator like this:
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
			return _mm_set_epi32( m_Perms[vec.m128i_i32[0]], m_Perms[vec.m128i_i32[1]], m_Perms[vec.m128i_i32[2]], m_Perms[vec.m128i_i32[3]]);
		}

		inline __m128 Poc1::Fast::SseNoise::Noise( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const
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
	};
};

#pragma managed(pop)
