using System;
using System.Windows.Forms;
using Poc0.LevelEditor.Core.Geometry;
using Poc0.LevelEditor.Properties;
using Rb.Log;
using Rb.Tools.LevelEditor.Core;

namespace Poc0.LevelEditor
{
	public partial class MaterialsEditorForm : Form
	{
		public MaterialsEditorForm( )
		{
			InitializeComponent( );
		}

		private void MaterialsEditorForm_Shown( object sender, EventArgs e )
		{
			MaterialSet matSet;
			try
			{
				matSet = MaterialSet.FromScene( EditorState.Instance.CurrentScene );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to retrieve material set from scene" );
				ErrorMessageBox.Show( this, Resources.NoMaterialSetInScene );
				Close( );
				return;
			}

			TreeNode rootNode = new TreeNode( matSet.Name );
			materialsTree.Nodes.Add( rootNode );

			foreach ( Material mat in matSet.Materials )
			{
				TreeNode matNode = new TreeNode( mat.Name );
				matNode.Tag = mat;
				rootNode.Nodes.Add( matNode );
			}
		}

		private void materialsTree_AfterSelect( object sender, TreeViewEventArgs e )
		{
			if ( materialsTree.SelectedNode == null )
			{
				return;
			}
			materialProperties.SelectedObject = materialsTree.SelectedNode.Tag;
		}

	}
}