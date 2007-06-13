using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Rb.Core.Resources;

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

			ResourceManager.Instance.Load( "scene0.components.xml" );
		}
    }
}