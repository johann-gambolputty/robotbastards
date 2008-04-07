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
			if ( m_Orbit != null )
			{
				m_Orbit.Update( this, context.RenderTime );
			}
		}

		#endregion

		#region IEntity Members

		public string Name
		{
			get { return m_Name; }
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

		public IList<IPlanet> Moons
		{
			get { return m_Moons; }
		}

		#endregion

		#region Private Members

		private readonly IList<IPlanet> m_Moons = new List<IPlanet>( );
		private readonly UniTransform m_Transform = new UniTransform( );
		private readonly string m_Name;
		private readonly IOrbit m_Orbit;

		#endregion
	}
}
