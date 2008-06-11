using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Rb.NiceControls
{
	[CustomUITypeEditor( typeof( IFunction1d ) )]
	public class GraphUITypeEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
		{
			IWindowsFormsEditorService edSvc = ( IWindowsFormsEditorService )provider.GetService( typeof( IWindowsFormsEditorService ) );

			GraphEditorControl control;
			if ( value == null )
			{
				control = new GraphEditorControl( );
			}
			else
			{
				control = new GraphEditorControl( );
			}
			edSvc.DropDownControl( control );

			return control.Function;
		}

		public override bool IsDropDownResizable
		{
			get { return true; }
		}
	}
}
