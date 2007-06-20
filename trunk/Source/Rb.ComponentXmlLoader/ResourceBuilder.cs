using System.Xml;

using Rb.Core.Resources;
using Rb.Core.Components;


namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// An element that references an existing resource, or loads it
    /// </summary>
    internal class ResourceBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        /// <param name="parentBuilder">Parent builder</param>
        public ResourceBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
        {
            string resourcePath = reader.GetAttribute( "path" );
            BuildObject = ResourceManager.Instance.Load( resourcePath );
        }
    }

}