using System.IO;
using System.Runtime.InteropServices;
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
		[StructLayout( LayoutKind.Sequential, Pack = 0 )]
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

			/// <summary>
			/// Writes this group to a stream
			/// </summary>
			public void Write( BinaryWriter writer )
			{
				writer.Write( ( int )GroupId );
				writer.Write( GroupSizeInBytes );
			}

			/// <summary>
			/// Reads a new group from a stream
			/// </summary>
			public static Group Read( BinaryReader reader )
			{
				Group newGroup = new Group( );
				newGroup.GroupId = ( GroupIdentifier )reader.ReadInt32( );
				newGroup.GroupSizeInBytes = reader.ReadInt64( );
				return newGroup;
			}
		}

		/// <summary>
		/// The header within a texture file
		/// </summary>
		[StructLayout( LayoutKind.Sequential, Pack = 0 )]
		internal struct Header
		{
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
			/// Writes this group to a stream
			/// </summary>
			public void Write( BinaryWriter writer )
			{
				writer.Write( Dimensions );
				writer.Write( ( int )Format );
				writer.Write( TextureDataEntries );
			}

			/// <summary>
			/// Reads a new group from a stream
			/// </summary>
			public static Header Read( BinaryReader reader )
			{
				Header newHeader = new Header( );
				newHeader.Dimensions = reader.ReadInt32( );
				newHeader.Format = ( TextureFormat )reader.ReadInt32( );
				newHeader.TextureDataEntries = reader.ReadInt32( );
				return newHeader;
			}
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
