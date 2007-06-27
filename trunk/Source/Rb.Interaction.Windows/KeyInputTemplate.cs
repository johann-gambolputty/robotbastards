using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;


namespace Rb.Interaction.Windows
{
    /// <summary>
    /// Keyboard input template
    /// </summary>
    class KeyInputTemplate : InputTemplate, IXmlSerializable
    {
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="key">Key to check for</param>
		public KeyInputTemplate( Keys key )
		{
			m_Key = key;
		}

		/// <summary>
		/// Default constructor. Assigns no key
		/// </summary>
        public KeyInputTemplate( )
		{
			m_Key = Keys.None;
		}

        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public override IInput CreateInput( InputContext context )
        {
            return new KeyInput( context, m_Key );
        }
        
		#region IXmlSerializable Members

		/// <summary>
		/// No schema supported
		/// </summary>
		/// <returns>Returns null</returns>
		public System.Xml.Schema.XmlSchema GetSchema( )
		{
			return null;
		}

		/// <summary>
		/// Reads this object from XML
		/// </summary>
		/// <param name="reader">Reader</param>
		public void ReadXml( XmlReader reader )
		{
			m_Key = ( Keys )Enum.Parse( typeof( Keys ), reader.GetAttribute( "key" ) );
		}

		/// <summary>
		/// Writes this object to XML
		/// </summary>
		/// <param name="writer">XML writer</param>
		public void WriteXml( XmlWriter writer )
		{
			writer.WriteAttributeString( "key", m_Key.ToString( ) );
		}

		#endregion

		private Keys m_Key;

	}
}
