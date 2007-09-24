using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Rb.Core.Utils;

namespace Rb.Interaction.Windows
{
    /// <summary>
    /// Mouse input template
    /// </summary>
    public class MouseCursorInputTemplate : InputTemplate, IXmlSerializable
    {
		/// <summary>
		/// Default constructor. Bindings to this input will trigger when the mouse moves with no buttons pressed
		/// </summary>
		public MouseCursorInputTemplate( )
		{
			m_Button = MouseButtons.None;
		}

		/// <summary>
		/// Setup constructor. Bindings to this input will trigger when the mouse moves with a particular button pressed
		/// </summary>
		/// <param name="button">Button to check for along with movement</param>
		public MouseCursorInputTemplate( MouseButtons button )
		{
			m_Button = button;
		}

		/// <summary>
		/// Setup constructor. Bindings to this input will trigger when the mouse moves with a particular button pressed
		/// </summary>
		/// <param name="button">Button to check for along with movement</param>
		/// <param name="key">Modifier key</param>
		public MouseCursorInputTemplate( MouseButtons button, Keys key )
		{
			m_Button = button;
			m_Key = key;
		}

        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public override IInput CreateInput( InputContext context )
        {
            return new MouseCursorInput( context, m_Button, m_Key );
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
			m_Button = StringHelpers.StringToEnum( reader.GetAttribute( "button" ), MouseButtons.None );
			m_Key = StringHelpers.StringToEnum( reader.GetAttribute( "key" ), Keys.None );

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
			writer.WriteAttributeString( "button", m_Button.ToString( ) );
			writer.WriteAttributeString( "key", m_Key.ToString( ) );
		}

		#endregion

		private Keys m_Key = Keys.None;
		private MouseButtons m_Button;
    }
}
