using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
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

			Construct( reader.Name, typeName, assemblyName );
        }
		
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this object</param>
        /// <param name="parentBuilder">Parent builder</param>
        /// <param name="typeName">Name of the type to create</param>
        public ObjectBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder, string typeName ) :
            base( parameters, errors, reader, parentBuilder )
		{
			string assemblyName = reader.GetAttribute( "assembly" );
			Construct( reader.Name, typeName, assemblyName );
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
					SafeResolve( builder, false );
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

		/// <summary>
		/// Resolves a type, read from a given XML element
		/// </summary>
		/// <param name="elementName">Element name</param>
		/// <param name="typeName">Type name</param>
		/// <param name="assemblyName">Assembly name</param>
		/// <returns>Returns the resolved type</returns>
		public static Type ResolveType( string elementName, string typeName, string assemblyName )
		{
			if ( typeName == null )
			{
				throw new ApplicationException( string.Format( "Element \"{0}\" requires a \"type\" attribute", elementName ) );
			}

			Type objectType;
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

			return objectType;
		}

		private Type m_BuildType;
        private readonly List< BaseBuilder > m_ParamBuilders = new List< BaseBuilder >( );
		
		/// <summary>
		/// Shared construction code
		/// </summary>
		/// <param name="elementName">XML element name</param>
		/// <param name="typeName">Object type name</param>
		/// <param name="assemblyName">Type assembly</param>
		private void Construct( string elementName, string typeName, string assemblyName )
		{
			m_BuildType = ResolveType( elementName, typeName, assemblyName );
		}
    }
}
