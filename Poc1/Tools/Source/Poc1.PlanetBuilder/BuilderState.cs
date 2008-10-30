
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Spherical;
using IPlanet=Poc1.Universe.Interfaces.Planets.IPlanet;

namespace Poc1.PlanetBuilder
{
	public class BuilderState
	{
		/// <summary>
		/// Gets the builder state singleton
		/// </summary>
		public static BuilderState Instance
		{
			get { return s_Instance; }
		}

		/// <summary>
		/// Gets the current terrain mesh
		/// </summary>
		//public TerrainMesh TerrainMesh
		//{
		//    get { return m_TerrainMesh; }
		//}

		public ISpherePlanet SpherePlanet
		{
			get { return ( ISpherePlanet )Planet; }
		}

		public IPlanet Planet
		{
			get { return m_Planet; }
			set
			{
				if ( m_Planet != null )
				{
					m_Planet.Dispose( );
				}
				m_Planet = value;
			}
		}

		#region Private Members

		private IPlanet m_Planet = Poc1.Universe.Planets.Spherical.SpherePlanet.DefaultPlanet( new Units.Metres( 200000 ) );
	//	private IPlanet m_Planet = Poc1.Universe.Planets.Spherical.SpherePlanet.DefaultGasGiant( new Units.Metres( 2000000 ) );
		private readonly static BuilderState s_Instance = new BuilderState( );

		#endregion
	}
}
