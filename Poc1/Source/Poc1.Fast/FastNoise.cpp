#include "stdafx.h"
#include "FastNoise.h"
#include "SseNoise.h"
#include "Mem.h"

namespace Poc1
{
	namespace Fast
	{
		FastNoise::FastNoise( )
		{
			m_pImpl = AlignedNew< SseNoise >( 16 );
		}

		FastNoise::FastNoise( unsigned int seed )
		{
			m_pImpl = AlignedNew< SseNoise >( 16, seed );
		}

		FastNoise::!FastNoise( )
		{
			AlignedDelete( m_pImpl );
			m_pImpl = 0;
		}

		FastNoise::~FastNoise( )
		{
			AlignedDelete( m_pImpl );
			m_pImpl = 0;
		}
	};
};