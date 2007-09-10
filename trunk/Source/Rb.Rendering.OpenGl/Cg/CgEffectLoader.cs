using System;
using System.IO;
using Rb.Core.Assets;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// Summary description for CgRenderEffectLoader.
	/// </summary>
	public class CgEffectLoader : AssetLoader
	{

		/// <summary>
		/// Creates the internal CG context that effects are created from
		/// </summary>
		public CgEffectLoader( )
		{
			m_Context = Tao.Cg.Cg.cgCreateContext( );
			
			Tao.Cg.CgGl.cgGLRegisterStates( m_Context );
		}

		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		public override object Load( ISource source, LoadParameters parameters )
		{
			parameters.CanCache = true;
			using ( Stream stream = source.Open( ) )
			{
				return new CgEffect( m_Context, stream, source.ToString( ) );
			}
		}

		/// <summary>
		/// Returns true if this loader can load the specified stream
		/// </summary>
		public override bool CanLoad( ISource source )
		{
			return source.ToString( ).EndsWith(".cgfx");
		}

		private readonly IntPtr m_Context;
	}
}
