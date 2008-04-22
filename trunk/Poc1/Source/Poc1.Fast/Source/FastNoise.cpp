#include "stdafx.h"
#include "FastNoise.h"
#include "Mem.h"

namespace Poc1
{
	namespace Fast
	{
		FastNoise::FastNoise( )
		{
			m_pImpl = new ( Aligned( 16 ) ) SseNoise;
		}

		FastNoise::FastNoise( unsigned int seed )
		{
			m_pImpl = new ( Aligned( 16 ) ) SseNoise( seed );
		}

		FastNoise::!FastNoise( )
		{
			AlignedDelete( m_pImpl );
		}

		FastNoise::~FastNoise( )
		{
			AlignedDelete( m_pImpl );
		}
	};
};