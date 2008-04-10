using System.Collections.Generic;
using Poc1.Universe.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes
{
	public abstract class Planet : IPlanet
	{
		public Planet( IOrbit orbit, string name )
		{
			m_Orbit = orbit;
			m_Name = name;
		}

		#region IRenderable Members

		public virtual void Render( IRenderContext context )
		{
		}

		#endregion

		#region IEntity Members

		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		public UniTransform Transform
		{
			get { return m_Transform; }
		}

		#endregion

		#region IPlanet Members

		public IOrbit Orbit
		{
			get { return m_Orbit; }
		}

		/// <summary>
		/// Enables/disables the rendering of the terrain of this planet
		/// </summary>
		/// <remarks>
		/// Only one planet should have this flag enabled
		/// </remarks>
		public bool EnableTerrainRendering
		{
			get { return m_EnableTerrainRendering; }
			set { m_EnableTerrainRendering = value; }
		}

		public IList<IPlanet> Moons
		{
			get { return m_Moons; }
		}

		#endregion

		#region Private Members

		private readonly IList<IPlanet> m_Moons = new List<IPlanet>( );
		private readonly UniTransform m_Transform = new UniTransform( );
		private readonly IOrbit m_Orbit;
		private string m_Name;
		private bool m_EnableTerrainRendering;

		#endregion
	}
}
