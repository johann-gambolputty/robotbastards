#pragma once

#include "TerrainFunction.h"
#include "UTerrainVertex.h"

//	Erk...
using namespace System::Drawing::Imaging;
using namespace Rb::Rendering::Interfaces::Objects;

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			class UTerrainGenerator;

			///	\brief	Generates terrain on a sphere
			public ref class TerrainGenerator
			{
				public :

					typedef Rb::Core::Maths::Point3 Point3;
					typedef Rb::Core::Maths::Vector3 Vector3;

					///	\brief	Sets the terrain type generated by this object, and the seed used to initialize the terrain PRN generators
					TerrainGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction );

					///	\brief	Sets the terrain type generated by this object, and the seed used to initialize the terrain PRN generators
					TerrainGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction, TerrainFunction^ groundFunction );

					///	\brief	Finalizer. Frees up unmanaged resources
					!TerrainGenerator( );

					///	\brief	Destructor. Frees up unmanaged resources
					~TerrainGenerator( );

					///	\brief	Sets the magnitude of smallest x and z steps that can be passed to GenerateVertices
					void SetSmallestStepSize( const float x, const float z );

					///	\brief	Sets the minimum and maximum heights that can be generated, and the radius of the terrain function
					void Setup( const float patchScale, const float minHeight, const float maxHeight, const float terrainFunctionRadius );

					///	\brief	Generates a side of a cube map texture used to render this terrain in marble mode
					///
					///	Pixel format must be r8g8b8a8
					///	r = S (S component of spherical coordinate encoded normal
					///	g = T (T component of spherical coordinate encoded normal
					///	b = Altitude (normalized)
					///	a = Unused, for now
					///
					void GenerateTerrainPropertyCubeMapFace( const CubeMapFace face, const int width, const int height, const int stride, unsigned char* pixels );

					///	\brief	Generates terrain vertex points and normals
					void GenerateVertices( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, float uvRes, void* vertices );

					///	\brief	Generates terrain vertex points and normals. Calculates maximum patch error
					void GenerateVertices( Point3^ origin, Vector3^ xStep, Vector3^ zStep, const int width, const int height, float uvRes, void* vertices, [System::Runtime::InteropServices::Out]float% error );

				private :

					UTerrainGenerator* m_pImpl;
			};

		}; //Terrain
	}; //Fast
}; //Poc1