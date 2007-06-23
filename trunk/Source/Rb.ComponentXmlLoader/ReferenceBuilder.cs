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
        public ReferenceBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
        {
            //  TODO: AP: Handle modelPath
            m_ObjectId = reader.GetAttribute( "objectId" );
            if ( m_ObjectId == null )
            {
                throw new ApplicationException( string.Format( "<{0}> element must contain an \"objectId\" attribute", reader.Name ) );
            }

            m_Properties = reader.GetAttribute( "access" );
        }

        /// <summary>
        /// Resolves the reference
        /// </summary>
        public override void PostCreate( )
        {
            if ( m_ObjectId == "this" )
            {
                BuildObject = ParentBuilder.BuildObject;
            }
            else if ( m_ObjectId == "parent" )
            {
                if ( ParentBuilder.IsRoot )
                {
                    Errors.Add( this, "The parent object does not exist" );
                }
                else
                {
                    BuildObject = ParentBuilder.ParentBuilder.BuildObject;
                }
            }
            else if ( m_ObjectId == "root" )
            {
                BaseBuilder rootBuilder = ParentBuilder;
                for ( ;!rootBuilder.IsRoot; rootBuilder = rootBuilder.ParentBuilder );

                BuildObject = rootBuilder.BuildObject;
            }
            else
            {
                BuildObject = Parameters.Objects[ new Guid( m_ObjectId ) ];
            }

            if ( m_Properties == null )
            {
                return;
            }

            string[] propertyChain = m_Properties.Split( new char[] { '.' } );

            foreach ( string property in propertyChain )
            {
                BuildObject = BuildObject.GetType( ).GetProperty( property ).GetValue( BuildObject, null );
            }

			base.PostCreate( );
        }

        private string m_ObjectId;
        private string m_Properties;
    }

}
