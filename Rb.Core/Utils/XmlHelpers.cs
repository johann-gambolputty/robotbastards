using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Rb.Core.Utils
{

    /// <summary>
    /// XML helper methods
    /// </summary>
    public static class XmlHelpers
    {
        /// <summary>
        /// Gets the value of an attribute, returning defaultValue if the attribute does not exist
        /// </summary>
        public static string GetAttributeValue( XmlElement element, string attributeName, string defaultValue )
        {
            XmlNode attributeNode = element.Attributes.GetNamedItem( attributeName );
            return attributeNode == null ? defaultValue : attributeNode.Value;
        }
    }
}
