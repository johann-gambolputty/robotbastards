using System;
using System.Windows.Forms;

using Rb.Core.Resources;
using Rb.Core.Components;
using Rb.Core.Utils;

using Rb.World;
using Rb.Log;

namespace Rb.TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
		}

		private void Form1_Load ( object sender, EventArgs e )
		{
		    AppLog.Info( "Beginning Rb.TestApp at {0}", DateTime.Now );

			ResourceManager.Instance.Setup( "../resourceSetup.xml" );

            Scene scene = new Scene( );

            //  Set the global scene builder (erk...)
            Builder.Instance = new SceneBuilder(scene, Builder.Instance );

            try
            {
                ComponentLoadParameters loadParams = new ComponentLoadParameters( scene );
                ResourceManager.Instance.Load( "scene0.components.xml", loadParams );
            }
            catch ( Exception ex )
            {
                ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
            }
		}
    }
}