#pragma once
#pragma managed(push, off)



namespace Poc1
{
	namespace Fast
	{
		//	Fast noise implementation using SSE SIMD instructions
		class SseNoise
		{
			public :

				//	TODO: AP: Add SSE detection
				
				//	3D noise for four positions in parallel
				static void Noise( const float* pVec0, const float* pVec1, const float* pVec2, const float* pVec3 );

			private :

				static const int SseCpuFeature = 0x0002;
		};
	};
};

#pragma managed(pop)
