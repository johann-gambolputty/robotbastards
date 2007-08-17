using System;
using System.Xml;
using System.Collections.Generic;
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

            m_PreLoad = ResourceManager.Instance.CreatePreLoadState( resourcePath, null );

			string useCurrentParams = reader.GetAttribute( "useCurrentParameters" );
			if ( useCurrentParams != null )
			{
				if ( bool.Parse( useCurrentParams ) )
				{
					m_PreLoad.Parameters = Parameters;
				}
			}
        }

        /// <summary>
        /// Pre-load. Loads the resource
        /// </summary>
        public override void PostCreate( )
        {
            if ( m_ParamBuilders.Count == 0 )
            {
                BuildObject = m_PreLoad.Load( );
            }
            else
            {
                //  Create default load parameters, assign them to BuildObject (cheeky way for parameter
                //  builders to set load parameters)
				if ( ReferenceEquals( m_PreLoad.Parameters, Parameters ) )
				{
					//	useCurrentLoadParameters was specified - to avoid modifying the original, clone it
					m_PreLoad.Parameters = ( LoadParameters )Parameters.Clone( );
				}
				else
				{
					m_PreLoad.Parameters = m_PreLoad.Loader.CreateDefaultLoadParameters( );
				}
                BuildObject = m_PreLoad.Parameters;

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
                    SafeResolve( builder, true );
                    parameters[ paramIndex ] = builder.BuildObject;
                    ++paramIndex;
                }

				BuildObject = m_PreLoad.Load( );
            }

            base.PostCreate( );
        }

        /// <summary>
        /// Reads "<parameters/>" elements
        /// </summary>
        /// <param name="reader">XML reader positioned at element</param>
        /// <param name="builders">Builder list</param>
        protected override void HandleElement( XmlReader reader, List< BaseBuilder > builders )
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

        private readonly ResourcePreLoadState	m_PreLoad;
		private readonly List< BaseBuilder >	m_ParamBuilders = new List< BaseBuilder >( );
    }

}