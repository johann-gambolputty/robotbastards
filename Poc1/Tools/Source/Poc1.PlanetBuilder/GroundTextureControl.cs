using System.Collections.Generic;
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;
using Rb.Core.Maths;

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

		private string m_SavePath;
		private readonly List<GroundTypeControl> m_SelectedControls = new List<GroundTypeControl>( );

		private static TerrainTypeSet TerrainTypes
		{
			get { return TerrainTypeTextureBuilder.Instance.TerrainTypes; }
		}

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
		//	UpdateSample( );
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
		//	UpdateSample( );
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
			//	UpdateSample( );

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

		private void Deselect( GroundTypeControl control )
		{
			control.TerrainType.AltitudeDistribution.ParametersChanged -= OnTerrainTypeDistributionsChanged;
			control.TerrainType.SlopeDistribution.ParametersChanged -= OnTerrainTypeDistributionsChanged;
			control.Selected = false;
			m_SelectedControls.Remove( control );
		}

		private static void OnTerrainTypeDistributionsChanged( IFunction1d function )
		{
			TerrainTypeTextureBuilder.Instance.Rebuild( true, false );
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

			altitudeGraphEditorControl.Function = control.TerrainType.AltitudeDistribution;
			slopeGraphEditorControl.Function = control.TerrainType.SlopeDistribution;
		}

		private bool IsSelected( GroundTypeControl control )
		{
			return m_SelectedControls.Contains( control );
		}

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
			if ( m_SavePath == null )
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


		private void loadButton_Click( object sender, System.EventArgs e )
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
			catch
			{
				MessageBox.Show( "Failed to open terrain types file" );
				return;
			}
			TerrainTypeTextureBuilder.Instance.TerrainTypes = newTypeSet;
			m_SavePath = path;
			groundTypeTableLayoutPanel.Controls.Clear( );

			foreach ( TerrainType type in newTypeSet.TerrainTypes )
			{
				AddTypeControls( type );
			}

			AddTypeControls( null );
		}

		private void saveButton_Click( object sender, System.EventArgs e )
		{
			Save( );
		}

		private void saveAsButton_Click( object sender, System.EventArgs e )
		{
			SaveAs( );
		}

		private void exportButton_Click( object sender, System.EventArgs e )
		{

		}

		private void exportAsButton_Click( object sender, System.EventArgs e )
		{

		}
		#endregion

	}
}
