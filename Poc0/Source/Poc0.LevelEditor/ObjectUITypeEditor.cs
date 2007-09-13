using System.Drawing.Design;
using System.Windows.Forms;

namespace Poc0.LevelEditor.Core
{
	public class ObjectUITypeEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle( System.ComponentModel.ITypeDescriptorContext context )
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue( System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value )
		{
			ObjectUITypeEditorForm form = new ObjectUITypeEditorForm( );

			if ( form.ShowDialog( ) == DialogResult.Cancel )
			{
				return value;
			}

			return form.NewObject;
		}
	}
}
