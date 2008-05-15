#pragma once

#ifdef POC1_FAST_EXPORTS

	#define FAST_API __declspec( dllexport )

#else

	#define FAST_API __declspec( dllimport )

#endif