using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.AssemblySelector
{
    [AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
    public class AssemblyIdentifierAttribute : Attribute
    {
        public AssemblyIdentifierAttribute( string identifier, string value )
        {
            m_Identifier = identifier;
            m_Value = value;
            m_AddToIdMap = false;
        }

        public string Identifier
        {
            get { return m_Identifier; }
        }

        public string Value
        {
            get { return m_Value; }
        }

        public bool AddToIdMap
        {
            get { return m_AddToIdMap; }
            set { m_AddToIdMap = value; }
        }

        private string m_Identifier;
        private string m_Value;
        private bool m_AddToIdMap;
    }
}
