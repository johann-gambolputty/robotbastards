using System;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Poc1.Bob.Controls.Biomes;
using Rb.Log.Controls.Vs;

namespace Poc1.Bob.Controls
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			InitializeComponent( );

			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = mainDisplay;
			m_DockingManager.OuterControl = this;

			m_LogDisplay = new VsLogListView( );
		}

		private readonly Control m_LogDisplay;
		private readonly DockingManager m_DockingManager;
		private Content m_LogDisplayContent;

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			m_LogDisplayContent = m_DockingManager.Contents.Add( m_LogDisplay, "Log" );
			m_DockingManager.AddContentWithState( m_LogDisplayContent, State.DockBottom );

			Content content = m_DockingManager.Contents.Add( new TerrainFrameLookupTextureControl( ), "Terrain Textures" );
			m_DockingManager.AddContentWithState( content, State.DockBottom );
		}
	}
}