using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Templates;
using Poc1.Bob.Properties;

namespace Poc1.Bob.Controls.Templates
{
	public partial class TemplateSelectorView : UserControl, ITemplateSelectorView
	{
		public TemplateSelectorView( )
		{
			InitializeComponent( );
		}

		#region ITemplateSelectorView Members

		/// <summary>
		/// Event raised when the user changes the current selection
		/// </summary>
		public event EventHandler SelectionChanged;

		/// <summary>
		/// Gets/sets the currently displayed descriptive text
		/// </summary>
		public string Description
		{
			get { return descriptionLabel.Text; }
			set { descriptionLabel.Text = value; }
		}

		/// <summary>
		/// Gets the currently selected template base. Returns null if no template group or template is selected
		/// </summary>
		public TemplateBase SelectedTemplateBase
		{
			get
			{
				TreeNode node = templateTreeView.SelectedNode;
				return node == null ? null : node.Tag as TemplateBase;
			}
		}

		/// <summary>
		/// Gets the currently selected template. Returns null if no template is selected
		/// </summary>
		public Template SelectedTemplate
		{
			get
			{
				TreeNode node = templateTreeView.SelectedNode;
				return node == null ? null : node.Tag as Template;
			}
		}

		/// <summary>
		/// Gets/sets the root template group
		/// </summary>
		public TemplateGroupContainer RootGroup
		{
			get { return m_RootGroup; }
			set
			{
				if ( m_RootGroup != value )
				{
					m_RootGroup = value;
					UpdateView( );
				}
			}
		}

		/// <summary>
		/// Refreshes the view (TODO: AP: Lazy... should watch for updates on the model)
		/// </summary>
		public new void Refresh( )
		{
			UpdateView( );
		}

		#endregion

		#region Private Members

		private TemplateGroupContainer m_RootGroup;
		private readonly Dictionary<Icon, int> m_ImageMap = new Dictionary<Icon, int>( );

		/// <summary>
		/// The default icon for groups
		/// </summary>
		private static Icon s_DefaultGroupIcon = Resources.templateGroupDefaultIcon;

		/// <summary>
		/// The default icon for templates
		/// </summary>
		private static Icon s_DefaultTemplateIcon = Resources.templateDefaultIcon;

		/// <summary>
		/// Updates the view
		/// </summary>
		private void UpdateView( )
		{
			templateTreeView.Nodes.Clear( );

			if ( m_RootGroup == null )
			{
				return;
			}
			templateTreeView.Nodes.Add( CreateNodeForGroup( m_RootGroup ) );
			templateTreeView.ExpandAll( );
		}

		/// <summary>
		/// Finds or adds an icon to the image list
		/// </summary>
		private int FindOrAddIcon( Icon icon )
		{
			int imgIndex;
			if ( !m_ImageMap.TryGetValue( icon, out imgIndex ) )
			{
				imgIndex = templateImageList.Images.Count;
				templateImageList.Images.Add( icon );
				m_ImageMap[ icon ] = imgIndex;
			}
			return imgIndex;
		}

		/// <summary>
		/// Creates a new tree node for a template group
		/// </summary>
		private TreeNode CreateNodeForGroup( TemplateGroupContainer groupContainer )
		{
			TreeNode node = new TreeNode( groupContainer.Name );
			node.Tag = groupContainer;
			node.SelectedImageIndex = node.ImageIndex = FindOrAddIcon( groupContainer.Icon ?? s_DefaultGroupIcon );

			foreach ( TemplateGroupContainer subGroup in groupContainer.SubGroups )
			{
				node.Nodes.Add( CreateNodeForGroup( subGroup ) );
			}

			TemplateGroup group = groupContainer as TemplateGroup;
			if ( group != null )
			{
				foreach ( Template template in group.Templates )
				{
					node.Nodes.Add( CreateNodeForTemplate( template ) );
				}
			}

			return node;
		}

		/// <summary>
		/// Creates a new tree node for a template
		/// </summary>
		private TreeNode CreateNodeForTemplate( Template template )
		{
			TreeNode node = new TreeNode( template.Name );
			node.Tag = template;
			node.SelectedImageIndex = node.ImageIndex = FindOrAddIcon( template.Icon ?? s_DefaultTemplateIcon );
			return node;
		}

		#region Event Handlers

		private void templateTreeView_AfterSelect( object sender, TreeViewEventArgs e )
		{
			if ( SelectionChanged != null )
			{
				SelectionChanged( sender, e );
			}
		}
		
		#endregion

		#endregion

	}
}
