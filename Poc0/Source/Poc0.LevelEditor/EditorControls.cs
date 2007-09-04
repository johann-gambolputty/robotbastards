using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Components;
using Rb.Core.Resources;
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

			IList templates = ( IList )ResourceManager.Instance.Load( "TestObjectTemplates.components.xml" );
			foreach ( ObjectPattern template in templates )
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

		private TileGrid					m_Grid;
		private EditModeContext				m_EditContext;
		private readonly List< ObjectPattern >	m_Templates = new List< ObjectPattern >( );

		private void PopulateObjectTemplates( IEnumerable< ObjectPattern > templates )
		{
			objectsTreeView.Nodes.Clear( );

			//	Populate the object types tree
			TreeNode allObjects = objectsTreeView.Nodes.Add( "All Objects" );

			foreach ( ObjectPattern template in templates )
			{
				TreeNode node = new TreeNode( template.TemplateName );
				node.Tag = template;

				allObjects.Nodes.Add( node );
			}
		}

		private void tileTypeSetView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( tileTypeSetView.SelectedItems.Count == 0 )
			{
				return;
			}

			TileType type = ( TileType )tileTypeSetView.SelectedItems[ 0 ].Tag;

			m_EditContext.AddEditMode( new PaintTileEditMode( MouseButtons.Right, type ) );
		}

		private void objectsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			ObjectPattern template = objectsTreeView.SelectedNode.Tag as ObjectPattern;
			if ( template != null )
			{
				m_EditContext.AddEditMode( new AddObjectEditMode( MouseButtons.Right, template ) );
			}
		}
	}
}
