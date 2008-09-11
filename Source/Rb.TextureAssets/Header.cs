using System;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.TextureAssets
{
	/// <summary>
	/// The header within a texture file
	/// </summary>
	public struct Header
	{
		/// <summary>
		/// Texture file identifier bits
		/// </summary>
		public const int TextureFileIdentifier = 0;

		/// <summary>
		/// Identifier of the file. Must be equal to <see cref="TextureFileIdentifier"/>
		/// </summary>
		public int FileIdentifier;

		/// <summary>
		/// Dimensions of the texture. Can be 2 or 3
		/// </summary>
		public int Dimensions;

		/// <summary>
		/// Number of mip map levels in the texture
		/// </summary>
		public int MipMapLevels;

		/// <summary>
		/// Width of the level 0 texture
		/// </summary>
		public int Width;

		/// <summary>
		/// Height of the level 0 texture
		/// </summary>
		public int Height;

		/// <summary>
		/// Depth of the level 0 texture (3d textures only)
		/// </summary>
		public int Depth;

		/// <summary>
		/// Texture format of the texture
		/// </summary>
		public TextureFormat Format;

		/// <summary>
		/// Creation timestamp
		/// </summary>
		public DateTime Created;
	}
}
