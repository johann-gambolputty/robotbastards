using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Rb.Core.Resources;
using Rb.Core.Components;

using Rb.World;

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
		    Rb.Log.App.Info( "Beginning Rb.TestApp at {0}", DateTime.Now );

			ResourceManager.Instance.Setup( "../resourceSetup.xml" );

            Scene scene = new Scene( );

            //  Set the global scene builder (erk...)
            Builder.Instance = new SceneBuilder(scene, Builder.Instance );

            ComponentLoadParameters loadParams = new ComponentLoadParameters( scene );
            ResourceManager.Instance.Load( "scene0.components.xml", loadParams );
		}
    }
}