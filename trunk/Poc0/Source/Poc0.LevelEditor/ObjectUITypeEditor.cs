using System;
using System.ComponentModel;
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
		/// Returns true if this editor can handle the specified type
		/// </summary>
		public static bool HandlesType( Type type )
		{
			return TypeDescriptor.GetConverter( type ) is ReferenceConverter;
		}

		/// <summary>
		/// Returns the UITypeEditorEditStyle for this class
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
		    return UITypeEditorEditStyle.Modal;
		}

		/// <summary>
		/// Edits a value
		/// </summary>
		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
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
