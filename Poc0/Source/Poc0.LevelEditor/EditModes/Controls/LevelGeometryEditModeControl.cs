using System;
using System.Windows.Forms;

namespace Poc0.LevelEditor.EditModes.Controls
{
	public partial class LevelGeometryEditModeControl : UserControl
	{
		public LevelGeometryEditModeControl( LevelGeometryEditMode editMode )
		{
			m_EditMode = editMode;
			InitializeComponent( );
		}

		private readonly LevelGeometryEditMode m_EditMode;

		private void polygonBrushRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			m_EditMode.UseBrush( m_EditMode.PolygonBrush );
		}

		private void circleBrushRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			m_EditMode.UseBrush( m_EditMode.CircleBrush );
		}

		private void savedBrushRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			m_EditMode.UseBrush( null );
		}

		private void circleEdgeCountUpDown_ValueChanged( object sender, EventArgs e )
		{
			m_EditMode.CircleBrush.EdgeCount = ( int )circleEdgeCountUpDown.Value;
		}

		private void showCollisionChamferCheckBox_CheckedChanged( object sender, EventArgs e )
		{
			//	TODO: AP: ...
		}
	}
}
