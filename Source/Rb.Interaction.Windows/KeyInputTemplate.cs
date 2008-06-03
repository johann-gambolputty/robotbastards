using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Rb.Core.Utils;

namespace Rb.Interaction.Windows
{
    /// <summary>
    /// Keyboard input template
    /// </summary>
	public class KeyInputTemplate : InputTemplate, IXmlSerializable
    {
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="key">Key to check for</param>
		/// <param name="state">Key state to detect</param>
		public KeyInputTemplate( Keys key, KeyState state )
		{
			m_Key = key;
			m_State = state;
		}

		/// <summary>
		/// Default constructor. Assigns no key
		/// </summary>
        public KeyInputTemplate( )
		{
			m_Key = Keys.None;
			m_State = KeyState.Down;
		}

        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public override IInput CreateInput( InputContext context )
        {
			return new KeyInput( context, m_Key, m_State );
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
			m_Key = StringHelpers.StringToEnum( reader.GetAttribute( "key" ), Keys.None );
			m_State = StringHelpers.StringToEnum( reader.GetAttribute( "state" ), KeyState.Held );
            
            if ( reader.IsEmptyElement )
            {
                reader.Read( );
            }
            else
            {
                reader.ReadStartElement( );
                reader.ReadEndElement( );
            }
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
		private KeyState m_State;

	}
}
