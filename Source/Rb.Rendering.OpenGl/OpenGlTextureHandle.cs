using System;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Just a wrapper around the standard opengl texture handle. It's disposable, so it can be used with the
	/// method <see cref="OpenGlRenderer.DisposeRenderingResource"/>
	/// </summary>
	public class OpenGlTextureHandle : IDisposable
	{
		public const int InvalidHandle = -1;

		public OpenGlTextureHandle( int handle )
		{
			m_Handle = handle;
		}

		/// <summary>
		/// Creates a texture handle
		/// </summary>
		public static int CreateHandle( )
		{
			//	Generate a texture name
			int[] handles = new int[ 1 ];
			Gl.glGenTextures( 1, handles );
			int textureHandle = handles[ 0 ];

			return textureHandle;
		}

		#region IDisposable Members

		public void Dispose( )
		{
			if ( m_Handle != InvalidHandle )
			{
				Gl.glDeleteTextures( 1, new int[] { m_Handle } );
				m_Handle = InvalidHandle;
			}
		}

		#endregion
	
		#region Private Members
	
		private int m_Handle; 

		#endregion
	}
}
