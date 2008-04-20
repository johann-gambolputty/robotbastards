#include "stdafx.h"
#include "FastNoise.h"
#include "SseNoise.h"

struct PlacementNew { };

inline void* operator new( const size_t numBytes, const PlacementNew&, void* mem )
{
	return mem;
}

inline void operator delete( void*, const PlacementNew&, void* )
{
}

namespace Poc1
{
	namespace Fast
	{
		FastNoise::FastNoise( )
		{
			void* mem = _aligned_malloc( sizeof( SseNoise ), 16 );
			m_pImpl = new ( PlacementNew( ), mem ) SseNoise;
		}

		FastNoise::FastNoise( unsigned int seed )
		{
			void* mem = _aligned_malloc( sizeof( SseNoise ), 16 );
			m_pImpl = new ( PlacementNew( ), mem ) SseNoise( seed );
		}

		FastNoise::~FastNoise( )
		{
			delete m_pImpl;
		}
	};
};