
#include "stdafx.h"
#include "LatitudeTerrainFunction.h"

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			LatitudeTerrainFunction::LatitudeTerrainFunction( const TerrainFunctionType functionType, const int numLatitudeBands )
			{
				m_Function = functionType;
				m_Bands = gcnew array<Band^>( numLatitudeBands );

				float step = 1.0f / ( float )numLatitudeBands;
				float lower = 0;
				float upper = step;
				for ( int bandIndex = 0; bandIndex < numLatitudeBands; ++bandIndex )
				{
					m_Bands[ bandIndex ] = gcnew Band( );
					m_Bands[ bandIndex ]->m_LowerBound = lower;
					m_Bands[ bandIndex ]->m_UpperBound = upper;
					m_Bands[ bandIndex ]->m_Parameters = TerrainFunction::CreateParameters( functionType );

					lower = upper;
					upper += step;
				}
			}

			int LatitudeTerrainFunction::NumberOfLatitudeBands::get()
			{
				return m_Bands->Length;
			}

			TerrainFunctionType LatitudeTerrainFunction::FunctionType::get()
			{
				return m_Function;
			}

			LatitudeTerrainFunction::Band^ LatitudeTerrainFunction::GetLatitudeBand( int index )
			{
				return m_Bands[ index ];
			}

		}; // Terrain
	}; //Fast
}; //Poc1