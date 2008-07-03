using Poc1.Universe.Classes;
using Poc1.Universe.Interfaces;

namespace Poc1.PlanetBuilder
{
	public class BuilderState
	{

		public const float TerrainWidth = 2048;
		public const float TerrainMaxHeight = 200;
		public const float TerrainDepth = 2048;
		public const double PlanetRadius = 80000;

		public BuilderState( )
		{
			m_Planet.EnableTerrainRendering = true;
		}

		/// <summary>
		/// Gets the builder state singleton
		/// </summary>
		public static BuilderState Instance
		{
			get { return ms_Instance; }
		}

		/// <summary>
		/// Gets the current terrain mesh
		/// </summary>
		//public TerrainMesh TerrainMesh
		//{
		//    get { return m_TerrainMesh; }
		//}

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


		/// <summary>
		/// Gets the current ocean renderer
		/// </summary>
	//	public PlaneOceanRenderer Ocean
	//	{
	//		get { return m_Ocean; }
	//	}

		#region Private Members

		private IPlanet m_Planet = new SpherePlanet( null, "", PlanetRadius );
	//	private PlaneOceanRenderer m_Ocean = new PlaneOceanRenderer( TerrainWidth, TerrainDepth, 8 );
	//	private TerrainMesh m_TerrainMesh = new TerrainMesh( TerrainWidth, TerrainMaxHeight, TerrainDepth );
		private readonly static BuilderState ms_Instance = new BuilderState( );

		#endregion
	}
}
