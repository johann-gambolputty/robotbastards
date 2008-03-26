using System;
using System.IO;

namespace Rb.Assets.Interfaces
{
	/// <summary>
	/// Asset stream source interface
	/// </summary>
	/// <remarks>
	/// Wraps up a <see cref="System.IO.Stream"/>.
	/// </remarks>
	public interface IStreamSource : ISource
	{
		/// <summary>
		/// Opens a stream from this source
		/// </summary>
		Stream Open( );
	}
}
