using System;
using System.Collections.Generic;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes
{
	public class SolarSystem : IRenderable
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
		private IPlanet						m_ClosestPlanetToCamera;

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

		private readonly SkyBox m_SkyBox = new SkyBox( );

		/// <summary>
		/// Renders this solar system
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			m_SkyBox.Render( context );

			//	Update all the planets
			Update( context.RenderTime );
			
			//	Sort the planets by distance from the camera
			IPlanet[] sortedPlanets = m_Planets.ToArray( );
			Array.Sort( sortedPlanets, PlanetDistanceComparison );

			if ( m_ClosestPlanetToCamera != null )
			{
				m_ClosestPlanetToCamera.EnableTerrainRendering = false;
			}
			if ( sortedPlanets.Length > 0 )
			{
				m_ClosestPlanetToCamera = sortedPlanets[ sortedPlanets.Length - 1 ];
				double distanceToCamera = UniCamera.Current.Position.DistanceTo( m_ClosestPlanetToCamera.Transform.Position );
				m_ClosestPlanetToCamera.EnableTerrainRendering = ( distanceToCamera < TerrainRenderingDistance );
			}

			foreach ( IPlanet planet in sortedPlanets )
			{
				planet.Render( context );
			}
		}


		/// <summary>
		/// The distance threshold that terrain rendering starts at for the closest planet
		/// </summary>
		private const double TerrainRenderingDistance = 1000000000;

		/// <summary>
		/// Planet to camera distance comparison, used by the render method for sorting planets in back to front order
		/// </summary>
		private readonly static Comparison<IPlanet> PlanetDistanceComparison =
			delegate( IPlanet p0, IPlanet p1 )
			{
				UniPoint3 cameraPos = UniCamera.Current.Position;
				long p0SqrDistToCam = p0.Transform.Position.SqrDistanceTo( cameraPos );
				long p1SqrDistToCam = p1.Transform.Position.SqrDistanceTo( cameraPos );
				return p0SqrDistToCam > p1SqrDistToCam ? -1 : ( p0SqrDistToCam == p1SqrDistToCam ? 0 : 1 );
			};

		#endregion
	}
}
