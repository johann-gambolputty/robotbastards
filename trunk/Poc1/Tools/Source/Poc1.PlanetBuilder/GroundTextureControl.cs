using System.Collections.Generic;
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;

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
		
		private readonly List<GroundTypeControl> m_SelectedControls = new List<GroundTypeControl>( );
		private TerrainTypeSet m_TerrainTypes = new TerrainTypeSet( );

		private void AddTypeControls( TerrainType terrainType )
		{
			GroundTypeControl newControl = new GroundTypeControl( );
			newControl.Enabled = terrainType != null;
			if ( terrainType != null )
			{
				newControl.TerrainType = terrainType;
			}
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
			m_TerrainTypes.TerrainTypes.Remove( control.TerrainType );
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
				m_TerrainTypes.TerrainTypes.Add( control.TerrainType );
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
			control.Selected = false;
			m_SelectedControls.Remove( control );
		}

		private void Select( GroundTypeControl control )
		{
			control.Selected = true;
			m_SelectedControls.Add( control );
		}

		private bool IsSelected( GroundTypeControl control )
		{
			return m_SelectedControls.Contains( control );
		}

		#endregion


	}
}
