using System;

namespace RbEngine
{
	/// <summary>
	/// Helpers for reading data by type
	/// </summary>
	public class BinaryReaderHelpers
	{
		/// <summary>
		/// Reads a bool value
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out bool val )
		{
			val = reader.ReadBoolean( );
		}

		/// <summary>
		/// Reads a 8-bit signed integer
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out sbyte val )
		{
			val = reader.ReadSByte( );
		}

		/// <summary>
		/// Reads a 8-bit unsigned integer
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out byte val )
		{
			val = reader.ReadByte( );
		}

		/// <summary>
		/// Reads a 16-bit signed integer
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out short val )
		{
			val = reader.ReadInt16( );
		}
		
		/// <summary>
		/// Reads a 16-bit unsigned integer
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out ushort val )
		{
			val = reader.ReadUInt16( );
		}

		/// <summary>
		/// Reads a 32-bit signed integer
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out int val )
		{
			val = reader.ReadInt32( );
		}

		/// <summary>
		/// Reads a 32-bit unsigned integer
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out uint val )
		{
			val = reader.ReadUInt32( );
		}

		/// <summary>
		/// Reads a 64-bit signed integer
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out long val )
		{
			val = reader.ReadInt64( );
		}

		/// <summary>
		/// Reads a 64-bit unsigned integer
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out ulong val )
		{
			val = reader.ReadUInt64( );
		}

		/// <summary>
		/// Reads a single precision float
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out float val )
		{
			val = reader.ReadSingle( );
		}

		/// <summary>
		/// Reads a double precision float
		/// </summary>
		public static void Read( System.IO.BinaryReader reader, out double val )
		{
			val = reader.ReadDouble( );
		}
	}
}
