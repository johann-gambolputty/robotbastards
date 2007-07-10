
namespace Rb.Core.Resources
{
	/// <summary>
	/// Resource provider
	/// </summary>
	public abstract class ResourceProvider
	{
		/// <summary>
		/// Opens a stream from a path
		/// </summary>
		public System.IO.Stream Open( string path )
		{
			return Open( ref path );
		}

		/// <summary>
		/// Opens a stream from a path
		/// </summary>
		/// <param name="path"> Stream identifier (e.g. filesystem path). If the provider resolves the path, the resolved string is stored back in path </param>
		/// <returns> Returns the opened stream, or null if the resource could not be found </returns>
		public abstract System.IO.Stream Open( ref string path );

		/// <summary>
		/// Sets up this provider from an XML definition
		/// </summary>
		/// <param name="element"> XML element that created this provider </param>
		public abstract void Setup( System.Xml.XmlElement element );

		/// <summary>
		/// Returns true if the specified directory is available using this provider
		/// </summary>
		public abstract bool DirectoryExists( ref string directory );

		/// <summary>
		/// Returns true if the named stream exists in this provider
		/// </summary>
		public abstract bool StreamExists( string path );

	}
}
