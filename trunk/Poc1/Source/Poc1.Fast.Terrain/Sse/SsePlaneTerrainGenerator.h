#pragma once
#pragma managed( push, off )

#include "SseTerrainGenerator.h"
#include "SsePlaneTerrainDisplacers.h"

#include <math.h>

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{

			
			//	----------------------------------------------------------------------------- Types

			///	\brief	Generates terrain for planar geometries
			template < typename DisplaceType = SseNoDisplacement >
			class SsePlaneTerrainGeneratorT : public SseTerrainGenerator
			{
				public :

					///	\brief	Sets the source dimensions (the scale used to define the initial patch dimensions)
					void SetSourceDimensions( const float width, const float height );

					///	\brief	Gets the object used to displace vertices from the sphere surface
					DisplaceType& GetDisplacer( );

					///	\brief	Gets the object used to displace vertices from the sphere surface
					const DisplaceType& GetDisplacer( ) const;

					///	\brief	Gets the displacer
					virtual SseTerrainDisplacer& GetBaseDisplacer( );

					///	\brief	Gets the displacer
					virtual const SseTerrainDisplacer& GetBaseDisplacer( ) const;

					///	\brief	Generates terrain vertex points and normals
					virtual void GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, const float* uv, float uvRes, UTerrainVertex* vertices );

					///	\brief	Generates terrain vertex points and normals. Gets maximum patch error
					virtual void GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, const float* uv, float uvRes, UTerrainVertex* vertices, float& maxError );

				private :

					DisplaceType m_Displacer;

					void SetVertices( UTerrainVertex& v0, UTerrainVertex& v1, UTerrainVertex& v2, UTerrainVertex& v3, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, const __m128& uuuu, const float v );

					void SetErrorVertices( UTerrainVertex& v0, UTerrainVertex& v1, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, const __m128& uuuu, const float v, float& lastHeight, float& lastIntHeight, float& maxError );

					void GetInitialErrorHeights( const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, float& lastHeight, float& lastIntHeight );

					void GetRowMaxError( __m128 xxxx, __m128 yyyy, __m128 zzzz, const __m128& incXxxx, const __m128& incYyyy, const __m128& incZzzz, const int rowLength, float& maxError );

			}; //SsePlaneTerrainGeneratorT

			
			//	-----------------------------------------------------------------------------------


			//	------------------------------------------------- SsePlaneTerrainGeneratorT Methods

			template < typename DisplaceType >
			inline DisplaceType& SsePlaneTerrainGeneratorT< DisplaceType >::GetDisplacer( )
			{
				return m_Displacer;
			}
			
			template < typename DisplaceType >
			inline const DisplaceType& SsePlaneTerrainGeneratorT< DisplaceType >::GetDisplacer( ) const
			{
				return m_Displacer;
			}

			template < typename DisplaceType >
			inline SseTerrainDisplacer& SsePlaneTerrainGeneratorT< DisplaceType >::GetBaseDisplacer( )
			{
				return m_Displacer;
			}
			
			template < typename DisplaceType >
			inline const SseTerrainDisplacer& SsePlaneTerrainGeneratorT< DisplaceType >::GetBaseDisplacer( ) const
			{
				return m_Displacer;
			}

			template < typename DisplaceType >
			void SsePlaneTerrainGeneratorT< DisplaceType >::SetVertices( UTerrainVertex& v0, UTerrainVertex& v1, UTerrainVertex& v2, UTerrainVertex& v3, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, const __m128& uuuu, const float v )
			{
				DisplaceVertices( m_Displacer, v0, v1, v2, v3, xxxx, yyyy, zzzz, uuuu, v );
			}

			template < typename DisplaceType >
			inline void SsePlaneTerrainGeneratorT< DisplaceType >::SetErrorVertices( UTerrainVertex& v0, UTerrainVertex& v1, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, const __m128& uuuu, const float v, float& lastHeight, float& lastIntHeight, float& maxError )
			{
				__m128 originXxxx = xxxx;
				__m128 originYyyy = yyyy;
				__m128 originZzzz = zzzz;
				__m128 heights = m_Displacer.Displace( originXxxx, originYyyy, originZzzz );

				__m128 leftXxxx = _mm_sub_ps( xxxx, m_ShiftRightXxxx );
				__m128 leftYyyy = _mm_sub_ps( yyyy, m_ShiftRightYyyy );
				__m128 leftZzzz = _mm_sub_ps( zzzz, m_ShiftRightZzzz );
				m_Displacer.Displace( leftXxxx, leftYyyy, leftZzzz );

				__m128 upXxxx = _mm_sub_ps( xxxx, m_ShiftDownXxxx );
				__m128 upYyyy = _mm_sub_ps( yyyy, m_ShiftDownYyyy );
				__m128 upZzzz = _mm_sub_ps( zzzz, m_ShiftDownZzzz );
				m_Displacer.Displace( upXxxx, upYyyy, upZzzz );
				
				__m128 rightXxxx = _mm_add_ps( xxxx, m_ShiftRightXxxx );
				__m128 rightYyyy = _mm_add_ps( yyyy, m_ShiftRightYyyy );
				__m128 rightZzzz = _mm_add_ps( zzzz, m_ShiftRightZzzz );
				m_Displacer.Displace( rightXxxx, rightYyyy, rightZzzz );

				__m128 downXxxx = _mm_add_ps( xxxx, m_ShiftDownXxxx );
				__m128 downYyyy = _mm_add_ps( yyyy, m_ShiftDownYyyy );
				__m128 downZzzz = _mm_add_ps( zzzz, m_ShiftDownZzzz );
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

				__m128 slopes = _mm_sub_ps( Constants::Fc_1, Dot( cpXxxx, cpYyyy, cpZzzz, Constants::Fc_0, Constants::Fc_1, Constants::Fc_0 ) );
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

			template < typename DisplaceType >
			inline void SsePlaneTerrainGeneratorT< DisplaceType >::GetRowMaxError( __m128 xxxx, __m128 yyyy, __m128 zzzz, const __m128& incXxxx, const __m128& incYyyy, const __m128& incZzzz, const int rowLength, float& maxError )
			{
				float lastHeight = 0;
				float lastIntHeight = 0;
				GetInitialErrorHeights( xxxx, yyyy, zzzz, lastHeight, lastIntHeight );

				for ( int i = 0; i < rowLength; ++i )
				{
					__m128 originXxxx = xxxx;
					__m128 originYyyy = yyyy;
					__m128 originZzzz = zzzz;
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

			template < typename DisplaceType >
			void SsePlaneTerrainGeneratorT< DisplaceType >::GenerateVertices( const float* origin, const float* xStep, const float* zStep, const int width, const int height, const float* uv, float uvRes, UTerrainVertex* vertices )
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
				float v = uv[ 1 ];
				__m128 uuuuInc = _mm_set1_ps( uInc * 4 );
				UTerrainVertex dummyVertex;

				for ( int row = 0; row < height; ++row, v += vInc )
				{
					__m128 uuuu = _mm_set_ps( uv[ 0 ] + uInc * 3, uv[ 0 ] + uInc * 2, uv[ 0 ] + uInc, uv[ 0 ] );
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
			}

			template < typename DisplaceType >
			inline void SsePlaneTerrainGeneratorT< DisplaceType >::GetInitialErrorHeights( const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, float& lastHeight, float& lastIntHeight )
			{
				__m128 originXxxx = xxxx;
				__m128 originYyyy = yyyy;
				__m128 originZzzz = zzzz;
				SetLength( originXxxx, originYyyy, originZzzz, m_Displacer.GetFunctionScale( ) );
				__m128 heights = m_Displacer.Displace( originXxxx, originYyyy, originZzzz );

				lastHeight = heights.m128_f32[ 2 ];
				lastIntHeight = heights.m128_f32[ 3 ];
			}

			template < typename DisplaceType >
			inline void SsePlaneTerrainGeneratorT< DisplaceType >::GenerateVertices( const float* origin, float* xStep, float* zStep, int width, int height, const float* uv, float uvRes, UTerrainVertex* vertices, float& error )
			{
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
				float v = uv[ 1 ];

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
						__m128 uuuu = _mm_set_ps( uv[ 0 ] + uInc * 3, uv[ 0 ] + uInc * 2, uv[ 0 ] + uInc, uv[ 0 ] );
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
				error = m_Displacer.MapToHeightScale( error );
			}

			//	-----------------------------------------------------------------------------------

		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed( pop )