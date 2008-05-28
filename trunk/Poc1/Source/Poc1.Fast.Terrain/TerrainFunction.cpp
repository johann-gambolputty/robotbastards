#include "stdafx.h"
#include "TerrainFunction.h"

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			ref class SimpleFractalTerrainParameters
			{
				public :

					SimpleFractalTerrainParameters( )
					{
						m_NumOctaves = 8;
						m_Frequency = 1.1f;
						m_Lacunarity = 1.2f;
					}

					property int Octaves
					{
						int get( ) { return m_NumOctaves; }
						void set( int value ) { m_NumOctaves = value; }
					}

					property float Frequency
					{
						float get( ) { return m_Frequency; }
						void set( float value ) { m_Frequency = value; }
					}
					
					property float Lacunarity
					{
						float get( ) { return m_Lacunarity; }
						void set( float value ) { m_Lacunarity = value; }
					}


				private :

					float m_Frequency;
					int m_NumOctaves;
					float m_Lacunarity;

			}; //SimpleFractalTerrainParameters


			System::String^ SimpleFractalTerrainFunction::Name::get( )
			{
				return "Simple Fractal";
			}

			System::Object^ SimpleFractalTerrainFunction::CreateParameters( )
			{
				return gcnew SimpleFractalTerrainParameters( );
			}
		};
	};
};