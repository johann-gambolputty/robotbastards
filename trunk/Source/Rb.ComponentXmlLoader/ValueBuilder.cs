using System.Xml;

using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{

    /// <summary>
    /// Extends ReferenceBuilder. Creates an instance from the referenced object
    /// </summary>
    internal class ValueBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        /// <param name="val">Value instance</param>
        public ValueBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, object val ) :
            base( parameters, errors, reader )
        {
            BuildObject = val;
        }
    }
}