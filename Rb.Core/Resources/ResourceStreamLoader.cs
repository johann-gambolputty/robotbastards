using System;

namespace Rb.Core.Resources
{
	/// <summary>
	/// Loads resources from streams
	/// </summary>
	public abstract class ResourceStreamLoader
	{
		#region	Stream loading

		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <returns> The loaded resource </returns>
		public abstract Object Load( System.IO.Stream input, string inputSource );

		/// <summary>
		/// Loads into a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <param name="parameters">Loading parameters</param>
		/// <returns>Returns resource</returns>
		public abstract Object Load( System.IO.Stream input, string inputSource, LoadParameters parameters );

		/// <summary>
		/// Returns true if this loader can load the specified stream
		/// </summary>
		/// <param name="path"> Stream path (contains extension that the loader can check)</param>
		/// <param name="input"> Input stream (file types can be identified by peeking at header bytes) </param>
		/// <returns> Returns true if the stream can </returns>
		/// <remarks>
		/// path can be null, in which case, the loader must be able to identify the resource type by checking the content in input (e.g. by peeking
		/// at the header bytes).
		/// </remarks>
		public abstract bool CanLoadStream( string path, System.IO.Stream input );

		#endregion

	}
}
