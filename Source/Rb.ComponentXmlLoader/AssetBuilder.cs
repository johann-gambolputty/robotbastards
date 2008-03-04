using System.Xml;
using System.Collections.Generic;
using Rb.Core.Components;
using Rb.Core.Assets;


namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// An element that references an existing resource, or loads it
    /// </summary>
    internal class AssetBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        /// <param name="parentBuilder">Parent builder</param>
        public AssetBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
        {
            string assetPath = reader.GetAttribute( "path" );

			string instance = reader.GetAttribute( "instance" );
        	m_Instance = string.IsNullOrEmpty( instance ) ? false : bool.Parse( instance );
            m_Loader = AssetManager.Instance.CreateLoadState( new Location( assetPath ), null );

			string useCurrentParams = reader.GetAttribute( "useCurrentParameters" );
			if ( useCurrentParams != null )
			{
				if ( bool.Parse( useCurrentParams ) )
				{
					m_Loader.Parameters = Parameters;
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
                BuildObject = m_Loader.Load( );
            }
            else
            {
                //  Create default load parameters, assign them to BuildObject (cheeky way for parameter
                //  builders to set load parameters)
				if ( ReferenceEquals( m_Loader.Parameters, Parameters ) )
				{
					//	useCurrentLoadParameters was specified - to avoid modifying the original, clone it
					m_Loader.Parameters = ( LoadParameters )Parameters.Clone( );
				}
				else
				{
					m_Loader.Parameters = m_Loader.Loader.CreateDefaultParameters( false );
				}
                BuildObject = m_Loader.Parameters;

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

				BuildObject = m_Loader.Load( );
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

		private readonly bool					m_Instance;
        private readonly LoadState				m_Loader;
		private readonly List< BaseBuilder >	m_ParamBuilders = new List< BaseBuilder >( );
    }

}