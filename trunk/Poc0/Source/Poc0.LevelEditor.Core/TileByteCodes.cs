//
//	Tile symmetry data
//	Generated at 27/07/2007 12:29:49 by SymTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//


namespace Poc0.LevelEditor.Core
{
	///<summary>
	///	Automatically generated class containing values from 0-255 that cannot be generated by rotating or mirroring
	///	other values from 0-255
	///</summary>
	///<remarks>
	///	Values from 0-255 represent on/off states at the corners and midpoints of a square (4 corners + 4 midpoints=
	///	256 possible state combinations). For example, 10000000b represents the top left corner on, and all others off.
	///	10101010b represents all corners on, and all midpoints off:
	///
	///	0--1--2
	///	|     |
	///	7     3
	///	|     |
	///	6--5--4
	///
	///	A given combination can be unique, or can be generated by rotating, or mirroring, or mirroring and rotating,
	///	another combination - for example, 01000000b (top midpoint on) can be rotated by 90 degrees to give 00010000b
	///	(right midpoint on), 180 degrees to give 00000100b (bottom midpoint on), and 270 degrees to give 00000001b
	///	(left midpoint on). The array returned by <see cref="OriginalBitCodes"/> contains all the unique bit codes
	///	that can be used to generate all other bit codes from 0 to 255, by either applying one or more 
	///	<see cref="Rotate"/> or <see cref="Mirror"/> operations.
	///</remarks>
	public static class TileByteCodes
	{
		///<summary>
		///	Bit codes that cannot be generated by rotational or mirror symmetries of other bit codes
		///</summary>
		public static byte[] OriginalBitCodes
		{
			get { return ms_OriginalBitCodes; }
		}

		/// <summary>
		/// Returns true if the specified code is original, false if derived
		/// </summary>
		public static bool IsOriginal(byte inBits)
		{
			return Original(inBits) == inBits;
		}

		///<summary>
		///	Rotates a byte code by 90 degrees
		///</summary>
		public static byte Rotate(byte inBits)
		{
			ushort outBits = (ushort)(inBits << 2);
			ushort hiByte = (ushort)(outBits & 0xff00);
			if (hiByte == 0)
			{
				return (byte)outBits;
			}
			outBits |= (ushort)((hiByte >> 8) & 0x3);
			return (byte)outBits;
		}

		///<summary>
		///	Mirrors a byte code around the 0-4 axis
		///</summary>
		public static byte Mirror(byte inBits)
		{
			byte outBits = inBits;

			outBits = SwapBits(outBits, 1, 7);
			outBits = SwapBits(outBits, 2, 6);
			outBits = SwapBits(outBits, 3, 5);

			return outBits;
		}

		///<summary>
		///	Returns the original source for a given code
		///</summary>
		public static byte Original(byte inBits)
		{
			return ms_Info[inBits].Original;
		}

		///<summary>
		///	Returns true if the specified code is a mirror of the original
		///</summary>
		public static bool MirrorRequired(byte inBits)
		{
			return ms_Info[inBits].Mirror;
		}

		///<summary>
		///	Returns the number of rotations of the original (after mirroring) that the specified code requires
		///</summary>
		public static int RotationsRequired(byte inBits)
		{
			return ms_Info[inBits].NumRotations;
		}

		#region Private stuff

		private static byte SwapBits(byte inBits,int bit0,int bit1)
		{
			byte outBits = inBits;

			byte bit0Mask = (byte)(1 << bit0);
			byte bit1Mask = (byte)(1 << bit1);

			outBits &= (byte)(~(bit0Mask | bit1Mask));

			if ((inBits & bit0Mask) != 0)
			{
				outBits |= bit1Mask;
			}
			if ((inBits & bit1Mask) != 0)
			{
				outBits |= bit0Mask;
			}
			return outBits;
		}

		private struct Info
		{
			public byte Original;
			public bool Mirror;
			public int NumRotations;
			public Info(byte original, bool mirror, int numRotations)
			{
				Original = original;
				Mirror = mirror;
				NumRotations = numRotations;
			}
		};

		private static readonly Info[] ms_Info = new Info[256]
		{
			new Info(0,false,0),
			new Info(1,false,0),
			new Info(2,false,0),
			new Info(3,false,0),
			new Info(1,false,1),
			new Info(5,false,0),
			new Info(3,true,0),
			new Info(7,false,0),
			new Info(2,false,1),
			new Info(9,false,0),
			new Info(10,false,0),
			new Info(11,false,0),
			new Info(3,false,1),
			new Info(13,false,0),
			new Info(14,false,0),
			new Info(15,false,0),
			new Info(1,false,2),
			new Info(17,false,0),
			new Info(9,true,1),
			new Info(19,false,0),
			new Info(5,false,1),
			new Info(21,false,0),
			new Info(13,true,1),
			new Info(23,false,0),
			new Info(3,true,1),
			new Info(19,true,1),
			new Info(11,true,1),
			new Info(27,false,0),
			new Info(7,false,1),
			new Info(23,true,1),
			new Info(15,true,1),
			new Info(31,false,0),
			new Info(2,false,2),
			new Info(9,true,3),
			new Info(34,false,0),
			new Info(35,false,0),
			new Info(9,false,1),
			new Info(37,false,0),
			new Info(35,true,0),
			new Info(39,false,0),
			new Info(10,false,1),
			new Info(41,false,0),
			new Info(42,false,0),
			new Info(43,false,0),
			new Info(11,false,1),
			new Info(45,false,0),
			new Info(46,false,0),
			new Info(47,false,0),
			new Info(3,false,2),
			new Info(19,false,2),
			new Info(35,false,2),
			new Info(51,false,0),
			new Info(13,false,1),
			new Info(53,false,0),
			new Info(54,false,0),
			new Info(55,false,0),
			new Info(14,false,1),
			new Info(57,false,0),
			new Info(46,true,2),
			new Info(59,false,0),
			new Info(15,false,1),
			new Info(61,false,0),
			new Info(62,false,0),
			new Info(63,false,0),
			new Info(1,false,3),
			new Info(5,false,3),
			new Info(9,false,3),
			new Info(13,false,3),
			new Info(17,false,1),
			new Info(21,false,3),
			new Info(19,true,0),
			new Info(23,true,0),
			new Info(9,true,2),
			new Info(37,false,3),
			new Info(41,false,3),
			new Info(45,false,3),
			new Info(19,false,1),
			new Info(53,false,3),
			new Info(57,false,3),
			new Info(61,false,3),
			new Info(5,false,2),
			new Info(21,false,2),
			new Info(37,false,2),
			new Info(53,false,2),
			new Info(21,false,1),
			new Info(85,false,0),
			new Info(53,true,2),
			new Info(87,false,0),
			new Info(13,true,2),
			new Info(53,true,3),
			new Info(45,true,2),
			new Info(91,false,0),
			new Info(23,false,1),
			new Info(87,false,1),
			new Info(61,true,2),
			new Info(95,false,0),
			new Info(3,true,2),
			new Info(13,true,3),
			new Info(35,true,2),
			new Info(54,false,2),
			new Info(19,true,2),
			new Info(53,true,0),
			new Info(51,true,0),
			new Info(55,true,0),
			new Info(11,true,2),
			new Info(45,true,3),
			new Info(43,true,2),
			new Info(107,false,0),
			new Info(27,false,1),
			new Info(91,false,1),
			new Info(59,true,2),
			new Info(111,false,0),
			new Info(7,false,2),
			new Info(23,false,2),
			new Info(39,false,2),
			new Info(55,false,2),
			new Info(23,true,2),
			new Info(87,false,2),
			new Info(55,true,2),
			new Info(119,false,0),
			new Info(15,true,2),
			new Info(61,true,3),
			new Info(47,true,2),
			new Info(111,true,2),
			new Info(31,false,1),
			new Info(95,false,1),
			new Info(63,true,2),
			new Info(127,false,0),
			new Info(2,false,3),
			new Info(3,true,3),
			new Info(10,false,3),
			new Info(14,false,3),
			new Info(9,true,0),
			new Info(13,true,0),
			new Info(11,true,0),
			new Info(15,true,0),
			new Info(34,false,1),
			new Info(35,true,3),
			new Info(42,false,3),
			new Info(46,false,3),
			new Info(35,false,1),
			new Info(54,false,3),
			new Info(46,true,1),
			new Info(62,false,3),
			new Info(9,false,2),
			new Info(19,true,3),
			new Info(41,false,2),
			new Info(57,false,2),
			new Info(37,false,1),
			new Info(53,true,1),
			new Info(45,true,1),
			new Info(61,true,1),
			new Info(35,true,1),
			new Info(51,true,1),
			new Info(43,true,1),
			new Info(59,true,1),
			new Info(39,false,1),
			new Info(55,true,1),
			new Info(47,true,1),
			new Info(63,true,1),
			new Info(10,false,2),
			new Info(11,true,3),
			new Info(42,false,2),
			new Info(46,true,0),
			new Info(41,false,1),
			new Info(45,true,0),
			new Info(43,true,0),
			new Info(47,true,0),
			new Info(42,false,1),
			new Info(43,true,3),
			new Info(170,false,0),
			new Info(171,false,0),
			new Info(43,false,1),
			new Info(107,false,1),
			new Info(171,false,1),
			new Info(175,false,0),
			new Info(11,false,2),
			new Info(27,false,2),
			new Info(43,false,2),
			new Info(59,false,2),
			new Info(45,false,1),
			new Info(91,false,2),
			new Info(107,false,2),
			new Info(111,true,0),
			new Info(46,false,1),
			new Info(59,true,3),
			new Info(171,false,2),
			new Info(187,false,0),
			new Info(47,false,1),
			new Info(111,false,1),
			new Info(175,false,1),
			new Info(191,false,0),
			new Info(3,false,3),
			new Info(7,false,3),
			new Info(11,false,3),
			new Info(15,false,3),
			new Info(19,false,3),
			new Info(23,false,3),
			new Info(27,false,3),
			new Info(31,false,3),
			new Info(35,false,3),
			new Info(39,false,3),
			new Info(43,false,3),
			new Info(47,false,3),
			new Info(51,false,1),
			new Info(55,false,3),
			new Info(59,false,3),
			new Info(63,false,3),
			new Info(13,false,2),
			new Info(23,true,3),
			new Info(45,false,2),
			new Info(61,false,2),
			new Info(53,false,1),
			new Info(87,false,3),
			new Info(91,false,3),
			new Info(95,false,3),
			new Info(54,false,1),
			new Info(55,true,3),
			new Info(107,false,3),
			new Info(111,false,3),
			new Info(55,false,1),
			new Info(119,false,1),
			new Info(111,true,1),
			new Info(127,false,3),
			new Info(14,false,2),
			new Info(15,true,3),
			new Info(46,false,2),
			new Info(62,false,2),
			new Info(57,false,1),
			new Info(61,true,0),
			new Info(59,true,0),
			new Info(63,true,0),
			new Info(46,true,3),
			new Info(47,true,3),
			new Info(171,false,3),
			new Info(175,false,3),
			new Info(59,false,1),
			new Info(111,true,3),
			new Info(187,false,1),
			new Info(191,false,3),
			new Info(15,false,2),
			new Info(31,false,2),
			new Info(47,false,2),
			new Info(63,false,2),
			new Info(61,false,1),
			new Info(95,false,2),
			new Info(111,false,2),
			new Info(127,false,2),
			new Info(62,false,1),
			new Info(63,true,3),
			new Info(175,false,2),
			new Info(191,false,2),
			new Info(63,false,1),
			new Info(127,false,1),
			new Info(191,false,1),
			new Info(255,false,0),
		};
		private static readonly byte[] ms_OriginalBitCodes = new byte[51]
		{
			0,
			1,
			2,
			3,
			5,
			7,
			9,
			10,
			11,
			13,
			14,
			15,
			17,
			19,
			21,
			23,
			27,
			31,
			34,
			35,
			37,
			39,
			41,
			42,
			43,
			45,
			46,
			47,
			51,
			53,
			54,
			55,
			57,
			59,
			61,
			62,
			63,
			85,
			87,
			91,
			95,
			107,
			111,
			119,
			127,
			170,
			171,
			175,
			187,
			191,
			255,
		};

		#endregion
	}
}

