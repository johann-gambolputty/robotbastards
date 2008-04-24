#pragma once
#pragma managed( push, off )

#include "UColour.h"
#include "Sse\SseNoise.h"
#include <list>
#include <vector>

namespace Poc1
{
	namespace Fast
	{
		class UTerrainDistributionBuilder
		{
			public :

				struct Sample
				{
					float m_Fraction;
					float m_Value;
				};

				typedef std::vector<Sample> SampleCollection;

				///	\brief	Adds a sample point
				UTerrainDistributionBuilder& Add( const float fraction, const float value );

				///	\brief	Adds a sample point
				UTerrainDistributionBuilder& AddSharp( const float fraction, const float value );

				///	\brief	Gets all samples
				const SampleCollection& Samples( ) const;

				///	\brief	Converts to a floating point array
				void Resample( const int size, float* samples ) const;

			private :

				SampleCollection m_Samples;

		}; //UTerrainDistributionBuilder

		class UTerrainDistribution
		{
			public :
				
				static const int SampleCount = 256;

				///	\brief	Sets all samples to zero
				UTerrainDistribution( );

				///	\brief	Builds this terrain distribution
				void Build( const UTerrainDistributionBuilder& altitudes, const UTerrainDistributionBuilder& slopes );

				float GetAltitude( const int sample ) const;
				float GetSlope( const int sample ) const;

			private :

				float	m_Altitudes[ SampleCount ];
				float	m_Slopes[ SampleCount ];

		}; //UTerrainDistribution

		
		inline float UTerrainDistribution::GetAltitude( const int sample ) const
		{
			return m_Altitudes[ sample ];
		}

		inline float UTerrainDistribution::GetSlope( const int sample ) const
		{
			return m_Slopes[ sample ];
		}


		///	\brief	Selects terrain based on latitude, elevation and slope
		class USphereTerrainTypeSelector
		{
			public :

				static const int MaxTypes = 16;

				///	\brief	Terrain type information
				struct TerrainType
				{
					std::string	m_Name;		///<	For debugging purposes
					UColour		m_Colour;	///<	Terrain colour
				};

				///	\brief	Distributions for terrain types over different latitudes
				struct LatitudeData
				{
					UTerrainDistribution m_Distributions[ MaxTypes ];
				};

				///	\brief	Releases all stored latitude distributions
				~USphereTerrainTypeSelector( );

				///	\brief	Adds a terrain type
				int AddType( const char* name, const UColour& colour );

				///	\brief	Gets an indexed terrain type
				TerrainType& GetType( const int index );

				///	\brief	Gets an indexed terrain type
				const TerrainType& GetType( const int index ) const;

				///	\brief	Adds a latitude distribution to the selector
				LatitudeData& AddLatitudeData( );

				///	\brief	Determines the colour of a particular (latitude, altitude, slope) vector
				const UColour GetColour( const float latitude, const float altitude, const float slope, const float weightMul ) const;

			private :

				typedef std::vector<LatitudeData*> LatitudeDataCollection;

				LatitudeDataCollection m_Latitudes;
				std::vector<TerrainType> m_TerrainTypes;

				SseNoise m_Noise;

		}; //USphereTerrainTypeSelector

		inline USphereTerrainTypeSelector::TerrainType& USphereTerrainTypeSelector::GetType( const int index )
		{
			return m_TerrainTypes[ index ];
		}
		
		inline const USphereTerrainTypeSelector::TerrainType& USphereTerrainTypeSelector::GetType( const int index ) const
		{
			return m_TerrainTypes[ index ];
		}
		
		inline const UColour USphereTerrainTypeSelector::GetColour( const float latitude, const float altitude, const float slope, const float weightMul ) const
		{
			LatitudeData& latitudeData = *m_Latitudes[ ( int )( latitude * m_Latitudes.size( ) ) ];

			const int altitudeSample = ( int )( altitude * UTerrainDistribution::SampleCount );
			const int slopeSample = ( int )( slope * UTerrainDistribution::SampleCount );
			const int numTypes = m_TerrainTypes.size( );
			float weights[ MaxTypes ];
			float totalWeight = 0;
			int typeIndex = 0;
			for ( ; typeIndex < numTypes; ++typeIndex )
			{
				const UTerrainDistribution& distribution = latitudeData.m_Distributions[ typeIndex ];
				float weight = distribution.GetAltitude( altitudeSample ) * distribution.GetSlope( slopeSample );
				weight *= weightMul;
				weights[ typeIndex ] = weight;
				totalWeight += weight;
			}

			float r = 0;
			float g = 0;
			float b = 0;
			for ( typeIndex = 0; typeIndex < numTypes; ++typeIndex )
			{
				const float contrib = weights[ typeIndex ] / totalWeight;
				r += contrib * ( float )( m_TerrainTypes[ typeIndex ].m_Colour.R( ) );
				g += contrib * ( float )( m_TerrainTypes[ typeIndex ].m_Colour.G( ) );
				b += contrib * ( float )( m_TerrainTypes[ typeIndex ].m_Colour.B( ) );
			}

			return UColour( ( unsigned char )r, ( unsigned char )g, ( unsigned char )b );
		}

	}; //Fast
}; //Poc1

#pragma managed( pop )
