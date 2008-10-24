using System;
using System.Collections.Generic;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes
{
	/// <summary>
	/// A solar system object acts as the scene graph
	/// </summary>
	public class SolarSystem : IRenderable, IDisposable
	{
		#region Public Members

		/// <summary>
		/// Gets the list of suns in the solar system
		/// </summary>
		public IList<ISun> Suns
		{
			get { return m_Suns; }
		}

		/// <summary>
		/// Gets the list of planets in the solar system
		/// </summary>
		public IList<IPlanet> Planets
		{
			get { return m_Planets;  }
		}

		#endregion

		#region Private Members

		private readonly IList<ISun> m_Suns = new List<ISun>( );
		private readonly List<IPlanet> m_Planets = new List<IPlanet>( );

		/// <summary>
		/// Updates the orbits for all planets
		/// </summary>
		/// <param name="updateTime">Update time value</param>
		private void Update( long updateTime )
		{
			foreach ( IPlanet planet in m_Planets )
			{
				if ( planet.Orbit != null )
				{
					planet.Orbit.Update( planet, updateTime );
				}
			}
		}

		#endregion

		#region IRenderable Members

		private readonly StarBox m_SkyBox = new StarBox( );

		/// <summary>
		/// Renders this solar system
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			m_SkyBox.Render( context );

			//	Update all the planets
			Update( context.RenderTime );

			foreach ( IPlanet planet in m_Planets )
			{
				planet.DeepRender( context );
			}
			Graphics.Renderer.ClearDepth( 1.0f );
			foreach ( IPlanet planet in m_Planets )
			{
				planet.Render( context );
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes of all the objects in the solar system
		/// </summary>
		public void Dispose( )
		{
			foreach ( IPlanet planet in m_Planets )
			{
				planet.Dispose( );
			}
		}

		#endregion
	}
}
