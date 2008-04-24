#pragma once
#pragma managed( push, off )

namespace Poc1
{
	namespace Fast
	{
		struct UColour
		{
			public :

				UColour( )
				{
					m_R = 0;
					m_G = 0;
					m_B = 0;
					m_A = 0;
				}

				UColour( const unsigned char r, const unsigned char g, const unsigned char b )
				{
					m_R = r;
					m_G = g;
					m_B = b;
					m_A = 0xff;
				}

				UColour( const unsigned char r, const unsigned char g, const unsigned char b, const unsigned char a )
				{
					m_R = r;
					m_G = g;
					m_B = b;
					m_A = a;
				}

				unsigned char R( ) const { return m_R; }
				
				unsigned char G( ) const { return m_G; }
				
				unsigned char B( ) const { return m_B; }
				
				unsigned char A( ) const { return m_A; }

			private :

				union
				{
					struct
					{
						unsigned char m_R;
						unsigned char m_G;
						unsigned char m_B;
						unsigned char m_A;
					};
					unsigned int m_Bits;
					unsigned char m_Array[ 4 ];
				};
		};
	}; //Fast
}; //Poc1

#pragma managed( pop )
