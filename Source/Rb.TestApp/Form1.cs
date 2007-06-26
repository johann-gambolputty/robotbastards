using System;
using System.Configuration;
using System.Windows.Forms;

using Rb.Core.Resources;
using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Rendering;
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

		private void Form1_Load( object sender, EventArgs e )
		{
			//	Load resource settings
			string resourceSetupPath = ConfigurationManager.AppSettings[ "resourceSetupPath" ];
			if ( resourceSetupPath == null )
			{
				resourceSetupPath = "../resourceSetup.xml";
			}
			ResourceManager.Instance.Setup( resourceSetupPath );

			//	Create the test command list (must come before scene creation, because it's referenced
			//	by the scene setup file)
			CommandList.BuildFromEnum( typeof( TestCommands ) );

			//	Test load a scene
            Scene scene = new Scene( );

            //  Set the global scene builder (erk...)
            Builder.Instance = new SceneBuilder( scene, Builder.Instance );

			//	Create a viewer for the scene
            try
            {
                ComponentLoadParameters loadParams = new ComponentLoadParameters( scene.Objects, scene );
                ResourceManager.Instance.Load( "scene0.components.xml", loadParams );

                display1.AddViewer( new Viewer( new Rendering.Cameras.SphereCamera( ), scene ) );
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