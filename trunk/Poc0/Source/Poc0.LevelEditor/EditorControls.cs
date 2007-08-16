using System.Windows.Forms;
using Poc0.LevelEditor.Core;
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
		/// <param name="editState">Edit state</param>
		/// <param name="templates">Object templates</param>
		public void Setup( Scene scene, TileGrid grid, TileGridEditState editState, ObjectTemplates templates )
		{
			m_Grid = grid;
			m_EditState = editState;
			m_Templates = templates;
			tileTypeSetView.TileTypes = grid.Set;

			PopulateObjectTemplates(scene);
		}

		/// <summary>
		/// Gets the edit state
		/// </summary>
		public TileGridEditState EditState
		{
			get { return m_EditState; }
		}

		/// <summary>
		/// Gets the tile grid
		/// </summary>
		public TileGrid Grid
		{
			get { return m_Grid; }
		}
		
		/// <summary>
		/// Tree node
		/// </summary>
		private class PaintObjectItem : TreeNode
		{
			/// <summary>
			/// Stores the specified template
			/// </summary>
			public PaintObjectItem( Scene scene, ObjectTemplate template ) :
				base( template.Name )
			{
				m_Scene = scene;
				m_Name = template.Name;
				m_Template = template;
			}

			/// <summary>
			/// Creates an instance of the stored object template
			/// </summary>
			public void Create( Tile tile, float x, float y )
			{
				object result = m_Template.CreateInstance( m_Scene.Builder );

				//	TODO: This is a bit sucky...
				TileObject tileObj = new TileObject( tile, x, y, result );
				tileObj.AddChild( new TileObjectRenderer( m_Scene, tileObj ) );
				tile.AddTileObject( new TileObject( tile, x, y, result ) );
			}

			/// <summary>
			/// Gets the name of the stored template
			/// </summary>
			public override string ToString( )
			{
				return m_Name;
			}

			private readonly ObjectTemplate m_Template;
			private readonly string m_Name;
			private readonly Scene m_Scene;
		}

		private TileGrid m_Grid;
		private TileGridEditState m_EditState;
		private ObjectTemplates m_Templates;
		
		private void PopulateObjectTemplates( Scene scene )
		{
			//	Populate the object types tree
			TreeNode allObjects = objectsTreeView.Nodes.Add( "All Objects" );

			foreach ( ObjectTemplate template in m_Templates.Templates )
			{
				allObjects.Nodes.Add( new PaintObjectItem( scene, template ) );
			}
		}

		private void tileTypeSetView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tileTypeSetView.SelectedItems.Count == 0)
			{
				return;
			}

			TileType type = ( TileType )tileTypeSetView.SelectedItems[ 0 ].Tag;
			m_EditState.OnPaint = type.SetTileToType;
		}

		private void objectsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			PaintObjectItem paintObject = objectsTreeView.SelectedNode as PaintObjectItem;
			if ( paintObject != null )
			{
				m_EditState.OnPaint = paintObject.Create;
			}
		}
	}
}
