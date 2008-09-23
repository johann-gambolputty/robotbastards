using System;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.TextureAssets
{
	/// <summary>
	/// Version 1 of the texture file format
	/// </summary>
	internal class TextureFileFormatVersion1
	{
		/// <summary>
		/// Texture file identifier bits
		/// </summary>
		public static readonly int TextureFileIdentifier = Make4CC( "RBTX" );

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
			/// Creation timestamp
			/// </summary>
			public DateTime Created;

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
		}

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
