using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Rb.Interaction.Windows
{
	public class MouseScrollDeltaInputTemplate : InputTemplate, IXmlSerializable
	{
        /// <summary>
        /// Default constructor
        /// </summary>
        public MouseScrollDeltaInputTemplate( )
        {
            m_Button = MouseButtons.None;
            m_Delta = 0.1f;
        }

        /// <summary>
        /// Setup constructor
        /// </summary>
		public MouseScrollDeltaInputTemplate( MouseButtons button, float delta )
        {
            m_Button = button;
            m_Delta = delta;
        }

        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public override IInput CreateInput( InputContext context )
        {
            return new MouseScrollDeltaInput( context, m_Button, m_Delta );
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
            string button   = reader.GetAttribute( "button" );
            string delta    = reader.GetAttribute( "delta" );

			m_Button    = button == null ? MouseButtons.None : ( MouseButtons )Enum.Parse( typeof( MouseButtons ), button );
            m_Delta     = delta == null ? 0.1f : float.Parse( delta );

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
            writer.WriteAttributeString( "delta", m_Delta.ToString( ) );
		}

		#endregion

		#region Private Members

		private MouseButtons	m_Button;
        private float           m_Delta;

		#endregion
	}
}
