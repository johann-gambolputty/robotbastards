using System;
using System.Collections.Generic;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes
{
	public class SolarSystem : IRenderable, IDisposable
	{

		#region Public Members

		public IList< ISun > Suns
		{
			get { return m_Suns; }
		}

		public IList< IPlanet > Planets
		{
			get { return m_Planets;  }
		}

		#endregion

		#region Private Members

		private readonly IList< ISun >		m_Suns = new List< ISun >( );
		private readonly List< IPlanet >	m_Planets = new List< IPlanet >( );

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
			if ( DebugInfo.ShowCameraPosition )
			{
				DebugText.Write( "Camera Pos: {0}", UniCamera.Current.Position.ToRenderUnitString( ) );
			}

			m_SkyBox.Render( context );

			//	Update all the planets
			Update( context.RenderTime );
			
			//	Sort the planets by distance from the camera
			IPlanet[] sortedPlanets = m_Planets.ToArray( );
			Array.Sort( sortedPlanets, PlanetDistanceComparison );

			foreach ( IPlanet planet in sortedPlanets )
			{
				double distanceToCamera = UniCamera.Current.Position.DistanceTo( planet.Transform.Position );
				planet.EnableTerrainRendering = ( distanceToCamera < TerrainRenderingDistance );
				planet.Render( context );
			}
		}


		/// <summary>
		/// The distance threshold that terrain rendering starts
		/// </summary>
		private const double TerrainRenderingDistance = 5200000000;

		/// <summary>
		/// Planet to camera distance comparison, used by the render method for sorting planets in back to front order
		/// </summary>
		private readonly static Comparison<IPlanet> PlanetDistanceComparison =
			delegate( IPlanet p0, IPlanet p1 )
			{
				UniPoint3 cameraPos = UniCamera.Current.Position;
				long p0Score = p0.Transform.Position.ManhattanDistanceTo( cameraPos );
				long p1Score = p1.Transform.Position.ManhattanDistanceTo( cameraPos );
				return p0Score > p1Score ? -1 : ( p0Score == p1Score ? 0 : 1 );
			};

		#endregion

		#region IDisposable Members

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
