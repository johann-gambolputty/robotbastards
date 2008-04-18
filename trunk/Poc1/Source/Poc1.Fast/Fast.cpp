#include "stdafx.h"
#include "FastNoise.h"
#include "SseNoise.h"

namespace Poc1
{
	namespace Fast
	{

		float FastNoise::Noise( float x, float y, float z )
		{
			float vec0[3] = { x, y, z };
			float vec1[3] = { x, y, z };
			float vec2[3] = { x, y, z };
			float vec3[3] = { x, y, z };
			SseNoise::Noise( vec0, vec1, vec2, vec3 );
			return x;
		}
	};
};