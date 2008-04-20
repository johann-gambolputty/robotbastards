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

		inline __m128 CubeFaceFractal( const SseRidgedFractal& fractal, const UCubeMapFace face, const __m128& uuuu, const __m128& vvvv )
		{
			//	TODO: AP: Normalize (x,y,z)?
			switch ( face )
			{
				default:
				case NegativeX:
				//	x = -1; y = v; z = u;
					return fractal.GetValue( Constants::Fc_Neg1, vvvv, uuuu );
				case PositiveX:
				//	x = 1; y = v; z = -u;
					return fractal.GetValue( Constants::Fc_1, vvvv, Neg( uuuu ) );
				case NegativeY:
					//x = -u; y = -1; z = -v;
					return fractal.GetValue( Neg( uuuu ), Constants::Fc_0, Neg( vvvv ) );
				case PositiveY:
				//	x = -u; y = 1; z = v;
					return fractal.GetValue( Neg( uuuu ), Constants::Fc_1, vvvv );
				case NegativeZ:
				//	x = -u; y = v; z = -1;
					return fractal.GetValue( Neg( uuuu ), vvvv, Constants::Fc_Neg1 );
				case PositiveZ:
					//x = u; y = v; z = 1;
					return fractal.GetValue( uuuu, vvvv, Constants::Fc_1 );
			};
		}

		static void GenerateCloudsFace( const SseRidgedFractal& fractal, const UCubeMapFace face, const int width, const int height, const int stride, unsigned char* pixels )
		{

			float incU = 1.0f / float( height );
			float incV = 1.0f / float( height );
			__m128 vvvv = _mm_add_ps( _mm_set1_ps( -1 ), _mm_set_ps( 0, incV, incV * 2, incV * 3 ) );
			__m128 vvvvInc = _mm_set1_ps( incV * 4 );

			__m128 uuuuStart = _mm_add_ps( _mm_set1_ps( -1 ), _mm_set_ps( 0, incV, incV * 2, incV * 3 ) );
			__m128 uuuuInc = _mm_set1_ps( incU * 4 );

			_CRT_ALIGN( 16 ) float res[ 4 ] = { 0, 0, 0, 0 };

			unsigned char* rowPixel = pixels;
			for ( int row = 0; row < height; ++row )
			{
				unsigned char* curPixel = rowPixel;
				__m128 uuuu = uuuuStart;
				for ( int col = 0; col < width; ++col )
				{
					const __m128 value = CubeFaceFractal( fractal, face, uuuu, vvvv );

					_mm_store_ps( res, value );
					unsigned char b0 = ( unsigned char )( res[ 3 ] );
					unsigned char b1 = ( unsigned char )( res[ 2 ] );
					unsigned char b2 = ( unsigned char )( res[ 1 ] );
					unsigned char b3 = ( unsigned char )( res[ 0 ] );

					//	TODO: AP: Handle pixel format

					curPixel[ 0 ] = curPixel[ 1 ] = curPixel[ 2 ] = curPixel[ 3 ] = b0;
					curPixel[ 4 ] = curPixel[ 5 ] = curPixel[ 6 ] = curPixel[ 7 ] = b1;
					curPixel[ 8 ] = curPixel[ 9 ] = curPixel[ 10 ] = curPixel[ 11 ] = b2;
					curPixel[ 12 ] = curPixel[ 13 ] = curPixel[ 14 ] = curPixel[ 15 ] = b3;

					curPixel += 16;

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
			m_pImpl = AlignedNew< SseRidgedFractal >( 16 );
		}

		SphereCloudsBitmap::~SphereCloudsBitmap( )
		{
			AlignedDelete( m_pImpl );
		}
		
		SphereCloudsBitmap::!SphereCloudsBitmap( )
		{
			AlignedDelete( m_pImpl );
		}

	//	inline static __m128 

		void SphereCloudsBitmap::GenerateFace( CubeMapFace face, PixelFormat format, const int width, const int height, const int stride, unsigned char* pixels )
		{
			GenerateCloudsFace( *m_pImpl, ( UCubeMapFace )face, width, height, stride, pixels );
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