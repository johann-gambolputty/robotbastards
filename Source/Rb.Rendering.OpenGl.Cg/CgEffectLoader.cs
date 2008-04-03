using System;
using Rb.Assets;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;

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
			m_Context = Tao.Cg.Cg.cgCreateContext( );

		//	Tao.Cg.CgGl.cgGLSetManageTextureParameters( m_Context, true );
			Tao.Cg.CgGl.cgGLRegisterStates( m_Context );
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
			parameters.CanCache = true;
			return new CgEffect( m_Context, ( IStreamSource )source );
		}

		private readonly IntPtr m_Context;
	}
}
