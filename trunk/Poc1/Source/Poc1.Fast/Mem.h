#pragma once

struct PlacementNew { };

inline void* operator new( const size_t numBytes, const PlacementNew&, void* mem )
{
	return mem;
}

inline void operator delete( void*, const PlacementNew&, void* )
{
}

template < typename T >
inline T* AlignedNew( const int alignment )
{
	void* mem = _aligned_malloc( sizeof( T ), alignment );
	T* result = new ( PlacementNew( ), mem ) T;
	return result;
}

template < typename T, typename TP1 >
inline T* AlignedNew( const int alignment, TP1 p1 )
{
	void* mem = _aligned_malloc( sizeof( T ), alignment );
	T* result = new ( PlacementNew( ), mem ) T( p1 );
	return result;
}

template < typename T >
inline void AlignedDelete( T* obj )
{
	if ( obj != 0 )
	{
		obj->~T( );
		_aligned_free( obj );
	}
}