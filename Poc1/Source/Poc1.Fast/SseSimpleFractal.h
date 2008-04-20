#pragma once
#include "SseNoise.h"

#pragma unmanaged

namespace Poc1
{
	namespace Fast
	{
		class _CRT_ALIGN( 16 ) SseSimpleFractal
		{
			public :

				///	\brief	Seed for the noise basis function is default (zero)
				SseSimpleFractal( );
				
				///	\brief	Sets the seed for the noise basis function
				SseSimpleFractal( const unsigned int seed );

				///	\brief	Sets up fractal parameters
				void Setup( const unsigned int seed, const float freq, const float persistence, const int numOctaves );

				///	\brief	Gets 4 fractal values from 4 points
				__m128 GetValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const;

			private :

				SseNoise	m_Noise;
				__m128		m_Max;
				__m128		m_Freq;
				__m128		m_Persistence;
				int			m_NumOctaves;
		};

		inline SseSimpleFractal::SseSimpleFractal( )
		{
		}
		
		inline SseSimpleFractal::SseSimpleFractal( const unsigned int seed ) :
			m_Noise( seed )
		{
		}

		inline void SseSimpleFractal::Setup( const unsigned int seed, const float freq, const float persistence, const int numOctaves )
		{
			m_Freq = _mm_set1_ps( freq );
			m_Persistence = _mm_set1_ps( persistence );
			m_NumOctaves = numOctaves;
			
			m_Max = Constants::Fc_0;
			__m128 amp = Constants::Fc_1;
			for ( int octave = 0; octave < numOctaves; ++octave )
			{
				m_Max = _mm_add_ps( m_Max, amp );
				amp = _mm_mul_ps( amp, m_Persistence );
			}
		}

		inline __m128 SseSimpleFractal::GetValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const
		{
			__m128 total = Constants::Fc_0;
			__m128 amp = Constants::Fc_1;

			for ( int octave = 0; octave < m_NumOctaves; ++octave )
			{
				total = _mm_add_ps( total, _mm_mul_ps( m_Noise.Noise( xxxx, yyyy, zzzz ), amp ) );
				amp = _mm_mul_ps( amp,m_Persistence );
				xxxx = _mm_mul_ps( xxxx, m_Freq );
				yyyy = _mm_mul_ps( yyyy, m_Freq );
				zzzz = _mm_mul_ps( zzzz, m_Freq );
			}

			return _mm_div_ps( _mm_add_ps( total, m_Max ), _mm_mul_ps( m_Max, Constants::Fc_2 ) );
		}
	};
};