using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

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
        /// <param name="parentBuilder">Parent builder</param>
        public MethodBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
        {
			m_MethodName = reader.GetAttribute( "call" );
        }

		/// <summary>
		/// Invokes the method to create the build object
		/// </summary>
        public override void PostCreate( )
		{
			base.PostCreate( );

			//	Call PostCreate() for all parameter builders
			foreach ( BaseBuilder builder in m_ParamBuilders )
			{
				SafePostCreate( builder );
			}

			//	Call Resolve() for all parameter builders, and create parameter type and
			//	object lists, for method resolution and calling respectively
			object[] parameters = new object[ m_ParamBuilders.Count ];
			Type[] parameterTypes = new Type[ m_ParamBuilders.Count ];
			int paramIndex = 0;
			foreach ( BaseBuilder builder in m_ParamBuilders )
			{
				// TODO: AP: A bit dodgy - doing resolution phase during post create phase...
				SafeResolve( builder, false );
				parameters[ paramIndex ] = builder.BuildObject;
				parameterTypes[ paramIndex ] = builder.BuildObject.GetType( );
			    ++paramIndex;
			}

			//	Find the method
			MethodInfo method = BuildObject.GetType( ).GetMethod( m_MethodName, parameterTypes );//, BindingFlags.Public | BindingFlags.Instance );
			if ( method == null )
			{
				throw new ApplicationException( string.Format( "Could not find method \"{0}\" in type \"{1}\" (may be incorrect parameter types)", m_MethodName, BuildObject.GetType( ) ) );
			}

			//	Invoke the method
			BuildObject = method.Invoke( BuildObject, parameters );
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

		private List< BaseBuilder >	m_ParamBuilders = new List< BaseBuilder >( );
		private string				m_MethodName;
    }
}
