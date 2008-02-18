using System;
using System.Windows.Forms;
using Rb.Tools.LevelEditor.Core.Properties;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public partial class ServicesEditorForm : Form
	{
		public ServicesEditorForm( )
		{
			InitializeComponent( );
		}

		private void ServicesEditorForm_Shown( object sender, EventArgs e )
		{
			Scene scene = EditorState.Instance.CurrentScene;

			TreeNode sceneNode = servicesTree.Nodes.Add( Resources.Scene );

			foreach ( object service in scene.Services )
			{
				TreeNode serviceNode = new TreeNode( service.GetType( ).Name );
				serviceNode.Tag = service;
				sceneNode.Nodes.Add( serviceNode );
			}
		}

		private void servicesTree_AfterSelect( object sender, TreeViewEventArgs e )
		{
			TreeNode node = servicesTree.SelectedNode;
			if ( node != null )
			{
				objectPropertyGrid.SelectedObject = node.Tag;
			}
		}
	}
}