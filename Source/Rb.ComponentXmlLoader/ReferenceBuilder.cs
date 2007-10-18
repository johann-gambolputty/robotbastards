using System;
using System.Xml;

using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// An element that refers to another existing, or created, object
    /// </summary>
    internal class ReferenceBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        /// <param name="parentBuilder">Parent builder object</param>
        public ReferenceBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
        {
            m_ObjectId = reader.GetAttribute( "objectId" );
            m_Properties = reader.GetAttribute( "access" );
        }

		/// <summary>
		/// Resolves the reference in the objectId attribute
		/// </summary>
		protected virtual object ResolveReference( string id )
		{
			//	Empty IDs not allowed
			if ( string.IsNullOrEmpty( id ) )
			{
				throw new ArgumentException( string.Format( "<ref> element must contain an \"objectId\" attribute" ) );
			}

			if ( id == "this" )
			{
				return ParentBuilder.BuildObject;
			}

			if ( id == "parent" )
			{
				if ( ParentBuilder.IsRoot )
				{
					Errors.Add( this, "The parent object does not exist" );
					return null;
				}
				return ParentBuilder.ParentBuilder.BuildObject;
			}

			if ( id == "parameters" )
			{
				return Parameters;
			}

			if ( id == "builder" )
			{
				return Parameters.Builder;
			}

			if ( id == "root" )
			{
				BaseBuilder rootBuilder = ParentBuilder;
				for ( ;!rootBuilder.IsRoot; rootBuilder = rootBuilder.ParentBuilder ) {}

				return rootBuilder.BuildObject;
			}
			
			return Parameters.Objects[ new Guid( id ) ];
		}

        /// <summary>
        /// Resolves the reference
        /// </summary>
        public override void PostCreate( )
        {
			BuildObject = ResolveReference( m_ObjectId );

            if ( m_Properties == null )
			{
				base.PostCreate( );
                return;
            }

            string[] propertyChain = m_Properties.Split( new char[] { '.' } );

            foreach ( string property in propertyChain )
            {
                BuildObject = BuildObject.GetType( ).GetProperty( property ).GetValue( BuildObject, null );
            }

			base.PostCreate( );
        }

        private readonly string m_ObjectId;
		private readonly string m_Properties;
    }

}
