using System.Windows.Forms;
using Goo.Common.Ui.Controllers.PropertyEditor;

namespace Goo.Common.Ui.WinForms.Controllers.PropertyEditor
{
	public partial class PropertyEditorView : PropertyGrid, IPropertyEditorView
	{
		public PropertyEditorView( )
		{
			InitializeComponent( );
		}
	}
}
