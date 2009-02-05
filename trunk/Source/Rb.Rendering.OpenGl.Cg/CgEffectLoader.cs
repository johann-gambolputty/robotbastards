using System;
using Rb.Assets;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;
using TaoCg = Tao.Cg.Cg;
using TaoCgGl = Tao.Cg.CgGl;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// Loads cgfx effects files
	/// </summary>
	public class CgEffectLoader : AssetLoader
	{

		/// <summary>
		/// Creates the internal CG context that effects are created from
		/// </summary>
		public CgEffectLoader( )
		{
		}

		/// <summary>
		/// Gets the asset name
		/// </summary>
		public override string Name
		{
			get { return "CG Effect"; }
		}

		/// <summary>
		/// Gets the asset extension
		/// </summary>
		public override string[] Extensions
		{
			get { return new string[] { "cgfx" }; }
		}

		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		public override object Load( ISource source, LoadParameters parameters )
		{
			if ( m_Context == IntPtr.Zero )
			{
				CreateContext( );
			}
			parameters.CanCache = true;
			return new CgEffect( m_Context, ( IStreamSource )source );
		}

		private IntPtr m_Context = IntPtr.Zero;

		/// <summary>
		/// Creates the current CG context
		/// </summary>
		private void CreateContext( )
		{
			GraphicsLog.Info( "Creating CG context" );
			m_Context = TaoCg.cgCreateContext( );

			int vsProfile = TaoCgGl.cgGLGetLatestProfile( TaoCgGl.CG_GL_VERTEX );
			int fsProfile = TaoCgGl.cgGLGetLatestProfile( TaoCgGl.CG_GL_FRAGMENT );
			GraphicsLog.Info( "Vertex shader profile: " + vsProfile );
			GraphicsLog.Info( "Fragment shader profile: " + fsProfile );
			TaoCgGl.cgGLSetOptimalOptions( vsProfile );

			//	Tao.Cg.CgGl.cgGLSetManageTextureParameters( m_Context, true );
			Tao.Cg.CgGl.cgGLRegisterStates( m_Context );
		}
	}
}
