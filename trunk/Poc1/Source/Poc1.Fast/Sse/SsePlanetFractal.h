#pragma once
#include "SseNoise.h"

#pragma unmanaged

namespace Poc1
{
	namespace Fast
	{
		///	\brief	Planet fractal. Uses a low-frequency fractal to define continent regions, then a much
		///	higher frequency fractal for terrain details (kind of breaks the notion of fractal scale...)
		class _CRT_ALIGN( 16 ) SsePlanetFractal
		{
			public :

				///	\brief	Seed for the noise basis function is default (zero)
				SsePlanetFractal( );
				
				///	\brief	Sets the seed for the noise basis function
				SsePlanetFractal( const unsigned int seed );

				///	\brief	Gets the noise object
				SseNoise& GetNoise( );
				
				///	\brief	Gets the noise object
				const SseNoise& GetNoise( ) const;

				///	\brief	Sets up fractal parameters
				void Setup( const float freq, const float gain, const int lowOctaves, const int highOctaves, const float coordinateMultiplier );

				///	\brief	Gets 4 fractal values from 4 points
				__m128 GetValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const;

				///	\brief	Gets 4 fractal values from 4 points
				__m128 GetSignedValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const;

			private :

				SseNoise	m_Noise;
				__m128		m_CoordinateMultiplier;
				__m128 		m_Max;
				__m128 		m_Freq;
				__m128 		m_Gain;
				int			m_NumOctaves;
		};
		
		inline SsePlanetFractal::SsePlanetFractal( )
		{
			Setup( 1.8f, 0.9f, 2, 8, 2.0f );
		}
		
		inline SsePlanetFractal::SsePlanetFractal( const unsigned int seed ) :
			m_Noise( seed )
		{
			Setup( 1.8f, 0.9f, 2, 8, 2.0f );
		}

		inline SseNoise& SsePlanetFractal::GetNoise( )
		{
			return m_Noise;
		}

		inline const SseNoise& SsePlanetFractal::GetNoise( ) const
		{
			return m_Noise;
		}

		inline void SsePlanetFractal::Setup( const float freq, const float gain, const int lowOctaves, const int highOctaves, const float coordinateMultiplier )
		{
			m_Freq = _mm_set1_ps( freq );
			m_Gain = _mm_set1_ps( gain );
			m_CoordinateMultiplier = _mm_set1_ps( coordinateMultiplier );
			m_NumOctaves = highOctaves;
			
			m_Max = Constants::Fc_1;
			__m128 amp = Constants::Fc_1;
			for ( int octave = 0; octave < lowOctaves; ++octave )
			{
				m_Max = _mm_add_ps( m_Max, _mm_div_ps( Constants::Fc_1, amp ) );
				amp = _mm_mul_ps( amp, m_Freq );
			}
		}

		inline __m128 SsePlanetFractal::GetValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const
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
		
		inline __m128 SsePlanetFractal::GetSignedValue( __m128 xxxx, __m128 yyyy, __m128 zzzz ) const
		{
			return _mm_sub_ps( _mm_mul_ps( GetValue( xxxx, yyyy, zzzz ), Constants::Fc_2 ), Constants::Fc_1 );
		}
	}; //Fast
}; //Poc1