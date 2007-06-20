using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Components
{
    public class DynamicProperty
    {
        public DynamicProperty( string name, object value )
        {
            m_Name  = name;
            m_Value = value;
        }

        public string Name
        {
            get { return m_Name; }
        }

        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        private string m_Name;
        private object m_Value;
    }
}
