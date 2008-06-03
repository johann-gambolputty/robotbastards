#pragma once
#pragma managed(push, on)

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			class TerrainGenerator;

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

			public ref class TerrainFunction
			{
				public :

					TerrainFunction( TerrainFunctionType functionType );

					property TerrainFunctionType FunctionType
					{
						TerrainFunctionType get( );
					}

					property System::Object^ Parameters
					{
						System::Object^ get( );
					}

					///	\brief	Gets the name of a specified terrain function type
					static System::String^ Name( TerrainFunctionType functionType );

					///	\brief	Creates a parameters object for a specified terrain function type
					static System::Object^ CreateParameters( TerrainFunctionType functionType );

					///	\brief	Creates a height-only terrain generator
					static TerrainGenerator* CreateGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction );

					///	\brief	Creates a height and ground displacement terrain generator
					static TerrainGenerator* CreateGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction, TerrainFunction^ groundFunction );

				private :

					TerrainFunctionType m_FunctionType;
					System::Object^ m_Parameters;
			};

			inline TerrainFunctionType TerrainFunction::FunctionType::get( )
			{
				return m_FunctionType;
			}

			inline System::Object^ TerrainFunction::Parameters::get( )
			{
				return m_Parameters;
			}

		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed(pop)
