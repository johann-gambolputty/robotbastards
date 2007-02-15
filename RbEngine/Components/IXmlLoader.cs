using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Loads an object from XML
	/// </summary>
	public interface IXmlLoader
	{
		/// <summary>
		/// Parses a given XML element
		/// </summary>
		/// <param name="element"> Element to parse </param>
		void ParseElement( System.Xml.XmlElement element );
	}
}
