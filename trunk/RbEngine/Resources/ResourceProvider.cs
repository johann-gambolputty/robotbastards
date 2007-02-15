using System;

namespace RbEngine.Resources
{
	/// <summary>
	/// Summary description for ResourceProvider.
	/// </summary>
	public abstract class ResourceProvider
	{

		/// <summary>
		/// Opens a stream from a path
		/// </summary>
		/// <param name="path"> Stream identifier (e.g. filesystem path). If the provider resolves the path, the resolved string is stored back in path </param>
		/// <returns> Returns the opened stream, or null if the resource could not be found </returns>
		public abstract System.IO.Stream	Open( ref string path );


		/// <summary>
		/// Sets up this provider from an XML definition
		/// </summary>
		/// <param name="element"> XML element that created this provider </param>
		public abstract void				Setup( System.Xml.XmlElement element );


	}
}
