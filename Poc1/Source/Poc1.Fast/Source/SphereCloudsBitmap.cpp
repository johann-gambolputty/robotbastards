#include "StdAfx.h"
#include "SphereCloudsBitmap.h"
#include "Sse/SseRidgedFractal.h"
#include "Mem.h"
#include "UEnums.h"

#pragma unmanaged

namespace Poc1
{
	namespace Fast
	{

		class _CRT_ALIGN( 16 ) SphereCloudsBitmapImpl
		{
			public :

				SphereCloudsBitmapImpl( )
				{
					m_Gen.Setup( 3.5f, 1.6f, 8 );
				}

				void Setup( const float xOffset, const float zOffset, const float cloudCutoff, const float cloudBorder )
				{
					m_XOffset = _mm_set1_ps( xOffset );
					m_ZOffset = _mm_set1_ps( zOffset );
					m_CloudCutoff = _mm_mul_ps( _mm_set1_ps( cloudCutoff ), _mm_set1_ps( 255.0f ) );
					m_CloudBorder = _mm_mul_ps( _mm_set1_ps( cloudBorder ), _mm_set1_ps( 255.0f ) );
					m_CloudBorderDiff = _mm_div_ps( _mm_set1_ps( 255.0f ), _mm_sub_ps( m_CloudBorder, m_CloudCutoff ) );
				}

				void GenerateCloudsFace( const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels );

			private :

				__m128 m_XOffset;
				__m128 m_ZOffset;
				__m128 m_CloudCutoff;
				__m128 m_CloudBorder;
				__m128 m_CloudBorderDiff;
				SseRidgedFractal m_Gen;

		};

		inline __m128 CubeFaceFractal( const SseRidgedFractal& fractal, const UCubeMapFace face, const __m128& uuuu, const __m128& vvvv, const __m128& xOffset, const __m128& zOffset )
		{
			__m128 xxxx, yyyy, zzzz;
			CubeFacePosition( face, uuuu, vvvv, xxxx, yyyy, zzzz );
			SetLength( xxxx, yyyy, zzzz, 3.0f );
			return fractal.GetValue( _mm_add_ps( xxxx, xOffset ), yyyy, _mm_add_ps( zzzz, zOffset ) );
		}

		void SphereCloudsBitmapImpl::GenerateCloudsFace( const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels )
		{
			float fRes = 2.0f;
			float hfRes = fRes / 2;

			float incU = fRes / float( width - 1 );
			float incV = fRes / float( height - 1 );
			__m128 vvvv = _mm_set1_ps( -hfRes );
			__m128 vvvvInc = _mm_set1_ps( incV );

			__m128 uuuuStart = _mm_add_ps( _mm_set1_ps( -hfRes ), _mm_set_ps( 0, incU, incU * 2, incU * 3 ) );
			__m128 uuuuInc = _mm_set1_ps( incU * 4 );

			_CRT_ALIGN( 16 ) float res[ 4 ] = { 0, 0, 0, 0 };
			_CRT_ALIGN( 16 ) float alphaValues[ 4 ] = { 0, 0, 0, 0 };

			int width4 = width / 4;
			unsigned char* rowPixel = pixels;
			for ( int row = 0; row < height; ++row )
			{
				unsigned char* curPixel = rowPixel;
				__m128 uuuu = uuuuStart;
				for ( int col = 0; col < width4; ++col )
				{
					//	TODO: AP: Could move face switch to outer loop
					__m128 value = CubeFaceFractal( m_Gen, face, uuuu, vvvv, m_XOffset, m_ZOffset );
					value = _mm_mul_ps( _mm_mul_ps( value, value ), _mm_set1_ps( 255.0f ) );
					__m128 cutMask = _mm_cmpgt_ps( value, m_CloudCutoff );
					value = _mm_and_ps( cutMask, value );

					_mm_store_ps( res, value );

					unsigned char b0 = ( unsigned char )( res[ 3 ] );
					unsigned char b1 = ( unsigned char )( res[ 2 ] );
					unsigned char b2 = ( unsigned char )( res[ 1 ] );
					unsigned char b3 = ( unsigned char )( res[ 0 ] );

					switch ( format )
					{
						case FormatR8G8B8A8 :
							{
								const __m128 borderMask = _mm_cmpgt_ps( value, m_CloudBorder );
								const __m128 fadeValue = _mm_mul_ps( _mm_sub_ps( value, m_CloudCutoff ), m_CloudBorderDiff );
								__m128 alpha = _mm_or_ps( _mm_and_ps( borderMask, _mm_set1_ps( 255.0f ) ), _mm_andnot_ps( borderMask, fadeValue ) );
								alpha = _mm_and_ps( cutMask, alpha );
								_mm_store_ps( alphaValues, alpha );
								unsigned char a0 = ( unsigned char )( alphaValues[ 3 ] );
								unsigned char a1 = ( unsigned char )( alphaValues[ 2 ] );
								unsigned char a2 = ( unsigned char )( alphaValues[ 1 ] );
								unsigned char a3 = ( unsigned char )( alphaValues[ 0 ] );

								curPixel[ 0 ] = curPixel[ 1 ] = curPixel[ 2 ] = b0; curPixel[ 3 ] = a0;
								curPixel[ 4 ] = curPixel[ 5 ] = curPixel[ 6 ] = b1; curPixel[ 7 ] = a1;
								curPixel[ 8 ] = curPixel[ 9 ] = curPixel[ 10 ] = b2; curPixel[ 11 ] = a2;
								curPixel[ 12 ] = curPixel[ 13 ] = curPixel[ 14 ] = b3; curPixel[ 15 ] = a3;
								curPixel += 16;
								break;
							};

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

	}; //Fast
}; //Poc1

#pragma managed
using namespace Rb::Rendering::Interfaces::Objects;

namespace Poc1
{
	namespace Fast
	{
		SphereCloudsBitmap::SphereCloudsBitmap( )
		{
			m_pImpl = new ( Aligned( 16 ) ) SphereCloudsBitmapImpl;
		}

		SphereCloudsBitmap::~SphereCloudsBitmap( )
		{
			AlignedDelete( m_pImpl );
		}

		SphereCloudsBitmap::!SphereCloudsBitmap( )
		{
			AlignedDelete( m_pImpl );
		}

		void SphereCloudsBitmap::Setup( float xOffset, float zOffset, float cloudCutoff, float cloudBorder )
		{
			m_pImpl->Setup( xOffset, zOffset, cloudCutoff, cloudBorder );
		}

		void SphereCloudsBitmap::GenerateFace( CubeMapFace face, PixelFormat format, const int width, const int height, const int stride, unsigned char* pixels )
		{
			m_pImpl->GenerateCloudsFace( GetUCubeMapFace( face ), GetUPixelFormat( format ), width, height, stride, pixels );
		}

	}; //Fast
}; //Poc1