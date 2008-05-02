#pragma once
#pragma managed(push, off)

#include "USphereTerrainTypeSelector.h"
#include "SseUtils.h"
#include "SseSphereTerrainDisplacers.h"
#include "UVector3.h"

#include <math.h>
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

				///	\brief	Generates terrain vertex points and normals. Gets maximum patch error
				virtual void GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, void* vertices, const int stride, const int positionOffset, const int normalOffset, float& maxError ) = 0;

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
					m_FpCacheSize = 0;
					m_FpCacheLines[ 0 ] = 0;
					m_FpCacheLines[ 1 ] = 0;
					m_FpCacheLines[ 2 ] = 0;
				}

				~SseSphereTerrainGeneratorT( )
				{
					AlignedArrayDelete( m_FpCacheLines[ 0 ] );
					AlignedArrayDelete( m_FpCacheLines[ 1 ] );
					AlignedArrayDelete( m_FpCacheLines[ 2 ] );
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

				///	\brief	Generates terrain vertex points and normals
				virtual void GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, void* vertices, const int stride, const int positionOffset, const int normalOffset, float& maxError );

				///	\brief	Generates a cube map texture face
				virtual void GenerateTexture( const USphereTerrainTypeSelector& selector, const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels );

			private :

				DisplaceType		m_Displacer;			///<	Height displacer object
				float*				m_FpCacheLines[ 3 ];	///<	Cache for 3 lines of height/position values in texture/vertex generation
				int					m_FpCacheSize;			///<	Size of each fp cache line	

				///	\brief	Sets the height of the FP cache
				void SetFpCacheSize( const int size );

				///	\brief	Fills a line in the fp cache with height values
				void FillHeightCacheLine( const int w4, float* line, const UCubeMapFace face, __m128 uuuu, const __m128& vvvv, const __m128& uuuuInc, float* latitudes );

				///	\brief	Fills a line in the fp cache with positions
				void FillPositionCacheLine( const int w4, float* line, __m128 xxxx, __m128 yyyy, __m128 zzzz, const __m128& colXInc, const __m128& colYInc, const __m128& colZInc );
				
				///	\brief	Fills a line in the fp cache with positions and base height values
				void FillPositionHeightCacheLine( const int w4, float* line, __m128 xxxx, __m128 yyyy, __m128 zzzz, const __m128& colXInc, const __m128& colYInc, const __m128& colZInc );

				///	\brief	Determines the maximum error between two arrays filled with height data
				static float GetMaximumError( const int count, const float* heights0 );

				///	\brief	Calculates a normal from stuff
				inline static void CalculateNormal( float* n, const int prev, const int next, const float* centre, const float* xSrc, const float* ySrc, const float* zSrc, const float* uXSrc, const float* uYSrc, const float* uZSrc, const float* dXSrc, const float* dYSrc, const float* dZSrc )
				{
					const UVector3 left	( xSrc [ prev  ] - centre[ 0 ], ySrc [ prev ] - centre[ 1 ], zSrc [ prev ] - centre[ 2 ] );
					const UVector3 right( xSrc [ next  ] - centre[ 0 ], ySrc [ next ] - centre[ 1 ], zSrc [ next ] - centre[ 2 ] );
					const UVector3 up	( uXSrc[  0  ] - centre[ 0 ], uYSrc[  0 ] - centre[ 1 ], uZSrc[  0 ] - centre[ 2 ] );
					const UVector3 lUp	( uXSrc[ prev  ] - centre[ 0 ], uYSrc[ prev ] - centre[ 1 ], uZSrc[ prev ] - centre[ 2 ] );
					const UVector3 down	( dXSrc[  0  ] - centre[ 0 ], dYSrc[  0 ] - centre[ 1 ], dZSrc[  0 ] - centre[ 2 ] );
					const UVector3 rDown( dXSrc[ next  ] - centre[ 0 ], dYSrc[ next ] - centre[ 1 ], dZSrc[ next ] - centre[ 2 ] );

					UVector3 acc;
					acc.Add( UVector3::Cross( lUp, left ) );
					acc.Add( UVector3::Cross( up, lUp ) );
					acc.Add( UVector3::Cross( right, up ) );
					acc.Add( UVector3::Cross( rDown, right ) );
					acc.Add( UVector3::Cross( down, rDown ) );
					acc.Add( UVector3::Cross( left, down ) );
					acc.Normalise( );

					n[ 0 ] = acc.m_X;
					n[ 1 ] = acc.m_Y;
					n[ 2 ] = acc.m_Z;
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
		void SseSphereTerrainGeneratorT< DisplaceType >::SetFpCacheSize( const int size )
		{
			if ( m_FpCacheSize >= size )
			{
				return;
			}
			AlignedArrayDelete( m_FpCacheLines[ 0 ] );
			AlignedArrayDelete( m_FpCacheLines[ 1 ] );
			AlignedArrayDelete( m_FpCacheLines[ 2 ] );
			m_FpCacheLines[ 0 ] = new ( Aligned( 16 ) ) float[ size ];
			m_FpCacheLines[ 1 ] = new ( Aligned( 16 ) ) float[ size ];
			m_FpCacheLines[ 2 ] = new ( Aligned( 16 ) ) float[ size ];
			m_FpCacheSize = size;
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::FillPositionCacheLine( const int w4, float* line, __m128 xxxx, __m128 yyyy, __m128 zzzz, const __m128& colXInc, const __m128& colYInc, const __m128& colZInc )
		{
			__m128 tmpXxxx, tmpYyyy, tmpZzzz;
			float* curPos = line;
			for ( int index = 0; index < w4; ++index )
			{
				tmpXxxx = xxxx;
				tmpYyyy = yyyy;
				tmpZzzz = zzzz;

				Normalize( tmpXxxx, tmpYyyy, tmpZzzz );
				m_Displacer.Displace( tmpXxxx, tmpYyyy, tmpZzzz );

				_mm_store_ps( curPos, tmpXxxx ); curPos += 4;
				_mm_store_ps( curPos, tmpYyyy ); curPos += 4;
				_mm_store_ps( curPos, tmpZzzz ); curPos += 4;

				xxxx = _mm_add_ps( xxxx, colXInc );
				yyyy = _mm_add_ps( yyyy, colYInc );
				zzzz = _mm_add_ps( zzzz, colZInc );
			}
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::FillPositionHeightCacheLine( const int w4, float* line, __m128 xxxx, __m128 yyyy, __m128 zzzz, const __m128& colXInc, const __m128& colYInc, const __m128& colZInc )
		{
			__m128 tmpXxxx, tmpYyyy, tmpZzzz;
			float* curPos = line;
			for ( int index = 0; index < w4; ++index )
			{
				tmpXxxx = xxxx;
				tmpYyyy = yyyy;
				tmpZzzz = zzzz;

				Normalize( tmpXxxx, tmpYyyy, tmpZzzz );
				__m128 heights = m_Displacer.Displace( tmpXxxx, tmpYyyy, tmpZzzz );
				heights = m_Displacer.MapToHeightRange( heights );

				//	Store heights alongside positions, to avoid cache hit
				_mm_store_ps( curPos, tmpXxxx ); curPos += 4;
				_mm_store_ps( curPos, tmpYyyy ); curPos += 4;
				_mm_store_ps( curPos, tmpZzzz ); curPos += 4;
				_mm_store_ps( curPos, heights ); curPos += 4;

				xxxx = _mm_add_ps( xxxx, colXInc );
				yyyy = _mm_add_ps( yyyy, colYInc );
				zzzz = _mm_add_ps( zzzz, colZInc );
			}
		}

		template < typename DisplaceType >
		inline float SseSphereTerrainGeneratorT< DisplaceType >::GetMaximumError( const int count, const float* heights0 )
		{
			//	TODO: AP: This is incomplete - it only calculates errors for vertices between x step positions
			float maxError = 0;
			for ( int i = 0; i < count; ++i )
			{
				const float currHeight0 = heights0[ 0 ];
				const float currHeight1 = heights0[ 2 ];
				const float currHeight2 = heights0[ 16 ];

				const float xEstHeight0 = ( currHeight0 + currHeight1 ) / 2;
				const float xEstHeight1 = ( currHeight1 + currHeight2 ) / 2;

				const float xActHeight0 = heights0[ 1 ];
				const float xActHeight1 = heights0[ 3 ];

				const float xError0 = abs( xActHeight0 - xEstHeight0 );
				const float xError1 = abs( xActHeight1 - xEstHeight1 );
				const float xError = xError0 > xError1 ? xError0 : xError1;

				maxError = xError > maxError ? xError : maxError;

				heights0 += 16;
			}

			return maxError;
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, void* vertices, const int stride, const int positionOffset, const int normalOffset )
		{
			//	Get start x, y and z positions for the first 4 vertices in the first row
			//	NOTE: AP: Vectors are apparently reversed, so memory access is more natural (xyzw comes out as [ w, z, y, x ] normally)
			__m128 startXxxx = _mm_set_ps( origin[ 0 ] - xStep[ 0 ], origin[ 0 ] - xStep[ 0 ] * 2, origin[ 0 ] - xStep[ 0 ] * 3, origin[ 0 ] - xStep[ 0 ] * 4 );
			__m128 startYyyy = _mm_set_ps( origin[ 1 ] - xStep[ 1 ], origin[ 1 ] - xStep[ 1 ] * 2, origin[ 1 ] - xStep[ 1 ] * 3, origin[ 1 ] - xStep[ 1 ] * 4 );
			__m128 startZzzz = _mm_set_ps( origin[ 2 ] - xStep[ 2 ], origin[ 2 ] - xStep[ 2 ] * 2, origin[ 2 ] - xStep[ 2 ] * 3, origin[ 2 ] - xStep[ 2 ] * 4 );

			//	Determine vectors for incrementing x, y and z positions in the column loop
			const __m128 colXInc = _mm_set1_ps( xStep[ 0 ] * 4 );
			const __m128 colYInc = _mm_set1_ps( xStep[ 1 ] * 4 );
			const __m128 colZInc = _mm_set1_ps( xStep[ 2 ] * 4 );

			//	Determine vectors for incrementing x, y and z positions in the row loop
			const __m128 rowXInc = _mm_set1_ps( zStep[ 0 ] );
			const __m128 rowYInc = _mm_set1_ps( zStep[ 1 ] );
			const __m128 rowZInc = _mm_set1_ps( zStep[ 2 ] );

			startXxxx = _mm_sub_ps( startXxxx, rowXInc );
			startYyyy = _mm_sub_ps( startYyyy, rowYInc );
			startZzzz = _mm_sub_ps( startZzzz, rowZInc );

			//	Create a cache for storing vertex positions
			int fullWidth = width + 8;
			fullWidth = ( fullWidth % 4 == 0 ) ? fullWidth : fullWidth + ( 4 - ( fullWidth % 4 ) );
			const int cacheSize = fullWidth * 3;
			SetFpCacheSize( cacheSize );
			const int fullWidthDiv4 = fullWidth / 4;
			float** cacheLines = m_FpCacheLines;

			int prevCacheLine = 0;
			int curCacheLine = 1;
			int nextCacheLine = 2;

			//	Fill the cache with positions from the first 3 vertex rows
			FillPositionCacheLine( fullWidthDiv4, cacheLines[ 0 ], startXxxx, startYyyy, startZzzz, colXInc, colYInc, colZInc );
			startXxxx = _mm_add_ps( startXxxx, rowXInc );
			startYyyy = _mm_add_ps( startYyyy, rowYInc );
			startZzzz = _mm_add_ps( startZzzz, rowZInc );

			FillPositionCacheLine( fullWidthDiv4, cacheLines[ 1 ], startXxxx, startYyyy, startZzzz, colXInc, colYInc, colZInc );
			startXxxx = _mm_add_ps( startXxxx, rowXInc );
			startYyyy = _mm_add_ps( startYyyy, rowYInc );
			startZzzz = _mm_add_ps( startZzzz, rowZInc );

			FillPositionCacheLine( fullWidthDiv4, cacheLines[ 2 ], startXxxx, startYyyy, startZzzz, colXInc, colYInc, colZInc );

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
			const int widthDiv4 = width / 4;
			const int widthMod4 = width % 4;
			const int vertexStride = stride;
			const int vertexLastStride = ( stride / 4 ) * widthMod4;

			for ( int row = 0; row < height; ++row )
			{
				const float* uXSrc = cacheLines[ prevCacheLine ] + 12;
				const float* uYSrc = cacheLines[ prevCacheLine ] + 16;
				const float* uZSrc = cacheLines[ prevCacheLine ] + 20;
				
				const float* xSrc = cacheLines[ curCacheLine ] + 12;
				const float* ySrc = cacheLines[ curCacheLine ] + 16;
				const float* zSrc = cacheLines[ curCacheLine ] + 20;
				
				const float* dXSrc = cacheLines[ nextCacheLine ] + 12;
				const float* dYSrc = cacheLines[ nextCacheLine ] + 16;
				const float* dZSrc = cacheLines[ nextCacheLine ] + 20;

				for ( int col = 0; col < widthDiv4; ++col )
				{
					//	Store positions
					p0[ 0 ] = xSrc[ 0 ]; p1[ 0 ] = xSrc[ 1 ]; p2[ 0 ] = xSrc[ 2 ]; p3[ 0 ] = xSrc[ 3 ];
					p0[ 1 ] = ySrc[ 0 ]; p1[ 1 ] = ySrc[ 1 ]; p2[ 1 ] = ySrc[ 2 ]; p3[ 1 ] = ySrc[ 3 ];
					p0[ 2 ] = zSrc[ 0 ]; p1[ 2 ] = zSrc[ 1 ]; p2[ 2 ] = zSrc[ 2 ]; p3[ 2 ] = zSrc[ 3 ];

					//	Calculate vertex normals
					CalculateNormal( n0, -9, 1, p0, xSrc, ySrc, zSrc, uXSrc, uYSrc, uZSrc, dXSrc, dYSrc, dZSrc );
					CalculateNormal( n1, -1, 1, p1, xSrc + 1, ySrc + 1, zSrc + 1, uXSrc + 1, uYSrc + 1, uZSrc + 1, dXSrc + 1, dYSrc + 1, dZSrc + 1 );
					CalculateNormal( n2, -1, 1, p2, xSrc + 2, ySrc + 2, zSrc + 2, uXSrc + 2, uYSrc + 2, uZSrc + 2, dXSrc + 2, dYSrc + 2, dZSrc + 2 );
					CalculateNormal( n3, -1, 9, p3, xSrc + 3, ySrc + 3, zSrc + 3, uXSrc + 3, uYSrc + 3, uZSrc + 3, dXSrc + 3, dYSrc + 3, dZSrc + 3 );

					//	Move vertex pointers on
					p0 += vertexStride; p1 += vertexStride; p2 += vertexStride; p3 += vertexStride;
					n0 += vertexStride; n1 += vertexStride; n2 += vertexStride; n3 += vertexStride;

					//	Move cache pointers on
					xSrc += 12; ySrc += 12; zSrc += 12;
					uXSrc += 12; uYSrc += 12; uZSrc += 12;
					dXSrc += 12; dYSrc += 12; dZSrc += 12;
				}
				if ( widthMod4 > 0 )
				{
					p0[ 0 ] = xSrc[ 0 ]; p0[ 1 ] = ySrc[ 0 ]; p0[ 2 ] = zSrc[ 0 ];
					CalculateNormal( n0, -9, 1, p0, xSrc, ySrc, zSrc, uXSrc, uYSrc, uZSrc, dXSrc, dYSrc, dZSrc );
					if ( widthMod4 > 1 )
					{
						p1[ 0 ] = xSrc[ 1 ]; p1[ 1 ] = ySrc[ 1 ]; p1[ 2 ] = zSrc[ 1 ];
						CalculateNormal( n1, -1, 1, p1, xSrc + 1, ySrc + 1, zSrc + 1, uXSrc + 1, uYSrc + 1, uZSrc + 1, dXSrc + 1, dYSrc + 1, dZSrc + 1 );
						if ( widthMod4 > 2 )
						{
							p2[ 0 ] = xSrc[ 2 ]; p2[ 1 ] = ySrc[ 2 ]; p2[ 2 ] = zSrc[ 2 ];
							CalculateNormal( n2, -1, 1, p2, xSrc + 2, ySrc + 2, zSrc + 2, uXSrc + 2, uYSrc + 2, uZSrc + 2, dXSrc + 2, dYSrc + 2, dZSrc + 2 );
						}
					}
					p0 += vertexLastStride; p1 += vertexLastStride; p2 += vertexLastStride; p3 += vertexLastStride;
					n0 += vertexLastStride; n1 += vertexLastStride; n2 += vertexLastStride; n3 += vertexLastStride;
				}

				prevCacheLine = curCacheLine;
				curCacheLine = nextCacheLine;
				nextCacheLine = ( nextCacheLine + 1 ) % 3;
				startXxxx = _mm_add_ps( startXxxx, rowXInc );
				startYyyy = _mm_add_ps( startYyyy, rowYInc );
				startZzzz = _mm_add_ps( startZzzz, rowZInc );
				if ( row < ( height - 1 ) )
				{
					FillPositionCacheLine( fullWidthDiv4, cacheLines[ nextCacheLine ], startXxxx, startYyyy, startZzzz, colXInc, colYInc, colZInc );
				}
			}
		}

		template < typename DisplaceType >
		inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, void* vertices, const int stride, const int positionOffset, const int normalOffset, float& error )
		{
			//	Same as GenerateVertices() without error, except that the resolution is doubled
			//	TODO: AP: Makes assumptions about relationship between resolutions
			xStep[ 0 ] /= 2; xStep[ 1 ] /= 2; xStep[ 2 ] /= 2;
			zStep[ 0 ] /= 2; zStep[ 1 ] /= 2; zStep[ 2 ] /= 2;
			width = ( width * 2 ) - 1;
			height = ( height * 2 ) - 1;

			//	Get start x, y and z positions for the first 4 vertices in the first row
			//	NOTE: AP: Vectors are apparently reversed, so memory access is more natural (xyzw comes out as [ w, z, y, x ] normally)
			__m128 startXxxx = _mm_set_ps( origin[ 0 ] - xStep[ 0 ], origin[ 0 ] - xStep[ 0 ] * 2, origin[ 0 ] - xStep[ 0 ] * 3, origin[ 0 ] - xStep[ 0 ] * 4 );
			__m128 startYyyy = _mm_set_ps( origin[ 1 ] - xStep[ 1 ], origin[ 1 ] - xStep[ 1 ] * 2, origin[ 1 ] - xStep[ 1 ] * 3, origin[ 1 ] - xStep[ 1 ] * 4 );
			__m128 startZzzz = _mm_set_ps( origin[ 2 ] - xStep[ 2 ], origin[ 2 ] - xStep[ 2 ] * 2, origin[ 2 ] - xStep[ 2 ] * 3, origin[ 2 ] - xStep[ 2 ] * 4 );

			//	Determine vectors for incrementing x, y and z positions in the column loop
			const __m128 colXInc = _mm_set1_ps( xStep[ 0 ] * 4 );
			const __m128 colYInc = _mm_set1_ps( xStep[ 1 ] * 4 );
			const __m128 colZInc = _mm_set1_ps( xStep[ 2 ] * 4 );

			//	Determine vectors for incrementing x, y and z positions in the row loop
			const __m128 rowXInc = _mm_set1_ps( zStep[ 0 ] );
			const __m128 rowYInc = _mm_set1_ps( zStep[ 1 ] );
			const __m128 rowZInc = _mm_set1_ps( zStep[ 2 ] );

			startXxxx = _mm_sub_ps( startXxxx, rowXInc );
			startYyyy = _mm_sub_ps( startYyyy, rowYInc );
			startZzzz = _mm_sub_ps( startZzzz, rowZInc );

			//	Create a cache for storing vertex positions
			int fullWidth = width + 8;
			fullWidth = ( fullWidth % 4 == 0 ) ? fullWidth : fullWidth + ( 4 - ( fullWidth % 4 ) );
			const int cacheSize = fullWidth * 4;	//	NOTE: AP: x4, not x3, because we need to store heights also
			SetFpCacheSize( cacheSize );
			const int fullWidthDiv4 = fullWidth / 4;
			float** cacheLines = m_FpCacheLines;

			int prevCacheLine = 0;
			int curCacheLine = 1;
			int nextCacheLine = 2;

			//	Fill the cache with positions and heights from the first 3 vertex rows
			FillPositionHeightCacheLine( fullWidthDiv4, cacheLines[ 0 ], startXxxx, startYyyy, startZzzz, colXInc, colYInc, colZInc );
			startXxxx = _mm_add_ps( startXxxx, rowXInc );
			startYyyy = _mm_add_ps( startYyyy, rowYInc );
			startZzzz = _mm_add_ps( startZzzz, rowZInc );

			FillPositionHeightCacheLine( fullWidthDiv4, cacheLines[ 1 ], startXxxx, startYyyy, startZzzz, colXInc, colYInc, colZInc );
			startXxxx = _mm_add_ps( startXxxx, rowXInc );
			startYyyy = _mm_add_ps( startYyyy, rowYInc );
			startZzzz = _mm_add_ps( startZzzz, rowZInc );

			FillPositionHeightCacheLine( fullWidthDiv4, cacheLines[ 2 ], startXxxx, startYyyy, startZzzz, colXInc, colYInc, colZInc );

			//	Point to the positions and normals in the first 4 vertices
			unsigned char* vertexBytes = ( unsigned char* )vertices;
			float* p0 = ( float* )( vertexBytes + positionOffset );
			float* p1 = ( float* )( vertexBytes + positionOffset + stride );
			float* n0 = ( float* )( vertexBytes + normalOffset );
			float* n1 = ( float* )( vertexBytes + normalOffset + stride );
			const int widthDiv4 = width / 4;
			const int widthMod4 = width % 4;
			const int vertexStride = stride / 2;
			const int vertexLastStride = ( stride / 4 ) * widthMod4;

			error = 0;

			for ( int row = 0; row < height; ++row )
			{
				float rowError = GetMaximumError( widthDiv4, cacheLines[ curCacheLine ] + 28 );
				error = rowError > error ? rowError : error;

				if ( ( row % 2 ) == 0 )
				{
					const float* uXSrc = cacheLines[ prevCacheLine ] + 16;
					const float* uYSrc = cacheLines[ prevCacheLine ] + 20;
					const float* uZSrc = cacheLines[ prevCacheLine ] + 24;
					
					const float* xSrc = cacheLines[ curCacheLine ] + 16;
					const float* ySrc = cacheLines[ curCacheLine ] + 20;
					const float* zSrc = cacheLines[ curCacheLine ] + 24;
					
					const float* dXSrc = cacheLines[ nextCacheLine ] + 16;
					const float* dYSrc = cacheLines[ nextCacheLine ] + 20;
					const float* dZSrc = cacheLines[ nextCacheLine ] + 24;

					//	Only get positions and normals from every second row (because its working at double resolution)
					for ( int col = 0; col < widthDiv4; ++col )
					{
						//	Store positions
						p0[ 0 ] = xSrc[ 0 ]; p1[ 0 ] = xSrc[ 2 ];
						p0[ 1 ] = ySrc[ 0 ]; p1[ 1 ] = ySrc[ 2 ];
						p0[ 2 ] = zSrc[ 0 ]; p1[ 2 ] = zSrc[ 2 ];

						//	Calculate vertex normals
						CalculateNormal( n0, -13, 2, p0, xSrc, ySrc, zSrc, uXSrc, uYSrc, uZSrc, dXSrc, dYSrc, dZSrc );
						CalculateNormal( n1, -2, 14, p1, xSrc + 2, ySrc + 2, zSrc + 2, uXSrc + 2, uYSrc + 2, uZSrc + 2, dXSrc + 2, dYSrc + 2, dZSrc + 2 );

						//	Move vertex pointers on
						p0 += vertexStride; p1 += vertexStride;
						n0 += vertexStride; n1 += vertexStride;

						//	Move cache pointers on
						xSrc += 16; ySrc += 16; zSrc += 16;
						uXSrc += 16; uYSrc += 16; uZSrc += 16;
						dXSrc += 16; dYSrc += 16; dZSrc += 16;
					}
					if ( widthMod4 > 0 )
					{
						p0[ 0 ] = xSrc[ 0 ]; p0[ 1 ] = ySrc[ 0 ]; p0[ 2 ] = zSrc[ 0 ];
						CalculateNormal( n0, -13, 2, p0, xSrc, ySrc, zSrc, uXSrc, uYSrc, uZSrc, dXSrc, dYSrc, dZSrc );
						if ( widthMod4 > 1 )
						{
							p1[ 0 ] = xSrc[ 1 ]; p1[ 1 ] = ySrc[ 1 ]; p1[ 2 ] = zSrc[ 1 ];
							CalculateNormal( n1, -2, 14, p1, xSrc + 2, ySrc + 2, zSrc + 2, uXSrc + 2, uYSrc + 2, uZSrc + 2, dXSrc + 2, dYSrc + 2, dZSrc + 2 );
						}
						p0 += vertexLastStride; p1 += vertexLastStride;
						n0 += vertexLastStride; n1 += vertexLastStride;
					}
				}

				prevCacheLine = curCacheLine;
				curCacheLine = nextCacheLine;
				nextCacheLine = ( nextCacheLine + 1 ) % 3;
				startXxxx = _mm_add_ps( startXxxx, rowXInc );
				startYyyy = _mm_add_ps( startYyyy, rowYInc );
				startZzzz = _mm_add_ps( startZzzz, rowZInc );
				if ( row < ( height - 1 ) )
				{
					FillPositionHeightCacheLine( fullWidthDiv4, cacheLines[ nextCacheLine ], startXxxx, startYyyy, startZzzz, colXInc, colYInc, colZInc );
				}
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
			SetFpCacheSize( heightCacheSize );
			float** cacheLines = m_FpCacheLines;
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
