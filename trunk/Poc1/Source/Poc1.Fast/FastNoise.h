#pragma once
#include "Sse\SseNoise.h"

namespace Poc1
{
	namespace Fast
	{
		///	\brief	Result returned from FastNoise::Noise()
		public value struct FastNoiseResult
		{
			public :

				const float X;	///<	Noise result from first point
				const float Y;	///<	Noise result from second point
				const float Z;	///<	Noise result from third point
				const float W;	///<	Noise result from fourth point

				///	\brief	Setup constructor
				FastNoiseResult( const float x, const float y, const float z, const float w ) :
					X( x ),
					Y( y ),
					Z( z ),
					W( w )
				{
				}
		};

		///	\brief	Managed wrapper around SSE2 noise implementation
		public ref class FastNoise
		{
			public :

				
				typedef Rb::Core::Maths::Point3 Point3;
				typedef Rb::Core::Maths::Vector3 Vector3;


				///	\brief	Default constructor. Noise is seeded with value zero
				FastNoise( );

				///	\brief	Setup constructor. Noise is seeded with specified value
				FastNoise( unsigned int seed );

				///	\brief	Finalizer. Frees up unmanaged resources
				!FastNoise( );

				///	\brief	Destructor. Frees up unmanaged resources
				~FastNoise( );

				///	\brief	Fills an R8G8B8 bitmap with noise values
				inline void GenerateRgbBitmap( const int width, const int height, unsigned char* pixels, Point3^ origin, Vector3^ xAxis, Vector3^ yAxis )
				{
					const float originArr[] = { origin->X, origin->Y, origin->Z };
					const float xAxisArr[] = { xAxis->X, xAxis->Y, xAxis->Z };
					const float yAxisArr[] = { yAxis->X, yAxis->Y, yAxis->Z };
					m_pImpl->GenerateRgbBitmap( width, height, pixels, originArr, xAxisArr, yAxisArr );
				}

				///	\brief	Fills an R8G8B8 bitmap with noise values (different noise in each channel). Noise is tiled
				inline void GenerateTiledBitmap( const int width, const int height, const int stride, unsigned char* pixels, const float startX, const float startY, const float noiseWidth, const float noiseHeight )
				{
					m_pImpl->GenerateTiledBitmap( width, height, stride, pixels, startX, startY, noiseWidth, noiseHeight );
				}

				///	\brief	Generates 4 noise values from 4 3d points
				inline FastNoiseResult Noise( Point3^ pt0, Point3^ pt1, Point3^ pt2, Point3^ pt3 )
				{
					const float pt0arr[] = { pt0->X, pt0->Y, pt0->Z };
					const float pt1arr[] = { pt1->X, pt1->Y, pt1->Z };
					const float pt2arr[] = { pt2->X, pt2->Y, pt2->Z };
					const float pt3arr[] = { pt3->X, pt3->Y, pt3->Z };
					
					float res[ 4 ];
					m_pImpl->Noise( pt0arr, pt1arr, pt2arr, pt3arr, res );
					return FastNoiseResult( res[ 0 ], res[ 1 ], res[ 2 ], res[ 3 ] );
				}
				
			private :

				SseNoise* m_pImpl;
		};
	};
};
