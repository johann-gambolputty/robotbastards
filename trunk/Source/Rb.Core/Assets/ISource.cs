using System;
using System.IO;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Asset source interface
	/// </summary>
	/// <remarks>
	/// An ISource is a data source containing serialized objects. The most commonly used 
	/// implementation of ISource is <see cref="Location"/>, which represents the location of a
	/// file. <see cref="StreamSource"/> wraps up existing streams (e.g. memory streams).
	/// Sources are loaded by a specific <see cref="IAssetLoader"/> object in the
	/// <see cref="AssetManager"/>, by calling <see cref="AssetManager.Load(ISource)"/> or some
	/// overload.
	/// It's also possible to directly access the stream, using <see cref="Open()"/> (remember
	/// to Dispose the resulting stream).
	/// </remarks>
	public interface ISource
	{
		/// <summary>
		/// Event, invoked if the source changes
		/// </summary>
		event EventHandler SourceChanged;

		/// <summary>
		/// Gets the provider of this source
		/// </summary>
		ILocationManager Provider
		{
			get;
		}

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
		/// Returns true if this source is a directory
		/// </summary>
		bool Directory
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
		ISource GetRelativeSource( string path );

		/// <summary>
		/// Opens this source as a stream
		/// </summary>
		Stream Open( );
	}
}
