using System;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
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

		/// <summary>
		/// Gets the default instance
		/// </summary>
		public static ModelTemplateInstanceContext Default
		{
			get { return s_Default; }
		}

		#region Private Members

		private readonly static ModelTemplateInstanceContext s_Default = new ModelTemplateInstanceContext( );
		private Random m_Random = new Random( ); 

		#endregion
	}
}
