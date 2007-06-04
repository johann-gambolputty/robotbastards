using System;
using System.Collections.Generic;
using System.Text;

using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
    internal class ObjectBuilder : BaseBuilder
    {
        public ObjectBuilder( ComponentLoadParameters parameters, System.Xml.XmlElement element ) :
            base( parameters, element )
        {

            Type objectType = Rb.Core.Utils.AppDomainUtils.FindType( element.Attributes[ "type" ].Value );
            BuildObject = parameters.Builder.CreateInstance( objectType );
        }
    }
}
