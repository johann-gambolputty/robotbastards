using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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

            Rb.Core.WorldLog.Verbose("World verbose");
            Rb.Core.ComponentLog.Info("Component info");
            Rb.Core.NetworkLog.Warning("Network warning");
            Rb.Core.ResourcesLog.Error("Resources error");
		}
    }
}