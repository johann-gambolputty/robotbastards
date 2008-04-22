#pragma once
#pragma managed(push, off)

#include "UEnums.h"
#include <xmmintrin.h>

namespace Poc1
{
	namespace Fast
	{
		///	\brief	Sphere terrain generator implementation - flat
		class SseSphereTerrainGenerator
		{
			public :

				///	\brief	Generates terrain vertex points and normals
				void GenerateVertices( const float* origin, const float* xAxis, const float* zAxis, void* vertices, const int stride, const int positionOffset, const int normalOffset ) const;

				///	\brief	Generates a cube map texture face
				void GenerateTexture( const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels ) const;

			protected :

				///	\brief	Displaces a point on the sphere
				virtual void Displace( __m128& xxxx, __m128& yyyy, __m128& zzzz ) const
				{
				}
		};
	};
};

#pragma managed(pop)
