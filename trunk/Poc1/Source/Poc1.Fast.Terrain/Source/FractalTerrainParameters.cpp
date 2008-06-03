#include "stdafx.h"
#include "FractalTerrainParameters.h"
#include "Sse\SseSimpleFractal.h"
#include "Sse\SseRidgedFractal.h"

#pragma managed

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{

			void FractalTerrainParameters::Setup( SseSimpleFractal& fractal )
			{
				if ( Seed != -1 )
				{
					fractal.GetNoise( ).SetNewSeed( Seed );
				}
				fractal.Setup( Frequency, Lacunarity, Octaves );
			}
			
			void FractalTerrainParameters::Setup( SseRidgedFractal& fractal )
			{
				if ( Seed != -1 )
				{
					fractal.GetNoise( ).SetNewSeed( Seed );
				}
				fractal.Setup( Frequency, Lacunarity, Octaves );
			}

		};
	};
};