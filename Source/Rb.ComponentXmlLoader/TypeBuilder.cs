using System;
using System.Xml;
using System.Reflection;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Resolves to a Type
    /// </summary>
    internal class TypeBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        /// <param name="parentBuilder">Parent builder</param>
        public TypeBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
        {
            //  Retrieve type name and optional assembly name from the element
            string typeName     = reader.GetAttribute( "value" );
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
                    throw new ApplicationException( string.Format( "Failed to find type \"{0}\" in app domain" , typeName ) );
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

			BuildObject = objectType;
        }

    }
}
