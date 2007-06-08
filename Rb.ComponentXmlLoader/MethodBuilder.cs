using System.Xml;
using System.Reflection;

using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Calls a method on a referenced object
    /// </summary>
    internal class MethodBuilder : ReferenceBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        public MethodBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader ) :
            base( parameters, errors, reader )
        {
            string methodName = reader.GetAttribute( "method" );
        }

        public override void PostCreate( BaseBuilder parentBuilder )
        {
            base.PostCreate( parentBuilder );

            parentBuilder.BuildObject.GetType( ).GetMethod( m_MethodName, BindingFlags.Public | BindingFlags.Instance );
        }
    }
}
