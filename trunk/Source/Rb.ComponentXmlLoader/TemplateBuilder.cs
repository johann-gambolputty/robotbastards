
using System;
using System.Reflection;
using System.Xml;
using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
	/// <summary>
	/// Creates an object pattern of a given type
	/// </summary>
	class TemplateBuilder : BaseBuilder
	{
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this object</param>
        /// <param name="parentBuilder">Parent builder</param>
        public TemplateBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
		{
			//  Retrieve type name and optional assembly name from the element
			string typeName = reader.GetAttribute( "type" );
			string assemblyName = reader.GetAttribute( "assembly" );

			m_Type = ObjectBuilder.ResolveType( reader.Name, typeName, assemblyName );
        }


		/// <summary>
		/// Invokes the method to create the build object
		/// </summary>
		public override void PostCreate( )
		{
			BuildObject = new ObjectTemplate( m_Type, "" );
			base.PostCreate( );
		}
		
        /// <summary>
        /// Links the BuildObject to the property of a parent
        /// </summary>
        /// <param name="parent">Parent object</param>
        /// <param name="property">Property information</param>
        protected virtual void LinkToProperty( object parent, PropertyInfo property )
        {
		}

		private readonly Type m_Type;
	}
}
