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
        }

		/// <summary>
		/// Invokes the method to create the build object
		/// </summary>
        public override void PostCreate( )
        {
            if ( m_ParamBuilders.Count == 0 )
            {
                BuildObject = Parameters.Builder.CreateInstance( ( Type )BuildObject );
            }
            else
            {
                //	Call PostCreate() for all parameter builders
                foreach ( BaseBuilder builder in m_ParamBuilders )
                {
                    SafePostCreate( builder );
                }

                //	Call Resolve() for all parameter builders, and create parameter type and
                //	object lists, for method resolution and calling respectively
                object[] parameters = new object[ m_ParamBuilders.Count ];
                int paramIndex = 0;
                foreach ( BaseBuilder builder in m_ParamBuilders )
                {
                    // TODO: AP: A bit dodgy - doing resolution phase during post create phase...
                    SafeResolve( builder );
                    parameters[ paramIndex ] = builder.BuildObject;
                    ++paramIndex;
                }

                BuildObject = Parameters.Builder.CreateInstance( ( Type )BuildObject, parameters );
            }

            base.PostCreate( );
        }

		/// <summary>
		/// Reads "<param/>" elements
		/// </summary>
		/// <param name="reader">XML reader positioned at element</param>
		/// <param name="builders">Builder list</param>
		protected override void HandleElement( XmlReader reader, List<BaseBuilder> builders )
		{
			if ( reader.Name == "parameters" )
			{
				HandleChildElements( reader, m_ParamBuilders );
			}
			else
			{
				base.HandleElement( reader, builders );
			}
		}

        private List< BaseBuilder > m_ParamBuilders = new List< BaseBuilder >( );
    }
}
