#pragma once

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			public ref class TerrainFunction abstract
			{
				public :

					property System::String^ Name
					{
						virtual System::String^ get( ) = 0;
					}

					virtual System::Object^ CreateParameters( ) = 0;
			};

			public ref class SimpleFractalTerrainFunction : TerrainFunction
			{
				public :

					property System::String^ Name
					{
						virtual System::String^ get( ) override;
					}

					virtual System::Object^ CreateParameters( ) override;
			};

		}; //Terrain
	}; //Fast
}; //Poc1