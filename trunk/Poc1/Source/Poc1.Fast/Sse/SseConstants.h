#pragma once
#pragma managed(push, off)

#include "Poc1.Fast.h"
#include <emmintrin.h>

namespace Poc1
{
	namespace Fast
	{
		///	\brief	Handy packed integer/floating point constants
		struct FAST_API Constants
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

			///	\brief	Constants setup
			///
			///	Constants setup initializes the static constant values - must be done explicitly
			///	because initalizer code containing intrinsics is not allowed in managed C++ assemblies
			///
			///	Trying to initialize directly causes the following warning, followed by error:
			///	warning C4793: 'aligned data types not supported in managed code' : causes native code generation for function '`dynamic initializer for 'Poc1::Fast::Constants::Ic_FF'''
			///
			///	InitializeConstants() is called by the SseNoise constructor
			///
			static void InitializeConstants( );
		};
	};
};

#pragma managed(pop)
