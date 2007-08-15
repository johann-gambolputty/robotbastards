using System;
using Rb.Core.Resources;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// Summary description for CgRenderEffectLoader.
	/// </summary>
	public class CgEffectLoader : ResourceStreamLoader
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
		/// <param name="input"> Input stream to load the resource from </param>
		/// <returns> The loaded resource </returns>
		public override Object Load( System.IO.Stream input, string inputSource, out bool canCache )
		{
			canCache = true;
			return new CgEffect( m_Context, input, inputSource );
		}

		/// <summary>
		/// Loads into a resource from a stream
		/// </summary>
		/// <exception cref="ApplicationException"></exception>
		public override Object Load( System.IO.Stream input, string inputSource, out bool canCache, LoadParameters parameters )
		{
			return Load( input, inputSource, out canCache );
		}

		/// <summary>
		/// Returns true if this loader can load the specified stream
		/// </summary>
		/// <param name="path"> Stream path (contains extension that the loader can check)</param>
		/// <param name="input"> Input stream (file types can be identified by peeking at header bytes) </param>
		/// <returns> Returns true if the stream can </returns>
		/// <remarks>
		/// path can be null, in which case, the loader must be able to identify the resource type by checking the content in input (e.g. by peeking
		/// at the header bytes).
		/// </remarks>
		public override bool CanLoadStream( string path, System.IO.Stream input )
		{
			return path.EndsWith( ".cgfx" );
		}

		private IntPtr	m_Context;
	}
}