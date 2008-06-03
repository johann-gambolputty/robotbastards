#pragma once
#pragma managed( push, off )

#include "SseTerrainDisplacer.h"

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			///	\brief	Displaces heights to the minimum height, for planar geometry
			class SseFlatPlaneTerrainDisplacer : SseTerrainDisplacer
			{
			};

		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed( pop )