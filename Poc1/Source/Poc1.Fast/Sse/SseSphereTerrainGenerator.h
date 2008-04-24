#pragma once
#pragma managed(push, off)

#include "USphereTerrainTypeSelector.h"
#include "SseUtils.h"
#include "SseSphereTerrainDisplacers.h"

#include <vector>

namespace Poc1
{
	namespace Fast
	{
		//	--------------------------------------------------------------------------------- Types
		
		///	\brief	Base class for fast sphere terrain generators
		class SseSphereTerrainGenerator
		{
			public :

				///	\brief	Gets the displacer
				virtual SseSphereTerrainDisplacer& GetBaseDisplacer( ) = 0;

				///	\brief	Gets the displacer
				virtual const SseSphereTerrainDisplacer& GetBaseDisplacer( ) const = 0;

				///	\brief	Generates terrain vertex points and normals
				virtual void GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, void* vertices, const int stride, const int positionOffset, const int normalOffset ) = 0;

				///	\brief	Generates a cube map texture face
				virtual void GenerateTexture( const USphereTerrainTypeSelector& selector, const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels ) = 0;
		};

		///	\brief	Sphere terrain generator implementation
		template < typename DisplaceType = SseNoDisplacement >
		class SseSphereTerrainGeneratorT : public SseSphereTerrainGenerator
		{
			public :

				SseSphereTerrainGeneratorT( )
				{
					m_HeightCacheSize = 0;
					m_HeightCacheLines[ 0 ] = 0;
					m_HeightCacheLines[ 1 ] = 0;
					m_HeightCacheLines[ 2 ] = 0;
				}

				~SseSphereTerrainGeneratorT( )
				{
					AlignedArrayDelete( m_HeightCacheLines[ 0 ] );
					AlignedArrayDelete( m_HeightCacheLines[ 1 ] );
					AlignedArrayDelete( m_HeightCacheLines[ 2 ] );
				}

				///	\brief	Gets the object used to displace vertices from the sphere surface
				DisplaceType& GetDisplacer( );

				///	\brief	Gets the object used to displace vertices from the sphere surface
				const DisplaceType& GetDisplacer( ) const;

				///	\brief	Gets the displacer
				virtual SseSphereTerrainDisplacer& GetBaseDisplacer( );

				///	\brief	Gets the displacer
				virtual const SseSphereTerrainDisplacer& GetBaseDisplacer( ) const;

				///	\brief	Generates terrain vertex points and normals
				virtual void GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, void* vertices, const int stride, const int positionOffset, const int normalOffset );

				///	\brief	Generates a cube map texture face
				virtual void GenerateTexture( const USphereTerrainTypeSelector& selector, const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels );

			protected :

				DisplaceType		m_Displacer;				///<	Height displacer object
				float*				m_HeightCacheLines[ 3 ];	///<	Cache for 3 lines of height values in texture/vertex generation
				int					m_HeightCacheSize;

				void SetHeightCacheSize( const int size )
				{
					if ( m_HeightCacheSize == size )
					{
						return;
					}
					AlignedArrayDelete( m_HeightCacheLines[ 0 ] );
					AlignedArrayDelete( m_HeightCacheLines[ 1 ] );
					AlignedArrayDelete( m_HeightCacheLines[ 2 ] );
					m_HeightCacheLines[ 0 ] = new ( Aligned( 16 ) ) float[ size ];
					m_HeightCacheLines[ 1 ] = new ( Aligned( 16 ) ) float[ size ];
					m_HeightCacheLines[ 2 ] = new ( Aligned( 16 ) ) float[ size ];
					m_HeightCacheSize = size;
				}

				void FillHeightCacheLine( const int w4, float* line, const UCubeMapFace face, __m128 uuuu, const __m128& vvvv, const __m128& uuuuInc, float* latitudes );

				inline static void Store( const __m128& src, float* n0, float* n1, float* n2, float* n3, const int index, float* tmp )
				{
					_mm_store_ps( tmp, src );
					n0[ index ] = tmp[ 3 ];
					n1[ index ] = tmp[ 2 ];
					n2[ index ] = tmp[ 1 ];
					n3[ index ] = tmp[ 0 ];
				}
				
				inline static void Store( const __m128& src, float* n0, float* n1, float* n2, const int index, float* tmp, const int max )
				{
					_mm_store_ps( tmp, src );
					n0[ index ] = tmp[ 3 ];
					if ( max > 1 )
					{
						n1[ index ] = tmp[ 2 ];
						if ( max > 2 )
						{
							n2[ index ] = tmp[ 1 ];
						}
					}
				}
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
		inline SseSphereTerrainDisplacer& SseSphereTerrainGeneratorT< DisplaceType >::GetBaseDisplacer( )
		{
			return m_Displacer;
		}
		
		template < typename DisplaceType >
		inline const SseSphereTerrainDisplacer& SseSphereTerrainGeneratorT< DisplaceType >::GetBaseDisplacer( ) const
		{
			return m_Displacer;
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, void* vertices, const int stride, const int positionOffset, const int normalOffset )
		{
			//	Get start x, y and z positions for the first 4 vertices in the first row
			__m128 startXxxx = _mm_set_ps( origin[ 0 ], origin[ 0 ] + xStep[ 0 ], origin[ 0 ] + 2 * xStep[ 0 ], origin[ 0 ] + 3 * xStep[ 0 ] );
			__m128 startYyyy = _mm_set_ps( origin[ 1 ], origin[ 1 ] + xStep[ 1 ], origin[ 1 ] + 2 * xStep[ 1 ], origin[ 1 ] + 3 * xStep[ 1 ] );
			__m128 startZzzz = _mm_set_ps( origin[ 2 ], origin[ 2 ] + xStep[ 2 ], origin[ 2 ] + 2 * xStep[ 2 ], origin[ 2 ] + 3 * xStep[ 2 ] );

			//	Determine vectors for incrementing x, y and z positions in the column loop
			const __m128 colXInc = _mm_set1_ps( xStep[ 0 ] * 4 );
			const __m128 colYInc = _mm_set1_ps( xStep[ 1 ] * 4 );
			const __m128 colZInc = _mm_set1_ps( xStep[ 2 ] * 4 );
			
			//	Determine vectors for incrementing x, y and z positions in the row loop
			const __m128 rowXInc = _mm_set1_ps( zStep[ 0 ] );
			const __m128 rowYInc = _mm_set1_ps( zStep[ 1 ] );
			const __m128 rowZInc = _mm_set1_ps( zStep[ 2 ] );

			//	Point to the positions and normals in the first 4 vertices
			unsigned char* vertexBytes = ( unsigned char* )vertices;
			float* p0 = ( float* )( vertexBytes + positionOffset );
			float* p1 = ( float* )( vertexBytes + positionOffset + stride );
			float* p2 = ( float* )( vertexBytes + positionOffset + stride * 2 );
			float* p3 = ( float* )( vertexBytes + positionOffset + stride * 3 );
			float* n0 = ( float* )( vertexBytes + normalOffset );
			float* n1 = ( float* )( vertexBytes + normalOffset + stride );
			float* n2 = ( float* )( vertexBytes + normalOffset + stride * 2 );
			float* n3 = ( float* )( vertexBytes + normalOffset + stride * 3 );

			_CRT_ALIGN( 16 ) float nArr[ 4 ];	//	Temporary array used to store normals/displaced points

			const int widthDiv4 = width / 4;
			const int widthRem4 = width % 4;
			int vertex4Stride = stride; // Not x4, because this is used to increment p0 - sizeof(*p0) == sizeof(float) == 4
			int vertexRemStride = ( stride / 4 ) * widthRem4;
			for ( int row = 0; row < height; ++row )
			{
				__m128 xxxx = startXxxx;
				__m128 yyyy = startYyyy;
				__m128 zzzz = startZzzz;
				int col = 0;
				for ( ; col < widthDiv4; ++col )
				{
					//	Create a normal from the current point
					__m128 tmpXxxx = xxxx;
					__m128 tmpYyyy = yyyy;
					__m128 tmpZzzz = zzzz;
					Normalize( tmpXxxx, tmpYyyy, tmpZzzz );

					//	Transfer normals to 4 vertex normals
					Store( tmpXxxx, n0, n1, n2, n3, 0, nArr );
					Store( tmpYyyy, n0, n1, n2, n3, 1, nArr );
					Store( tmpZzzz, n0, n1, n2, n3, 2, nArr );

					//	Displace points on sphere, transfer positions to 4 vertex positions
					m_Displacer.Displace( tmpXxxx, tmpYyyy, tmpZzzz );
					Store( tmpXxxx, p0, p1, p2, p3, 0, nArr );
					Store( tmpYyyy, p0, p1, p2, p3, 1, nArr );
					Store( tmpZzzz, p0, p1, p2, p3, 2, nArr );

					//	Move to next positions on patch column
					xxxx = _mm_add_ps( xxxx, colXInc );
					yyyy = _mm_add_ps( yyyy, colYInc );
					zzzz = _mm_add_ps( zzzz, colZInc );

					//	Move vertex position and normal pointers on
					p0 += vertex4Stride; p1 += vertex4Stride; p2 += vertex4Stride; p3 += vertex4Stride;
					n0 += vertex4Stride; n1 += vertex4Stride; n2 += vertex4Stride; n3 += vertex4Stride;
				}

				//	Handle remainder
				if ( widthRem4 != 0 )
				{
					__m128 tmpXxxx = xxxx;
					__m128 tmpYyyy = yyyy;
					__m128 tmpZzzz = zzzz;
					Normalize( tmpXxxx, tmpYyyy, tmpZzzz );

					//	Transfer normals to 1-3 vertex normals
					Store( tmpXxxx, n0, n1, n2, 0, nArr, widthRem4 );
					Store( tmpYyyy, n0, n1, n2, 1, nArr, widthRem4 );
					Store( tmpZzzz, n0, n1, n2, 2, nArr, widthRem4 );

					//	Displace points on sphere, transfer positions to 1-3 vertex positions
					m_Displacer.Displace( tmpXxxx, tmpYyyy, tmpZzzz );
					Store( tmpXxxx, p0, p1, p2, 0, nArr, widthRem4 );
					Store( tmpYyyy, p0, p1, p2, 1, nArr, widthRem4 );
					Store( tmpZzzz, p0, p1, p2, 2, nArr, widthRem4 );

					p0 += vertexRemStride; p1 += vertexRemStride; p2 += vertexRemStride; p3 += vertexRemStride;
					n0 += vertexRemStride; n1 += vertexRemStride; n2 += vertexRemStride; n3 += vertexRemStride;
				}

				//	Move to next positions on patch row
				startXxxx = _mm_add_ps( startXxxx, rowXInc );
				startYyyy = _mm_add_ps( startYyyy, rowYInc );
				startZzzz = _mm_add_ps( startZzzz, rowZInc );
			}
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::FillHeightCacheLine( const int w4, float* line, const UCubeMapFace face, __m128 uuuu, const __m128& vvvv, const __m128& uuuuInc, float* latitudes )
		{
			bool storeLatitides = ( latitudes != 0 );
			for ( int index = 0; index < w4; ++index )
			{
				__m128 xxxx, yyyy, zzzz;
				CubeFacePosition( face, uuuu, vvvv, xxxx, yyyy, zzzz );
				Normalize( xxxx, yyyy, zzzz );
				if ( storeLatitides )
				{
					_mm_store_ps( latitudes, _mm_sub_ps( Constants::Fc_1, Abs( yyyy ) ) );
					latitudes += 4;
				}

				__m128 heights = m_Displacer.Displace( xxxx, yyyy, zzzz );
				_mm_store_ps( line, heights );
				line += 4;
				uuuu = _mm_add_ps( uuuu, uuuuInc );
			}
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateTexture( const USphereTerrainTypeSelector& selector, const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels )
		{
			float incU 			= 2.0f / float( width - 1 );
			float incV 			= 2.0f / float( height - 1 );
			__m128 vvvv			= _mm_set1_ps( -1 - incV );
			__m128 vvvvInc		= _mm_set1_ps( incV );
			__m128 uuuuStart	= _mm_set_ps( -1 - incU * 4, -1 - incU * 3, -1 - incU * 2, -1 - incU );
			__m128 uuuuInc		= _mm_set1_ps( incU * 4 );

			//	Determine the required size of the height cache
			//	This is the width + 8 (4 pixels boundary either side), multiplied by 3 (3 rows of samples are
			//	needed to calculate derivatives)
			const int heightCacheSize = ( width + 8 );
			const int heightCacheSizeDiv4 = heightCacheSize / 4;
			SetHeightCacheSize( heightCacheSize );
			float** cacheLines = m_HeightCacheLines;
			float* latitudes = new ( Aligned( 16 ) ) float[ heightCacheSize ];

			FillHeightCacheLine( heightCacheSizeDiv4, cacheLines[ 0 ], face, uuuuStart, vvvv, uuuuInc, 0 );
			vvvv = _mm_add_ps( vvvv, vvvvInc );

			FillHeightCacheLine( heightCacheSizeDiv4, cacheLines[ 1 ], face, uuuuStart, vvvv, uuuuInc, 0 );
			vvvv = _mm_add_ps( vvvv, vvvvInc );
			
			FillHeightCacheLine( heightCacheSizeDiv4, cacheLines[ 2 ], face, uuuuStart, vvvv, uuuuInc, &latitudes[ 0 ] );
			vvvv = _mm_add_ps( vvvv, vvvvInc );

			int w4 = width / 4;
			int prevCacheLine = 0;
			int currentCacheLine = 1;
			int nextCacheLine = 2;
			unsigned char* rowPixel = pixels;
			for ( int row = 0; row < height; ++row )
			{
				unsigned char* curPixel = rowPixel;
				float* curHeight = cacheLines[ currentCacheLine ] + 4;
				float* curLatitude = &( latitudes[ 0 ] );
				for ( int col = 0; col < w4; ++col, curHeight += 4, curLatitude += 4 )
				{
					//	TODO: AP: Can determine slope from the 3 cache lines. Use this to determine terrain type from selector
					//	NOTE: AP: The way latitude is cached means the next line of latitude is being used, rather than the current - should be OK though
					const UColour b0 = selector.GetColour( curLatitude[ 3 ], curHeight[ 3 ], 0, 1 );
					const UColour b1 = selector.GetColour( curLatitude[ 2 ], curHeight[ 2 ], 0, 1 );
					const UColour b2 = selector.GetColour( curLatitude[ 1 ], curHeight[ 1 ], 0, 1 );
					const UColour b3 = selector.GetColour( curLatitude[ 0 ], curHeight[ 0 ], 0, 1 );

					switch ( format )
					{
						case FormatR8G8B8A8 :
							curPixel[ 0 ]  = b0.R( ); curPixel[ 1 ]  = b0.G( ); curPixel[ 2 ]  = b0.B( );  curPixel[ 3 ]  = 0xff;
							curPixel[ 4 ]  = b1.R( ); curPixel[ 5 ]  = b1.G( ); curPixel[ 6 ]  = b1.B( );  curPixel[ 7 ]  = 0xff;
							curPixel[ 8 ]  = b2.R( ); curPixel[ 9 ]  = b2.G( ); curPixel[ 10 ] = b2.B( );  curPixel[ 11 ] = 0xff;
							curPixel[ 12 ] = b3.R( ); curPixel[ 13 ] = b3.G( ); curPixel[ 14 ] = b3.B( );  curPixel[ 15 ] = 0xff;
							curPixel += 16;
							break;
							
						case FormatR8G8B8 :
							curPixel[ 0 ] = b0.R( ); curPixel[ 1 ]  = b0.G( ); curPixel[ 2 ]  = b0.B( );
							curPixel[ 3 ] = b1.R( ); curPixel[ 4 ]  = b1.G( ); curPixel[ 5 ]  = b1.B( );
							curPixel[ 6 ] = b2.R( ); curPixel[ 7 ]  = b2.G( ); curPixel[ 8 ]  = b2.B( );
							curPixel[ 9 ] = b3.R( ); curPixel[ 10 ] = b3.G( ); curPixel[ 11 ] = b3.B( );
							curPixel += 12;
							break;
					};
				}

				//	Move to next row
				rowPixel += stride;
				vvvv = _mm_add_ps( vvvv, vvvvInc );
				prevCacheLine = currentCacheLine;
				currentCacheLine = nextCacheLine;
				nextCacheLine = ( nextCacheLine + 1 ) % 3;
				if ( row < ( height - 1 ) )
				{
					FillHeightCacheLine( heightCacheSizeDiv4, cacheLines[ nextCacheLine ], face, uuuuStart, vvvv, uuuuInc, &latitudes[ 0 ] );
				}
			}

			AlignedArrayDelete( latitudes );
		}
		
		//	---------------------------------------------------------------------------------------
	};
};

#pragma managed(pop)
