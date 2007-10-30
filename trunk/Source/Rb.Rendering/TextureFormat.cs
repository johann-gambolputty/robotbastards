using System;

namespace Rb.Rendering
{
	/// <summary>
	/// Texture formats
	/// </summary>
	public enum TextureFormat
	{
		Undefined,

		Depth16,
		Depth24,
		Depth32,

		R8G8B8,
		B8G8R8,

		R8G8B8X8,
		B8G8R8X8,

		R8G8B8A8,
		B8G8R8A8,

		A8R8G8B8,
		A8B8G8R8,
	}

	/// <summary>
	/// Information about <see cref="TextureFormat"/>
	/// </summary>
	public static class TextureFormatInfo
	{
		/// <summary>
		/// Gets the size of a given texture format in bits
		/// </summary>
		/// <param name="format">Format to query</param>
		/// <returns>Returns the number of bits required by a single texel in the given format</returns>
		/// <exception cref="ArgumentException">Thrown if format is unrecognised</exception>
		public static int GetBitSize( TextureFormat format )
		{
			switch ( format )
			{
				case TextureFormat.Undefined	:	return 0;

				case TextureFormat.Depth16		:	return 16;
				case TextureFormat.Depth24		:	return 24;
				case TextureFormat.Depth32		:	return 32;

				case TextureFormat.R8G8B8		:	return 24;
				case TextureFormat.B8G8R8		:	return 24;

				case TextureFormat.R8G8B8X8		:	return 32;
				case TextureFormat.B8G8R8X8		:	return 32;

				case TextureFormat.R8G8B8A8		:	return 32;
				case TextureFormat.B8G8R8A8		:	return 32;

				case TextureFormat.A8R8G8B8		:	return 32;
				case TextureFormat.A8B8G8R8		:	return 32;
			}

			throw new ArgumentException( string.Format( "Unknown texture format \"{0}\"", format ), "format" );
		}
	}

}
