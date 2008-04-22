#pragma once
#include <stdlib.h>

///	\brief	Tag for placement new
struct PlacementNew { };

///	\brief	Tag for aligned new
struct Aligned
{
	const int m_Alignment;

	///	\brief	Sets the required alignment
	Aligned( int alignment ) : m_Alignment( alignment ) { }
};

///	\bref	Placement new
///
///	Usage:
///	\code
///	new ( PlacementNew(), mem ) Type( ... );
///	\endcode
inline void* operator new( const size_t numBytes, const PlacementNew&, void* mem )
{
	return mem;
}

///	\brief	Placement delete (required partner of placement new - does nothing)
inline void operator delete( void*, const PlacementNew&, void* )
{
}

///	\bref	Aligned new
///
///	Usage:
///	\code
///	new ( Aligned( 16 ) ) Type( ... );
///	\endcode
inline void* operator new( const size_t numBytes, const Aligned& aligned )
{
	return _aligned_malloc( numBytes, aligned.m_Alignment );
}

///	\brief	Aligned delete
inline void operator delete( void* mem, const Aligned& aligned )
{
	_aligned_free( mem );
}

///	\brief	Frees an object that was allocated with aligned new
template < typename T >
inline void AlignedDelete( T* obj )
{
	if ( obj != 0 )
	{
		obj->~T( );
		_aligned_free( obj );
	}
}