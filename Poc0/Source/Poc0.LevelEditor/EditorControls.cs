using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Components;
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
		}


		/// <summary>
		/// Sets up the control
		/// </summary>
		/// <param name="scene">Scene that objects are added to</param>
		/// <param name="grid">Grid being edited</param>
		/// <param name="editContext">Editing context</param>
		/// <param name="templates">Object templates</param>
		public void Setup( Scene scene, TileGrid grid, EditModeContext editContext, ObjectTemplates templates )
		{
			m_Grid = grid;
			m_EditContext = editContext;
			m_Templates = templates;
			tileTypeSetView.TileTypes = grid.Set;

			PopulateObjectTemplates(scene);
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

		private TileGrid m_Grid;
		private EditModeContext m_EditContext;
		private ObjectTemplates m_Templates;
		
		private void PopulateObjectTemplates( Scene scene )
		{
			//	Populate the object types tree
			TreeNode allObjects = objectsTreeView.Nodes.Add( "All Objects" );

			foreach ( ObjectTemplate template in m_Templates.Templates )
			{
				TreeNode node = new TreeNode( template.Name );
				node.Tag = template;

				allObjects.Nodes.Add( node );
			}
		}

		private void tileTypeSetView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tileTypeSetView.SelectedItems.Count == 0)
			{
				return;
			}

			TileType type = ( TileType )tileTypeSetView.SelectedItems[ 0 ].Tag;

			m_EditContext.AddEditMode( new PaintTileEditMode( MouseButtons.Right, type ) );
			//m_EditState.OnPaint = type.SetTileToType;
		}

		private void objectsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			ObjectTemplate template = objectsTreeView.SelectedNode.Tag as ObjectTemplate;
			if ( template != null )
			{
				m_EditContext.AddEditMode( new AddObjectEditMode( MouseButtons.Right, template ) );
			}
		}
	}
}
