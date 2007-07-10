using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

using Rb.Core.Resources;
using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Network;
using Rb.Network.Runt;
using Rb.Rendering;
using Rb.Rendering.Cameras;
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

		private HostSetup m_Setup;

        private static Camera3 CreateSimpleCamera( IBuilder builder, CommandList cameraCommands )
        {
            Camera3 result = new SphereCamera( );

			SphereCameraController controller = Builder.CreateInstance<SphereCameraController>( builder );
            result.AddChild( controller );
            result.AddChild( Builder.CreateInstance < CommandInputListener >( builder, controller, cameraCommands ) );

            return result;
        }

		private void HostForm_Load( object sender, EventArgs e )
		{
			//	Create the test and camera command lists (must come before scene creation, because it's referenced
			//	by the scene setup file)
			CommandList.BuildFromEnum( typeof( TestCommands ) );
            CommandList cameraCommands = CommandList.BuildFromEnum( typeof( CameraCommands ) );

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
                ResourceManager.Instance.Load( m_Setup.SceneFile, loadParams );

                Viewer viewer = new Viewer( CreateSimpleCamera( scene.Builder, cameraCommands ), scene );
                viewer.ShowFps = true;
				viewer.Technique = Builder.CreateInstance< World.Rendering.SceneShadowBufferTechnique >( scene.Builder );
                display1.AddViewer( viewer );
				
                viewer = new Viewer( CreateSimpleCamera( scene.Builder, cameraCommands ), scene );
				//viewer.Technique = Builder.CreateInstance< World.Rendering.SceneShadowBufferTechnique >( scene.Builder );
				viewer.ViewRect = new RectangleF( 0, 0, 0.3f, 0.3f );
                display1.AddViewer( viewer );
            }
            catch ( Exception ex )
            {
                ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
            }

			//	Test load a command list
			try
			{
				CommandInputTemplateMap map = ( CommandInputTemplateMap )ResourceManager.Instance.Load( m_Setup.InputFile );
				map.CreateInputsForContext( new InputContext( display1.Viewers[ 0 ], display1 ) );
			}
			catch ( Exception ex )
			{
				ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
			}

		}
    }
}