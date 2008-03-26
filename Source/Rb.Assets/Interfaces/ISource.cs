using System;

namespace Rb.Assets.Interfaces
{
	/// <summary>
	/// Base interface for files, folders and streams
	/// </summary>
	/// <remarks>
	/// A source can be any location (<see cref="ILocation"/>), or a stream wrapper (<see cref="IStreamSource"/>).
	/// </remarks>
	public interface ISource
	{
		/// <summary>
		/// Event, invoked if the source changes
		/// </summary>
		event Action<ISource> SourceChanged;

		/// <summary>
		/// Path to this source (e.g. "c:\a\b\c.txt")
		/// </summary>
		string Path
		{
			get;
		}

		/// <summary>
		/// Gets the name of this source. For files, this includes the extension (e.g. "c.txt")
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the extension of this source. For folders, this returns an empty string
		/// </summary>
		string Extension
		{
			get;
		}

		/// <summary>
		/// Returns true if this source has a specified extension
		/// </summary>
		bool HasExtension( string extension );

		/// <summary>
		/// Gets the location of this source
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Thrown if the source has no location (e.g. this is a memory stream wrapper).
		/// </exception>
		ILocation Location
		{
			get;
		}
	}
}
