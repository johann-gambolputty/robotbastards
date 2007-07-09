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
    public partial class Form1 : Form
    {
        public Form1()
		{
			LogViewer viewer = new LogViewer( );
			viewer.Show( this );

			AppLog.Info( "Beginning Rb.TestApp at {0}", DateTime.Now );
			AppLog.GetSource( Severity.Info ).WriteEnvironment( );

			string renderAssembly = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssembly  == null )
			{
				renderAssembly = "Rb.Rendering.OpenGl.Windows";
			}
			RenderFactory.Load( renderAssembly );

            InitializeComponent();
		}

        private Camera3 CreateSimpleCamera( IBuilder builder, CommandList cameraCommands )
        {
            Camera3 result = new SphereCamera( );

			SphereCameraController controller = Builder.CreateInstance<SphereCameraController>( builder );
            result.AddChild( controller );
            result.AddChild( Builder.CreateInstance < CommandInputListener >( builder, controller, cameraCommands ) );

            return result;
        }

		private void Form1_Load( object sender, EventArgs e )
		{
			//	Load resource settings
			string resourceSetupPath = ConfigurationManager.AppSettings[ "resourceSetupPath" ];
			if ( resourceSetupPath == null )
			{
				resourceSetupPath = "../resourceSetup.xml";
			}
			ResourceManager.Instance.Setup( resourceSetupPath );

			//	Create the test and camera command lists (must come before scene creation, because it's referenced
			//	by the scene setup file)
			CommandList.BuildFromEnum( typeof( TestCommands ) );
            CommandList cameraCommands = CommandList.BuildFromEnum( typeof( CameraCommands ) );

			//	Test load a scene
            Scene scene = new Scene( );

			//	Add a scene host
			//	TODO: AP: Remove host hard-coding
			HostType hostType = HostType.Server;
			//Guid hostGuid = Guid.NewGuid( );
			Guid hostGuid = new Guid( "{8D7200EA-0D49-4384-9A44-2532ECB1FE55}" );
			scene.AddService( new Host( hostType, hostGuid ) );
			if ( hostType != HostType.Local )
			{
				IConnections connections = new Connections( );
				scene.AddService( connections );
				scene.AddService( new UpdateTarget( connections ) );
				scene.AddService( new UpdateSource( connections ) );
			}

			//	Create a viewer for the scene
            try
            {
				ComponentLoadParameters loadParams = new ComponentLoadParameters( scene.Objects, scene.Builder, scene );
                ResourceManager.Instance.Load( "scene0.components.xml", loadParams );

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
				CommandInputTemplateMap map = ( CommandInputTemplateMap )ResourceManager.Instance.Load( "testCommandInputs0.components.xml" );
				map.CreateInputsForContext( new InputContext( display1.Viewers[ 0 ], display1 ) );
			}
			catch ( Exception ex )
			{
				ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
			}

		}
    }
}