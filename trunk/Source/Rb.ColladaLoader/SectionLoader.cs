using System;
using System.Xml;

namespace Rb.ColladaLoader
{
	/// <summary>
	/// COLLADA section types
	/// </summary>
	public enum Section
	{
		Mesh,
		NumSections
	}

	/// <summary>
	/// Abstract base class for classes that can load collada sections
	/// </summary>
	public abstract class SectionLoader
	{
		/// <summary>
		/// Returns the section that this section loader handles
		/// </summary>
		public abstract Section Section
		{
			get;
		}

		/// <summary>
		/// Loads a COLLADA section
		/// </summary>
		/// <param name="reader">XML reader</param>
		public abstract object LoadSection( XmlReader reader );

		/// <summary>
		/// Reads past an XML element and all its sub eements
		/// </summary>
		/// <param name="reader"></param>
		public static void ReadPastElement( XmlReader reader )
		{
			if ( !reader.IsEmptyElement )
			{
				while ( reader.Read( ) && reader.NodeType != XmlNodeType.EndElement )
				{
					if ( reader.NodeType == XmlNodeType.Element )
					{
						ReadPastElement( reader );
					}
				}
			}
		}
	}
}
