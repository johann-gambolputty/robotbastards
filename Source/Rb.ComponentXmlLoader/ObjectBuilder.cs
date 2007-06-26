using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

using Rb.Core.Components;
using Rb.Core.Utils;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Creates an object of a given type
    /// </summary>
    internal class ObjectBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this object</param>
        /// <param name="parentBuilder">Parent builder</param>
        public ObjectBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
		{
			//  Retrieve type name and optional assembly name from the element
			string typeName = reader.GetAttribute( "type" );
			string assemblyName = reader.GetAttribute( "assembly" );

			if ( typeName == null )
			{
				throw new ApplicationException( string.Format( "Element \"{0}\" requires a \"type\" attribute", reader.Name ) );
			}

			Type objectType = null;
			if ( assemblyName == null )
			{
				//  Get the object type from the currently loaded set of assemblies
				objectType = AppDomainUtils.FindType( typeName );
				if ( objectType == null )
				{
					throw new ApplicationException( string.Format( "Failed to find type \"{0}\" in app domain", typeName ) );
				}
			}
			else
			{
				//  Get the object type from the specified assembly
				Assembly assembly = AppDomain.CurrentDomain.Load( assemblyName );
				objectType = assembly.GetType( typeName );
				if ( objectType == null )
				{
					throw new ApplicationException( string.Format( "Failed to find type \"{0}\" in assembly \"{1}\"", typeName, assemblyName ) );
				}
			}

			m_BuildType = objectType;
        }

		/// <summary>
		/// Invokes the method to create the build object
		/// </summary>
        public override void PostCreate( )
        {
			CreateBuildObject( );

            base.PostCreate( );
        }

		/// <summary>
		/// Creates the build object
		/// </summary>
		private void CreateBuildObject( )
		{
			if ( BuildObject != null )
			{
				return;
			}

			if ( m_ParamBuilders.Count == 0 )
			{
				BuildObject = Parameters.Builder.CreateInstance( m_BuildType );
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

				BuildObject = Parameters.Builder.CreateInstance( m_BuildType, parameters );
			}
		}

		/// <summary>
		/// Reads "<parameters/>" elements
		/// </summary>
		/// <param name="reader">XML reader positioned at element</param>
		/// <param name="builders">Builder list</param>
		protected override void HandleElement( XmlReader reader, List<BaseBuilder> builders )
		{
			if ( ( reader.Name == "serialise" ) || ( reader.Name == "serialize" ) )
			{
				//	Gotta create the object now to deserialize it
				CreateBuildObject( );

				IXmlSerializable objectReader = BuildObject as IXmlSerializable;
				if ( objectReader == null )
				{
					throw new ApplicationException( string.Format( "Object type \"{0}\" does not support serialization", BuildObject.GetType( ) ) );
				}
				objectReader.ReadXml( reader );
			}
			else if ( reader.Name == "parameters" )
			{
				if ( BuildObject != null )
				{
					throw new ApplicationException( string.Format( "Object of type\"{0}\" has already been created due to <serialize> element - can't handle <parameters>", BuildObject.GetType( ) ) );
				}
				HandleChildElements( reader, m_ParamBuilders );
			}
			else
			{
				base.HandleElement( reader, builders );
			}
		}

		private Type				m_BuildType;
        private List< BaseBuilder >	m_ParamBuilders = new List< BaseBuilder >( );
    }
}
