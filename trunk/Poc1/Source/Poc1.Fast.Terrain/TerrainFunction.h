#pragma once
#pragma managed( push, on )

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			#pragma managed( off )

			class UTerrainGenerator;
			class SseTerrainDisplacer;

			#pragma managed( on )

			public enum class TerrainGeometry
			{
				Sphere,
				Plane
			};

			public enum class TerrainFunctionType
			{
				Flat,
				SimpleFractal,
				RidgedFractal
			};

			///	\brief	Base class for terrain function parameter classes
			public ref class TerrainFunctionParameters
			{
				public :

					TerrainFunctionParameters( )
					{
						m_Scale = 8.0f;
						m_OutputScale = 1.0f;
					}

					///	\brief	Function scale
					property float FunctionScale
					{
						float get( ) { return m_Scale; }
						void set( float scale ) { m_Scale = scale; }
					}

					///	\brief	Result scaling factor
					property float OutputScale
					{
						float get( ) { return m_OutputScale; }
						void set( float scale ) { m_OutputScale = scale; }
					}


					///	\brief	Sets up a terrain displacer
					void Setup( SseTerrainDisplacer& displacer );

				private :

					float m_Scale;
					float m_OutputScale;
			};

			///	\brief	Terrain function object
			public ref class TerrainFunction
			{
				public :

					TerrainFunction( TerrainFunctionType functionType );

					///	\brief	Gets the function type
					property TerrainFunctionType FunctionType
					{
						TerrainFunctionType get( );
					}

					///	\brief	Gets the function parameters
					property TerrainFunctionParameters^ Parameters
					{
						TerrainFunctionParameters^ get( );
					}

					///	\brief	Gets the name of a specified terrain function type
					static System::String^ Name( TerrainFunctionType functionType );

					///	\brief	Creates a parameters object for a specified terrain function type
					static TerrainFunctionParameters^ CreateParameters( TerrainFunctionType functionType );

					///	\brief	Creates a height-only terrain generator
					static UTerrainGenerator* CreateGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction );

					///	\brief	Creates a height and ground displacement terrain generator
					static UTerrainGenerator* CreateGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction, TerrainFunction^ groundFunction );

				private :

					TerrainFunctionType m_FunctionType;
					TerrainFunctionParameters^ m_Parameters;
			};

			inline TerrainFunctionType TerrainFunction::FunctionType::get( )
			{
				return m_FunctionType;
			}

			inline  TerrainFunctionParameters^ TerrainFunction::Parameters::get( )
			{
				return m_Parameters;
			}

		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed(pop)
