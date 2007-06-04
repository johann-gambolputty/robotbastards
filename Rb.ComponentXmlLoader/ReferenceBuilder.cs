using System;
using System.Collections.Generic;
using System.Text;

using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
    internal class ReferenceBuilder : BaseBuilder
    {
        public ReferenceBuilder( ComponentLoadParameters parameters, System.Xml.XmlElement element ) :
            base( parameters, element )
        {
        }

        public ReferenceBuilder( ComponentLoadParameters parameters, object obj ) :
            base( parameters, null )
        {
            BuildObject = obj;
        }
    }
}
