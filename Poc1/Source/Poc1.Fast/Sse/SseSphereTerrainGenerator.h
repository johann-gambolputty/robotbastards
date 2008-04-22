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

				DisplaceType m_Displacer;
		};
		
		//	---------------------------------------------------------------------------------------

		//	--------------------------------------------- SseSphereTerrainGeneratorT Inline Methods

		template < typename DisplaceType >
		inline DisplaceType& SseSphereTerrainGeneratorT< DisplaceType >::GetDisplacer( )
		{
			return m_Displacer;
		}
		
		template < typename DisplaceType >
		inline const DisplaceType& SseSphereTerrainGeneratorT< DisplaceType >::GetDisplacer( ) const
		{
			return m_Displacer;
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, void* vertices, const int stride, const int positionOffset, const int normalOffset ) const
		{
			__m128 startXxxx = _mm_set_ps( origin[ 0 ], origin[ 0 ] + xStep[ 0 ], origin[ 0 ] + 2 * xStep[ 0 ], origin[ 0 ] + 3 * xStep[ 0 ] );
			__m128 startYyyy = _mm_set_ps( origin[ 1 ], origin[ 1 ] + xStep[ 1 ], origin[ 1 ] + 2 * xStep[ 1 ], origin[ 1 ] + 3 * xStep[ 1 ] );
			__m128 startZzzz = _mm_set_ps( origin[ 2 ], origin[ 2 ] + xStep[ 2 ], origin[ 2 ] + 2 * xStep[ 2 ], origin[ 2 ] + 3 * xStep[ 2 ] );
			const __m128 colXInc = _mm_set1_ps( xAxis[ 0 ] );
			const __m128 colYInc = _mm_set1_ps( xAxis[ 1 ] );
			const __m128 colZInc = _mm_set1_ps( xAxis[ 2 ] );
			const __m128 rowXInc = _mm_set1_ps( zAxis[ 0 ] );
			const __m128 rowYInc = _mm_set1_ps( zAxis[ 1 ] );
			const __m128 rowZInc = _mm_set1_ps( zAxis[ 2 ] );

			unsigned char* vertexBytes = ( unsigned char* )vertices;
			float* p0 = ( float* )( vertexBytes + positionOffset );
			float* p1 = ( float* )( vertexBytes + positionOffset + stride );
			float* p2 = ( float* )( vertexBytes + positionOffset + stride * 2 );
			float* p3 = ( float* )( vertexBytes + positionOffset + stride * 3 );
			float* n0 = ( float* )( vertexBytes + normalOffset );
			float* n1 = ( float* )( vertexBytes + normalOffset + stride );
			float* n2 = ( float* )( vertexBytes + normalOffset + stride * 2 );
			float* n3 = ( float* )( vertexBytes + normalOffset + stride * 3 );


			const int w4 = width / 4;
			for ( int row = 0; row < height; ++row )
			{
				__m128 xxxx = startXxxx;
				__m128 yyyy = startXxxx;
				__m128 zzzz = startZzzz;
				for ( int col = 0; col < w4; ++col )
				{
					Normalize( xxxx, yyyy, zzzz );

					_CRT_ALIGN( 16 ) float nArr[ 4 ];
					_mm_store_ps( xxxx, nArr );

					n0[ 0 ] = nArr[ 0 ]; n1[ 0 ] = nArr[ 1 ]; n2[ 0 ] = nArr[ 2 ]; n3[ 0 ] = nArr[ 2 ];
					
					_mm_store_ps( yyyy, nArr );
					n0[ 1 ] = nArr[ 0 ]; n1[ 1 ] = nArr[ 1 ]; n2[ 1 ] = nArr[ 2 ]; n3[ 1 ] = nArr[ 2 ];
					
					_mm_store_ps( yyyyZ, nArr );
					n0[ 2 ] = nArr[ 0 ]; n1[ 2 ] = nArr[ 1 ]; n2[ 2 ] = nArr[ 2 ]; n3[ 2 ] = nArr[ 2 ];

					const __m128 heights = m_Displacer.Displace( xxxx, yyyy, zzzz );


					xxxx = _mm_add_ps( colXInc );
					yyyy = _mm_add_ps( colYInc );
					zzzz = _mm_add_ps( colZInc );
					p0 += stride; p1 += stride;
					curNormal += stride;
				}
				startXxxx = _mm_add_ps( startXxxx, rowXInc );
				startYyyy = _mm_add_ps( startYyyy, rowYInc );
				startZzzz = _mm_add_ps( startZzzz, rowZInc );
			}
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateTexture( const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels ) const
		{
			float incU 			= 2.0f / float( width - 1 );
			float incV 			= 2.0f / float( height - 1 );
			__m128 vvvv			= _mm_set1_ps( -1 );
			__m128 vvvvInc		= _mm_set1_ps( incV );
			__m128 uuuuStart	= _mm_set_ps( -1, -1 + incU, -1 + incU * 2, -1 + incU * 3 );
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
					__m128 heights = m_Displacer.Displace( xxxx, yyyy, zzzz );

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

					uuuu = _mm_add_ps( uuuu, uuuuInc );
				}
				vvvv = _mm_add_ps( vvvv, vvvvInc );
				rowPixel += stride;
			}
		}
		
		//	---------------------------------------------------------------------------------------
	};
};

#pragma managed(pop)
