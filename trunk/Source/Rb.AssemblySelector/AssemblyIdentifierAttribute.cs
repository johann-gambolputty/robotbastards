using System;

namespace Rb.AssemblySelector
{
	/// <summary>
	/// Tags an assembly with a key/value pair used by IdentifierMap
	/// </summary>
	/// <example>
	/// [assembly: AssemblyIdentifier( "fish", "pie" )]
	/// </example>
    [AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
    public class AssemblyIdentifierAttribute : Attribute
    {
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="identifier">Identifier key</param>
		/// <param name="value">Identifier value</param>
        public AssemblyIdentifierAttribute( string identifier, string value )
        {
            m_Identifier = identifier;
            m_Value = value;
            m_AddToIdMap = false;
        }

		/// <summary>
		/// Identifier name
		/// </summary>
        public string Identifier
        {
            get { return m_Identifier; }
        }

		/// <summary>
		/// Identifier value
		/// </summary>
        public string Value
        {
            get { return m_Value; }
        }

		/// <summary>
		/// If true, this identifier is added to the <see cref="IdentifierMap"/>
		/// </summary>
        public bool AddToIdMap
        {
            get { return m_AddToIdMap; }
            set { m_AddToIdMap = value; }
		}

		#region Private stuff

		private string m_Identifier;
        private string m_Value;
        private bool m_AddToIdMap;

		#endregion
	}
}
