#include "stdafx.h"
#include "Sse/SseConstants.h"
#include <emmintrin.h>

#pragma unmanaged

namespace Poc1
{
	namespace Fast
	{
		static const int SseCpuFeature = 0x0002;

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
		__m128 Constants::Fc_Neg1;

		void Constants::InitializeConstants( )
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
			Fc_Neg1 = _mm_set1_ps( -1 );
		}

	}; //Fast
}; //Poc1