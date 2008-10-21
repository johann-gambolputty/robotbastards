#pragma once
#include "TerrainFunction.h"

#pragma managed( push, on )

namespace Poc1
{
	namespace Fast
	{
		#pragma managed( off )

		class SseSimpleFractal;
		class SseRidgedFractal;

		#pragma managed( on )


		namespace Terrain
		{
			public ref class FractalTerrainParameters : public TerrainFunctionParameters
			{
				public :

					///	\brief	Sets up default fractal parameters
					FractalTerrainParameters( );

					///	\brief	Sets up a simple fractal from these parameters
					void Setup( SseSimpleFractal& fractal );
					
					///	\brief	Sets up a ridged fractal from these parameters
					void Setup( SseRidgedFractal& fractal );

					///	\brief	Gets/sets the seed value used to initialize the noise basis function of the fractal
					property int Seed
					{
						int get( ) { return m_Seed; }
						void set( int value ) { m_Seed = value; }
					}

					///	\brief	Gets/sets the number of octaves in the fractal. Default is 8
					property int Octaves
					{
						int get( ) { return m_NumOctaves; }
						void set( int value ) { m_NumOctaves = value; }
					}

					///	\brief	Gets/sets the fractal frequency. Default is 1.1
					property float Frequency
					{
						float get( ) { return m_Frequency; }
						void set( float value ) { m_Frequency = value; }
					}
					
					///	\brief	Gets/sets the fractal lacunarity. Default is 1.2
					property float Lacunarity
					{
						float get( ) { return m_Lacunarity; }
						void set( float value ) { m_Lacunarity = value; }
					}


				private :

					int		m_Seed;
					float	m_Frequency;
					int		m_NumOctaves;
					float	m_Lacunarity;

			}; //FractalTerrainParameters

		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed( pop )