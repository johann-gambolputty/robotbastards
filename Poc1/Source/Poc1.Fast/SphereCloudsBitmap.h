#pragma once

//	Erk...
using namespace System::Drawing::Imaging;
using namespace Rb::Rendering::Interfaces::Objects;

namespace Poc1
{
	namespace Fast
	{
		class SseRidgedFractal;

		///	\brief	Generates cloud bitmaps for cube mapping onto a sphere
		public ref class SphereCloudsBitmap
		{
			public :

				SphereCloudsBitmap( );

				~SphereCloudsBitmap( );

				!SphereCloudsBitmap( );

				///	\brief	Generates a face of a cube map
				void GenerateFace( CubeMapFace face, PixelFormat format, const int width, const int height, const int stride, unsigned char* pixels );

			private :

				SseRidgedFractal* m_pImpl;
		};
	};
};