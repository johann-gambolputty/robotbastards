#pragma once
#pragma managed( push, off )

#include "SseUtils.h"

namespace Poc1
{
	namespace Fast
	{
		enum XAxis { kXAxis };
		enum YAxis { kYAxis };
		enum ZAxis { kZAxis };


		///	\brief	A group of 4 vectors
		class _CRT_ALIGN( 16 ) SseVectorGroup
		{

			public :

				///	\brief	Default constructor
				SseVectorGroup()
				{
				}
				
				///	\brief	Adds a vector group to this vector group in-place
				void Add( const SseVectorGroup& vecGroup )
				{
					m_Xxxx = _mm_add_ps( m_Xxxx, vecGroup.m_Xxxx );
					m_Yyyy = _mm_add_ps( m_Yyyy, vecGroup.m_Yyyy );
					m_Zzzz = _mm_add_ps( m_Zzzz, vecGroup.m_Zzzz );
				}

				///	\brief	In-place normalization of the vectors
				void Normalize( )
				{
					Normalize( m_Xxxx, m_Yyyy, m_Zzzz );
				}

				///	\brief	Gets the X components of the group
				const __m128& Xxxx( ) const { return m_Xxxx; }

				///	\brief	Sets the X components of the group
				void Xxxx( const __m128& xxxx ) { m_Xxxx = xxxx; }
				
				///	\brief	Gets the Y components of the group
				const __m128& Yyyy( ) const { return m_Yyyy; }
				
				///	\brief	Sets the Y components of the group
				void Yyyy( const __m128& yyyy ) { m_Yyyy = yyyy; }

				///	\brief	Gets the Z components of the group
				const __m128& Zzzz( ) const { return m_Zzzz; }
				
				///	\brief	Sets the Z components of the group
				void Zzzz( const __m128& zzzz ) { m_Zzzz = zzzz; }

			private :

				__m128 m_Xxxx;
				__m128 m_Yyyy;
				__m128 m_Zzzz;

		}; //SseVectorGroup

	}; //Fast
}; //Poc1