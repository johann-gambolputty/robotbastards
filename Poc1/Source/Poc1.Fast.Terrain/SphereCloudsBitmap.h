#pragma once

//	Erk...
using namespace System::Drawing::Imaging;
using namespace Rb::Rendering::Interfaces::Objects;

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			class SphereCloudsBitmapImpl;

			///	\brief	Generates cloud bitmaps for cube mapping onto a sphere
			public ref class SphereCloudsBitmap
			{
				public :

					SphereCloudsBitmap( );

					~SphereCloudsBitmap( );

					!SphereCloudsBitmap( );

					///	\brief	Sets generation parameters
					void Setup( float xOffset, float zOffset, float cloudCutoff, float cloudBorder );

					///	\brief	Generates a face of a cube map
					void GenerateFace( CubeMapFace face, PixelFormat format, const int width, const int height, const int stride, unsigned char* pixels );

				private :

					SphereCloudsBitmapImpl* m_pImpl;

			}; //SphereCloudsBitmap

		}; //Terrain

	}; //Fast

}; //Poc1