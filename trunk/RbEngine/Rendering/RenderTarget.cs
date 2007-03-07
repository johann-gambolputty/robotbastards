using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Render target
	/// </summary>
	public abstract class RenderTarget
	{
		/// <summary>
		/// Gets the underlying texture. This is null if the render target has not been created, or was created without a colour buffer
		/// </summary>
		public Texture2d	Texture
		{
			get
			{
				return m_Texture;
			}
		}

		/// <summary>
		/// Gets the width of the render target
		/// </summary>
		public int			Width
		{
			get
			{
				return m_Width;
			}
		}

		/// <summary>
		/// Gets the height of the render target
		/// </summary>
		public int			Height
		{
			get
			{
				return m_Height;
			}
		}

		/// <summary>
		/// Creates the render target
		/// </summary>
		/// <param name="width">Width of the render target</param>
		/// <param name="height">Height of the render target</param>
		/// <param name="colourFormat">Format of the render target colour buffer. If this is Undefined, then no colour buffer is created</param>
		/// <param name="depthBits">Number of bits per element in the depth buffer (0 for no depth buffer)</param>
		/// <param name="stencilBits">Number of bits per element in the stencil buffer (0 for no stencil buffer)</param>
		public abstract void	Create( int width, int height, System.Drawing.Imaging.PixelFormat colourFormat, int depthBits, int stencilBits );

		/// <summary>
		/// Starts rendering to the render target
		/// </summary>
		public abstract void	Begin( );

		/// <summary>
		/// Stops rendering to the render target
		/// </summary>
		public abstract void	End( );

		#region	Protected Members

		/// <summary>
		/// Texture that colours are being rendered to
		/// </summary>
		protected Texture2d	m_Texture;

		/// <summary>
		/// Width of the render target
		/// </summary>
		protected int		m_Width;

		/// <summary>
		/// Height of the render target
		/// </summary>
		protected int		m_Height;

		#endregion
	}
}
