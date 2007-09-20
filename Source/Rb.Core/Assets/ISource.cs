using System.IO;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Source interface
	/// </summary>
	public interface ISource
	{
		/// <summary>
		/// Returns true if the source exists
		/// </summary>
		bool Exists
		{
			get;
		}

		/// <summary>
		/// Gets the name of the source (e.g. filename)
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the full path of the source (e.g. rooted file path)
		/// </summary>
		string Path
		{
			get;
		}

		/// <summary>
		/// Checks if this source has the specified extension
		/// </summary>
		bool HasExtension( string ext );

		/// <summary>
		/// Gets a source relative to this one
		/// </summary>
		/// <param name="path">Relative path</param>
		/// <returns>New source</returns>
		ISource GetSource( string path );

		/// <summary>
		/// Opens this source as a stream
		/// </summary>
		Stream Open( );
	}
}
