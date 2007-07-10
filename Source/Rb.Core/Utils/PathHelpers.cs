using System.Text;

namespace Rb.Core.Utils
{
	/// <summary>
	/// File path helper functions
	/// </summary>
	public class PathHelpers
	{
		private static char[] DirectorySplitters = new char[] { '\\', '/' };

		/// <summary>
		/// Makes a relative path from 2 absolute paths
		/// </summary>
		/// <remarks>
		/// Code from http://mrpmorris.blogspot.com/2007/05/convert-absolute-path-to-relative-path.html
		/// </remarks>
		public static string MakeRelativePath( string absolutePath, string relativeTo )
		{
			string[] absoluteDirectories = absolutePath.Split( DirectorySplitters );
			string[] relativeDirectories = relativeTo.Split( DirectorySplitters );

			//	Get the shortest of the two paths
			int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

			//	Use to determine where in the loop we exited
			int lastCommonRoot = -1;
			int index;

			//Find common root
			for ( index = 0; index < length; ++index )
			{
				if ( absoluteDirectories[ index ] == relativeDirectories[ index ] )
				{
					lastCommonRoot = index;
				}
				else
				{
					break;
				}
			}
			//	If we didn't find a common prefix then just return the input path
			if ( lastCommonRoot == -1 )
			{
				return relativeTo;
			}

			//Build up the relative path
			StringBuilder relativePath = new StringBuilder( );

			//Add on the ..
			for ( index = lastCommonRoot + 1; index < absoluteDirectories.Length; ++index )
			{
				if ( absoluteDirectories[ index ].Length > 0 )
				{
					relativePath.Append( "..\\" );
				}
			}
			//	Add on the folders
			for ( index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; ++index )
			{
				relativePath.Append( relativeDirectories[ index ] + "\\" );
			}
			relativePath.Append( relativeDirectories[ relativeDirectories.Length - 1 ] );
			return relativePath.ToString( );
		}
	}
}
