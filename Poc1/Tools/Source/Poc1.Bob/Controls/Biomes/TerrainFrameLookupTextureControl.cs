using System;
using System.Windows.Forms;
using Poc1.Bob.Controls.Terrain;
using Poc1.Tools.TerrainTextures.Core;
using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class TerrainFrameLookupTextureControl : UserControl
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public TerrainFrameLookupTextureControl( )
		{
			InitializeComponent( );
			m_Builder = new TerrainTypeTextureBuilder( m_TerrainTypes );
			m_Builder.PackTextureReady += OnPackTextureBuilt;
			m_Builder.LookupTextureReady += OnLookupTextureBuilt;
		}

		/// <summary>
		/// Gets/sets the terrain model edited by this control
		/// </summary>
		/// <remarks>
		/// If the control changes the terrain texturing parameters, it will
		/// generate new lookup and pack textures that it assigns to this
		/// model using <see cref="IPlanetTerrainModel.TerrainPackTexture"/>
		/// and <see cref="IPlanetTerrainModel.TerrainTypesTexture"/>.
		/// </remarks>
		public IPlanetTerrainModel TerrainModel
		{
			get { return m_TerrainModel; }
			set { m_TerrainModel = value; }
		}

		#region Private Members

		private IPlanetTerrainModel m_TerrainModel;
		private TerrainTypeTextureBuilder m_Builder;
		private readonly TerrainTypeSet m_TerrainTypes = new TerrainTypeSet( );

		/// <summary>
		/// Handles the <see cref="TerrainTypeTextureBuilder.PackTextureReady"/> event
		/// </summary>
		private void OnPackTextureBuilt( ITexture2d packTexture )
		{
			if ( TerrainModel != null )
			{
				TerrainModel.TerrainPackTexture = packTexture;
			}
		}

		/// <summary>
		/// Handles the <see cref="TerrainTypeTextureBuilder.LookupTextureReady"/> event
		/// </summary>
		private void OnLookupTextureBuilt( ITexture2d lookupTexture )
		{
			if ( TerrainModel != null )
			{
				TerrainModel.TerrainTypesTexture = lookupTexture;
			}
		}

		/// <summary>
		/// Adds a new disabled terrain type control. 
		/// </summary>
		private void AddNewTerrainTypeControl( )
		{
			TerrainTypeControl control = new TerrainTypeControl( );
			control.Enabled = false;
			tableLayoutPanel1.Controls.Add( control );
		}

		#region Event Handlers

		private void TerrainSeLookupTextureControl_Load( object sender, EventArgs e )
		{
			tableLayoutPanel1.RowCount = 0;
			AddNewTerrainTypeControl( );
		}

		private void tableLayoutPanel1_MouseClick( object sender, MouseEventArgs e )
		{
			TerrainTypeControl control = tableLayoutPanel1.GetChildAtPoint( e.Location ) as TerrainTypeControl;
			if ( control == null || control.Enabled )
			{
				return;
			}

			//	User clicked on the disabled control. Enable it
			control.Enabled = true;
			control.Initialise( altitudeGraphControl, slopeGraphControl );
			control.Deleted +=
				delegate
				{
					m_TerrainTypes.Remove(control.TerrainType);
					control.Uninitialise( );
					tableLayoutPanel1.Controls.Remove( control );
				};
			control.Changed +=
				delegate
				{
					
				};
			m_TerrainTypes.Add(control.TerrainType);
			AddNewTerrainTypeControl( );
		}

		#endregion

		#endregion
	}
}
