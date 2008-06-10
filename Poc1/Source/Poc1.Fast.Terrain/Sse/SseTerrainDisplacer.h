#pragma once
#include <emmintrin.h>

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			///	\brief	Base class for terrain displacers
			class SseTerrainDisplacer
			{
				public :

					///	\brief	Sets up reasonable defaults
					SseTerrainDisplacer( )
					{
						SetFunctionScale( 6.0f );
						Setup( 64, 1.0f, 3.0f );
					}

					///	\brief Gets the scale of the displacement function input (e.g. sphere radius)
					const __m128& GetFunctionScale( ) const
					{
						return m_Scale;
					}
					
					///	\brief Gets the minimum height generated by this displacer
					const __m128& GetMinimumHeight( ) const
					{
						return m_MinHeight;
					}

					///	\brief	Changes the function scale. Must be called before Setup(), for premultiplied displacers
					void SetFunctionScale( float functionScale )
					{
						m_ScaleF = functionScale;
						m_Scale = _mm_set1_ps( functionScale );
					}

					///	\brief	Sets the output scale
					void SetOutputScale( float outputScale )
					{
						m_OutputScale = _mm_set1_ps( outputScale );
					}

					///	\brief	Sets up this displacer. SetFunctionScale() must be called first or pre-multiplied displacers
					virtual void Setup( float patchScale, float minHeight, float maxHeight )
					{
						//	TODO: AP: Try pre-multiplying the height range by output scale... would save a multiply
						m_PatchScaleToFunctionScale = _mm_div_ps( m_Scale, _mm_set1_ps( patchScale ) );

						m_MinHeight = _mm_set1_ps( minHeight );
						m_MaxHeight = _mm_set1_ps( maxHeight );
						m_HeightRange = _mm_sub_ps( m_MaxHeight, m_MinHeight );

						if ( PreMultiplyHeightRange( ) )
						{
							m_HeightRange = _mm_div_ps( m_HeightRange, m_Scale );
							m_MinHeight = _mm_div_ps( m_MinHeight, m_Scale );
							m_MaxHeight = _mm_div_ps( m_MaxHeight, m_Scale );
						}

						m_MinHeightF = minHeight;
						m_HeightRangeF = maxHeight - minHeight;
					}

					///	\brief	Maps 4 height values into the height range of this displacer
					__m128 MapToHeightRange( const __m128 heights ) const
					{
						return _mm_add_ps( m_MinHeight, _mm_mul_ps( heights, m_HeightRange ) );
					}

					///	\brief	Maps a single normalized height value into the height scale of this displacer (does not add minimum height)
					float MapToHeightScale( float height ) const
					{
						return m_HeightRangeF * height;
					}

				protected :

					float m_ScaleF;
					float m_MinHeightF;
					float m_HeightRangeF;
					__m128 m_OutputScale;
					__m128 m_Scale;
					__m128 m_PatchScaleToFunctionScale;
					__m128 m_MinHeight;
					__m128 m_MaxHeight;
					__m128 m_HeightRange;

					///	\brief	Returns true if the height ranges for this displacer should be divided through by the function scale
					///
					///	This is handy for sphere geometries, because the (x,y,z) patch positions can be projected
					///	directly onto the function sphere, then displaced back onto the target sphere by the
					///	height range mapping.
					///
					virtual bool PreMultiplyHeightRange( ) { return false; }
			};

		};
	};
};