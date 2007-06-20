using System;
using System.Xml;

using Rb.Core.Utils;
using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Handles the root element in a component XML document
    /// </summary>
    internal class RootBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error log</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        public RootBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader ) :
            base( parameters, errors, reader, null )
        {
            if ( reader.Name != "rb" )
            {
                errors.Add( reader, "Expected root node of component XML document to be named <rb>, not <{0}>", reader.Name );
            }
            m_ExternalTarget = parameters.Target;
            BuildObject = m_ExternalTarget;
        }

        /// <summary>
        /// Called prior to Resolve()
        /// </summary>
        public override void PostCreate( )
        {
            base.PostCreate( );
            if ( m_ExternalTarget == null )
            {
                GetBuildObject( LinkStep.PreLink );
                GetBuildObject( LinkStep.PostLink );
            }
        }

        /// <summary>
        /// Gets the BuildObject for this builder.
        /// </summary>
        /// <param name="step">Child builder list to check</param>
        private void GetBuildObject( LinkStep step )
        {
            foreach ( BaseBuilder builder in GetBuilders( step ) )
            {
                if ( BuildObject != null )
                {
                    Errors.Add( builder, "The root <rb> object can only have one child element, if it has no external target" );
                    return;
                }
                BuildObject = builder.BuildObject;
            }
        }

        private object m_ExternalTarget;
    }
}
