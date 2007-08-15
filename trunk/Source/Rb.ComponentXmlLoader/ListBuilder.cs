using System.Collections;
using System.Xml;
using Rb.Core.Components;


namespace Rb.ComponentXmlLoader
{
    internal class ListBuilder : BaseBuilder
    {
        
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        /// <param name="parentBuilder">Parent builder</param>
        public ListBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
        {
            //  NOTE: AP: This Just Works, because the base builder implementation checks if BuildObject is an
            //  IList, and adds child build objects to it if so... this is all we need to do (in fact, <list>
            //  is just a shorthand for <object type="System.Collections.ArrayList">)
            BuildObject = m_BuildObjects;
        }

        private readonly ArrayList m_BuildObjects = new ArrayList( );
    }
}
