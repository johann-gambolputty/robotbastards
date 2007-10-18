using System;
using System.Xml;
using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{

    /// <summary>
    /// Extends ReferenceBuilder. Creates an instance from the referenced object
    /// </summary>
    internal class InstanceBuilder : ReferenceBuilder
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

			//	TODO: AP: Also support ICloneable
            BuildObject = ( ( IInstanceBuilder )BuildObject ).CreateInstance( Parameters.Builder );
        }

		/// <summary>
		/// Resolves the reference in the objectId attribute
		/// </summary>
		protected override object ResolveReference( string id )
		{
			if ( string.IsNullOrEmpty( id ) )
			{
				//	Just a reference to the first child
				if ( GetBuilders( LinkStep.Default ).Count == 0 )
				{
					throw new ArgumentException( "An <instance> element without an objectId attribute instances the first child element - no children exist" );
				}
				return GetBuilders( LinkStep.Default )[ 0 ].BuildObject;
			}
			return base.ResolveReference( id );
		}
    }
}