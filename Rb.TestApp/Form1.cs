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
        public void MessageGenerator( )
        {
            Random rnd = new Random( );
            for ( int i = 0; i < 10; ++i )
            {
                Rb.Log.Source src = Rb.Log.App.Info;
                switch ( rnd.Next( ) % 4 )
                {
                    case 0: src = Rb.Log.App.Verbose;   break;
                    case 1: src = Rb.Log.App.Info;      break;
                    case 2: src = Rb.Log.App.Warning;   break;
                    case 3: src = Rb.Log.App.Error;     break;
                }

                src.Write("badgers {0}\r\ntest", i);
                
                System.Threading.Thread.Sleep( rnd.Next( ) % 1000 );
            }
        }

        public Form1()
        {
            InitializeComponent();

            new System.Threading.Thread( new System.Threading.ThreadStart( MessageGenerator ) ).Start( );
            new System.Threading.Thread( new System.Threading.ThreadStart( MessageGenerator ) ).Start( );
            new System.Threading.Thread( new System.Threading.ThreadStart( MessageGenerator ) ).Start( );
        }
    }
}