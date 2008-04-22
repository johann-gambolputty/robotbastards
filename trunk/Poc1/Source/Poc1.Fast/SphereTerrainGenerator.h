#pragma once


//	Erk...
using namespace System::Drawing::Imaging;
using namespace Rb::Rendering::Interfaces::Objects;

namespace Poc1
{
	namespace Fast
	{
		class SseSphereTerrainGenerator;

		///	\brief	Available terrain generation types
		public enum class TerrainGeneratorType
		{
			Flat,	///<	Totally flat terrain
			Simple,	///<	Simple fractal terrain
			Ridged,	///<	Ridged fractal terrain
			Voronoi	///<	Voronoi fractal terrain (TODO: ... http://www.righto.com/fractals/vor.html)
		};

		///	\brief	Generates terrain on a sphere
		public ref class SphereTerrainGenerator
		{
			public :

				typedef Rb::Core::Maths::Point3 Point3;
				typedef Rb::Core::Maths::Vector3 Vector3;

				///	\brief	Sets the terrain type generated by this object, and the seed used to initialize the terrain PRN generators
				SphereTerrainGenerator( const TerrainGeneratorType generatorType, const unsigned int seed );

				///	\brief	Finalizer. Frees up unmanaged resources
				!SphereTerrainGenerator( );

				///	\brief	Destructor. Frees up unmanaged resources
				~SphereTerrainGenerator( );

				///	\brief	Generates a side of a cube map texture used to render this terrain in marble mode
				void GenerateTexture( const CubeMapFace face, const PixelFormat format, const int width, const int height, const int stride, unsigned char* pixels );

				///	\brief	Generates terrain vertex points and normals
				void GenerateVertices( Point3^ origin, Vector3^ xAxis, Vector3^ zAxis, void* vertices, const int stride, const int positionOffset, const int normalOffset );

			private :

				SseSphereTerrainGenerator* m_pImpl;
		};

	};
};