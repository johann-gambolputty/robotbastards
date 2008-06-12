
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// 3d texture interface
	/// </summary>
	public interface ITexture3d : ITexture
	{
		/// <summary>
		/// Gets the width of the texture
		/// </summary>
		int Width
		{
			get;
		}

		/// <summary>
		/// Gets the height of the texture
		/// </summary>
		int Height
		{
			get;
		}

		/// <summary>
		/// Gets the depth of the texture
		/// </summary>
		int Depth
		{
			get;
		}
		
		/// <summary>
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="depth">Depth of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		void Create( int width, int height, int depth, TextureFormat format );

		/// <summary>
		/// Creates this texture from a texture data object
		/// </summary>
		void Create( Texture3dData data );
	}
}
