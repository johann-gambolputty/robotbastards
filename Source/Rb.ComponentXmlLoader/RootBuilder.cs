using System;
using System.Collections;
using System.Xml;

using Rb.Core.Utils;
using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Handles the root element in a component XML document
    /// </summary>
    internal class RootBuilder : BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error log</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        public RootBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader ) :
            base( parameters, errors, reader, null )
        {
            if ( reader.Name != "rb" )
            {
                errors.Add( reader, "Expected root node of component XML document to be named <rb>, not <{0}>", reader.Name );
            }
			BuildObject = parameters.Target ?? m_ChildObjects;
        }

		/// <summary>
		/// Gets the list of child objects
		/// </summary>
		public ArrayList Children
		{
			get { return m_ChildObjects; }
		}

		/// <summary>
		/// Adds child to the child object list, if the current build object is the external target
		/// </summary>
		protected override void OnLink( object child )
		{
			base.OnLink( child );
			if ( BuildObject != m_ChildObjects )
			{
				m_ChildObjects.Add( child );
			}
		}
		private readonly ArrayList m_ChildObjects = new ArrayList( );
    }
}
