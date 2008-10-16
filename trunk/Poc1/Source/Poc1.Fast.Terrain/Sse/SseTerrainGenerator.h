#pragma once
#pragma managed( push, off )

#include "UTerrainGenerator.h"

#include <Sse\SseUtils.h>
#include <UVector3.h>

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			///	\brief	Handy base class for terrain generators
			class SseTerrainGenerator : public UTerrainGenerator
			{
				protected :

					__m128 m_ShiftRightXxxx;
					__m128 m_ShiftRightYyyy;
					__m128 m_ShiftRightZzzz;
					__m128 m_ShiftDownXxxx;
					__m128 m_ShiftDownYyyy;
					__m128 m_ShiftDownZzzz;

					void AssignCubeFaceShiftVectors( const UCubeMapFace face );

					void AssignShiftVectors( const float* xStep, const float* zStep );

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

					template < typename DisplaceType >
					inline void DisplaceVertices( DisplaceType& displacer, UTerrainVertex& v0, UTerrainVertex& v1, UTerrainVertex& v2, UTerrainVertex& v3, const __m128& xxxx, const __m128& yyyy, const __m128& zzzz, const __m128& uuuu, const float v )
					{
						__m128 normalXxxx = xxxx;
						__m128 normalYyyy = yyyy;
						__m128 normalZzzz = zzzz;
						displacer.GetUpVector( normalXxxx, normalYyyy, normalZzzz );

						__m128 originXxxx = xxxx;
						__m128 originYyyy = yyyy;
						__m128 originZzzz = zzzz;
						displacer.MapToDisplacementSpace( originXxxx, originYyyy, originZzzz );
						__m128 heights = displacer.Displace( originXxxx, originYyyy, originZzzz );

						__m128 leftXxxx = _mm_sub_ps( xxxx, m_ShiftRightXxxx );
						__m128 leftYyyy = _mm_sub_ps( yyyy, m_ShiftRightYyyy );
						__m128 leftZzzz = _mm_sub_ps( zzzz, m_ShiftRightZzzz );
						displacer.MapToDisplacementSpace( leftXxxx, leftYyyy, leftZzzz );
						displacer.Displace( leftXxxx, leftYyyy, leftZzzz );

						__m128 upXxxx = _mm_sub_ps( xxxx, m_ShiftDownXxxx );
						__m128 upYyyy = _mm_sub_ps( yyyy, m_ShiftDownYyyy );
						__m128 upZzzz = _mm_sub_ps( zzzz, m_ShiftDownZzzz );
						displacer.MapToDisplacementSpace( upXxxx, upYyyy, upZzzz );
						displacer.Displace( upXxxx, upYyyy, upZzzz );
						
						__m128 rightXxxx = _mm_add_ps( xxxx, m_ShiftRightXxxx );
						__m128 rightYyyy = _mm_add_ps( yyyy, m_ShiftRightYyyy );
						__m128 rightZzzz = _mm_add_ps( zzzz, m_ShiftRightZzzz );
						displacer.MapToDisplacementSpace( rightXxxx, rightYyyy, rightZzzz );
						displacer.Displace( rightXxxx, rightYyyy, rightZzzz );

						__m128 downXxxx = _mm_add_ps( xxxx, m_ShiftDownXxxx );
						__m128 downYyyy = _mm_add_ps( yyyy, m_ShiftDownYyyy );
						__m128 downZzzz = _mm_add_ps( zzzz, m_ShiftDownZzzz );
						displacer.MapToDisplacementSpace( downXxxx, downYyyy, downZzzz );
						displacer.Displace( downXxxx, downYyyy, downZzzz );

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
						slopes = _mm_div_ps( slopes, _mm_set1_ps( 0.3f ) );
						Clamp( slopes, Constants::Fc_0, Constants::Fc_1 );

						//	TODO: AP: Clamp slopes to 0-1 range
						SetupVertex( v0, 0, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );
						SetupVertex( v1, 1, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );
						SetupVertex( v2, 2, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );
						SetupVertex( v3, 3, originXxxx, originYyyy, originZzzz, cpXxxx, cpYyyy, cpZzzz, slopes, heights, uuuu, v );
					}

			}; //SseTerrainGenerator

			inline void SseTerrainGenerator::AssignCubeFaceShiftVectors( const UCubeMapFace face )
			{
				float xStep[ 3 ] = { 0, 0, 0 };
				float yStep[ 3 ] = { 0, 0, 0 };
				switch ( face )
				{
					case NegativeX :
						xStep[ 2 ] = 1; yStep[ 1 ] = -1;
						break;
					case PositiveX :
						xStep[ 2 ] = -1; yStep[ 1 ] = 1;
						break;
					case NegativeY :
						xStep[ 0 ] = 1; yStep[ 2 ] = 1;
						break;
					case PositiveY :
						xStep[ 0 ] = 1; yStep[ 2 ] = 1;
						break;
					case NegativeZ :
						xStep[ 0 ] = 1; yStep[ 1 ] = 1;
						break;
					case PositiveZ :
						xStep[ 0 ] = -1; yStep[ 1 ] = 1;
						break;
				}
				AssignShiftVectors( xStep, yStep );
			}

			inline void SseTerrainGenerator::AssignShiftVectors( const float* xStep, const float* zStep )
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
		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed( pop )