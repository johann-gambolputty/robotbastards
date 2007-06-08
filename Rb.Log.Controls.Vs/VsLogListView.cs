using System;
using EnvDTE;
using System.Windows.Forms;

namespace Rb.Log.Controls.Vs
{
    public partial class VsLogListView : Rb.Log.Controls.LogListView
    {
        public VsLogListView()
        {
            InitializeComponent();
        }

        private void VsLogListView_DoubleClick(object sender, EventArgs e)
        {
            Rb.Log.Entry entry = ( Rb.Log.Entry )SelectedItems[ 0 ].Tag;

            //  Get an existing vs or create one
            DTE dte = ( DTE )System.Runtime.InteropServices.Marshal.GetActiveObject( "VisualStudio.DTE.8.0" );
            if ( dte == null )
            {
                dte = ( DTE )Microsoft.VisualBasic.Interaction.CreateObject("VisualStudio.DTE.8.0", "");
            }

            //  Open the file and goto the correct line
            try 
            {
                Window win = dte.ItemOperations.OpenFile( entry.File, Constants.vsViewKindTextView );
                Document doc = win == null ? null : win.Document;
                if ( doc != null )
                {
                    TextDocument textDoc = doc.Object( "TextDocument" ) as TextDocument;
                    if ( textDoc != null )
                    {
                        textDoc.Selection.GotoLine( entry.Line, false );
                    }
                }
            }
            catch ( System.Runtime.InteropServices.COMException ex )
            {
                MessageBox.Show( string.Format( "Couldn't get Visual Studio to open document \"{0}\"\n{1}", System.IO.Path.GetFileName( entry.File ), ex.Message ) );
            }
        }
    }
}