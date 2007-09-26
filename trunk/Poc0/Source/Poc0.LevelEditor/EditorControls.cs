using System;
using System.Collections;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Assets;
using Rb.Core.Components;

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

			EditModeContext.Instance.PostSetup += OnContextSetup;
		}


		/// <summary>
		/// Sets up the control
		/// </summary>
		/// <param name="editContext">Editing context</param>
		public void OnContextSetup( EditModeContext editContext )
		{
			m_Grid = editContext.Grid;
			m_EditContext = editContext;
			tileTypeSetView.TileTypes = editContext.Grid.Set;
		}

		/// <summary>
		/// Gets the edit state
		/// </summary>
		public EditModeContext EditContext
		{
			get { return m_EditContext; }
		}

		/// <summary>
		/// Gets the tile grid
		/// </summary>
		public TileGrid Grid
		{
			get { return m_Grid; }
		}

		private TileGrid			m_Grid;
		private EditModeContext		m_EditContext;
		private readonly ArrayList	m_Templates = new ArrayList( );

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

		private void tileTypeSetView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ( tileTypeSetView.SelectedItems.Count == 0 )
			{
				return;
			}

			TileType type = ( TileType )tileTypeSetView.SelectedItems[ 0 ].Tag;

			m_EditContext.AddEditMode( new PaintTileEditMode( MouseButtons.Right, type ) );
		}

		private void objectsTreeView_AfterSelect( object sender, TreeViewEventArgs e )
		{
			object template = objectsTreeView.SelectedNode.Tag;
			if ( template != null )
			{
				m_EditContext.AddEditMode( new AddObjectEditMode( MouseButtons.Right, template ) );
			}
		}

		private void userBrushRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			Csg.Operation csg = ( Csg.Operation )csgComboBox.SelectedItem;

			m_CurrentEditMode = new UserBrushEditMode( csg );
			m_EditContext.AddEditMode( m_CurrentEditMode );
		}

		private void brushPage_Enter( object sender, EventArgs e )
		{
			Csg.Operation csg = ( Csg.Operation )csgComboBox.SelectedItem;

			m_CurrentEditMode = new UserBrushEditMode( csg );
			m_EditContext.AddEditMode( m_CurrentEditMode );
		}

		private UserBrushEditMode m_CurrentEditMode;

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
