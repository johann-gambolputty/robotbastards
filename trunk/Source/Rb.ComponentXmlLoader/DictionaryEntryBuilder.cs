using System.Collections;
using System.Xml;
using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
	/// <summary>
	/// Extends ValueBuilder. Stores a DictionaryEntry value
	/// </summary>
	internal class DictionaryEntryBuilder : ValueBuilder
	{
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        /// <param name="parentBuilder">Parent builder</param>
        public DictionaryEntryBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder ) :
            base( parameters, errors, reader, parentBuilder, new DictionaryEntry( ) )
        {
        }

		/// <summary>
		/// Gets the default link step for children of this builder
		/// </summary>
		/// <remarks>
		/// Returns <see cref="BaseBuilder.LinkStep.PreLink"/>, because the key and value of the dictionary
		/// entry must be set before the link (which adds the dictionary entry to the dictionary)
		/// </remarks>
		protected override LinkStep DefaultLinkStep
		{
			get { return LinkStep.PreLink; }
		}
	}
}
