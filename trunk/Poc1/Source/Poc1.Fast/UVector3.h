#pragma once
#pragma managed( push, off )

#include <math.h>

namespace Poc1
{
	namespace Fast
	{
		class UVector3
		{
			public :

				static const UVector3 XAxis;
				static const UVector3 YAxis;
				static const UVector3 ZAxis;
				
				float m_X, m_Y, m_Z;

				UVector3( ) :
					m_X( 0 ), m_Y( 0 ), m_Z( 0 )
				{
				}

				UVector3( const float* src ) :
					m_X( src[ 0 ] ), m_Y( src[ 1 ] ), m_Z( src[ 2 ] )
				{
				}

				UVector3( const UVector3& src ) :
					m_X( src.m_X ), m_Y( src.m_Y ), m_Z( src.m_Z )
				{
				}

				UVector3( const float x, const float y, const float z ) :
					m_X( x ), m_Y( y ), m_Z( z )
				{
				}

				void Add( const UVector3& src )
				{
					m_X += src.m_X;
					m_Y += src.m_Y;
					m_Z += src.m_Z;
				}

				void Sub( const UVector3& src )
				{
					m_X -= src.m_X;
					m_Y -= src.m_Y;
					m_Z -= src.m_Z;
				}

				void Sub( const float* point )
				{
					m_X -= point[ 0 ];
					m_Y -= point[ 1 ];
					m_Z -= point[ 2 ];
				}

				void Normalise( )
				{
					const float rcpLen = 1.0f / sqrtf( m_X * m_X + m_Y * m_Y + m_Z * m_Z );
					m_X *= rcpLen;
					m_Y *= rcpLen;
					m_Z *= rcpLen;
				}

				static float Dot( const UVector3& lhs, const UVector3& rhs )
				{
					return ( lhs.m_X * rhs.m_X + lhs.m_Y * rhs.m_Y + lhs.m_Z * rhs.m_Z ); 
				}

				static const UVector3 Cross( const UVector3& lhs, const UVector3& rhs )
				{
					const float x = ( lhs.m_Y * rhs.m_Z ) - ( lhs.m_Z * rhs.m_Y );
					const float y = ( lhs.m_Z * rhs.m_X ) - ( lhs.m_X * rhs.m_Z );
					const float z = ( lhs.m_X * rhs.m_Y ) - ( lhs.m_Y * rhs.m_X );

					return UVector3( x, y, z );
				}
		};
	};
};

#pragma managed( pop )
