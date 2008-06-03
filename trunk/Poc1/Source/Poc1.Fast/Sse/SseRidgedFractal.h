#pragma once
#include "SseNoise.h"

#pragma unmanaged

namespace Poc1
{
	namespace Fast
	{
		class _CRT_ALIGN( 16 ) SseRidgedFractal
		{
			public :

				///	\brief	Seed for the noise basis function is default (zero)
				SseRidgedFractal( );
				
				///	\brief	Sets the seed for the noise basis function
				SseRidgedFractal( const unsigned int seed );

				///	\brief	Gets the noise object
				SseNoise& GetNoise( );
				
				///	\brief	Gets the noise object
				const SseNoise& GetNoise( ) const;

				///	\brief	Sets up fractal parameters
				void Setup( const float freq, const float gain, const int numOctaves );

				///	\brief	Gets 4 fractal values from 4 points
				__m128 GetValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const;

				///	\brief	Gets 4 fractal values from 4 points
				__m128 GetSignedValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const;

			private :

				SseNoise	m_Noise;
				__m128 		m_Max;
				__m128 		m_Freq;
				__m128 		m_Gain;
				int			m_NumOctaves;
		};

		inline SseRidgedFractal::SseRidgedFractal( )
		{
			Setup( 1.8f, 0.9f, 8 );
		}
		
		inline SseRidgedFractal::SseRidgedFractal( const unsigned int seed ) :
			m_Noise( seed )
		{
			Setup( 1.8f, 0.9f, 8 );
		}

		inline SseNoise& SseRidgedFractal::GetNoise( )
		{
			return m_Noise;
		}

		inline const SseNoise& SseRidgedFractal::GetNoise( ) const
		{
			return m_Noise;
		}

		inline void SseRidgedFractal::Setup( const float freq, const float gain, const int numOctaves )
		{
			m_Freq = _mm_set1_ps( freq );
			m_Gain = _mm_set1_ps( gain );
			m_NumOctaves = numOctaves;
			
			m_Max = Constants::Fc_1;
			__m128 amp = Constants::Fc_1;
			for ( int octave = 0; octave < numOctaves; ++octave )
			{
				m_Max = _mm_add_ps( m_Max, _mm_div_ps( Constants::Fc_1, amp ) );
				amp = _mm_mul_ps( amp, m_Freq );
			}
		}

		inline __m128 SseRidgedFractal::GetValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const
		{
			__m128 offset = Constants::Fc_1;
			__m128 signal = _mm_sub_ps( offset, Abs( m_Noise.Noise( xxxx, yyyy, zzzz ) ) );
			signal = _mm_mul_ps( signal, signal );
			__m128 result = signal;
			__m128 exp = Constants::Fc_1;

			for ( int octave = 1; octave < m_NumOctaves; ++octave )
			{
				xxxx = _mm_mul_ps( xxxx, m_Freq );
				yyyy = _mm_mul_ps( yyyy, m_Freq );
				zzzz = _mm_mul_ps( zzzz, m_Freq );

				__m128 weight = _mm_mul_ps( signal, m_Gain );
				weight = _mm_and_ps( weight, _mm_cmpgt_ps( weight, Constants::Fc_0 ) );

				__m128 weightMask = _mm_cmple_ps( weight, Constants::Fc_1 );
				weight = _mm_or_ps( _mm_and_ps( weightMask, weight ), _mm_andnot_ps( weightMask, Constants::Fc_1 ) );

				__m128 basis = m_Noise.Noise( xxxx, yyyy, zzzz );
				basis = Abs( basis );
				signal = _mm_sub_ps( offset, basis );
				signal = _mm_mul_ps( signal, signal );
				signal = _mm_mul_ps( signal, weight );
				result = _mm_add_ps( result, _mm_div_ps( signal, exp ) );
				exp = _mm_mul_ps( exp, m_Freq );
			}

			return _mm_div_ps( result, m_Max );
		}
		
		inline __m128 SseRidgedFractal::GetSignedValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const
		{
			return _mm_sub_ps( _mm_mul_ps( GetValue( xxxx, yyyy, zzzz ), Constants::Fc_2 ), Constants::Fc_1 );
		}
	};
};
