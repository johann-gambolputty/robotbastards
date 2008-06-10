using System;
using System.ComponentModel;
using System.Drawing.Design;
using Rb.Core.Utils;

namespace Rb.Rendering.Platform
{
	public class Texture2dUITypeEditor : UITypeEditor
	{
		public Texture2dUITypeEditor( )
		{
			m_Impl = ( UITypeEditor )Activator.CreateInstance( ms_ImplType );
		}

		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
		{
			return m_Impl.EditValue( context, provider, value );
		}

		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
			return m_Impl.GetEditStyle( context );
		}

		public override bool GetPaintValueSupported( ITypeDescriptorContext context )
		{
			return m_Impl.GetPaintValueSupported(context);
		}

		public override bool IsDropDownResizable
		{
			get { return m_Impl.IsDropDownResizable; }
		}

		public override void PaintValue( PaintValueEventArgs e )
		{
			m_Impl.PaintValue( e );
		}

		private readonly UITypeEditor m_Impl;
		private readonly static Type ms_ImplType;

		static Texture2dUITypeEditor( )
		{
			//	TODO: AP: Remove this filthy hack. Add a platform factory to IGraphicsFactory, or something
			ms_ImplType = AppDomainUtils.FindType( "Rb.Rendering.Windows.Texture2dAssetEditor" );
		}
	}
}
