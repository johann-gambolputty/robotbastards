using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Rb.Core.Resources;
using Rb.Log;
using Rb.Rendering;
using Rb.World;

namespace Rb.TestApp
{
	public partial class ControlPanelForm : Form
	{
		public ControlPanelForm( )
		{
			//	Display a log viewer
			LogViewer viewer = new LogViewer( );
			viewer.Show( );

			//	Write greeting
			AppLog.Info( "Beginning Rb.TestApp at {0}", DateTime.Now );
			AppLog.GetSource( Severity.Info ).WriteEnvironment( );

			//	Load the rendering assembly
			string renderAssembly = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssembly == null )
			{
				renderAssembly = "Rb.Rendering.OpenGl.Windows";
			}
			RenderFactory.Load( renderAssembly );

			//	Load resource settings
			//	NOTE: AP: May cause problems
			string resourceSetupPath = ConfigurationManager.AppSettings[ "resourceSetupPath" ];
			if ( resourceSetupPath == null )
			{
				resourceSetupPath = "../resourceSetup.xml";
			}
			ResourceManager.Instance.Setup( resourceSetupPath );

			//	Load user settings
			m_Settings = UserSettings.Load( );

			InitializeComponent( );

			Color transparent = Color.FromArgb( 0xff, 0x00, 0xff );
			( ( Bitmap )editServerIpAddressButton.Image ).MakeTransparent( transparent );
			( ( Bitmap )browseSceneFileButton.Image ).MakeTransparent( transparent );
			( ( Bitmap )browseInputFileButton.Image ).MakeTransparent( transparent );

			foreach ( ResourceProvider provider in ResourceManager.Instance.Providers )
			{
				resourceProviderCombo.Items.Add( provider );
			}
			resourceProviderCombo.SelectedIndex = 0;

			//	Populate the control panel with the user settings
			sceneFileCombo.Items.AddRange( m_Settings.SceneFileHistory.ToArray( ) );
			inputFileCombo.Items.AddRange( m_Settings.InputFileHistory.ToArray( ) );
			serverIpCombo.Items.AddRange( m_Settings.ServerIpHistory.ToArray( ) );

			sceneFileCombo.SelectedIndex = 0;
			inputFileCombo.SelectedIndex = 0;
			serverIpCombo.SelectedIndex = 0;
		}

		private UserSettings m_Settings;

		private HostSetup CreateSetup( )
		{
			HostSetup setup 	= new HostSetup( );
			setup.SceneFile 	= sceneFileCombo.Text;
			setup.InputFile 	= inputFileCombo.Text;
			setup.ServerAddress	= ( RemoteHostAddress )serverIpCombo.SelectedItem;

			return setup;
		}

		private void clientHostButton_Click( object sender, EventArgs e )
		{
			//	Create a new host
			HostSetup setup = CreateSetup( );
			setup.HostType = HostType.Client;
			setup.HostGuid = new Guid( "{8D7200EA-0D49-4384-9A44-2532ECB1FE55}" );
			HostForm host = new HostForm( setup );
			host.Show( );
		}

		private void serverHostButton_Click( object sender, EventArgs e )
		{
			//	Create a new host
			HostSetup setup = CreateSetup( );
			setup.HostType = HostType.Server;
			setup.HostGuid = new Guid( "{8D7200EA-0D49-4384-9A44-2532ECB1FE55}" );
			HostForm host = new HostForm( setup );
			host.Show( );
		}

		private void localHostButton_Click( object sender, EventArgs e )
		{
			//	Create a new host
			HostSetup setup = CreateSetup( );
			setup.HostType = HostType.Local;
			setup.HostGuid = new Guid( "{8D7200EA-0D49-4384-9A44-2532ECB1FE55}" );
			HostForm host = new HostForm( setup );
			host.Show( );
		}

		private void ControlPanelForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			m_Settings.Save( );
		}

		private void editServerIpAddressButton_Click( object sender, EventArgs e )
		{
			HostAddressForm form = new HostAddressForm( ( RemoteHostAddress )serverIpCombo.SelectedItem );
			if ( form.ShowDialog( this ) == DialogResult.OK )
			{
				int index = serverIpCombo.Items.IndexOf( form.Address );
				if ( index == -1 )
				{
					index = serverIpCombo.Items.Add( form.Address );
					m_Settings.ServerIpHistory.Add( form.Address );
					m_Settings.Save( );
				}
				serverIpCombo.SelectedIndex = index;
			}
		}

		private string GetComponentXmlFilename( )
		{
			IResourceProviderUi providerUi = resourceProviderCombo.SelectedItem as IResourceProviderUi;
			if ( providerUi == null )
			{
				string msg = string.Format( Properties.Resources.ResourceProviderHasNoUi, resourceProviderCombo.SelectedItem );

				MessageBox.Show( msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return null;
			}

			return providerUi.GetResourcePath( Properties.Resources.ComponentXmlFilter );
		}

		private void browseSceneFileButton_Click( object sender, EventArgs e )
		{
			string path = GetComponentXmlFilename( );
			if ( path != null )
			{
				int index = sceneFileCombo.Items.IndexOf( path );
				if ( index == -1 )
				{
					index = sceneFileCombo.Items.Add( path );
					m_Settings.SceneFileHistory.Add( path );
					m_Settings.Save( );
				}
				sceneFileCombo.SelectedIndex = index;
			}
		}

		private void browseInputFileButton_Click( object sender, EventArgs e )
		{
			string path = GetComponentXmlFilename( );
			if ( path != null )
			{
				int index = inputFileCombo.Items.IndexOf( path );
				if ( index == -1 )
				{
					index = inputFileCombo.Items.Add( path );
					m_Settings.InputFileHistory.Add( path );
					m_Settings.Save( );
				}
				inputFileCombo.SelectedIndex = index;
			}
		}
	}
}
