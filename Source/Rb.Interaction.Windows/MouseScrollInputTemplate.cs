using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Rb.Interaction.Windows
{
    /// <summary>
    /// Mouse scroll input template
    /// </summary>
    public class MouseScrollInputTemplate : InputTemplate, IXmlSerializable
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MouseScrollInputTemplate( )
        {
            m_Button = MouseButtons.None;
            m_Min = 0;
            m_Max = 1;
            m_Delta = 0.1f;
            m_Initial = 0.5f;
        }

        /// <summary>
        /// Setup constructor
        /// </summary>
        public MouseScrollInputTemplate( MouseButtons button, float initial, float min, float max, float delta )
        {
            m_Button = button;
            m_Min = min;
            m_Max = max;
            m_Delta = delta;
            m_Initial = initial;
        }

        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public override IInput CreateInput(InputContext context)
        {
            return new MouseScrollInput( context, m_Button, m_Initial, m_Min, m_Max, m_Delta );
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
            string min      = reader.GetAttribute( "min" );
            string max      = reader.GetAttribute( "max" );
            string delta    = reader.GetAttribute( "delta" );
            string initial  = reader.GetAttribute( "initial" );

			m_Button    = button == null ? MouseButtons.None : ( MouseButtons )Enum.Parse( typeof( MouseButtons ), button );
            m_Min       = min == null ? 0.0f : float.Parse( min );
            m_Max       = max == null ? 1.0f : float.Parse( max );
            m_Delta     = delta == null ? 0.1f : float.Parse( delta );
            m_Initial   = initial == null ? 0.0f : float.Parse( initial );

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
            writer.WriteAttributeString( "min", m_Min.ToString( ) );
            writer.WriteAttributeString( "max", m_Max.ToString( ) );
            writer.WriteAttributeString( "delta", m_Delta.ToString( ) );
            writer.WriteAttributeString( "initial", m_Initial.ToString( ) );
		}

		#endregion

        private MouseButtons    m_Button;
        private float           m_Min;
        private float           m_Max;
        private float           m_Initial;
        private float           m_Delta;
    }
}
