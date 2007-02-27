// RbParticleSystem.h

#pragma once

using namespace System;

namespace RbParticleSystem
{
	public __gc class ParticleSystemRenderer
	{
	};

	public __gc class ParticleSystemUpdater
	{
		public :

		~ParticleSystemUpdater( );
	};

	public __gc class ParticleSystem
	{
		public :

			void		AddUpdater( ParticleSystemUpdater* updater );
	};
}
