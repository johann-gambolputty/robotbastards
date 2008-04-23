#pragma once

namespace Poc1
{
	namespace Fast
	{
		struct Colour
		{
			public :

				Colour( )
				{
					m_R = 0;
					m_G = 0;
					m_B = 0;
					m_A = 0;
				}

				Colour( const unsigned char r, const unsigned char g, const unsigned char b )
				{
					m_R = r;
					m_G = g;
					m_B = b;
					m_A = 0xff;
				}

				Colour( const unsigned char r, const unsigned char g, const unsigned char b, const unsigned char a )
				{
					m_R = r;
					m_G = g;
					m_B = b;
					m_A = a;
				}

			private :

				union
				{
					struct
					{
						unsigned char m_R;
						unsigned char m_G;
						unsigned char m_B;
						unsigned char m_A;
					};
					unsigned int m_Bits;
					unsigned char m_Array[ 4 ];
				};
		};

		//	TODO: AP: This doesn't need to be SSE... probably.
		class SseSphereTerrainTypeSelector
		{
			public :

				static const int MaxTypes = 16;
				static const int SampleCount = 256;

				///	\brief	Parameters for terrain type distribution over altitude and slope ranges
				struct Distribution
				{
					float	m_Altitude[ SampleCount ];
					float	m_Slope[ SampleCount ];
				};

				///	\brief	Distributions for terrain types over different latitudes
				struct LatitudeDistributions
				{
					Distribution m_TypeDistributions[ MaxTypes ];
				};

				///	\brief	Releases all stored latitude distributions
				~SseSphereTerrainTypeSelector( );

				///	\brief	Adds a terrain type
				void AddType( const char* name, const Colour& colour );

				///	\brief	Adds a 
				LatitudeDistributions& AddLatitudeDistribution( );

				///	\brief	Determines the colour of a particular (latitude, altitude, slope) vector
				const Colour GetColour( const float latitude, const float altitude, const float slope ) const;

			private :

				std::vector<LatitudeDistributions*> m_LatitudeDistributions;
				int m_NumTypes;

		}; //SseSphereTerrainTypeSelector

		inline void SseSphereTerrainTypeSelector::AddType( const char* name, const Colour& colour )
		{
			++m_NumTypes;
		}

		inline LatitudeDistributions& AddLatitudeDistribution( )
		{
			LatitudeDistributions* distributions = new LatitudeDistributions( );
			m_LatitudeDistributions.push_back( distributions );
			return *distributions;
		}

		const Colour GetColour( const float latitude, const float altitude, const float slope ) const
		{
			LatitudeDistributions& distributions = m_LatitudeDistributions[ ( int )( latitude * m_LatitudeDistributions.size( ) ) ];

			int altitudeSample = ( int )( altitude * SampleCount );
			int slopeSample = ( int )( slope * SampleCount );
			
			float weights[ MaxTypes ];
			float totalWeight = 0;
			int typeIndex = 0;
			for ( ; typeIndex < m_NumTypes; ++typeIndex )
			{
				const Distribution& distribution = distributions.m_TypeDistributions;
				float weight = distribution.m_Altitude[ altitudeSample ] * distribution.m_Altitude[ slopeSample ];
				weights[ typeIndex ] = weight;
				totalWeight += weight;
			}

			float r = 0;
			float g = 0;
			float b = 0;
			for ( typeIndex = 0; typeIndex < m_NumTypes; ++typeIndex )
			{
				r += ( weights[ typeIndex ] / totalWeight ) * ( float )
			}

			return colour;
		}

	}; //Fast
}; //Poc1
