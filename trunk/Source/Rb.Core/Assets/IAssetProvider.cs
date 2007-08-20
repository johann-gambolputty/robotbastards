using System.IO;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Opens asset streams and shit
	/// </summary>
	public interface IAssetProvider
	{
		/// <summary>
		/// Gets the full path of a location
		/// </summary>
		/// <param name="location">Location</param>
		/// <returns>Full path of the location (</returns>
		string GetFullPath( string location );

		/// <summary>
		/// Returns true if a specified asset's location path is valid (can be opened as a stream or folder)
		/// </summary>
		/// <param name="path">Asset location path</param>
		/// <returns>true if the location path refers to an existing asset</returns>
		bool IsPathValid( string path );

		/// <summary>
		/// Opens a stream
		/// </summary>
		/// <param name="location">Location of the stream</param>
		/// <returns>Returns the opened stream</returns>
		/// <exception cref="AssetNotFoundException">Thrown if no asset exists at the specified location</exception>
		Stream OpenStream( Location location );
	}
}
