using System;

namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Context for planet model template instanciation
	/// </summary>
	/// <see cref="IPlanetModelTemplate.CreateModelInstance"/>
	public class ModelTemplateInstanceContext
	{
		/// <summary>
		/// Gets/sets the rng used to create randomised template instances
		/// </summary>
		public Random Random
		{
			get { return m_Random; }
			set { m_Random = value; }
		}

		#region Private Members

		private Random m_Random = new Random( ); 

		#endregion
	}
}
