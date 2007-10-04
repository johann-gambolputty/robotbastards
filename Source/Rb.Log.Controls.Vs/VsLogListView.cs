using System;
using System.Windows.Forms;
using EnvDTE80;
using EnvDTE;

namespace Rb.Log.Controls.Vs
{
    public partial class VsLogListView : LogListView
    {
        public VsLogListView()
        {
            InitializeComponent();
        }

        private void VsLogListView_DoubleClick(object sender, EventArgs e)
        {
            Entry entry = ( Entry )SelectedItems[ 0 ].Tag;

			if ( !System.IO.File.Exists( entry.File ) )
			{
				string error = string.Format( Properties.Resources.VsLogListView_CouldNotFindFile, entry.File );
				MessageBox.Show( error );
				return;
			}

            //  Get an existing vs or create one
            DTE2 dte = ( DTE2 )System.Runtime.InteropServices.Marshal.GetActiveObject( "VisualStudio.DTE.8.0" );
            if ( dte == null )
            {
                dte = ( DTE2 )Microsoft.VisualBasic.Interaction.CreateObject( "VisualStudio.DTE.8.0", "" );
            }

            //  Open the file and goto the correct line
            try 
            {
                Window win = dte.ItemOperations.OpenFile( entry.File, Constants.vsViewKindCode );
                Document doc = win == null ? null : win.Document;
                if ( doc != null )
                {
                    TextDocument textDoc = doc.Object( "TextDocument" ) as TextDocument;
                    if ( textDoc != null )
                    {
                        textDoc.Selection.GotoLine( entry.Line, false );
                    }
					dte.MainWindow.SetFocus( );
					dte.MainWindow.Activate( );
                }
            }
            catch ( System.Runtime.InteropServices.COMException ex )
            {
                MessageBox.Show( string.Format( "Couldn't get Visual Studio to open document \"{0}\"\n{1}", System.IO.Path.GetFileName( entry.File ), ex.Message ) );
            }
        }
    }
}