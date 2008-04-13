
namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Interface for opengl-specific texture information
	/// </summary>
	public interface IOpenGlTexture
	{
		/// <summary>
		/// Gets the opengl handle for this texture
		/// </summary>
		int TextureHandle
		{
			get;
		}
	}
}
