using System.Xml;

using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{

    /// <summary>
    /// Extends ReferenceBuilder. Creates an instance from the referenced object
    /// </summary>
    internal class InstanceBuilder : ReferenceBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        public InstanceBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader ) :
            base( parameters, errors, reader )
        {
        }

        /// <summary>
        /// Resolves the reference
        /// </summary>
        public override void PostCreate( )
        {
            base.PostCreate( );
            BuildObject = ( ( IInstanceBuilder )BuildObject ).CreateInstance( );
        }
    }
}