using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using Poc0.LevelEditor.Properties;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Log;
using Rb.Tools.LevelEditor.Core;

namespace Poc0.LevelEditor.EditModes.Controls
{
	public partial class ObjectEditModeControl : UserControl
	{
		public ObjectEditModeControl( ObjectEditMode editMode )
		{
			InitializeComponent( );

			m_EditMode = editMode;

			BuildObjectTemplateTree( editMode.Templates );
		}

		private readonly ObjectEditMode m_EditMode;

		/// <summary>
		/// Builds the object tree view from a list of object templates
		/// </summary>
		private void BuildObjectTemplateTree( IEnumerable templates )
		{
			objectsTreeView.Nodes.Clear( );

			//	Populate the object types tree
			TreeNode allObjects = objectsTreeView.Nodes.Add( "All Objects" );

			foreach ( object template in templates )
			{
				TreeNode node = new TreeNode( template.ToString( ) );
				node.Tag = template;

				allObjects.Nodes.Add( node );

				if ( template == m_EditMode.CurrentTemplate )
				{
					objectsTreeView.SelectedNode = node;
				}
			}
		}

		private void objectsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			m_EditMode.CurrentTemplate = objectsTreeView.SelectedNode.Tag;
		}

	}
}
