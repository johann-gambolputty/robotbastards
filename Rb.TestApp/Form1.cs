using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Rb.Log;
using Rb.Core;

namespace Rb.TestApp
{
    public partial class Form1 : Form
    {
        public void MessageGenerator( )
        {
            Random rnd = new Random( );
            for ( int i = 0; i < 10; ++i )
            {
				Severity severity = ( Severity )( rnd.Next( ) % ( int )Severity.Count );

                App.GetSource( severity ).Write("badgers {0}\r\ntest", i);
                
                System.Threading.Thread.Sleep( rnd.Next( ) % 1000 );
            }
        }

		private void TestTag( Tag t )
		{
			t.Verbose( "blah" );
			t.Info( "blah" );
			t.Warning( "blah" );
			t.Error( "blah" );
		}

        public Form1()
        {
            InitializeComponent();

        }

		private void Form1_Load ( object sender, EventArgs e )
		{
			TestTag( ResourcesLog.Tag );
			TestTag( MathsLog.Tag );
			TestTag( WorldLog.Tag );
			TestTag( NetworkLog.RuntLog.Tag );

			new System.Threading.Thread( new System.Threading.ThreadStart( MessageGenerator ) ).Start( );
			new System.Threading.Thread( new System.Threading.ThreadStart( MessageGenerator ) ).Start( );
			new System.Threading.Thread( new System.Threading.ThreadStart( MessageGenerator ) ).Start( );

		}
    }
}