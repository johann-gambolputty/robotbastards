using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Rb.Core.Assets.Windows
{
	/// <summary>
	/// Icon reader
	/// </summary>
	public class FileIconReader
	{
		/// <summary>
		/// Gets an icon for a given file
		/// </summary>
		/// <param name="filePath">Path to the file</param>
		/// <param name="addLinkOverlay">If true, adds a link overlay to the icon</param>
		/// <param name="small">If true, gets the small version of the icon. true returns the large version</param>
		/// <returns>Returns the file's icon</returns>
		public static Icon GetFileIcon( string filePath, bool addLinkOverlay, bool small ) 
		{
			IconFlags flags = ( IconFlags.Icon | IconFlags.AddOverlays | IconFlags.UseFileAttributes );
			if ( addLinkOverlay )
			{
				flags |= IconFlags.LinkOverlay;
			}
			if ( small )
			{
				flags |= IconFlags.SmallIcon;
			}

			ShellFileInfo shellFileInfo = new ShellFileInfo( );

			SHGetFileInfo( filePath, FileAttributeNormal, ref shellFileInfo, ( uint )Marshal.SizeOf( shellFileInfo ), ( uint )flags );

			Icon icon = (Icon)Icon.FromHandle( shellFileInfo.hIcon ).Clone( );

			DestroyIcon( shellFileInfo.hIcon );

			return icon;
		}

		/// <summary>
		/// Gets an icon for a given folder
		/// </summary>
		/// <param name="path">Path to the folder</param>
		/// <param name="small">If true, gets the small version of the icon. true returns the large version</param>
		/// <param name="open">Gets the open version of the folder's icon</param>
		/// <returns>Returns the folder's icon</returns>
		public static Icon GetFolderIcon( string path, bool small, bool open )
		{
			IconFlags flags = ( IconFlags.Icon | IconFlags.AddOverlays | IconFlags.UseFileAttributes );
			if ( open )
			{
				flags |= IconFlags.OpenIcon;
			}
			if ( small )
			{
				flags |= IconFlags.SmallIcon;
			}

			ShellFileInfo shellFileInfo = new ShellFileInfo( );

			SHGetFileInfo( path, FileAttributeDirectory, ref shellFileInfo, ( uint )Marshal.SizeOf( shellFileInfo ), ( uint )flags );

			Icon icon = ( Icon )Icon.FromHandle( shellFileInfo.hIcon ).Clone( );

			DestroyIcon( shellFileInfo.hIcon );

			return icon;
		}

		#region Private stuff

		[StructLayout( LayoutKind.Sequential )]
		private struct ShellFileInfo
		{
			public IntPtr	hIcon;
			public int		iIndex;
			public uint		dwAttributes; 

			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
			public string	szDisplayName; 

			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 )]
			public string	szTypeName; 
		};

		[Flags]
		private enum IconFlags : uint
		{
			SmallIcon = 0x1,
			OpenIcon = 0x2,
			UseFileAttributes = 0x10,
			AddOverlays = 0x20,
			Icon = 0x100,
			LinkOverlay = 0x8000
		}

		private const uint FileAttributeNormal = 0x80;
		private const uint FileAttributeDirectory = 0x10;


		[DllImport("User32.dll")]
		private static extern int DestroyIcon( IntPtr hIcon );

		[DllImport("Shell32.dll")]
		private static extern IntPtr SHGetFileInfo( string pszPath, uint dwFileAttributes, ref ShellFileInfo psfi, uint cbFileInfo, uint uFlags );


		#endregion
	}
}
