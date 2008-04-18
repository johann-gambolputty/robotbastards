using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Rb.Core.Utils;

namespace Rb.ProfileViewerControls
{
	public partial class ProfileSectionTreeView : TreeView
	{
		public ProfileSectionTreeView( )
		{
			CheckBoxes = true;
			FullRowSelect = true;
			ShowPlusMinus = false;
			ShowRootLines = false;
			AfterCheck += ProfileSectionTreeView_AfterCheck;
			BeforeCollapse += ProfileSectionTreeView_BeforeCollapse;

			InitializeComponent( );
		}

		public event EventHandler SectionSelectionChanged;

		public ProfileSection[] SectionSelection
		{
			get { return m_CheckedSections.ToArray( ); }
		}
		
		public ProfileSection RootSection
		{
			set
			{
				Nodes.Clear( );

				foreach ( ProfileSection section in value.SubSections )
				{
					TreeNode node = CreateNodeForSection( section );
					node.ExpandAll( );
					Nodes.Add( node );
				}
			}
		}

		private readonly List<ProfileSection> m_CheckedSections = new List<ProfileSection>();
		
		private TreeNode CreateNodeForSection( ProfileSection section )
		{
			TreeNode node = new TreeNode( section.Name );
			node.Tag = section;

			foreach ( ProfileSection subSection in section.SubSections )
			{
				node.Nodes.Add( CreateNodeForSection( subSection ) );
			}

			return node;
		}

		private void CheckNode( TreeNode node, bool check )
		{
			ProfileSection section = ( ProfileSection )( node.Tag );
			node.Checked = check;
			if ( check )
			{
				if ( !m_CheckedSections.Contains( section ) )
				{
					m_CheckedSections.Add( section );
				}
			}
			else
			{
				m_CheckedSections.Remove( section );
			}
			
			foreach ( TreeNode childNode in node.Nodes )
			{
				CheckNode( childNode, check );
			}
			
		}

		private static void ProfileSectionTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
		{
			e.Cancel = true;
		}

		private void ProfileSectionTreeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			AfterCheck -= ProfileSectionTreeView_AfterCheck;
			CheckNode( e.Node, e.Node.Checked );
			AfterCheck += ProfileSectionTreeView_AfterCheck;

			if ( SectionSelectionChanged != null )
			{
				SectionSelectionChanged( this, null );
			}
		}
	}
}
