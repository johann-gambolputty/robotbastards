using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Rb.NiceControls
{
	/// <summary>
	/// A type editor for IFunction1d parameters
	/// </summary>
	/// <remarks>
	/// Example:
	/// <code>
	/// class MyClass
	/// {
	///		[Editor( typeof( GraphUITypeEditor ) )]
	///		public IFunction1d Function
	///		{
	///			get {...} set {...}
	///		}
	/// }
	/// </code>
	/// </remarks>
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

			GraphEditorControl control = new GraphEditorControl( );
			if ( value != null )
			{
				control.Function = ( IFunction1d )value;
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
