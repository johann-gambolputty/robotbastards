using System.Windows.Forms;
using Rb.Core.Components;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public partial class SelectionControl : UserControl
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SelectionControl( )
		{
			InitializeComponent( );

			if ( EditorState.Instance.CurrentSelection != null )
			{
				EditorState.Instance.CurrentSelection.ObjectSelected += SelectionChanged;
				EditorState.Instance.CurrentSelection.ObjectSelected += SelectionChanged;
			}
			EditorState.Instance.SceneOpened += OnSceneOpened;
			EditorState.Instance.SceneOpened += OnSceneClosed;
		}

		/// <summary>
		/// Called when a new scene is opened
		/// </summary>
		/// <param name="state">Opened scene's edit state</param>
		private void OnSceneOpened( SceneEditState state )
		{
			state.SelectedObjects.ObjectSelected += SelectionChanged;
			state.SelectedObjects.ObjectSelected += SelectionChanged;
		}

		/// <summary>
		/// Called when a scene is about to be closed
		/// </summary>
		/// <param name="state">Closing scene's edit state</param>
		private void OnSceneClosed( SceneEditState state )
		{
			state.SelectedObjects.ObjectSelected -= SelectionChanged;
			state.SelectedObjects.ObjectSelected -= SelectionChanged;
		}

		/// <summary>
		/// Populates the selection tree view
		/// </summary>
		private void PopulateSelection( )
		{
			selectionTreeView.Nodes.Clear( );

			foreach ( object obj in EditorState.Instance.CurrentSelection.Selection )
			{
				object nodeObject = obj;
				if ( nodeObject is IObjectEditor )
				{
				}
				selectionTreeView.Nodes.Add( CreateObjectTreeNode( obj ) );
			}
		}

		/// <summary>
		/// Creates a tree node for an object
		/// </summary>
		/// <param name="obj">Object to create a tree node for</param>
		private static TreeNode CreateObjectTreeNode( object obj )
		{
			string name = obj.GetType( ).ToString( );
			name = name.Substring( name.LastIndexOf( '.' ) + 1 );

			TreeNode node = new TreeNode( name );

			IParent parent = obj as IParent;
			if ( parent == null )
			{
				return node;
			}

			foreach ( object childObj in parent.Children )
			{
				node.Nodes.Add( CreateObjectTreeNode( childObj ) );
			}

			return node;
		}

		/// <summary>
		/// Called when the selection in the current edit mode context is modified
		/// </summary>
		/// <param name="obj">Object that was (de)selected</param>
		private void SelectionChanged( object obj )
		{
			PopulateSelection( );
		}
	}
}
