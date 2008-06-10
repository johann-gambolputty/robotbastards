using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Rb.NiceControls
{
	[CustomUITypeEditor( typeof( Functions.FunctionDelegate ) )]
	public class GraphUITypeEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
		{
			IWindowsFormsEditorService edSvc = ( IWindowsFormsEditorService )provider.GetService( typeof( IWindowsFormsEditorService ) );

			edSvc.DropDownControl( new GraphEditorControl( ) );

			return base.EditValue( context, provider, value );
		}

		public override bool IsDropDownResizable
		{
			get { return true; }
		}
	}
}
