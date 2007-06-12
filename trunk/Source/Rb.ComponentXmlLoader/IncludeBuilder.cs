using System;
using System.Xml;
using Rb.Core.Components;
using Rb.Core.Resources;


namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Includes the contents of another XML file
    /// </summary>
    internal class IncludeBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        public IncludeBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader ) :
            base( parameters, errors, reader )
        {
            string path = reader.GetAttribute( "path" );
            if ( path == null )
            {
                throw new ApplicationException( string.Format( "Element <{0}> must have a \"path\" attribute", reader.Name ) );
            }
        }
    }
}
