#include "StdAfx.h"
#include "SphereCloudsBitmap.h"
#include "SseRidgedFractal.h"
#include "Mem.h"

#pragma unmanaged

namespace Poc1
{
	namespace Fast
	{
		///	\brief	Unmanaged cube map faces. MUST MATCH values in CubeMapFace
		enum UCubeMapFace
		{
			NegativeX,
			PositiveX,
			NegativeY,
			PositiveY,
			NegativeZ,
			PositiveZ
		};
		
		///	\brief	Unmanaged pixel formats
		enum UPixelFormat
		{
			FormatR8G8B8,
			FormatR8G8B8A8,
		};

		class _CRT_ALIGN( 16 ) SphereCloudsBitmapImpl
		{
			public :

				SphereCloudsBitmapImpl( )
				{
					m_Gen.Setup( 1.8f, 1.6f, 8 );
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
			//	TODO: AP: Normalize (x,y,z)?
			__m128 xxxx, yyyy, zzzz;
			switch ( face )
			{
				default:
				case NegativeX:
				//	x = -1; y = v; z = u;
					xxxx = Constants::Fc_Neg1;
					yyyy = vvvv;
					zzzz = uuuu;
					break;
				case PositiveX:
				//	x = 1; y = v; z = -u;
					xxxx = Constants::Fc_1;
					yyyy = vvvv;
					zzzz = Neg( uuuu );
					break;
				case NegativeY:
					//x = -u; y = -1; z = -v;
					xxxx = Neg( uuuu );
					yyyy = Constants::Fc_Neg1;
					zzzz = Neg( vvvv );
					break;
				case PositiveY:
				//	x = -u; y = 1; z = v;
					xxxx = Neg( uuuu );
					yyyy = Constants::Fc_1;
					zzzz = vvvv;
					break;
				case NegativeZ:
				//	x = -u; y = v; z = -1;
					xxxx = Neg( uuuu );
					yyyy = vvvv;
					zzzz = Constants::Fc_Neg1;
					break;
				case PositiveZ:
					//x = u; y = v; z = 1;
					xxxx = uuuu;
					yyyy = vvvv;
					zzzz = Constants::Fc_1;
					break;
			};

			Normalize( xxxx, yyyy, zzzz );
			return fractal.GetValue( _mm_add_ps( xxxx, xOffset ), yyyy, _mm_add_ps( zzzz, zOffset ) );
		}

		void SphereCloudsBitmapImpl::GenerateCloudsFace( const UCubeMapFace face, const UPixelFormat format, const int width, const int height, const int stride, unsigned char* pixels )
		{
			const int a = 0;
			const int r = 1;
			const int g = 2;
			const int b = 3;


			float incU = 2.0f / float( width - 1 );
			float incV = 2.0f / float( height - 1 );
			__m128 vvvv = _mm_set1_ps( -1 );
			__m128 vvvvInc = _mm_set1_ps( incV );

			__m128 uuuuStart = _mm_add_ps( _mm_set1_ps( -1 ), _mm_set_ps( 0, incU, incU * 2, incU * 3 ) );
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
					__m128 value = _mm_mul_ps( CubeFaceFractal( m_Gen, face, uuuu, vvvv, m_XOffset, m_ZOffset ), _mm_set1_ps( 255.0f ) );
					
					__m128 cutMask = _mm_cmpgt_ps( value, m_CloudCutoff );
					__m128 borderMask = _mm_cmpgt_ps( value, m_CloudBorder );
					__m128 fadeValue = _mm_mul_ps( _mm_sub_ps( value, m_CloudCutoff ), m_CloudBorderDiff );
					__m128 alpha = _mm_or_ps( _mm_and_ps( borderMask, _mm_set1_ps( 255.0f ) ), _mm_andnot_ps( borderMask, fadeValue ) );
					value = _mm_and_ps( cutMask, value );
					alpha = _mm_and_ps( cutMask, alpha );

					_mm_store_ps( res, value );
					_mm_store_ps( alphaValues, alpha );

					unsigned char b0 = ( unsigned char )( res[ 3 ] );
					unsigned char b1 = ( unsigned char )( res[ 2 ] );
					unsigned char b2 = ( unsigned char )( res[ 1 ] );
					unsigned char b3 = ( unsigned char )( res[ 0 ] );
					
					unsigned char a0 = ( unsigned char )( alphaValues[ 3 ] );
					unsigned char a1 = ( unsigned char )( alphaValues[ 2 ] );
					unsigned char a2 = ( unsigned char )( alphaValues[ 1 ] );
					unsigned char a3 = ( unsigned char )( alphaValues[ 0 ] );

					//	TODO: AP: Move switch to outer loop
					switch ( format )
					{
						case FormatR8G8B8A8 :
							curPixel[ 0 ] = curPixel[ 1 ] = curPixel[ 2 ] = a0; curPixel[ 3 ] = a0;
							curPixel[ 4 ] = curPixel[ 5 ] = curPixel[ 6 ] = a1; curPixel[ 7 ] = a1;
							curPixel[ 8 ] = curPixel[ 9 ] = curPixel[ 10 ] = a2; curPixel[ 11 ] = a2;
							curPixel[ 12 ] = curPixel[ 13 ] = curPixel[ 14 ] = a3; curPixel[ 15 ] = a3;
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
			/*
			Fractals.Basis3dFunction basis = m_Noise.GetNoise;

			float xOffset = Functions.Sin( m_XOffset );
			float zOffset = Functions.Cos( m_ZOffset );
			float density = 0.3f + Functions.Cos( m_CloudCoverage ) * 0.2f;
			float cloudCut = density;
			float cloudBorder = cloudCut + 0.2f;
			float incU = 2.0f / ( width - 1 );
			float incV = 2.0f / ( height - 1 );
			float v = -1;
			for ( int row = 0; row < height; ++row, v += incV )
			{
				float u = -1;
				byte* curPixel = pixels + row * stride;
				for ( int col = 0; col < width; ++col, u += incU )
				{
					float x, y, z;
					SphereTerrainGenerator.UvToXyz( u, v, face, out x, out y, out z );

					float val = Fractals.RidgedFractal( x + xOffset, y, z + zOffset, 1.8f, 8, 1.6f, basis );
					float alpha = 0;
					if ( val < cloudCut )
					{
						val = 0;
					}
					else
					{
						alpha = val < cloudBorder ? ( val - cloudCut ) / ( cloudBorder - cloudCut ) : 1.0f;
					}

					byte colour = ( byte )( val * 255.0f );
					curPixel[ 0 ] = colour;
					curPixel[ 1 ] = colour;
					curPixel[ 2 ] = colour;
					curPixel[ 3 ] = ( byte )( alpha * 255.0f );

					curPixel += 4;
				}

			}
			*/
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
		//	m_pImpl = AlignedNew< SseRidgedFractal >( 16 );
		//	m_pImpl->Setup( 1.8f, 1.6f, 8 );
			m_pImpl = AlignedNew< SphereCloudsBitmapImpl >( 16 );
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
			UPixelFormat uFormat;
			switch ( format )
			{
				case PixelFormat::Format32bppArgb : uFormat = FormatR8G8B8A8; break;
				case PixelFormat::Format24bppRgb : uFormat = FormatR8G8B8; break;
			default :
				throw gcnew System::ArgumentException( "Unhandled pixel format", "format" );
			}

			m_pImpl->GenerateCloudsFace( ( UCubeMapFace )face, uFormat, width, height, stride, pixels );
		}

	}; //Fast
}; //Poc1