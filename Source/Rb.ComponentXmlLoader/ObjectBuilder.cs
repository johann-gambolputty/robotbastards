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
    internal class ObjectBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this object</param>
        public ObjectBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader ) :
            base( parameters, errors, reader )
        {
            //  Retrieve type name and optional assembly name from the element
            string typeName     = reader.GetAttribute( "type" );
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

            BuildObject = parameters.Builder.CreateInstance( objectType );
        }
    }
}
