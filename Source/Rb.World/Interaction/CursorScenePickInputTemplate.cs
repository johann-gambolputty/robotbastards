using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Rb.Interaction;

namespace Rb.World.Interaction
{
    /// <summary>
    /// Generates CursorScenePickInput objects for a given context
    /// </summary>
    public class CursorScenePickInputTemplate : InputTemplate, IXmlSerializable
    {
        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public override IInput CreateInput( InputContext context )
        {
            return new CursorScenePickInput( context, ( CursorInput )m_InputTemplate.CreateInput( context ) );
        }

        #region IXmlSerializable Members

        /// <summary>
        /// No schema for you
        /// </summary>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads this template from XML
        /// </summary>
        public void ReadXml( System.Xml.XmlReader reader )
        {
            if ( !reader.IsStartElement( ) || reader.IsEmptyElement )
            {
                throw new ApplicationException( "Expected <input> element inside CursorScenePickTemplate definition" );
            }

            reader.ReadStartElement( );

            m_InputTemplate = CommandInputTemplateMap.ReadInputTemplate(reader);

            reader.ReadEndElement( );
        }

        /// <summary>
        /// Writes this template to XML
        /// </summary>
        public void WriteXml( System.Xml.XmlWriter writer )
        {
            throw new ApplicationException( "CursorScenePickTemplate.WriteXml unimplemented, sorry" );
        }

        #endregion

        #region Private stuff

        private InputTemplate m_InputTemplate;

        #endregion
    }
}
