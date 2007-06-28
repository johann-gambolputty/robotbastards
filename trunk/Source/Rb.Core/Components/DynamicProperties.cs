using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Components
{
    /// <summary>
    /// Implementation of the IDynamicProperties interface, using a Dictionary of DynamicProperty objects
    /// </summary>
    public class DynamicProperties : IDynamicProperties
    {
        #region IDynamicProperties Members
        
        /// <summary>
        /// Property indexer
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Property value</returns>
        public object this[ string name ]
        {
            get
            {
                return m_Properties[ name ].Value;
            }
            set
            {
                DynamicProperty property;
                if ( m_Properties.TryGetValue( name, out property ) )
                {
                    property.Value = value;
                }
                else
                {
                    m_Properties[ name ] = new DynamicProperty( name, value );
                }
            }
        }

        /// <summary>
        /// Adds a new named property to the set
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">New property value</param>
        public void Add( string name, object value )
        {
            DynamicProperty property = new DynamicProperty( name, value );
            m_Properties.Add( property.Name, property );
        }

        /// <summary>
        /// Removes an existing property from the set
        /// </summary>
        /// <param name="name">Name of the property to remove</param>
        public void Remove( string name )
        {
            m_Properties.Remove( name );
        }

        /// <summary>
        /// Gets the value of an existing property
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Property value. Returns null if the property could not be found</returns>
        public object Get( string name )
        {
            DynamicProperty property;
            return m_Properties.TryGetValue( name, out property ) ? property.Value : null;
        }

        /// <summary>
        /// Sets the value of an existing property
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        public void Set( string name, object value )
        {
            m_Properties[ name ].Value = value;
        }

        #endregion

        #region IEnumerable<DynamicProperty> Members

        /// <summary>
        /// Returns an enumerator that iterates through the set of properties
        /// </summary>
        public IEnumerator< DynamicProperty > GetEnumerator( )
        {
            foreach ( KeyValuePair< string, DynamicProperty > kvp in m_Properties )
            {
                yield return kvp.Value;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the set of properties
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator( )
        {
            foreach ( KeyValuePair< string, DynamicProperty > kvp in m_Properties )
            {
                yield return kvp.Value;
            }
        }

        #endregion

		#region IDynamicProperties Static Helpers

		/// <summary>
		/// Tries to get a property from properties with a given name. If it doesn't exist, defaultValue
		/// is returned. If it does exist, the property's value is returned
		/// </summary>
		public static T GetProperty< T >( IDynamicProperties properties, string name, T defaultValue )
		{
			object val = properties.Get( name );
			return ( val == null ) ? defaultValue : ( T )val;
		}

		#endregion

		#region Private stuff

		private Dictionary< string, DynamicProperty > m_Properties = new Dictionary< string, DynamicProperty >( );

        #endregion
    }
}
