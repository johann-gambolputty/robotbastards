#pragma once
#pragma managed(push, off)

#include <emmintrin.h>


namespace Poc1
{
	namespace Fast
	{
		//	Fast noise implementation using SSE SIMD instructions
		class _CRT_ALIGN(16) SseNoise
		{
			public :

				SseNoise( );

				SseNoise( unsigned int seed );

				//	TODO: AP: Add SSE detection

				void GenerateRgbBitmap( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow );
				void GenerateRgbSimpleFractal( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow );
				void GenerateRgbRidgedFractal( const int width, const int height, unsigned char* pixels, const float* origin, const float* incCol, const float* incRow );

				//	3D noise for four positions in parallel
				void Noise( const float* pVec0, const float* pVec1, const float* pVec2, const float* pVec3, float* pResults );

			private :

				void InitializePerms( const unsigned int seed );

				__m128i m_Prime0;
				__m128i m_Prime1;
				__m128i m_Prime2;
				int m_Perms[ 512 ];

				__m128i Perm( __m128i vec );
				__m128 Noise( __m128 xxxx, __m128 yyyy, __m128 zzzz );
				__m128 SimpleFractal( __m128 xxxx, __m128 yyyy, __m128 zzzz );
				__m128 RidgedFractal( __m128 xxxx, __m128 yyyy, __m128 zzzz );
				static const int SseCpuFeature = 0x0002;
		};
	};
};

#pragma managed(pop)
