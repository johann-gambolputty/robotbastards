using System.Collections.Generic;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Rendering;
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

		/// <summary>
		/// Gets the description of the orbit of this planet
		/// </summary>
		public IOrbit Orbit
		{
			get { return m_Orbit; }
		}

		/// <summary>
		/// Gets the planetary atmosphere model
		/// </summary>
		public abstract IPlanetAtmosphereModel Atmosphere
		{
			get;
		}

		/// <summary>
		/// Gets the planetary terrain model
		/// </summary>
		public abstract IPlanetTerrainModel Terrain
		{
			get;
		}

		/// <summary>
		/// Regenerates the 
		/// </summary>
		public abstract void RegenerateTerrain( );

		/// <summary>
		/// Gets the sea level of this planet. If this is zero, the planet has no seas. Otherwise, it's measured
		/// as an offset, in metres, from the planet's radius
		/// </summary>
		public float SeaLevel
		{
			get { return m_SeaLevel; }
			set { m_SeaLevel = value; }
		}

		/// <summary>
		/// Gets the range of heights of the planet's terrain. 
		/// </summary>
		public float TerrainHeightRange
		{
			get { return m_TerrainHeightRange; }
			set { m_TerrainHeightRange = value; }
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

		/// <summary>
		/// Gets the planet atmosphere renderer
		/// </summary>
		public abstract IAtmosphereRenderer AtmosphereRenderer
		{
			get;
		}

		public IList<IPlanet> Moons
		{
			get { return m_Moons; }
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes of this planet. Away with you, I say
		/// </summary>
		public virtual void Dispose( )
		{
		}

		#endregion

		#region Private Members

		private readonly IList<IPlanet> m_Moons = new List<IPlanet>( );
		private readonly UniTransform m_Transform = new UniTransform( );
		private readonly IOrbit m_Orbit;
		private string m_Name;
		private bool m_EnableTerrainRendering;
		private float m_SeaLevel = 0;
		private float m_TerrainHeightRange = 4000;

		#endregion
	}
}
