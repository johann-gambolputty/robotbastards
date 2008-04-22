#pragma once
#pragma managed(push, off)

#include "SseUtils.h"
#include "SseSphereTerrainDisplacers.h"

namespace Poc1
{
	namespace Fast
	{
		//	--------------------------------------------------------------------------------- Types
		
		///	\brief	Base class for fast sphere terrain generators
		class SseSphereTerrainGenerator
		{
			public :

				///	\brief	Generates terrain vertex points and normals
				virtual void GenerateVertices( const float* origin, const float* xAxis, const float* zAxis, void* vertices, const int stride, const int positionOffset, const int normalOffset ) const = 0;

				///	\brief	Generates a cube map texture face
				virtual void GenerateTexture( const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels ) const = 0;
		};

		///	\brief	Sphere terrain generator implementation
		template < typename DisplaceType = SseNoDisplacement >
		class SseSphereTerrainGeneratorT : public SseSphereTerrainGenerator
		{
			public :

				///	\brief	Gets the object used to displace vertices from the sphere surface
				DisplaceType& GetDisplacer( );

				///	\brief	Gets the object used to displace vertices from the sphere surface
				const DisplaceType& GetDisplacer( ) const;

				///	\brief	Generates terrain vertex points and normals
				void GenerateVertices( const float* origin, const float* xAxis, const float* zAxis, void* vertices, const int stride, const int positionOffset, const int normalOffset ) const;

				///	\brief	Generates a cube map texture face
				void GenerateTexture( const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels ) const;

			protected :

				DisplaceType m_DisplaceType;
		};
		
		//	---------------------------------------------------------------------------------------

		//	--------------------------------------------- SseSphereTerrainGeneratorT Inline Methods

		template < typename DisplaceType >
		inline DisplaceType& SseSphereTerrainGeneratorT< DisplaceType >::GetDisplacer( )
		{
			return m_DisplaceType;
		}
		
		template < typename DisplaceType >
		inline const DisplaceType& SseSphereTerrainGeneratorT< DisplaceType >::GetDisplacer( ) const
		{
			return m_DisplaceType;
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateVertices( const float* origin, const float* xAxis, const float* zAxis, void* vertices, const int stride, const int positionOffset, const int normalOffset ) const
		{
			//	TODO: AP: ...
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateTexture( const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels ) const
		{
			float incU 			= 2.0f / float( width - 1 );
			float incV 			= 2.0f / float( height - 1 );
			__m128 vvvv			= _mm_set1_ps( -1 );
			__m128 vvvvInc		= _mm_set1_ps( incV );
			__m128 uuuuStart	= _mm_add_ps( _mm_set1_ps( -1 ), _mm_set_ps( 0, incU, incU * 2, incU * 3 ) );
			__m128 uuuuInc		= _mm_set1_ps( incU * 4 );

			_CRT_ALIGN( 16 ) float heightsArr[ 4 ] = { 0, 0, 0, 0 };

			int w4 = width / 4;
			unsigned char* rowPixel = pixels;
			for ( int row = 0; row < height; ++row )
			{
				unsigned char* curPixel = rowPixel;
				__m128 uuuu = uuuuStart;
				for ( int col = 0; col < w4; ++col )
				{
					__m128 xxxx, yyyy, zzzz;
					CubeFacePosition( face, uuuu, vvvv, xxxx, yyyy, zzzz );
					Normalize( xxxx, yyyy, zzzz );
					__m128 heights = m_DisplaceType.Displace( xxxx, yyyy, zzzz );

					//	TODO: AP: Need to inject colour from height/angle solution here... 
					_mm_store_ps( heightsArr, _mm_mul_ps( heights, _mm_set1_ps( 255 ) ) );

					const unsigned char b0 = ( unsigned char )( heightsArr[ 3 ] );
					const unsigned char b1 = ( unsigned char )( heightsArr[ 2 ] );
					const unsigned char b2 = ( unsigned char )( heightsArr[ 1 ] );
					const unsigned char b3 = ( unsigned char )( heightsArr[ 0 ] );

					switch ( format )
					{
						case FormatR8G8B8A8 :
							curPixel[ 0 ] = curPixel[ 1 ] = curPixel[ 2 ] = b0; curPixel[ 3 ] = 0xff;
							curPixel[ 4 ] = curPixel[ 5 ] = curPixel[ 6 ] = b1; curPixel[ 7 ] = 0xff;
							curPixel[ 8 ] = curPixel[ 9 ] = curPixel[ 10 ] = b2; curPixel[ 11 ] = 0xff;
							curPixel[ 12 ] = curPixel[ 13 ] = curPixel[ 14 ] = b3; curPixel[ 15 ] = 0xff;
							curPixel += 16;
							break;
							
						case FormatR8G8B8 :
							curPixel[ 0 ] = curPixel[ 1 ] = curPixel[ 2 ] = b0;
							curPixel[ 3 ] = curPixel[ 4 ] = curPixel[ 5 ] = b1;
							curPixel[ 6 ] = curPixel[ 7 ] = curPixel[ 8 ] = b2;
							curPixel[ 9 ] = curPixel[ 10 ] = curPixel[ 11 ] = b3;
							curPixel += 12;
							break;
					};
				}
				vvvv = _mm_add_ps( vvvv, vvvvInc );
				rowPixel += stride;
			}
		}
		
		//	---------------------------------------------------------------------------------------
	};
};

#pragma managed(pop)
