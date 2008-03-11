using System;
using System.Collections.Generic;
using System.Xml;
using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{

    /// <summary>
    /// Builds an instance from the child object
    /// </summary>
	internal class InstanceBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        /// <param name="parentBuilder">Parent builder</param>
        public InstanceBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder )
        {
        }

        /// <summary>
        /// Resolves the reference
        /// </summary>
		public override void PostCreate( )
        {
			base.PostCreate( );

			List< BaseBuilder > builders = GetBuilders( LinkStep.PreLink );
			if ( builders.Count == 0 )
			{
				builders = GetBuilders( LinkStep.PostLink );
			}
			if ( builders.Count == 0 )
			{
				throw new ApplicationException( "<instance> element had no child element to instance" );
			}
			if ( builders.Count > 1 )
			{
				throw new ApplicationException( "<instance> element can have only instance one child element" );
			}

        	//	TODO: AP: Also support ICloneable
			BuildObject = ( ( IInstanceBuilder )builders[ 0 ].BuildObject ).CreateInstance( Parameters.Builder );
        }

    }
}