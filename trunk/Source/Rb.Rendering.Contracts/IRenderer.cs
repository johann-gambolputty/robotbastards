using System.Drawing;
using Rb.Rendering.Contracts.Objects;

namespace Rb.Rendering.Contracts
{
	/// <summary>
	/// Manages overall state of the rendering process
	/// </summary>
	public interface IRenderer
	{
		#region Render state

		/// <summary>
		/// Retrieves the topmost render state on the render state stack
		/// </summary>
		IRenderState CurrentRenderState
		{
			get;
		}

		/// <summary>
		/// Pushes a render state onto the render state stack, and applies it
		/// </summary>
		void PushRenderState( IRenderState state );

		/// <summary>
		/// Pops a render state from the render state stack, and applies the new topmost render state
		/// </summary>
		void PopRenderState( );

		#endregion

		#region Texture units
		
        /// <summary>
        /// Pushes the current set of textures onto the texture stack and unbinds them all
        /// </summary>
        void PushTextures( );

        /// <summary>
        /// Pops the set of textures from the texture stack and rebinds them
        /// </summary>
        void PopTextures( );

		/// <summary>
		/// Binds a texture to a given texture unit
		/// </summary>
		/// <param name="unit">Unit to bind to</param>
		/// <param name="texture">Texture to bind</param>
		void BindTexture( int unit, ITexture2d texture );
		
		/// <summary>
		/// Unbinds a texture from its texture unit
		/// </summary>
		/// <param name="texture">Texture to unbind</param>
		void UnbindTexture( ITexture2d texture );

		/// <summary>
		/// Unbinds all textures from all texture units
		/// </summary>
		void UnbindAllTextures( );

		/// <summary>
		/// Gets a texture from a given texture unit
		/// </summary>
		/// <param name="unit">Unit index</param>
		/// <returns>Returns the texture current bound to the specified unit</returns>
		ITexture2d GetTexture( int unit );

		#endregion

		#region Lights

		#endregion
		
		#region	Frame dumps

		/// <summary>
		/// Creates an Image object from the colour buffer
		/// </summary>
		Image ColourBufferToImage( );

		/// <summary>
		/// Creates an Image object from the depth buffer
		/// </summary>
		Image DepthBufferToImage( );

		#endregion
	}
}
