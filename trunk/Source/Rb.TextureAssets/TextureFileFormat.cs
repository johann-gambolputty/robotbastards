using System;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.TextureAssets
{
	/// <summary>
	/// Texture file group identifiers
	/// </summary>
	internal enum GroupIdentifier : int
	{
		HeaderGroup			= 0x00000100,
		Texture2dDataGroup	= 0x00000101,
		Texture3dDataGroup	= 0x00000102
	}

	/// <summary>
	/// Group information
	/// </summary>
	internal struct Group
	{
		/// <summary>
		/// The group identifier
		/// </summary>
		public GroupIdentifier GroupId;

		/// <summary>
		/// The size of the group in bytes (not including this group header)
		/// </summary>
		public long GroupSizeInBytes;
	}

	/// <summary>
	/// The header within a texture file
	/// </summary>
	internal struct Header
	{
		/// <summary>
		/// Texture file identifier bits
		/// </summary>
		public static readonly int TextureFileIdentifier = Make4CC( "RBTX" );

		/// <summary>
		/// Dimensions of the texture. Can be 2 or 3
		/// </summary>
		public int Dimensions;

		/// <summary>
		/// The format of the texture
		/// </summary>
		public TextureFormat Format;

		/// <summary>
		/// The number of texture data entries in the file.
		/// </summary>
		public int TextureDataEntries;

		/// <summary>
		/// Creation timestamp
		/// </summary>
		public DateTime Created;

		#region Private Members

		/// <summary>
		/// Makes a four character code from a string
		/// </summary>
		private static int Make4CC( string chars )
		{
			byte b0 = ( byte )( chars.Length > 0 ? chars[ 0 ] : 0 );
			byte b1 = ( byte )( chars.Length > 1 ? chars[ 1 ] : 0 );
			byte b2 = ( byte )( chars.Length > 2 ? chars[ 2 ] : 0 );
			byte b3 = ( byte )( chars.Length > 3 ? chars[ 3 ] : 0 );
			return b0 | b1 << 8 | b2 << 16 | b3 << 24;
		}

		#endregion
	}
}
