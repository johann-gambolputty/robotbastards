using System;

namespace Rb.Core.Components
{
	/// <summary>
	/// Dynamic property
	/// </summary>
	[Serializable]
	public class DynamicProperty : IDynamicProperty
    {
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="value">Property value</param>
        public DynamicProperty( string name, object value )
        {
            m_Name  = name;
            m_Value = value;
        }

		/// <summary>
		/// Gets the name of this property
		/// </summary>
        public string Name
        {
            get { return m_Name; }
        }

		/// <summary>
		/// Accessor of the value of this property
		/// </summary>
        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        private readonly string m_Name;
        private object m_Value;
    }
}
