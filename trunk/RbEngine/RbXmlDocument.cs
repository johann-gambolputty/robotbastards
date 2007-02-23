using System;
using System.Xml;

namespace RbEngine
{

	/// <summary>
	/// Helpful extension to XmlException that gets file and line number from an IXmlLineInfo supporting node
	/// </summary>
	public class RbXmlException : XmlException
	{
		/// <summary>
		/// Stores the message, determines linenumber of position from the node (if it implements IXmlLineInfo)
		/// </summary>
		/// <param name="node">Node to get line information from</param>
		/// <param name="message">Exception message</param>
		public RbXmlException( XmlNode node, string message, params string[] messageParams ) :
			base( string.Format( message, messageParams ), null, GetLineNumber( node ), GetLinePosition( node ) )
		{
		}

		/// <summary>
		/// Stores the message, determines linenumber of position from the node (if it implements IXmlLineInfo)
		/// </summary>
		/// <param name="innerException">Inner exception</param>
		/// <param name="node">Node to get line information from</param>
		/// <param name="message">Exception message</param>
		public RbXmlException( Exception innerException, XmlNode node, string message, params string[] messageParams ) :
			base( string.Format( message, messageParams ), innerException )
		{
		}

		/// <summary>
		/// Helper. Returns the line number of a node, if it implements IXmlLineInfo, or 0
		/// </summary>
		private static int GetLineNumber( XmlNode node )
		{
			return ( node is IXmlLineInfo ) ? ( ( IXmlLineInfo )node ).LineNumber : 0;
		}

		/// <summary>
		/// Helper. Returns the line position of a node, if it implements IXmlLineInfo, or 0
		/// </summary>
		private static int GetLinePosition( XmlNode node )
		{
			return ( node is IXmlLineInfo ) ? ( ( IXmlLineInfo )node ).LinePosition : 0;
		}

	}

	/// <summary>
	/// Adds line information to XmlElement (why can't this be as standard, eh?)
	/// </summary>
	/// <remarks>
	/// Line information is available in XPathDocument and XmlReader, but not in XmlDocument. It's an odd omission. This class
	/// and <see cref="RbXmlDocument"/> provide support, through the IXmlLineInfo interface.
	/// </remarks>
	public class RbXmlElement : XmlElement, IXmlLineInfo
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="line">Line that this element was defined on</param>
		/// <param name="column">Column that this element was defined on</param>
		/// <param name="prefix">Namespace</param>
		/// <param name="localname">Local name</param>
		/// <param name="nsURI">namespace URI</param>
		/// <param name="doc">Document that created this element</param>
		public RbXmlElement( int line, int column, string prefix, string localname, string nsURI, XmlDocument doc ) :
			base( prefix, localname, nsURI, doc ) 
		{
			m_Line		= line;
			m_Column	= column;
		}

		#region IXmlLineInfo Members

		/// <summary>
		/// The line number that this element was defined on
		/// </summary>
		public int LineNumber
		{
			get { return m_Line; }
		}

		/// <summary>
		/// The column that this element was defined on
		/// </summary>
		public int LinePosition
		{
			get { return m_Column; }
		}

		/// <summary>
		/// Yep, we got line info
		/// </summary>
		/// <returns>true</returns>
		public bool HasLineInfo( )
		{
			return true;
		}

		#endregion

		#region	Private stuff

		private int	m_Line;
		private int m_Column;

		#endregion
	}

	/// <summary>
	/// Extends XmlDocument to provide line information in created nodes (well, elements only, at the moment)
	/// </summary>
	/// <remarks>
	/// Line information is available in XPathDocument and XmlReader, but not in XmlDocument. Nodes created by this
	/// document implement the IXmlLineInfo interface that provide this information.
	/// <note>
	/// Forces preservation of whitespace (it's the only way to track the current line)
	/// </note>
	/// </remarks>
	public class RbXmlDocument : XmlDocument
	{
		/// <summary>
		/// Document constructor
		/// </summary>
		public RbXmlDocument( )
		{
			PreserveWhitespace = true;
		}

		/// <summary>
		/// Creates a new element
		/// </summary>
		public override XmlElement CreateElement( string prefix, string localName, string namespaceURI )
		{
			return new RbXmlElement( m_Line, m_Column, prefix, localName, namespaceURI, this );
		}

		/// <summary>
		/// Creates whitespace (tracks position)
		/// </summary>
		/// <param name="text">Whitespace text</param>
		/// <returns>Returns a new XmlWhitespace object</returns>
		public override XmlWhitespace CreateWhitespace( string text )
		{
			for ( int index = 0; index < text.Length; ++index )
			{
				switch ( text[ index ] )
				{
					case '\n'	:
						++m_Line;
						m_Column = 1;
						break;

					case '\t'	:
					case ' '	:
						++m_Column;
						break;
				}
			}
			return base.CreateWhitespace( text );
		}

		private int		m_Line						= 1;
		private int		m_Column					= 1;
	}
}
