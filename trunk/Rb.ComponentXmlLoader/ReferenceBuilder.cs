using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Rb.Core.Utils;
using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// An element that refers to another existing, or created, object
    /// </summary>
    internal class ReferenceBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        public ReferenceBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader ) :
            base( parameters, errors, reader )
        {
            //  TODO: AP: Handle modelPath
            m_ObjectId = reader.GetAttribute( "objectId" );
            if ( m_ObjectId == null )
            {
                throw new ApplicationException( string.Format( "<{0}> element must contain an \"objectId\" attribute", reader.Name ) );
            }
        }

        /// <summary>
        /// Resolves the reference
        /// </summary>
        public override void PostCreate( )
        {
            BuildObject = Parameters.Objects[ new Guid( m_ObjectId ) ];
        }

        private string m_ObjectId;
    }

}
