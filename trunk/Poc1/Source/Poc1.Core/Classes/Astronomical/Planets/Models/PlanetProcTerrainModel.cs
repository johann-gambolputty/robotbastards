using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Fast.Terrain;

namespace Poc1.Core.Classes.Astronomical.Planets.Models
{
	/// <summary>
	/// Planet procedural terrain model interface
	/// </summary>
	public class PlanetProcTerrainModel : PlanetTerrainModel, IPlanetProcTerrainModel
	{
		#region IPlanetProcTerrainModel Members

		/// <summary>
		/// Gets/sets the height function
		/// </summary>
		public TerrainFunction HeightFunction
		{
			get { return m_HeightFunction; }
			set
			{
				if ( m_HeightFunction != value )
				{
					m_HeightFunction = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the ground function
		/// </summary>
		public TerrainFunction GroundFunction
		{
			get { return m_GroundFunction; }
			set
			{
				if ( m_GroundFunction != value )
				{
					m_GroundFunction = value;
					OnModelChanged( );
				}
			}
		}

		#endregion

		#region Private Members

		private TerrainFunction m_HeightFunction;
		private TerrainFunction m_GroundFunction;

		#endregion
	}
}
