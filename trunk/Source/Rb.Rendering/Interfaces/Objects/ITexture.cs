using System;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Base texture interface
	/// </summary>
	public interface ITexture : IDisposable
	{
		/// <summary>
		/// Gets the format of the texture
		/// </summary>
		TextureFormat Format
		{
			get;
		}

		/// <summary>
		/// Binds this texture
		/// </summary>
		/// <param name="unit">Texture unit to bind this texture to</param>
		void Bind( int unit );

		/// <summary>
		/// Unbinds this texture
		/// </summary>
		/// <param name="unit">Texture unit that this texture is bound to</param>
		void Unbind( int unit );
	}
}
