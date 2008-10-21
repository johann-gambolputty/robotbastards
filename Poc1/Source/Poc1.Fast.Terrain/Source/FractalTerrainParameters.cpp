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
			
			FractalTerrainParameters::FractalTerrainParameters( )
			{
				m_Seed = -1;

#ifdef _DEBUG
				m_NumOctaves = 8;
#else
				m_NumOctaves = 16;
#endif
				m_Frequency = 1.826098f;
				m_Lacunarity = 1.18897f;
			}

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