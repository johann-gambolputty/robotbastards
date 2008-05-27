#pragma once
#pragma managed(push, off)

#include "UTerrainVertex.h"
#include "SseSphereTerrainDisplacers.h"

#include <Sse\SseUtils.h>
#include <UVector3.h>
#include <UColour.h>

#include <math.h>
#include <vector>

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			//	----------------------------------------------------------------------------- Types
			
			///	\brief	Base class for fast sphere terrain generators
			class SseSphereTerrainGenerator
			{
				public :

					///	\brief	Sets the smallest possible step size (finest LOD)
					void SetSmallestStepSize( const float x, const float z );

					///	\brief	Gets the displacer
					virtual SseSphereTerrainDisplacer& GetBaseDisplacer( ) = 0;

					///	\brief	Gets the displacer
					virtual const SseSphereTerrainDisplacer& GetBaseDisplacer( ) const = 0;

					///	\brief	Generates terrain vertex points and normals
					virtual void GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, float uvRes, UTerrainVertex* vertices ) = 0;

					///	\brief	Generates terrain vertex points and normals. Gets maximum patch error
					virtual void GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, float uvRes, UTerrainVertex* vertices, float& maxError ) = 0;

					///	\brief	Determines the error on a patch
					virtual float GetMaximumPatchError( const float* origin, const float* xStep, const float* zStep, const int width, const int height, const int subdivisions ) const = 0;

					///	\brief	Generates a cube map texture face that encodes terrain properties
					///
					///	Pixel format must be r8g8b8a8
					///	r = S (S component of spherical coordinate encoded normal
					///	g = T (T component of spherical coordinate encoded normal
					///	b = Altitude (normalized)
					///	a = Unused, for now
					///
					virtual void GenerateTerrainPropertyCubeMapFace( const UCubeMapFace face, const int width, const int height, const int stride, unsigned char* pixels ) = 0;

				protected :

					float m_SmallestX;
					float m_SmallestZ;
			};


			///	\brief	Sphere terrain generator implementation
			template < typename DisplaceType = SseNoDisplacement >
			class _CRT_ALIGN( 16 ) SseSphereTerrainGeneratorT : public SseSphereTerrainGenerator
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
					virtual void GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, float uvRes, UTerrainVertex* vertices );

					///	\brief	Generates terrain vertex points and normals
					virtual void GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, float uvRes, UTerrainVertex* vertices, float& maxError );

					///	\brief	Determines the error on a patch
					virtual float GetMaximumPatchError( const float* origin, const float* xStep, const float* zStep, const int width, const int height, const int subdivisions ) const;

					///	\brief	Generates a cube map texture face
					virtual void GenerateTerrainPropertyCubeMapFace( const UCubeMapFace face, const int width, const int height, const int stride, unsigned char* pixels );

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
					float GetMaximumError( const int count, const float* heights0 );

					__m128 m_ShiftRightXxxx;
					__m128 m_ShiftRightYyyy;
					__m128 m_ShiftRightZzzz;
					__m128 m_ShiftDownXxxx;
					__m128 m_ShiftDownYyyy;
					__m128 m_ShiftDownZzzz;

					inline void SetupVertex( UTerrainVertex& vertex, const int offset, const __m128& x, const __m128& y, const __m128& z, const __m128& nX, const __m128& nY, const __m128& nZ, const __m128& s, const __m128& e, const __m128& u, const float v  )
					{
						vertex.SetPosition( x.m128_f32[ offset ], y.m128_f32[ offset ], z.m128_f32[ offset ] );
						vertex.SetNormal( nX.m128_f32[ offset ], nY.m128_f32[ offset ], nZ.m128_f32[ offset ] );
						vertex.SetTerrainUv( u.m128_f32[ offset ], v );
						vertex.SetTerrainParameters( e.m128_f32[ offset ], s.m128_f32[ offset ] );
					}

					inline void SetupVertex( UTerrainVertex& vertex, const int offset, const float* x, const float* y, const float* z, const float* nX, const float* nY, const float* nZ, const float* s, const float* e, const float* u, const float v )
					{
						vertex.SetPosition( x[ offset ], y[ offset ], z[ offset ] );
						vertex.SetNormal( nX[ offset ], nY[ offset ], nZ[ offset ] );
						vertex.SetTerrainUv( u[ offset ], v );
						vertex.SetTerrainParameters( s[ offset ], e[ offset ] );
					}

					inline void GetInitialErrorHeights( const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, float& lastHeight, float& lastIntHeight )
					{
						__m128 originXxxx = xxxx;
						__m128 originYyyy = yyyy;
						__m128 originZzzz = zzzz;
						SetLength( originXxxx, originYyyy, originZzzz, m_Displacer.GetSphereRadius( ) );
						__m128 heights = m_Displacer.Displace( originXxxx, originYyyy, originZzzz );

						lastHeight = heights.m128_f32[ 2 ];
						lastIntHeight = heights.m128_f32[ 3 ];
					}
					
					inline void GetRowMaxError( __m128 xxxx, __m128 yyyy, __m128 zzzz, const __m128& incXxxx, const __m128& incYyyy, const __m128& incZzzz, const int rowLength, float& maxError )
					{
						float lastHeight = 0;
						float lastIntHeight = 0;
						GetInitialErrorHeights( xxxx, yyyy, zzzz, lastHeight, lastIntHeight );

						for ( int i = 0; i < rowLength; ++i )
						{
							__m128 originXxxx = xxxx;
							__m128 originYyyy = yyyy;
							__m128 originZzzz = zzzz;
							SetLength( originXxxx, originYyyy, originZzzz, m_Displacer.GetSphereRadius( ) );
							__m128 heights = m_Displacer.Displace( originXxxx, originYyyy, originZzzz );

							const float currHeight0 = lastHeight;
							const float currHeight1 = heights.m128_f32[ 0 ];
							const float currHeight2 = heights.m128_f32[ 2 ];

							const float estHeight0 = ( currHeight0 + currHeight1 ) / 2;
							const float estHeight1 = ( currHeight1 + currHeight2 ) / 2;
							const float actHeight0 = lastIntHeight;
							const float actHeight1 = heights.m128_f32[ 1 ];

							const float error0 = abs( estHeight0 - actHeight0 );
							const float error1 = abs( estHeight1 - actHeight1 );

							lastHeight = currHeight2;
							lastIntHeight = heights.m128_f32[ 3 ];

							const float bigError = error0 > error1 ? error0 : error1;
							maxError = bigError > maxError ? bigError : maxError;

							xxxx = _mm_add_ps( xxxx, incXxxx );
							yyyy = _mm_add_ps( yyyy, incYyyy );
							zzzz = _mm_add_ps( zzzz, incZzzz );
						}
					}

					inline void SetErrorVertices( UTerrainVertex& v0, UTerrainVertex& v1, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, const __m128& uuuu, const float v, float& lastHeight, float& lastIntHeight, float& maxError )
					{
						__m128 normalXxxx = xxxx;
						__m128 normalYyyy = yyyy;
						__m128 normalZzzz = zzzz;
						SetLength( normalXxxx, normalYyyy, normalZzzz, Constants::Fc_1 );	//	Don't trust that normalize...

						__m128 originXxxx = xxxx;
						__m128 originYyyy = yyyy;
						__m128 originZzzz = zzzz;
						SetLength( originXxxx, originYyyy, originZzzz, m_Displacer.GetSphereRadius( ) );
						__m128 heights = m_Displacer.Displace( originXxxx, originYyyy, originZzzz );

						__m128 leftXxxx = _mm_sub_ps( xxxx, m_ShiftRightXxxx );
						__m128 leftYyyy = _mm_sub_ps( yyyy, m_ShiftRightYyyy );
						__m128 leftZzzz = _mm_sub_ps( zzzz, m_ShiftRightZzzz );
						SetLength( leftXxxx, leftYyyy, leftZzzz, m_Displacer.GetSphereRadius( ) );
						m_Displacer.Displace( leftXxxx, leftYyyy, leftZzzz );

						__m128 upXxxx = _mm_sub_ps( xxxx, m_ShiftDownXxxx );
						__m128 upYyyy = _mm_sub_ps( yyyy, m_ShiftDownYyyy );
						__m128 upZzzz = _mm_sub_ps( zzzz, m_ShiftDownZzzz );
						SetLength( upXxxx, upYyyy, upZzzz, m_Displacer.GetSphereRadius( ) );
						m_Displacer.Displace( upXxxx, upYyyy, upZzzz );
						
						__m128 rightXxxx = _mm_add_ps( xxxx, m_ShiftRightXxxx );
						__m128 rightYyyy = _mm_add_ps( yyyy, m_ShiftRightYyyy );
						__m128 rightZzzz = _mm_add_ps( zzzz, m_ShiftRightZzzz );
						SetLength( rightXxxx, rightYyyy, rightZzzz, m_Displacer.GetSphereRadius( ) );
						m_Displacer.Displace( rightXxxx, rightYyyy, rightZzzz );

						__m128 downXxxx = _mm_add_ps( xxxx, m_ShiftDownXxxx );
						__m128 downYyyy = _mm_add_ps( yyyy, m_ShiftDownYyyy );
						__m128 downZzzz = _mm_add_ps( zzzz, m_ShiftDownZzzz );
						SetLength( downXxxx, downYyyy, downZzzz, m_Displacer.GetSphereRadius( ) );
						m_Displacer.Displace( downXxxx, downYyyy, downZzzz );

						//	Move positions to the origin
						leftXxxx = _mm_sub_ps( leftXxxx, originXxxx );
						leftYyyy = _mm_sub_ps( leftYyyy, originYyyy );
						leftZzzz = _mm_sub_ps( leftZzzz, originZzzz );
						upXxxx = _mm_sub_ps( upXxxx, originXxxx );
						upYyyy = _mm_sub_ps( upYyyy, originYyyy );
						upZzzz = _mm_sub_ps( upZzzz, originZzzz );
						rightXxxx = _mm_sub_ps( rightXxxx, originXxxx );
						rightYyyy = _mm_sub_ps( rightYyyy, originYyyy );
						rightZzzz = _mm_sub_ps( rightZzzz, originZzzz );
						downXxxx = _mm_sub_ps( downXxxx, originXxxx );
						downYyyy = _mm_sub_ps( downYyyy, originYyyy );
						downZzzz = _mm_sub_ps( downZzzz, originZzzz );

						__m128 cpXxxx, cpYyyy, cpZzzz;
						GetCrossProducts( cpXxxx, cpYyyy, cpZzzz, upXxxx, upYyyy, upZzzz, leftXxxx, leftYyyy, leftZzzz );
						AccumulateCrossProducts( cpXxxx, cpYyyy, cpZzzz, rightXxxx, rightYyyy, rightZzzz, upXxxx, upYyyy, upZzzz );
						AccumulateCrossProducts( cpXxxx, cpYyyy, cpZzzz, downXxxx, downYyyy, downZzzz, rightXxxx, rightYyyy, rightZzzz );
						AccumulateCrossProducts( cpXxxx, cpYyyy, cpZzzz, leftXxxx, leftYyyy, leftZzzz, downXxxx, downYyyy, downZzzz );
						SetLength( cpXxxx, cpYyyy, cpZzzz, Constants::Fc_1 );

						__m128 slopes = _mm_sub_ps( Constants::Fc_1, Dot( cpXxxx, cpYyyy, cpZzzz, normalXxxx, normalYyyy, normalZzzz ) );
						slopes = _mm_div_ps( slopes, _mm_set1_ps( 0.4f ) );

						//	TODO: AP: Clamp slopes to 0-1 range
						SetupVertex( v0, 0, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );
						SetupVertex( v1, 2, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );

						const float currHeight0 = lastHeight;
						const float currHeight1 = heights.m128_f32[ 0 ];
						const float currHeight2 = heights.m128_f32[ 2 ];

						const float estHeight0 = ( currHeight0 + currHeight1 ) / 2;
						const float estHeight1 = ( currHeight1 + currHeight2 ) / 2;
						const float actHeight0 = lastIntHeight;
						const float actHeight1 = heights.m128_f32[ 1 ];

						const float error0 = abs( estHeight0 - actHeight0 );
						const float error1 = abs( estHeight1 - actHeight1 );

						lastHeight = currHeight2;
						lastIntHeight = heights.m128_f32[ 3 ];

						const float bigError = error0 > error1 ? error0 : error1;
						maxError = bigError > maxError ? bigError : maxError;
					}

					inline void SetVertices( UTerrainVertex& v0, UTerrainVertex& v1, UTerrainVertex& v2, UTerrainVertex& v3, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, const __m128& uuuu, const float v )
					{
						__m128 normalXxxx = xxxx;
						__m128 normalYyyy = yyyy;
						__m128 normalZzzz = zzzz;
						SetLength( normalXxxx, normalYyyy, normalZzzz, Constants::Fc_1 );	//	Don't trust that normalize...

						__m128 originXxxx = xxxx;
						__m128 originYyyy = yyyy;
						__m128 originZzzz = zzzz;
						SetLength( originXxxx, originYyyy, originZzzz, m_Displacer.GetSphereRadius( ) );
						__m128 heights = m_Displacer.Displace( originXxxx, originYyyy, originZzzz );

						__m128 leftXxxx = _mm_sub_ps( xxxx, m_ShiftRightXxxx );
						__m128 leftYyyy = _mm_sub_ps( yyyy, m_ShiftRightYyyy );
						__m128 leftZzzz = _mm_sub_ps( zzzz, m_ShiftRightZzzz );
						SetLength( leftXxxx, leftYyyy, leftZzzz, m_Displacer.GetSphereRadius( ) );
						m_Displacer.Displace( leftXxxx, leftYyyy, leftZzzz );

						__m128 upXxxx = _mm_sub_ps( xxxx, m_ShiftDownXxxx );
						__m128 upYyyy = _mm_sub_ps( yyyy, m_ShiftDownYyyy );
						__m128 upZzzz = _mm_sub_ps( zzzz, m_ShiftDownZzzz );
						SetLength( upXxxx, upYyyy, upZzzz, m_Displacer.GetSphereRadius( ) );
						m_Displacer.Displace( upXxxx, upYyyy, upZzzz );
						
						__m128 rightXxxx = _mm_add_ps( xxxx, m_ShiftRightXxxx );
						__m128 rightYyyy = _mm_add_ps( yyyy, m_ShiftRightYyyy );
						__m128 rightZzzz = _mm_add_ps( zzzz, m_ShiftRightZzzz );
						SetLength( rightXxxx, rightYyyy, rightZzzz, m_Displacer.GetSphereRadius( ) );
						m_Displacer.Displace( rightXxxx, rightYyyy, rightZzzz );

						__m128 downXxxx = _mm_add_ps( xxxx, m_ShiftDownXxxx );
						__m128 downYyyy = _mm_add_ps( yyyy, m_ShiftDownYyyy );
						__m128 downZzzz = _mm_add_ps( zzzz, m_ShiftDownZzzz );
						SetLength( downXxxx, downYyyy, downZzzz, m_Displacer.GetSphereRadius( ) );
						m_Displacer.Displace( downXxxx, downYyyy, downZzzz );

						//	Move positions to the origin
						leftXxxx = _mm_sub_ps( leftXxxx, originXxxx );
						leftYyyy = _mm_sub_ps( leftYyyy, originYyyy );
						leftZzzz = _mm_sub_ps( leftZzzz, originZzzz );
						upXxxx = _mm_sub_ps( upXxxx, originXxxx );
						upYyyy = _mm_sub_ps( upYyyy, originYyyy );
						upZzzz = _mm_sub_ps( upZzzz, originZzzz );
						rightXxxx = _mm_sub_ps( rightXxxx, originXxxx );
						rightYyyy = _mm_sub_ps( rightYyyy, originYyyy );
						rightZzzz = _mm_sub_ps( rightZzzz, originZzzz );
						downXxxx = _mm_sub_ps( downXxxx, originXxxx );
						downYyyy = _mm_sub_ps( downYyyy, originYyyy );
						downZzzz = _mm_sub_ps( downZzzz, originZzzz );

						__m128 cpXxxx, cpYyyy, cpZzzz;
						GetCrossProducts( cpXxxx, cpYyyy, cpZzzz, upXxxx, upYyyy, upZzzz, leftXxxx, leftYyyy, leftZzzz );
						AccumulateCrossProducts( cpXxxx, cpYyyy, cpZzzz, rightXxxx, rightYyyy, rightZzzz, upXxxx, upYyyy, upZzzz );
						AccumulateCrossProducts( cpXxxx, cpYyyy, cpZzzz, downXxxx, downYyyy, downZzzz, rightXxxx, rightYyyy, rightZzzz );
						AccumulateCrossProducts( cpXxxx, cpYyyy, cpZzzz, leftXxxx, leftYyyy, leftZzzz, downXxxx, downYyyy, downZzzz );
						SetLength( cpXxxx, cpYyyy, cpZzzz, Constants::Fc_1 );
						
						__m128 slopes = _mm_sub_ps( Constants::Fc_1, Dot( cpXxxx, cpYyyy, cpZzzz, normalXxxx, normalYyyy, normalZzzz ) );
						slopes = _mm_div_ps( slopes, _mm_set1_ps( 0.3f ) );
						Clamp( slopes, Constants::Fc_0, Constants::Fc_1 );

						//	TODO: AP: Clamp slopes to 0-1 range
						SetupVertex( v0, 0, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );
						SetupVertex( v1, 1, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );
						SetupVertex( v2, 2, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );
						SetupVertex( v3, 3, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );
						
						//_CRT_ALIGN( 16 ) float arrX[ 4 ], arrY[ 4 ], arrZ[ 4 ];
						//_CRT_ALIGN( 16 ) float arrNx[ 4 ], arrNy[ 4 ], arrNz[ 4 ];
						//_CRT_ALIGN( 16 ) float arrS[ 4 ], arrH[ 4 ];
						//_CRT_ALIGN( 16 ) float arrU[ 4 ];

						//_mm_store_ps( arrX, originXxxx );
						//_mm_store_ps( arrY, originYyyy );
						//_mm_store_ps( arrZ, originZzzz );
						//
						//_mm_store_ps( arrNx, cpXxxx );
						//_mm_store_ps( arrNy, cpYyyy );
						//_mm_store_ps( arrNz, cpZzzz );

						//_mm_store_ps( arrS, slopes );
						//_mm_store_ps( arrH, heights );
						//_mm_store_ps( arrU, uuuu );

						//SetupVertex( v0, 0, arrX, arrY, arrZ, arrNx, arrNy, arrNz, arrS, arrH, arrU, v );
						//SetupVertex( v1, 1, arrX, arrY, arrZ, arrNx, arrNy, arrNz, arrS, arrH, arrU, v );
						//SetupVertex( v2, 2, arrX, arrY, arrZ, arrNx, arrNy, arrNz, arrS, arrH, arrU, v );
						//SetupVertex( v3, 3, arrX, arrY, arrZ, arrNx, arrNy, arrNz, arrS, arrH, arrU, v );
					}

					///	\brief	Calculates a normal from stuff
					inline static void CalculateNormal( UTerrainVertex* v, const int prev, const int next, const float height, const float* xSrc, const float* ySrc, const float* zSrc, const float* uXSrc, const float* uYSrc, const float* uZSrc, const float* dXSrc, const float* dYSrc, const float* dZSrc )
					{
						const UVector3 left	( xSrc [ prev  ] - v->X( ), ySrc [ prev ] - v->Y( ), zSrc [ prev ] - v->Z( ) );
						const UVector3 right( xSrc [ next  ] - v->X( ), ySrc [ next ] - v->Y( ), zSrc [ next ] - v->Z( ) );
						const UVector3 up	( uXSrc[  0  ] - v->X( ), uYSrc[  0 ] - v->Y( ), uZSrc[  0 ] - v->Z( ) );
						const UVector3 lUp	( uXSrc[ prev  ] - v->X( ), uYSrc[ prev ] - v->Y( ), uZSrc[ prev ] - v->Z( ) );
						const UVector3 down	( dXSrc[  0  ] - v->X( ), dYSrc[  0 ] - v->Y( ), dZSrc[  0 ] - v->Z( ) );
						const UVector3 rDown( dXSrc[ next  ] - v->X( ), dYSrc[ next ] - v->Y( ), dZSrc[ next ] - v->Z( ) );

						UVector3 acc;
						acc.Add( UVector3::Cross( lUp, left ) );
						acc.Add( UVector3::Cross( up, lUp ) );
						acc.Add( UVector3::Cross( right, up ) );
						acc.Add( UVector3::Cross( rDown, right ) );
						acc.Add( UVector3::Cross( down, rDown ) );
						acc.Add( UVector3::Cross( left, down ) );
						acc.Normalise( );

						v->SetNormal( acc.m_X, acc.m_Y, acc.m_Z );

						UVector3 yAxis( v->X( ), v->Y( ), v->Z( ) );
						yAxis.Normalise( );

						//	Slope of 0 is flat, slope of 1 is vertical
						float slope = 1.0f - UVector3::Dot( yAxis, acc );
						slope /= 0.7f;
						slope = ( slope < 0 ? 0 : slope > 1 ? 1 : slope );

						v->SetTerrainParameters( height, slope );
					}

					inline void AssignShiftVectors( const float* xStep, const float* zStep )
					{
						UVector3 xStepVec( xStep );
						UVector3 zStepVec( zStep );
						xStepVec.SetLength( m_SmallestX );
						zStepVec.SetLength( m_SmallestZ );

						m_ShiftRightXxxx 	= _mm_set1_ps( xStepVec.m_X );
						m_ShiftRightYyyy 	= _mm_set1_ps( xStepVec.m_Y );
						m_ShiftRightZzzz 	= _mm_set1_ps( xStepVec.m_Z );
						m_ShiftDownXxxx	 	= _mm_set1_ps( zStepVec.m_X );
						m_ShiftDownYyyy		= _mm_set1_ps( zStepVec.m_Y );
						m_ShiftDownZzzz		= _mm_set1_ps( zStepVec.m_Z );
					}

			};
			
			//	---------------------------------------------------------------------------------------

			//	---------------------------------------------- SseSphereTerrainGenerator Inline Methods

			inline void SseSphereTerrainGenerator::SetSmallestStepSize( const float x, const float z )
			{
				m_SmallestX = x;
				m_SmallestZ = z;
			}
			
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

					//	Disable normalize to remove sphere mapping
					SetLength( tmpXxxx, tmpYyyy, tmpZzzz, m_Displacer.GetSphereRadius( ) );
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

					//	Disable normalize to remove sphere mapping
				//	Normalize( tmpXxxx, tmpYyyy, tmpZzzz );
					SetLength( tmpXxxx, tmpYyyy, tmpZzzz, m_Displacer.GetSphereRadius( ) );
					__m128 heights = m_Displacer.Displace( tmpXxxx, tmpYyyy, tmpZzzz );
				
					//	Uncomment following to clamp the points to the sphere
				//	tmpXxxx = xxxx;
				//	tmpYyyy = yyyy;
				//	tmpZzzz = zzzz;
				//	SetLength( tmpXxxx, tmpYyyy, tmpZzzz, _mm_mul_ps( m_Displacer.GetSphereRadius( ), m_Displacer.GetMinimumHeight( ) ) );


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

				return m_Displacer.MapErrorToHeightRange( maxError );
			}

			template < typename DisplaceType >
			inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, float uvRes, UTerrainVertex* vertices )
			{
				//*
				AssignShiftVectors( xStep, zStep );

				//	Get start x, y and z positions for the first 4 vertices in the first row
				//	NOTE: AP: Vectors are apparently reversed, so memory access is more natural (xyzw comes out as [ w, z, y, x ] normally)
				__m128 startXxxx = _mm_set_ps( origin[ 0 ] + xStep[ 0 ] * 3, origin[ 0 ] + xStep[ 0 ] * 2, origin[ 0 ] + xStep[ 0 ], origin[ 0 ] );
				__m128 startYyyy = _mm_set_ps( origin[ 1 ] + xStep[ 1 ] * 3, origin[ 1 ] + xStep[ 1 ] * 2, origin[ 1 ] + xStep[ 1 ], origin[ 1 ] );
				__m128 startZzzz = _mm_set_ps( origin[ 2 ] + xStep[ 2 ] * 3, origin[ 2 ] + xStep[ 2 ] * 2, origin[ 2 ] + xStep[ 2 ], origin[ 2 ] );

				//	Determine vectors for incrementing x, y and z positions in the column loop
				const __m128 colXInc = _mm_set1_ps( xStep[ 0 ] * 4 );
				const __m128 colYInc = _mm_set1_ps( xStep[ 1 ] * 4 );
				const __m128 colZInc = _mm_set1_ps( xStep[ 2 ] * 4 );

				//	Determine vectors for incrementing x, y and z positions in the row loop
				const __m128 rowXInc = _mm_set1_ps( zStep[ 0 ] );
				const __m128 rowYInc = _mm_set1_ps( zStep[ 1 ] );
				const __m128 rowZInc = _mm_set1_ps( zStep[ 2 ] );
				


				//	Point to the positions and normals in the first 4 vertices
				UTerrainVertex* v0 = vertices;
				UTerrainVertex* v1 = vertices + 1;
				UTerrainVertex* v2 = vertices + 2;
				UTerrainVertex* v3 = vertices + 3;
				
				const int widthDiv4 = width / 4;
				const int widthMod4 = width % 4;
				float uInc = uvRes / ( float )( width - 1 );
				float vInc = uvRes / ( float )( height - 1 );
				float v = 0;
				__m128 uuuuInc = _mm_set1_ps( uInc * 4 );
				UTerrainVertex dummyVertex;

				for ( int row = 0; row < height; ++row, v += vInc )
				{
					__m128 uuuu = _mm_set_ps( uInc * 3, uInc * 2, uInc, 0 );
					__m128 xxxx = startXxxx;
					__m128 yyyy = startYyyy;
					__m128 zzzz = startZzzz;
					for ( int col = 0; col < widthDiv4; ++col )
					{
						SetVertices( *v0, *v1, *v2, *v3, xxxx, yyyy, zzzz, uuuu, v );

						//	Move vertex pointers on
						v0 += 4; v1 += 4; v2 += 4; v3 += 4;
						xxxx = _mm_add_ps( xxxx, colXInc );
						yyyy = _mm_add_ps( yyyy, colYInc );
						zzzz = _mm_add_ps( zzzz, colZInc );
						uuuu = _mm_add_ps( uuuu, uuuuInc );
					}

					//	Very very lazy (just fill out dummy vertices, so SetVertices() doesn't have to be overloaded)
					switch ( widthMod4 )
					{
						case 1 : SetVertices( *v0, dummyVertex, dummyVertex, dummyVertex, xxxx, yyyy, zzzz, uuuu, v ); break;
						case 2 : SetVertices( *v0, *v1, dummyVertex, dummyVertex, xxxx, yyyy, zzzz, uuuu, v ); break;
						case 3 : SetVertices( *v0, *v1, *v2, dummyVertex, xxxx, yyyy, zzzz, uuuu, v ); break;
					}

					v0 += widthMod4;
					v1 += widthMod4;
					v2 += widthMod4;
					v3 += widthMod4;

					startXxxx = _mm_add_ps( startXxxx, rowXInc );
					startYyyy = _mm_add_ps( startYyyy, rowYInc );
					startZzzz = _mm_add_ps( startZzzz, rowZInc );
				}
				/*/
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
				const int cacheSize = fullWidth * 4; // NOTE: AP: x4, not x3, because we need to store heights also
				SetFpCacheSize( cacheSize );
				const int fullWidthDiv4 = fullWidth / 4;
				float** cacheLines = m_FpCacheLines;

				int prevCacheLine = 0;
				int curCacheLine = 1;
				int nextCacheLine = 2;

				//	Fill the cache with positions from the first 3 vertex rows
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
				UTerrainVertex* v0 = vertices;
				UTerrainVertex* v1 = vertices + 1;
				UTerrainVertex* v2 = vertices + 2;
				UTerrainVertex* v3 = vertices + 3;

				const int widthDiv4 = width / 4;
				const int widthMod4 = width % 4;

				float uInc = uvRes / ( float )( width - 1 );
				float vInc = uvRes / ( float )( height - 1 );
				float v = 0;
				__m128 uuuuInc = _mm_set1_ps( uInc * 4 );
				_CRT_ALIGN( 16 ) float uArr[ 4 ];

				for ( int row = 0; row < height; ++row, v += vInc )
				{
					const float* uXSrc = cacheLines[ prevCacheLine ] + 16;
					const float* uYSrc = cacheLines[ prevCacheLine ] + 20;
					const float* uZSrc = cacheLines[ prevCacheLine ] + 24;
					
					const float* xSrc = cacheLines[ curCacheLine ] + 16;
					const float* ySrc = cacheLines[ curCacheLine ] + 20;
					const float* zSrc = cacheLines[ curCacheLine ] + 24;
					const float* hSrc = cacheLines[ curCacheLine ] + 28;
					
					const float* dXSrc = cacheLines[ nextCacheLine ] + 16;
					const float* dYSrc = cacheLines[ nextCacheLine ] + 20;
					const float* dZSrc = cacheLines[ nextCacheLine ] + 24;

					__m128 uuuu = _mm_set_ps( uInc * 3, uInc * 2, uInc, 0 );

					for ( int col = 0; col < widthDiv4; ++col )
					{
						//	Store positions
						v0->SetPosition( xSrc[ 0 ], ySrc[ 0 ], zSrc[ 0 ] );
						v1->SetPosition( xSrc[ 1 ], ySrc[ 1 ], zSrc[ 1 ] );
						v2->SetPosition( xSrc[ 2 ], ySrc[ 2 ], zSrc[ 2 ] );
						v3->SetPosition( xSrc[ 3 ], ySrc[ 3 ], zSrc[ 3 ] );

						_mm_store_ps( uArr, uuuu );

						v0->SetTerrainUv( uArr[ 0 ], v );
						v1->SetTerrainUv( uArr[ 1 ], v );
						v2->SetTerrainUv( uArr[ 2 ], v );
						v3->SetTerrainUv( uArr[ 3 ], v );

						//	Calculate vertex normals
						CalculateNormal( v0, -13, 1, hSrc[ 0 ], xSrc, ySrc, zSrc, uXSrc, uYSrc, uZSrc, dXSrc, dYSrc, dZSrc );
						CalculateNormal( v1, -1, 1, hSrc[ 1 ], xSrc + 1, ySrc + 1, zSrc + 1, uXSrc + 1, uYSrc + 1, uZSrc + 1, dXSrc + 1, dYSrc + 1, dZSrc + 1 );
						CalculateNormal( v2, -1, 1, hSrc[ 2 ], xSrc + 2, ySrc + 2, zSrc + 2, uXSrc + 2, uYSrc + 2, uZSrc + 2, dXSrc + 2, dYSrc + 2, dZSrc + 2 );
						CalculateNormal( v3, -1, 13, hSrc[ 3 ], xSrc + 3, ySrc + 3, zSrc + 3, uXSrc + 3, uYSrc + 3, uZSrc + 3, dXSrc + 3, dYSrc + 3, dZSrc + 3 );

						//	Move vertex pointers on
						v0 += 4;
						v1 += 4;
						v2 += 4;
						v3 += 4;

						//	Move cache pointers on
						xSrc += 16; ySrc += 16; zSrc += 16; hSrc += 16;
						uXSrc += 16; uYSrc += 16; uZSrc += 16;
						dXSrc += 16; dYSrc += 16; dZSrc += 16;

						uuuu = _mm_add_ps( uuuu, uuuuInc );
					}
					if ( widthMod4 > 0 )
					{
						_mm_store_ps( uArr, uuuu );

						v0->SetPosition( xSrc[ 0 ], ySrc[ 0 ], zSrc[ 0 ] );
						v0->SetTerrainUv( uArr[ 0 ], v );
						CalculateNormal( v0, -13, 1, hSrc[ 0 ], xSrc, ySrc, zSrc, uXSrc, uYSrc, uZSrc, dXSrc, dYSrc, dZSrc );
						if ( widthMod4 > 1 )
						{
							v1->SetPosition( xSrc[ 1 ], ySrc[ 1 ], zSrc[ 1 ] );
							v1->SetTerrainUv( uArr[ 1 ], v );
							CalculateNormal( v1, -1, 1, hSrc[ 1 ], xSrc + 1, ySrc + 1, zSrc + 1, uXSrc + 1, uYSrc + 1, uZSrc + 1, dXSrc + 1, dYSrc + 1, dZSrc + 1 );
							if ( widthMod4 > 2 )
							{
								v2->SetPosition( xSrc[ 2 ], ySrc[ 2 ], zSrc[ 2 ] );
								v2->SetTerrainUv( uArr[ 2 ], v );
								CalculateNormal( v2, -1, 1, hSrc[ 2 ], xSrc + 2, ySrc + 2, zSrc + 2, uXSrc + 2, uYSrc + 2, uZSrc + 2, dXSrc + 2, dYSrc + 2, dZSrc + 2 );
							}
						}
						v0 += widthMod4;
						v1 += widthMod4;
						v2 += widthMod4;
						v3 += widthMod4;
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
				//*/
			}

			template < typename DisplaceType >
			inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, float uvRes, UTerrainVertex* vertices, float& error )
			{
				//*
				AssignShiftVectors( xStep, zStep );

				//	Same as GenerateVertices() without error, except that the resolution is doubled
				xStep[ 0 ] /= 2; xStep[ 1 ] /= 2; xStep[ 2 ] /= 2;
				zStep[ 0 ] /= 2; zStep[ 1 ] /= 2; zStep[ 2 ] /= 2;
				width = ( width * 2 ) - 1;
				height = ( height * 2 ) - 1;
				
				//	Get start x, y and z positions for the first 4 vertices in the first row
				//	NOTE: AP: Vectors are apparently reversed, so memory access is more natural (xyzw comes out as [ w, z, y, x ] normally)
				__m128 startXxxx = _mm_set_ps( origin[ 0 ] + xStep[ 0 ] * 3, origin[ 0 ] + xStep[ 0 ] * 2, origin[ 0 ] + xStep[ 0 ], origin[ 0 ] );
				__m128 startYyyy = _mm_set_ps( origin[ 1 ] + xStep[ 1 ] * 3, origin[ 1 ] + xStep[ 1 ] * 2, origin[ 1 ] + xStep[ 1 ], origin[ 1 ] );
				__m128 startZzzz = _mm_set_ps( origin[ 2 ] + xStep[ 2 ] * 3, origin[ 2 ] + xStep[ 2 ] * 2, origin[ 2 ] + xStep[ 2 ], origin[ 2 ] );

				//	Determine vectors for incrementing x, y and z positions in the column loop
				const __m128 colXInc = _mm_set1_ps( xStep[ 0 ] * 4 );
				const __m128 colYInc = _mm_set1_ps( xStep[ 1 ] * 4 );
				const __m128 colZInc = _mm_set1_ps( xStep[ 2 ] * 4 );
				
				//	Determine vectors for incrementing x, y and z positions in the row loop
				const __m128 rowXInc = _mm_set1_ps( zStep[ 0 ] );
				const __m128 rowYInc = _mm_set1_ps( zStep[ 1 ] );
				const __m128 rowZInc = _mm_set1_ps( zStep[ 2 ] );
				
				startXxxx = _mm_sub_ps( startXxxx, colXInc );
				startYyyy = _mm_sub_ps( startYyyy, colYInc );
				startZzzz = _mm_sub_ps( startZzzz, colZInc );

				float uInc = uvRes / ( float )( width - 1 );
				float vInc = uvRes / ( float )( height - 1 );
				const __m128 uuuuInc = _mm_set1_ps( uInc * 4 );
				float v = 0;

				UTerrainVertex* v0 = vertices;
				UTerrainVertex* v1 = vertices + 1;
				UTerrainVertex dummyVertex;
				const int widthDiv4 = width / 4;
				const int widthMod4 = width % 4;
				
				error = 0;

				float lastHeight = 0;
				float lastIntHeight = 0;

				for ( int row = 0; row < height; ++row, v += vInc )
				{
					if ( ( row % 2 ) != 0 )
					{
						GetRowMaxError( startXxxx, startYyyy, startZzzz, colXInc, colYInc, colZInc, widthDiv4, error );
					}
					else
					{
						__m128 uuuu = _mm_set_ps( uInc * 3, uInc * 2, uInc, 0 );
						__m128 xxxx = startXxxx;
						__m128 yyyy = startYyyy;
						__m128 zzzz = startZzzz;

						GetInitialErrorHeights( xxxx, yyyy, zzzz, lastHeight, lastIntHeight );

						xxxx = _mm_add_ps( xxxx, colXInc );
						yyyy = _mm_add_ps( yyyy, colYInc );
						zzzz = _mm_add_ps( zzzz, colZInc );

						//	Only get positions and normals from every second row (because its working at double resolution)
						for ( int col = 0; col < widthDiv4; ++col )
						{
							SetErrorVertices( *v0, *v1, xxxx, yyyy, zzzz, uuuu, v, lastHeight, lastIntHeight, error );

							xxxx = _mm_add_ps( xxxx, colXInc );
							yyyy = _mm_add_ps( yyyy, colYInc );
							zzzz = _mm_add_ps( zzzz, colZInc );
							uuuu = _mm_add_ps( uuuu, uuuuInc );

							//	Move vertex pointers on
							v0 += 2;
							v1 += 2;
						}

						//	Very very lazy (just fill out dummy vertices, so SetVertices() doesn't have to be overloaded)
						switch ( widthMod4 )
						{
							case 1 : SetErrorVertices( *v0, dummyVertex, xxxx, yyyy, zzzz, uuuu, v, lastHeight, lastIntHeight, error ); break;
							case 2 : SetErrorVertices( *v0, dummyVertex, xxxx, yyyy, zzzz, uuuu, v, lastHeight, lastIntHeight, error ); break;
							case 3 : SetErrorVertices( *v0, *v1, xxxx, yyyy, zzzz, uuuu, v, lastHeight, lastIntHeight, error ); break;
						}

						v0 += widthMod4;
						v1 += widthMod4;
					}

					startXxxx = _mm_add_ps( startXxxx, rowXInc );
					startYyyy = _mm_add_ps( startYyyy, rowYInc );
					startZzzz = _mm_add_ps( startZzzz, rowZInc );
				}
				error = m_Displacer.MapErrorToHeightRange( error );

				/*/
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
				UTerrainVertex* v0 = vertices;
				UTerrainVertex* v1 = vertices + 1;
				const int widthDiv4 = width / 4;
				const int widthMod4 = width % 4;

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
						const float* hSrc = cacheLines[ curCacheLine ] + 28;
						
						const float* dXSrc = cacheLines[ nextCacheLine ] + 16;
						const float* dYSrc = cacheLines[ nextCacheLine ] + 20;
						const float* dZSrc = cacheLines[ nextCacheLine ] + 24;

						//	Only get positions and normals from every second row (because its working at double resolution)
						for ( int col = 0; col < widthDiv4; ++col )
						{
							//	Store positions
							v0->SetPosition( xSrc[ 0 ], ySrc[ 0 ], zSrc[ 0 ] );
							v1->SetPosition( xSrc[ 2 ], ySrc[ 2 ], zSrc[ 2 ] );

							//	Calculate vertex normals
							CalculateNormal( v0, -13, 2, hSrc[ 0 ], xSrc, ySrc, zSrc, uXSrc, uYSrc, uZSrc, dXSrc, dYSrc, dZSrc );
							CalculateNormal( v1, -2, 14, hSrc[ 2 ], xSrc + 2, ySrc + 2, zSrc + 2, uXSrc + 2, uYSrc + 2, uZSrc + 2, dXSrc + 2, dYSrc + 2, dZSrc + 2 );

							//	Move vertex pointers on
							v0 += 2;
							v1 += 2;

							//	Move cache pointers on
							xSrc += 16; ySrc += 16; zSrc += 16; hSrc += 16;
							uXSrc += 16; uYSrc += 16; uZSrc += 16;
							dXSrc += 16; dYSrc += 16; dZSrc += 16;
						}
						if ( widthMod4 > 0 )
						{
							v0->SetPosition( xSrc[ 0 ], ySrc[ 0 ], zSrc[ 0 ] );
							CalculateNormal( v0, -13, 2, hSrc[ 0 ], xSrc, ySrc, zSrc, uXSrc, uYSrc, uZSrc, dXSrc, dYSrc, dZSrc );
							if ( widthMod4 > 1 )
							{
								v1->SetPosition( xSrc[ 1 ], ySrc[ 1 ], zSrc[ 1 ] );
								CalculateNormal( v1, -2, 14, hSrc[ 2 ], xSrc + 2, ySrc + 2, zSrc + 2, uXSrc + 2, uYSrc + 2, uZSrc + 2, dXSrc + 2, dYSrc + 2, dZSrc + 2 );
							}
							v0 += widthMod4;
							v1 += widthMod4;
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
				//*/
			}

			template < typename DisplaceType >
			inline float SseSphereTerrainGeneratorT< DisplaceType >::GetMaximumPatchError( const float* origin, const float* xStep, const float* zStep, const int width, const int height, const int subdivisions ) const
			{
				//	TODO: AP: ...
				return 0;
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
			inline void SseSphereTerrainGeneratorT< DisplaceType >::GenerateTerrainPropertyCubeMapFace( const UCubeMapFace face, const int width, const int height, const int stride, unsigned char* pixels )
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

						const UColour b0;// = selector.GetColour( curLatitude[ 3 ], curHeight[ 3 ], 0, 1 );
						const UColour b1;// = selector.GetColour( curLatitude[ 2 ], curHeight[ 2 ], 0, 1 );
						const UColour b2;// = selector.GetColour( curLatitude[ 1 ], curHeight[ 1 ], 0, 1 );
						const UColour b3;// = selector.GetColour( curLatitude[ 0 ], curHeight[ 0 ], 0, 1 );

						curPixel[ 0 ]  = b0.R( ); curPixel[ 1 ]  = b0.G( ); curPixel[ 2 ]  = b0.B( );  curPixel[ 3 ]  = 0xff;
						curPixel[ 4 ]  = b1.R( ); curPixel[ 5 ]  = b1.G( ); curPixel[ 6 ]  = b1.B( );  curPixel[ 7 ]  = 0xff;
						curPixel[ 8 ]  = b2.R( ); curPixel[ 9 ]  = b2.G( ); curPixel[ 10 ] = b2.B( );  curPixel[ 11 ] = 0xff;
						curPixel[ 12 ] = b3.R( ); curPixel[ 13 ] = b3.G( ); curPixel[ 14 ] = b3.B( );  curPixel[ 15 ] = 0xff;
						curPixel += 16;
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

			//	-----------------------------------------------------------------------------------

		}; //Terrain

	};
};

#pragma managed(pop)
