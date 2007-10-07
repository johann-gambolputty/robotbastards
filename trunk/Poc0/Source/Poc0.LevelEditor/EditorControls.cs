using System;
using System.Collections;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.World;

namespace Poc0.LevelEditor
{
	public partial class EditorControls : UserControl
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public EditorControls( )
		{
			InitializeComponent( );

			foreach ( object csgOp in Enum.GetValues( typeof( Csg.Operation ) ) )
			{
				csgComboBox.Items.Add( csgOp );
			}
			csgComboBox.SelectedIndex = 0;

			IList templates = ( IList )AssetManager.Instance.Load( "Editor/EntityTemplates.components.xml" );
			foreach ( ObjectTemplate template in templates )
			{
				m_Templates.Add( template );
			}

			PopulateObjectTemplates( m_Templates );
		}

		private UserBrushEditMode m_CurrentEditMode;
		private readonly ArrayList m_Templates = new ArrayList( );
		private static readonly RayCastOptions ms_PickOptions = new RayCastOptions( RayCastLayers.Grid | RayCastLayers.StaticGeometry );


		private void PopulateObjectTemplates( IEnumerable templates )
		{
			objectsTreeView.Nodes.Clear( );

			//	Populate the object types tree
			TreeNode allObjects = objectsTreeView.Nodes.Add( "All Objects" );

			foreach ( object template in templates )
			{
				TreeNode node = new TreeNode( template.ToString( ) );
				node.Tag = template;

				allObjects.Nodes.Add( node );
			}
		}

		private void objectsTreeView_AfterSelect( object sender, TreeViewEventArgs e )
		{
			object template = objectsTreeView.SelectedNode.Tag;
			if ( template != null )
			{
				EditorState.Instance.AddEditMode( new AddObjectEditMode( MouseButtons.Right, template, ms_PickOptions ) );
			}
		}

		private void userBrushRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			Csg.Operation csg = ( Csg.Operation )csgComboBox.SelectedItem;

			m_CurrentEditMode = new UserBrushEditMode( csg );
			EditorState.Instance.AddEditMode( m_CurrentEditMode );
		}

		private void brushPage_Enter( object sender, EventArgs e )
		{
			Csg.Operation csg = ( Csg.Operation )csgComboBox.SelectedItem;

			m_CurrentEditMode = new UserBrushEditMode( csg );
			EditorState.Instance.AddEditMode( m_CurrentEditMode );
		}


		private void csgComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( m_CurrentEditMode != null )
			{
				Csg.Operation csg = ( Csg.Operation )csgComboBox.SelectedItem;
				m_CurrentEditMode.Operation = csg;
			}
		}
	}
}
