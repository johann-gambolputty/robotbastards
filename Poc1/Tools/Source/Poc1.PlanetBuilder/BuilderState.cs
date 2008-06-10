namespace Poc1.PlanetBuilder
{
	public class BuilderState
	{
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
		public TerrainMesh TerrainMesh
		{
			get { return m_TerrainMesh; }
		}

		#region Private Members

		private TerrainMesh m_TerrainMesh = new TerrainMesh( 2048, 200, 2048 );
		private readonly static BuilderState ms_Instance = new BuilderState( );

		#endregion
	}
}
