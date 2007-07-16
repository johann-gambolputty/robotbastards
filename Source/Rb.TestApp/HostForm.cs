using System;
using System.Windows.Forms;

using Rb.Core.Resources;
using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Network;
using Rb.Network.Runt;
using Rb.Rendering;
using Rb.World;
using Rb.Log;
using Rb.Interaction;

namespace Rb.TestApp
{
    public partial class HostForm : Form
    {
        public HostForm( HostSetup setup )
		{
			m_Setup = setup;
            InitializeComponent();

			Text = string.Format( "{0} - {1}", m_Setup.HostType, m_Setup.HostGuid );
		}

		private HostSetup	m_Setup;
		private CommandUser	m_User = new CommandUser( );

		/// <summary>
		/// Updates the user every tick of the inputClock
		/// </summary>
		private void UpdateUser( Clock clock )
		{
			//	TODO: AP: Kludge
			m_User.Update( );
		}

		private void HostForm_Load( object sender, EventArgs e )
		{
			//	Load input bindings
			CommandInputTemplateMap map = ( CommandInputTemplateMap )ResourceManager.Instance.Load( m_Setup.InputFile );
			m_User.InitialiseAllCommandListBindings( );

			//	Test load a scene
            Scene scene = new Scene( );

			//	Add a scene host
			scene.AddService( new Host( m_Setup.HostType, m_Setup.HostGuid ) );
			if ( m_Setup.HostType != HostType.Local )
			{
				IConnections connections = new Connections( );
				scene.AddService( connections );
				scene.AddService( new UpdateTarget( connections ) );
				scene.AddService( new UpdateSource( connections ) );

				RemoteHostAddress server = m_Setup.ServerAddress;
				if ( m_Setup.HostType == HostType.Client )
				{
					TcpSocketConnection connection = new TcpSocketConnection( server.Address, server.Port );
					connection.OpenConnection( );
					connections.Add( connection );
				}
				else
				{
					TcpConnectionListener listener = new TcpConnectionListener( server.Address, server.Port );
					listener.Listen( connections );
					scene.AddService( listener );
				}
			}

			//	Create a viewer for the scene
            try
            {
				ComponentLoadParameters loadParams = new ComponentLoadParameters( scene.Objects, scene.Builder, scene );
				loadParams.Properties[ "User" ] = m_User;
                ResourceManager.Instance.Load( m_Setup.SceneFile, loadParams );

				//	Naughty, just reuse loadParams (null out target because we don't want to load -into- the scene)
            	loadParams.Target = null;
				loadParams.Properties[ "Subject" ] = scene;
				Viewer viewer = ( Viewer )ResourceManager.Instance.Load( m_Setup.ViewerFile, loadParams );
				display1.AddViewer( viewer );
            }
            catch ( Exception ex )
            {
                ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
            }

			scene.GetClock( "inputClock" ).Subscribe( UpdateUser );

			//	Test load a command list
			try
			{
				//	TODO: AP: May need to move
				map.AddContextInputsToUser( new InputContext( display1.Viewers[ 0 ], display1 ), m_User );
			}
			catch ( Exception ex )
			{
				ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
			}
		}
    }
}