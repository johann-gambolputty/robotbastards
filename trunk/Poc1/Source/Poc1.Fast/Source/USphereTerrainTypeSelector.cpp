#include "stdafx.h"
#include "USphereTerrainTypeSelector.h"

namespace Poc1
{
	namespace Fast
	{
		UTerrainDistributionBuilder& UTerrainDistributionBuilder::Add( const float fraction, const float value )
		{
			Sample s;
			s.m_Fraction = fraction;
			s.m_Value = value;
			m_Samples.push_back( s );
			return *this;
		}
		
		UTerrainDistributionBuilder& UTerrainDistributionBuilder::AddSharp( const float fraction, const float value )
		{
			if ( !m_Samples.empty( ) )
			{
				Add( fraction - 0.1f, m_Samples.back( ).m_Value );
			}
			Sample s;
			s.m_Fraction = fraction;
			s.m_Value = value;
			m_Samples.push_back( s );
			return *this;
		}

		const UTerrainDistributionBuilder::SampleCollection& UTerrainDistributionBuilder::Samples( ) const
		{
			return m_Samples;
		}

		inline float Lerp( float a, float b, float t )
		{
			return a + ( b - a ) * t;
		}

		void UTerrainDistributionBuilder::Resample( const int size, float* samples ) const
		{
			int numSamples = ( int )m_Samples.size( );
			if ( m_Samples.size( ) == 1 )
			{
				for( int i = 0; i < numSamples; ++i )
				{
					samples[ i ] = m_Samples[ 0 ].m_Value;
				}
				return;
			}

			float inc = 1.0f / ( size - 1 );
			int sampleIndex = 0;

			float t = 0.0f;
			float startT = m_Samples[ 0 ].m_Fraction;
			float endT = m_Samples[ 1 ].m_Fraction;
			float curSample = m_Samples[ 0 ].m_Value;
			float nextSample = m_Samples[ 1 ].m_Value;

			for ( int i = 0; i < size; ++i )
			{
				samples[ i ] = t < startT ? curSample : Lerp( curSample, nextSample, ( t - startT ) / ( endT - startT ) );

				t += inc;
				if ( t >= endT )
				{
					++sampleIndex;
					bool lastSample = sampleIndex == numSamples - 1;
					curSample = m_Samples[ sampleIndex ].m_Value;
					nextSample = lastSample ? curSample : m_Samples[ sampleIndex + 1 ].m_Value;
					startT = m_Samples[ sampleIndex ].m_Fraction;
					endT = lastSample ? 2.0f : m_Samples[ sampleIndex + 1 ].m_Fraction;
				}
			}
		}

		UTerrainDistribution::UTerrainDistribution( )
		{
			for ( int index = 0; index < SampleCount; ++index )
			{
				m_Altitudes[ index ] = m_Slopes[ index ] = 0;
			}
		}

		void UTerrainDistribution::Build( const UTerrainDistributionBuilder& altitudes, const UTerrainDistributionBuilder& slopes )
		{
			altitudes.Resample( SampleCount, m_Altitudes );
			slopes.Resample( SampleCount, m_Slopes );
		}

		//	---------------------------------------------------- USphereTerrainTypeSelector Methods

		USphereTerrainTypeSelector::~USphereTerrainTypeSelector( )
		{
			for ( LatitudeDataCollection::iterator distPos = m_Latitudes.begin( ); distPos != m_Latitudes.end( ); ++distPos )
			{
				delete *distPos;
			}
		}

		int USphereTerrainTypeSelector::AddType( const char* name, const UColour& colour )
		{
			TerrainType type;
			type.m_Name = name;
			type.m_Colour = colour;
			m_TerrainTypes.push_back( type );

			return m_TerrainTypes.size( ) - 1;
		}

		USphereTerrainTypeSelector::LatitudeData& USphereTerrainTypeSelector::AddLatitudeData( )
		{
			LatitudeData* data = new LatitudeData( );
			m_Latitudes.push_back( data );
			return *data;
		}

		//	---------------------------------------------------------------------------------------

	}; //Fast
}; //Poc1