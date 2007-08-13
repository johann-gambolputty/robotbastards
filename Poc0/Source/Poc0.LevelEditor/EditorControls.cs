using System.Windows.Forms;
using Poc0.LevelEditor.Core;

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

			//	Populate the object types tree
			TreeNode allObjects = objectsTreeView.Nodes.Add( "All Objects" );
			allObjects.Nodes.Add( new PaintObjectItem( "Player Start", CreateObject< object > ) );
		}

		/// <summary>
		/// Creates a new object of type T
		/// </summary>
		/// <typeparam name="T">Object type to create</typeparam>
		public static void CreateObject< T >( Tile tile, float x, float y ) where T : new( )
		{
			tile.AddTileObject( new TileObject( tile, x, y, new T( ) ) );
		}

		/// <summary>
		/// Tree node
		/// </summary>
		private class PaintObjectItem : TreeNode
		{
			public PaintObjectItem( string name, TileGridEditState.PaintDelegate paintObject ) :
				base( name )
			{
				m_Name = name;
				m_PaintObject = paintObject;
			}

			public TileGridEditState.PaintDelegate PaintObject
			{
				get { return m_PaintObject; }
			}

			public override string ToString( )
			{
				return m_Name;
			}

			private readonly string m_Name;
			private readonly TileGridEditState.PaintDelegate m_PaintObject;
		}

		/// <summary>
		/// Sets up the control
		/// </summary>
		/// <param name="grid">Grid being edited</param>
		/// <param name="editState">Edit state</param>
		public void Setup( TileGrid grid, TileGridEditState editState )
		{
			m_Grid = grid;
			m_EditState = editState;
			tileTypeSetView.TileTypes = grid.Set;
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

		private TileGrid m_Grid;
		private TileGridEditState m_EditState;

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
				m_EditState.OnPaint = paintObject.PaintObject;
			}
		}
	}
}
