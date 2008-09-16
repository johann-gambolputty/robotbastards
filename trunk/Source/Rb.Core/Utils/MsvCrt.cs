using System;
using System.Runtime.InteropServices;

namespace Rb.Core.Utils
{
	/// <summary>
	/// msvcrt p/invoke
	/// </summary>
	public static class MsvCrt
	{
		[DllImport( "msvcrt.dll" )]
		public unsafe static extern IntPtr memcpy( void* dest, void* src, int count );

	}
}
