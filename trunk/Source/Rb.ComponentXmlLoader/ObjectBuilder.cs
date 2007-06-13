using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;

using Rb.Core.Components;
using Rb.Core.Utils;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Creates an object of a given type
    /// </summary>
    internal class ObjectBuilder : TypeBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this object</param>
        /// <param name="parentBuilder">Parent builder</param>
        public ObjectBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder, "type" )
        {
            BuildObject = parameters.Builder.CreateInstance( ( Type )BuildObject );
        }
    }
}
