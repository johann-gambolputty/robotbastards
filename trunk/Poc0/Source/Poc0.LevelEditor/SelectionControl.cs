using System.Windows.Forms;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Components;

namespace Poc0.LevelEditor
{
	public partial class SelectionControl : UserControl
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SelectionControl( )
		{
			InitializeComponent( );

			EditModeContext.Instance.Selection.ObjectSelected += SelectionChanged;
			EditModeContext.Instance.Selection.ObjectDeselected += SelectionChanged;
		}

		/// <summary>
		/// Populates the selection tree view
		/// </summary>
		private void PopulateSelection( )
		{
			selectionTreeView.Nodes.Clear( );

			foreach ( object obj in EditModeContext.Instance.Selection.Selection )
			{
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
