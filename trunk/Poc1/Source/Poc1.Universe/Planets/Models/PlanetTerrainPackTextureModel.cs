using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Implementation of planet terrain pack texturing model
	/// </summary>
	public class PlanetTerrainPackTextureModel : PlanetEnvironmentModel, IPlanetTerrainPackTextureModel
	{
		#region IPlanetTerrainPackTextureModel Members

		/// <summary>
		/// Gets/sets the terrain types texture
		/// </summary>
		public ITexture2d TerrainTypesTexture
		{
			get { return m_TerrainTypesTexture; }
			set
			{
				if ( m_TerrainTypesTexture != value )
				{
					m_TerrainTypesTexture = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the terrain pack texture
		/// </summary>
		public ITexture2d TerrainPackTexture
		{
			get { return m_TerrainPackTexture; }
			set
			{
				if ( m_TerrainPackTexture != value )
				{
					m_TerrainPackTexture = value;
					OnModelChanged( );
				}
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Assigns this model to a planet model
		/// </summary>
		protected override void AssignToModel( IPlanetModel planetModel, bool remove )
		{
			if ( remove )
			{
				planetModel.Remove( this );	
			}
			else
			{
				planetModel.Add( this );
			}
		}

		#endregion

		#region Private Members

		private ITexture2d m_TerrainTypesTexture;
		private ITexture2d m_TerrainPackTexture;

		#endregion
	}
}
