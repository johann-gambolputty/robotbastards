#pragma once
#include "SseNoise.h"

namespace Poc1
{
	namespace Fast
	{
		public value struct FastNoiseResult
		{
			public :

				const float X;
				const float Y;
				const float Z;
				const float W;

				FastNoiseResult( const float x, const float y, const float z, const float w ) :
					X( x ),
					Y( y ),
					Z( z ),
					W( w )
				{
				}


		};

		//	Fast noise implementation
		public ref class FastNoise
		{
			public :

				typedef Rb::Core::Maths::Point3 Point3;

				FastNoise( );

				FastNoise( unsigned int seed );

				~FastNoise( );

				inline void GenerateRgbBitmap( const int width,
					const int height,
					unsigned char* pixels,
					Rb::Core::Maths::Point3^ origin, Rb::Core::Maths::Vector3^ incCol, Rb::Core::Maths::Vector3^ incRow )
				{
					const float originArr[] = { origin->X, origin->Y, origin->Z };
					const float incColArr[] = { incCol->X, incCol->Y, incCol->Z };
					const float incRowArr[] = { incRow->X, incRow->Y, incRow->Z };

				//	m_pImpl->GenerateRgbBitmap( width, height, pixels, originArr, incColArr, incRowArr );
				//	m_pImpl->GenerateRgbSimpleFractal( width, height, pixels, originArr, incColArr, incRowArr );
					m_pImpl->GenerateRgbRidgedFractal( width, height, pixels, originArr, incColArr, incRowArr );
				}

				inline FastNoiseResult Noise( Rb::Core::Maths::Point3^ pt0, Rb::Core::Maths::Point3^ pt1, Rb::Core::Maths::Point3^ pt2, Rb::Core::Maths::Point3^ pt3 )
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
