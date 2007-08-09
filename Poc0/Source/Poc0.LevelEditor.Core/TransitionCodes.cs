using System;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Tile transition codes
	/// </summary>
	public static class TransitionCodes
	{
		public const byte None				= 0x0;

		public const byte TopLeftCorner		= 0x1;
		public const byte TopRightCorner	= 0x2;
		public const byte BottomRightCorner	= 0x4;
		public const byte BottomLeftCorner	= 0x8;
		public const byte Corners			= TopLeftCorner | TopRightCorner | BottomRightCorner | BottomLeftCorner;

		public const byte TopEdge			= 0x10;
		public const byte RightEdge			= 0x20;
		public const byte BottomEdge		= 0x40;
		public const byte LeftEdge			= 0x80;
		public const byte Edges				= TopEdge | RightEdge | BottomEdge | LeftEdge;

		public const byte All				= 0xff;
	}
}
