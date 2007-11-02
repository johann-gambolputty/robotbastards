
using Rb.Rendering.Textures;

namespace Rb.Rendering
{
	/// <summary>
	/// Render target
	/// </summary>
	public abstract class RenderTarget
	{
		/// <summary>
		/// Gets the underlying texture. This is null if the render target has not been created, or was created without a colour buffer
		/// </summary>
		public ITexture2d Texture
		{
			get { return m_Texture; }
		}

		/// <summary>
		/// Gets the underlying depth texture. This is null if the render target has not been created, or was created without a depth buffer as texture
		/// </summary>
		public ITexture2d DepthTexture
		{
			get { return m_DepthTexture; }
		}

		/// <summary>
		/// Gets the width of the render target
		/// </summary>
		public int Width
		{
			get { return m_Width; }
		}

		/// <summary>
		/// Gets the height of the render target
		/// </summary>
		public int Height
		{
			get { return m_Height; }
		}

		/// <summary>
		/// Creates the render target
		/// </summary>
		/// <param name="width">Width of the render target</param>
		/// <param name="height">Height of the render target</param>
		/// <param name="colourFormat">Format of the render target colour buffer. If this is Undefined, then no colour buffer is created</param>
		/// <param name="depthBits">Number of bits per element in the depth buffer (0 for no depth buffer)</param>
		/// <param name="stencilBits">Number of bits per element in the stencil buffer (0 for no stencil buffer)</param>
		/// <param name="depthBufferAsTexture">If true, then depth buffer storage is created in a texture</param>
		public abstract void Create( int width, int height, TextureFormat colourFormat, int depthBits, int stencilBits, bool depthBufferAsTexture );

		/// <summary>
		/// Starts rendering to the render target
		/// </summary>
		public abstract void Begin( );

		/// <summary>
		/// Stops rendering to the render target
		/// </summary>
		public abstract void End( );

		/// <summary>
		/// Diagnostic function for saving the depth buffer to a file
		/// </summary>
		public abstract void SaveDepthBuffer( string path );

		#region	Protected Members

		/// <summary>
		/// Texture that colours are being rendered to
		/// </summary>
		protected ITexture2d m_Texture;


		/// <summary>
		/// Texture that depths are renderer to (if depthBufferAsTexture=true in call to Create())
		/// </summary>
		protected ITexture2d m_DepthTexture;

		/// <summary>
		/// Width of the render target
		/// </summary>
		protected int m_Width;

		/// <summary>
		/// Height of the render target
		/// </summary>
		protected int m_Height;

		#endregion
	}
}
