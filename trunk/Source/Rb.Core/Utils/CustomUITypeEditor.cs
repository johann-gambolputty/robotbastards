
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Rb.Core.Utils
{
	/// <summary>
	/// UI type editor, that decorates another UI type editor created from the <see cref="UITypeEditorFactory"/>
	/// </summary>
	public class CustomUITypeEditor<T> : UITypeEditor
	{
		public CustomUITypeEditor( )
		{
			if ( s_ImplType != null )
			{
				m_Impl = ( UITypeEditor )Activator.CreateInstance( s_ImplType );
			}
		}

		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
		{
			if ( m_Impl == null )
			{
				return base.EditValue( context, provider, value );
			}
			if ( ( value != null ) && ( !( value is T ) ) )
			{
				throw new ArgumentException( string.Format( "Value was invalid type ({0}) - must be castable to type {1}", value.GetType( ), typeof( T ) ), "value" );
			}
			return m_Impl.EditValue( context, provider, value );
		}

		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
			if ( m_Impl == null )
			{
				return base.GetEditStyle( context );
			}
			return m_Impl.GetEditStyle( context );
		}

		public override bool GetPaintValueSupported( ITypeDescriptorContext context )
		{
			if ( m_Impl == null )
			{
				return base.GetPaintValueSupported( context );
			}
			return m_Impl.GetPaintValueSupported(context);
		}

		public override bool IsDropDownResizable
		{
			get { return m_Impl == null ? base.IsDropDownResizable : m_Impl.IsDropDownResizable; }
		}

		public override void PaintValue( PaintValueEventArgs e )
		{
			if ( m_Impl == null )
			{
				base.PaintValue( e );
			}
			else
			{
				m_Impl.PaintValue( e );
			}
		}

		private readonly UITypeEditor m_Impl;
		private readonly static Type s_ImplType;

		static CustomUITypeEditor( )
		{
			s_ImplType = UITypeEditorFactory.GetCustomUITypeEditorType( typeof( T ) );
		}
	}
}
