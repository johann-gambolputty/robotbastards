using System.Drawing.Design;
using System.Windows.Forms;

namespace Poc0.LevelEditor
{
	/// <summary>
	/// Object type editor
	/// </summary>
	public class ObjectUITypeEditor : UITypeEditor
	{
		/// <summary>
		/// Returns the UITypeEditorEditStyle for this class
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle( System.ComponentModel.ITypeDescriptorContext context )
		{
		    return UITypeEditorEditStyle.DropDown;
		}

		/// <summary>
		/// Edits a value
		/// </summary>
		public override object EditValue( System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value )
		{
			ObjectUITypeEditorForm form = new ObjectUITypeEditorForm( context.PropertyDescriptor.PropertyType );

			if ( form.ShowDialog( ) == DialogResult.Cancel )
			{
				return value;
			}

			return form.NewObject;
		}
	}
}
