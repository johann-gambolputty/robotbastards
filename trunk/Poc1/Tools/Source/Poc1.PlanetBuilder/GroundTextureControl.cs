using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Poc1.PlanetBuilder.Properties;
using Poc1.Tools.TerrainTextures.Core;
using Rb.Core.Maths;
using Rb.Log;
using Rb.NiceControls.Graph;

namespace Poc1.PlanetBuilder
{
	public partial class GroundTextureControl : UserControl
	{
		public GroundTextureControl( )
		{
			InitializeComponent( );

			AddTypeControls( null );
		}


		#region Private Members

		private string m_ExportDirectory;
		private string m_SavePath;
		private readonly List<GroundTypeControl> m_SelectedControls = new List<GroundTypeControl>( );

		#region Save and Export Methods
		
		private void SaveAs( )
		{
			SaveFileDialog dialog = new SaveFileDialog( );
			dialog.Filter = "Terrain Type Set (*.tts)|*.TTS|All Files (*.*)|*.*";
			dialog.DefaultExt = "tts";
			if ( dialog.ShowDialog( this ) != DialogResult.OK )
			{
				return;
			}
			Save( dialog.FileName );
		}

		private void Save( )
		{
			if ( string.IsNullOrEmpty( m_SavePath ) )
			{
				SaveAs( );
			}
			else
			{
				Save( m_SavePath );
			}
		}

		private void Save( string path )
		{
			try
			{
				TerrainTypeTextureBuilder.Instance.TerrainTypes.Save( path );
			}
			catch
			{
				MessageBox.Show( "Failed to save terrain types to file" );
			}
			m_SavePath = path;
		}
		
		private void ExportTo( )
		{
			FolderBrowserDialog folderDialog = new FolderBrowserDialog( );
			folderDialog.Description = Resources.ChooseExportFolder;
			folderDialog.SelectedPath = string.IsNullOrEmpty( m_ExportDirectory ) ? Directory.GetCurrentDirectory( ) : m_ExportDirectory;
			if ( folderDialog.ShowDialog( this ) != DialogResult.OK )
			{
				return;
			}
			m_ExportDirectory = folderDialog.SelectedPath;
			Export( m_ExportDirectory );
		}

		private void Export( )
		{
			if ( string.IsNullOrEmpty( m_ExportDirectory ) )
			{
				ExportTo( );
			}
			else
			{
				Export( m_ExportDirectory );
			}
		}

		private void Export( string directory )
		{
			TerrainTypes.Export( directory );
		}

		#endregion

		#region Selected Controls
		
		private void Deselect( GroundTypeControl control )
		{
			altitudeGraphControl.RemoveFunction( control.TerrainType.AltitudeDistribution );
			slopeGraphControl.RemoveFunction( control.TerrainType.SlopeDistribution );
			control.TerrainType.AltitudeDistribution.ParametersChanged -= OnTerrainTypeDistributionsChanged;
			control.TerrainType.SlopeDistribution.ParametersChanged -= OnTerrainTypeDistributionsChanged;
			control.Selected = false;
			m_SelectedControls.Remove( control );
		}

		private void Select( GroundTypeControl control )
		{
			control.Selected = true;
			m_SelectedControls.Add( control );

			if ( control.TerrainType.AltitudeDistribution == null )
			{
				control.TerrainType.AltitudeDistribution = new LineFunction1d( );
			}
			if ( control.TerrainType.SlopeDistribution == null )
			{
				control.TerrainType.SlopeDistribution = new LineFunction1d( );
			}

			control.TerrainType.AltitudeDistribution.ParametersChanged += OnTerrainTypeDistributionsChanged;
			control.TerrainType.SlopeDistribution.ParametersChanged += OnTerrainTypeDistributionsChanged;

			IGraphInputHandler altitudeHandler = GraphInputHandler.CreateHandlerForFunction( control.TerrainType.AltitudeDistribution );
			IGraphInputHandler slopeHandler = GraphInputHandler.CreateHandlerForFunction( control.TerrainType.SlopeDistribution );

			altitudeGraphControl.AddGraph( altitudeHandler );
			slopeGraphControl.AddGraph( slopeHandler );
		}

		private bool IsSelected( GroundTypeControl control )
		{
			return m_SelectedControls.Contains( control );
		}

		#endregion

		/// <summary>
		/// Gets/sets the current terrain type set. Alias for the terrain type set stored in the <see cref="TerrainTypeTextureBuilder"/> singleton.
		/// </summary>
		private TerrainTypeSet TerrainTypes
		{
			get { return TerrainTypeTextureBuilder.Instance.TerrainTypes; }
			set
			{
				TerrainTypeTextureBuilder.Instance.TerrainTypes = value;
				groundTypeTableLayoutPanel.Controls.Clear( );
				
				foreach ( TerrainType type in value.TerrainTypes )
				{
					AddTypeControls( type );
				}

				AddTypeControls( null );
			}
		}

		/// <summary>
		/// Adds a <see cref="GroundTypeControl"/> for a given terrain type
		/// </summary>
		private void AddTypeControls( TerrainType terrainType )
		{
			GroundTypeControl newControl = new GroundTypeControl( );
			newControl.Enabled = terrainType != null;
			if ( terrainType != null )
			{
				newControl.TerrainType = terrainType;
			}
			newControl.Anchor |= AnchorStyles.Right;
			newControl.ControlSelected += GroundTypeControl_Selected;
			newControl.MoveControlUp += GroundTypeControl_MoveControlUp;
			newControl.MoveControlDown += GroundTypeControl_MoveControlDown;
			newControl.RemoveControl += GroundTypeControl_RemoveControl;
			newControl.TerrainTypeChanged += GroundTypeControl_TerrainTypeChanged;
			groundTypeTableLayoutPanel.Controls.Add( newControl );
		}


		#endregion

		#region Control Event Handlers
		
		private static void GroundTypeControl_TerrainTypeChanged( TerrainType terrainType )
		{
		}

		private void GroundTypeControl_Selected( GroundTypeControl control )
		{
			if ( !IsSelected( control ) )
			{
				Select( control );
			}
			else
			{
				Deselect( control );
			}
		}

		private void GroundTypeControl_MoveControlUp( GroundTypeControl control )
		{
			TableLayoutPanelCellPosition pos = groundTypeTableLayoutPanel.GetCellPosition( control );
			if ( pos.Row == 0 )
			{
				return;
			}
			--pos.Row;
			groundTypeTableLayoutPanel.SetCellPosition( control, pos );
		}

		private void GroundTypeControl_MoveControlDown( GroundTypeControl control )
		{
			TableLayoutPanelCellPosition pos = groundTypeTableLayoutPanel.GetCellPosition( control );
			if ( pos.Row >= groundTypeTableLayoutPanel.RowCount - 1 )
			{
				return;
			}
			++pos.Row;
			groundTypeTableLayoutPanel.SetCellPosition( control, pos );
		}

		private void GroundTypeControl_RemoveControl( GroundTypeControl control )
		{
			TerrainTypes.Remove( control.TerrainType );
			groundTypeTableLayoutPanel.Controls.Remove( control );
			Deselect( control );
		}

		private void groundTypeTableLayoutPanel_MouseClick( object sender, MouseEventArgs e )
		{
			GroundTypeControl control = groundTypeTableLayoutPanel.GetChildAtPoint( e.Location ) as GroundTypeControl;
			if ( control == null )
			{
				return;
			}
			if ( !control.Enabled )
			{
				control.Enabled = true;
				control.TerrainType = new TerrainType( );
				TerrainTypes.Add( control.TerrainType );

				AddTypeControls( null );
			}
			else
			{
				if ( IsSelected( control ) )
				{
					Deselect( control );
				}
				else
				{
					Select( control );
				}
			}
		}

		private static void OnTerrainTypeDistributionsChanged( IFunction1d function )
		{
			TerrainTypeTextureBuilder.Instance.Rebuild( true, false );
		}
		
		private void newToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_SavePath = string.Empty;
			TerrainTypes = new TerrainTypeSet( );
		}

		private void openToolStripMenuItem_Click( object sender, EventArgs e )
		{
			OpenFileDialog dialog = new OpenFileDialog( );
			dialog.Filter = "Terrain Type Set (*.tts)|*.TTS|All Files (*.*)|*.*";
			if ( dialog.ShowDialog( this ) != DialogResult.OK )
			{
				return;
			}
			string path = dialog.FileName;

			TerrainTypeSet newTypeSet;
			try
			{
				newTypeSet = TerrainTypeSet.Load( path );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to load terrain type set from \"{0}\"", path );
				MessageBox.Show( "Failed to open terrain types file" );
				return;
			}
			TerrainTypes = newTypeSet;
			m_SavePath = path;
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Save( );
		}

		private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveAs( );
		}

		private void exportToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Export( );
		}

		private void exportToToolStripMenuItem_Click( object sender, EventArgs e )
		{
			ExportTo( );
		}

		#endregion
	}
}
