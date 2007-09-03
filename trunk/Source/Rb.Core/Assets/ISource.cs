using System.IO;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Source interface
	/// </summary>
	public interface ISource
	{
		/// <summary>
		/// Returns true if the source is valid
		/// </summary>
		bool IsValid
		{
			get;
		}

		/// <summary>
		/// Gets the source extension
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
