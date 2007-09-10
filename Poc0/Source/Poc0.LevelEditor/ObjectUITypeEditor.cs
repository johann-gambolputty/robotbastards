using System.Drawing.Design;

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
			return base.EditValue( context, provider, value );
		}
	}
}
