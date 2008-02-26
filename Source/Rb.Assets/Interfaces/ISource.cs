using System;
using System.IO;

namespace Rb.Assets.Interfaces
{
	/// <summary>
	/// Asset source interface
	/// </summary>
	/// <remarks>
	/// An ISource is a data source containing serialized objects - for example, a file location
	/// Sources are loaded by a specific <see cref="IAssetLoader"/> object in the
	/// <see cref="IAssetManager"/>, by calling <see cref="IAssetManager.Load(ISource)"/> or some
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
		/// Opens a stream from this source
		/// </summary>
		Stream Open( );
	}
}
