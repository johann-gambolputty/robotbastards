
using System;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// A target that can be rendered to, instead of the frame buffer
	/// </summary>
	/// <remarks>
	/// Beginning a render target pass redirects all further rendering to the target rather than the
	/// frame buffer. Ending a render target pass reverts to frame buffer rendering
	/// </remarks>
	public interface IRenderTarget : IPass, IDisposable
	{
		/// <summary>
		/// Gets the underlying texture. This is null if the render target has not been created, or was created without a colour buffer
		/// </summary>
		ITexture2d Texture
		{
			get;
		}

		/// <summary>
		/// Gets the underlying depth texture. This is null if the render target has not been created, or was created without a depth buffer as texture
		/// </summary>
		ITexture2d DepthTexture
		{
			get;
		}

		/// <summary>
		/// Gets the width of the render target
		/// </summary>
		int Width
		{
			get;
		}

		/// <summary>
		/// Gets the height of the render target
		/// </summary>
		int Height
		{
			get;
		}

		/// <summary>
		/// Creates the render target, with optional colour, depth and stencil buffers, and optionally storing
		/// the depth buffer in a separate texture
		/// </summary>
		/// <param name="width">Width of the render target</param>
		/// <param name="height">Height of the render target</param>
		/// <param name="colourFormat">Format of the render target colour buffer. If this is Undefined, then no colour buffer is created</param>
		/// <param name="depthBits">Number of bits per element in the depth buffer (0 for no depth buffer)</param>
		/// <param name="stencilBits">Number of bits per element in the stencil buffer (0 for no stencil buffer)</param>
		/// <param name="depthBufferAsTexture">If true, then depth buffer storage is created in a texture</param>
		void Create( int width, int height, TextureFormat colourFormat, int depthBits, int stencilBits, bool depthBufferAsTexture );

		/// <summary>
		/// Diagnostic function for saving the depth buffer to a file
		/// </summary>
		void SaveDepthBuffer( string path );
	}
}
