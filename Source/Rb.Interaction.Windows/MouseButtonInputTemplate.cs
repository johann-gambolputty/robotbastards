using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Rb.Core.Utils;

namespace Rb.Interaction.Windows
{
	/// <summary>
	/// Input tempate for mouse button presses
	/// </summary>
	class MouseButtonInputTemplate : InputTemplate, IXmlSerializable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="button">Button to check for</param>
		/// <param name="state">Mouse button state change to detect</param>
		public MouseButtonInputTemplate( MouseButtons button, MouseButtonState state )
		{
			m_Button = button;
			m_State = state;
			m_Key = Keys.None;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="button">Button to check for</param>
		/// <param name="state">Mouse button state change to detect</param>
		/// <param name="key">Key to check for</param>
		public MouseButtonInputTemplate( MouseButtons button, MouseButtonState state, Keys key )
		{
			m_Button = button;
			m_State = state;
			m_Key = key;
		}

		/// <summary>
		/// Default constructor. Assigns no key
		/// </summary>
		public MouseButtonInputTemplate( )
		{
			m_Button = MouseButtons.None;
			m_State = MouseButtonState.Down;
			m_Key = Keys.None;
		}

        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public override IInput CreateInput( InputContext context )
        {
            return new MouseButtonInput( context, m_Button, m_State, m_Key );
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
			m_Button = StringHelpers.StringToEnum( reader.GetAttribute( "button" ), MouseButtons.Left );
			m_State = StringHelpers.StringToEnum( reader.GetAttribute( "state" ), MouseButtonState.Down );
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
			writer.WriteAttributeString( "state", m_State.ToString( ) );
			writer.WriteAttributeString( "key", m_Key.ToString( ) );
		}

		#endregion

		private MouseButtons m_Button;
		private MouseButtonState m_State;
		private Keys m_Key;
	}
}
