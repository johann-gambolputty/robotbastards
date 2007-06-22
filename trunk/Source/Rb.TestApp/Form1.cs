using System;
using System.Configuration;
using System.Windows.Forms;

using Rb.Core.Resources;
using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.World;
using Rb.Log;

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
			Rb.Rendering.RenderFactory.Load( renderAssembly );

            InitializeComponent();
		}

		private void Form1_Load( object sender, EventArgs e )
		{
			string resourceSetupPath = ConfigurationManager.AppSettings[ "resourceSetupPath" ];
			if ( resourceSetupPath == null )
			{
				resourceSetupPath = "../resourceSetup.xml";
			}
			ResourceManager.Instance.Setup( resourceSetupPath );

            Scene scene = new Scene( );

            //  Set the global scene builder (erk...)
            Builder.Instance = new SceneBuilder( scene, Builder.Instance );

            try
            {
                ComponentLoadParameters loadParams = new ComponentLoadParameters( scene.Objects, scene );
                ResourceManager.Instance.Load( "scene0.components.xml", loadParams );

                display1.AddViewer( new Viewer( new Rb.Rendering.Cameras.SphereCamera( ), scene ) );
            }
            catch ( Exception ex )
            {
                ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
            }
		}
    }
}