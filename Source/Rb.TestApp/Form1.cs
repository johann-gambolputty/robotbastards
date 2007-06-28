using System;
using System.Configuration;
using System.Windows.Forms;

using Rb.Core.Resources;
using Rb.Core.Components;
using Rb.Core.Utils;
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

			string renderAssembly = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssembly  == null )
			{
				renderAssembly = "Rb.Rendering.OpenGl.Windows";
			}
			RenderFactory.Load( renderAssembly );

            InitializeComponent();
		}

        private Camera3 CreateSimpleCamera( CommandList cameraCommands )
        {
            Camera3 result = new SphereCamera( );
            
            SphereCameraController controller = Builder.CreateInstance< SphereCameraController >( Builder.Instance );
            result.AddChild( controller );
            result.AddChild( Builder.CreateInstance < CommandInputListener >( Builder.Instance, controller, cameraCommands ) );

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
            CommandList cameraCommands = CommandList.BuildFromEnum(typeof(CameraCommands));

			//	Test load a scene
            Scene scene = new Scene( );

            //  Set the global scene builder (erk...)
            Builder.Instance = new SceneBuilder( scene, Builder.Instance );

			//	Create a viewer for the scene
            try
            {
                ComponentLoadParameters loadParams = new ComponentLoadParameters( scene.Objects, scene );
                ResourceManager.Instance.Load( "scene0.components.xml", loadParams );

                Viewer viewer = new Viewer( CreateSimpleCamera( cameraCommands ), scene );
                display1.AddViewer( viewer );
                viewer.ShowFps = true;
                viewer.Technique = Builder.CreateInstance< World.Rendering.SceneShadowBufferTechnique >( Builder.Instance );
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