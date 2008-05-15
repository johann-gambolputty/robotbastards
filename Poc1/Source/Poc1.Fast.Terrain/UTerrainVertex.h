#pragma once
#pragma managed(push, off)

namespace Poc1
{
	namespace Fast
	{
		class UTerrainVertex
		{
			public :

				float X( ) const { return m_X; }
				float Y( ) const { return m_Y; }
				float Z( ) const { return m_Z; }

				///	\brief	Sets the position of the terrain vertex
				void SetPosition( float x, float y, float z );

				///	\brief	Sets the normal of the terrain vertex
				void SetNormal( float x, float y, float z );

				///	\brief	Sets the terrain UV of the vertex
				void SetTerrainUv( float u, float v );

				///	\brief	Sets the terrain parameters of the vertex
				void SetTerrainParameters( float slope, float elevation );

			private :

				float m_X;
				float m_Y;
				float m_Z;
				
				float m_Nx;
				float m_Ny;
				float m_Nz;

				float m_U;
				float m_V;

				float m_Slope;
				float m_Elevation;

		}; //UTerrainVertex

		inline void UTerrainVertex::SetPosition( float x, float y, float z )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
		}


		inline void UTerrainVertex::SetNormal( float x, float y, float z )
		{
			m_Nx = x;
			m_Ny = y;
			m_Nz = z;
		}

		inline void UTerrainVertex::SetTerrainUv( float u, float v )
		{
			m_U = u;
			m_V = v;
		}

		inline void UTerrainVertex::SetTerrainParameters( float slope, float elevation )
		{
			m_Slope = slope;
			m_Elevation = elevation;
		}
	};
};

#pragma managed(pop)
