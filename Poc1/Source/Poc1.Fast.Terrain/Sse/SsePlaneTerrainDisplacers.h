#pragma once
#pragma managed( push, off )

#include "SseTerrainDisplacer.h"

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			///	\brief	Base class for plane geometry displacers. Provides functions required by SseTerrainGenerator
			class SsePlaneTerrainDisplacer : public SseTerrainDisplacer
			{
				public :

					inline void GetUpVector( __m128& xxxx, __m128& yyyy, __m128& zzzz )
					{
						xxxx = Constants::Fc_0;
						yyyy = Constants::Fc_1;
						zzzz = Constants::Fc_0;
					}

					inline void MapToDisplacementSpace( __m128& xxxx, __m128& yyyy, __m128& zzzz )
					{
					}
			};

			///	\brief	Displaces heights to the minimum height, for planar geometry
			class SseFlatPlaneTerrainDisplacer : public SsePlaneTerrainDisplacer
			{
				public :

					///	\brief	Maps 4 (x,y,z) vectors onto the minimum distance of this displacer.
					inline __m128 Displace( __m128& xxxx, __m128& yyyy, __m128& zzzz ) const
					{
						yyyy = _mm_add_ps( yyyy, m_MinHeight );	//	TODO: AP: Take into account function scale, etc.
						return _mm_set1_ps( 0 );
					}
			}; //SseFlatPlaneTerrainDisplacer
			
			
			///	\brief	Displacer decorator class. Adds x-z displacement to an existing displacer
			template < typename BaseDisplacer, typename FunctionType >
			class _CRT_ALIGN( 16 ) SsePlaneFunction3dGroundDisplacer : public SsePlaneTerrainDisplacer
			{
				public :

					SsePlaneFunction3dGroundDisplacer( )
					{
						m_XOffset = _mm_set1_ps( 3.14f );
						m_ZOffset = _mm_set1_ps( 6.28f );
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
					virtual void Setup( float patchScale, float minHeight, float maxHeight )
					{
						SseTerrainDisplacer::Setup( patchScale, minHeight, maxHeight );

						//	Multiply output scale by height range
						m_OutputScale = _mm_mul_ps( m_OutputScale, m_HeightRange );

						//	Add scale to offsets, so x-z values are always > 0 (removes distinctive pattern in noise)
						m_XOffset = _mm_add_ps( m_XOffset, m_Scale );
						m_ZOffset = _mm_add_ps( m_ZOffset, m_Scale );
						m_Base.Setup( patchScale, minHeight, maxHeight );
					}
					
					///	\brief	Maps 4 (x,y,z) vectors onto the minimum distance of this displacer.
					inline __m128 Displace( __m128& xxxx, __m128& yyyy, __m128& zzzz ) const
					{
						__m128 fXxxx = _mm_mul_ps( xxxx, m_PatchScaleToFunctionScale );
						__m128 fYyyy = _mm_mul_ps( yyyy, m_PatchScaleToFunctionScale );
						__m128 fZzzz = _mm_mul_ps( zzzz, m_PatchScaleToFunctionScale );
						__m128 dispXxxx = m_Function.GetSignedValue( fXxxx, fYyyy, fZzzz );
						__m128 dispZzzz = m_Function.GetSignedValue( _mm_add_ps( fXxxx, m_XOffset ), fYyyy, _mm_add_ps( fZzzz, m_ZOffset ) );
						dispXxxx = _mm_mul_ps( dispXxxx, m_OutputScale );
						dispZzzz = _mm_mul_ps( dispZzzz, m_OutputScale );

						__m128 magnitudes = GetLengths( dispXxxx, Constants::Fc_0, dispZzzz );

						xxxx = _mm_add_ps( xxxx, dispXxxx );
						yyyy = _mm_add_ps( yyyy, magnitudes );
						zzzz = _mm_add_ps( zzzz, dispZzzz );

						return m_Base.Displace( xxxx, yyyy, zzzz );
					}

				private :

					__m128 m_XOffset;
					__m128 m_ZOffset;
					_CRT_ALIGN( 16 ) BaseDisplacer m_Base;
					_CRT_ALIGN( 16 ) FunctionType m_Function;

			}; //SseFractalOffsetDisplacer


			///	\brief	SsePlaneTerrainGenerator Displacer type. Uses a function taking 4 3d input vectors to generate 4 heights
			template < typename FunctionType >
			class _CRT_ALIGN( 16 ) SsePlaneFunction3dDisplacer : public SsePlaneTerrainDisplacer
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
						//	TODO: AP: This gets done twice if there's a ground displacer decorating this height displacer
						__m128 fXxxx = _mm_add_ps( _mm_mul_ps( xxxx, m_PatchScaleToFunctionScale ), m_Scale );
						__m128 fYyyy = _mm_add_ps( _mm_mul_ps( yyyy, m_PatchScaleToFunctionScale ), m_Scale );
						__m128 fZzzz = _mm_add_ps( _mm_mul_ps( zzzz, m_PatchScaleToFunctionScale ), m_Scale );
						__m128 heights = m_Function.GetValue( fXxxx, fYyyy, fZzzz );
						yyyy = _mm_add_ps( yyyy, _mm_mul_ps( MapToHeightRange( heights ), m_OutputScale ) );
						return heights;
					}

				private :

					_CRT_ALIGN( 16 ) FunctionType m_Function;
			};


		}; //Terrain
	}; //Fast
}; //Poc1

#pragma managed( pop )