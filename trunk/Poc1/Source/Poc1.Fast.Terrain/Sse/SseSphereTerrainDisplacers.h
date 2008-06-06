#pragma once
#pragma managed(push, off)

#include <Sse\SseSimpleFractal.h>
#include <Sse\SseRidgedFractal.h>

#include "SseTerrainDisplacer.h"

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			//	----------------------------------------------------------------------------- Types

			///	\brief	Base class for sphere geometry displacers. Provides functions required by SseTerrainGenerator
			class SseSphereTerrainDisplacer : public SseTerrainDisplacer
			{
				public :

					inline void GetUpVector( __m128& xxxx, __m128& yyyy, __m128& zzzz )
					{
						SetLength( xxxx, yyyy, zzzz, Constants::Fc_1 );
					}

					inline void MapToDisplacementSpace( __m128& xxxx, __m128& yyyy, __m128& zzzz )
					{
						SetLength( xxxx, yyyy, zzzz, GetFunctionScale( ) );
					}

				protected :
					
					///	\brief	Returns true if the height ranges for this displacer should be divided through by the function scale
					virtual bool PreMultiplyHeightRange( ) { return true; }
			};

			///	\brief	SseSphereTerrainGenerator Displacer type. Does not perturb input positions
			class SseFlatSphereTerrainDisplacer : public SseSphereTerrainDisplacer
			{
				public :

					///	\brief	Maps 4 (x,y,z) vectors onto the minimum distance of this displacer.
					inline __m128 Displace( __m128& xxxx, __m128& yyyy, __m128& zzzz ) const
					{
						xxxx = _mm_mul_ps( xxxx, m_MinHeight );
						yyyy = _mm_mul_ps( yyyy, m_MinHeight );
						zzzz = _mm_mul_ps( zzzz, m_MinHeight );
						return _mm_set1_ps( 1 );
					}
			};

			///	\brief	Displacer decorator class. Adds x-z displacement to an existing displacer
			template < typename BaseDisplacer, typename FunctionType >
			class _CRT_ALIGN( 16 ) SseSphereFunction3dGroundDisplacer : public SseSphereTerrainDisplacer
			{
				public :

					SseSphereFunction3dGroundDisplacer( )
					{
						m_XOffset = _mm_set1_ps( 3.14f );
						m_ZOffset = _mm_set1_ps( 6.28f );
						m_Influence = _mm_set1_ps( 0.1f );
					}

					///	\brief	Sets the influence of this displacer. 0 means no influence, 1 means that offsets
					///	generated by this displacer have the same effect on ground displacement, as the base
					///	displacer has on heights
					void SetInfluence( const float influence )
					{
						m_Influence = _mm_set1_ps( influence );
					}

					///	\brief	Gets the function object used to generate ground displacement values
					FunctionType& GetFunction( )
					{
						return m_Function;
					}

					///	\brief	Gets the function object used to generate ground displacement values
					const FunctionType& GetFunction( ) const
					{
						return m_Function;
					}

					///	\brief	Gets the base displacer object
					BaseDisplacer& GetBaseDisplacer( )
					{
						return m_Base;
					}

					///	\brief	Gets the base displacer object				
					const BaseDisplacer& GetBaseDisplacer( ) const
					{
						return m_Base;
					}

					///	\brief	Sets up this function object
					virtual void Setup( float patchScale, float minHeight, float seaLevel, float maxHeight, float sphereRadius )
					{
						SseTerrainDisplacer::Setup( patchScale, minHeight, seaLevel, maxHeight, sphereRadius );
						m_Base.Setup( patchScale, minHeight, seaLevel, maxHeight, sphereRadius );
					}
					
					///	\brief	Maps 4 (x,y,z) vectors onto the minimum distance of this displacer.
					inline __m128 Displace( __m128& xxxx, __m128& yyyy, __m128& zzzz ) const
					{
						__m128 dispXxxx = m_Function.GetSignedValue( xxxx, yyyy, zzzz );
						__m128 dispZzzz = m_Function.GetSignedValue( _mm_add_ps( xxxx, m_XOffset ), yyyy, _mm_add_ps( zzzz, m_ZOffset ) );
						dispXxxx = _mm_mul_ps( dispXxxx, m_Influence );
						dispZzzz = _mm_mul_ps( dispZzzz, m_Influence );

						__m128 xAxisX, xAxisY, xAxisZ;
						__m128 yAxisX, yAxisY, yAxisZ;
						__m128 zAxisX, zAxisY, zAxisZ;
						yAxisX = xxxx; yAxisY = yyyy; yAxisZ = zzzz;
						GetCrossProducts( xAxisX, xAxisY, xAxisZ, xxxx, yyyy, zzzz, Constants::Fc_0, Constants::Fc_1, Constants::Fc_0 );
						GetCrossProducts( zAxisX, zAxisY, zAxisZ, xxxx, yyyy, zzzz, xAxisX, xAxisY, xAxisZ );
						SetLength( xAxisX, xAxisY, xAxisZ, dispXxxx );
						SetLength( yAxisX, yAxisY, yAxisZ, dispZzzz );
						SetLength( zAxisX, zAxisY, zAxisZ, dispZzzz );

						xxxx = _mm_add_ps( xxxx, xAxisX );
						yyyy = _mm_add_ps( yyyy, xAxisY );
						zzzz = _mm_add_ps( zzzz, xAxisZ );
						
						xxxx = _mm_add_ps( xxxx, yAxisX );
						yyyy = _mm_add_ps( yyyy, yAxisY );
						zzzz = _mm_add_ps( zzzz, yAxisZ );
						
						xxxx = _mm_add_ps( xxxx, zAxisX );
						yyyy = _mm_add_ps( yyyy, zAxisY );
						zzzz = _mm_add_ps( zzzz, zAxisZ );
						
					//	xxxx = _mm_add_ps( xxxx, dispXxxx );
					//	zzzz = _mm_add_ps( zzzz, dispZzzz );

						__m128 heights = m_Base.Displace( xxxx, yyyy, zzzz );
						return heights;
					}

				private :

					__m128 m_XOffset;
					__m128 m_ZOffset;
					__m128 m_Influence;
					_CRT_ALIGN( 16 ) BaseDisplacer m_Base;
					_CRT_ALIGN( 16 ) FunctionType m_Function;

			}; //SseFractalOffsetDisplacer


			///	\brief	SseSphereTerrainGenerator Displacer type. Uses a function taking 4 3d input vectors to generate 4 heights
			template < typename FunctionType >
			class _CRT_ALIGN( 16 ) SseSphereFunction3dDisplacer : public SseSphereTerrainDisplacer
			{
				public :

					///	\brief	Gets the function object
					FunctionType& GetFunction( )
					{
						return m_Function;
					}
					
					///	\brief	Gets the function object
					const FunctionType& GetFunction( ) const
					{
						return m_Function;
					}

					///	\brief	Maps 4 (x,y,z) vectors onto the minimum distance of this displacer.
					inline __m128 Displace( __m128& xxxx, __m128& yyyy, __m128& zzzz ) const
					{
						__m128 heights = m_Function.GetValue( xxxx, yyyy, zzzz );

						__m128 actualHeights = MapToHeightRange( heights );
						xxxx = _mm_mul_ps( xxxx, actualHeights );
						yyyy = _mm_mul_ps( yyyy, actualHeights );
						zzzz = _mm_mul_ps( zzzz, actualHeights );
						return heights;
					}

				private :

					_CRT_ALIGN( 16 ) FunctionType m_Function;
			};
			
			///	\brief	Helper class
			template < typename GroundFunction, typename HeightFunction >
			class SseSphereFull3dDisplacer : public SseSphereFunction3dGroundDisplacer< SseSphereFunction3dDisplacer< HeightFunction >, GroundFunction >
			{
			};
		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed(pop)
