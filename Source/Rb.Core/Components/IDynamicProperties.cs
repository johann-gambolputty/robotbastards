using System.Collections.Generic;

namespace Rb.Core.Components
{
    /// <summary>
    /// Interface for classes that store sets of DynamicProperty objects
    /// </summary>
    public interface IDynamicProperties : IEnumerable< IDynamicProperty >
    {
        /// <summary>
        /// Property indexer
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Property value</returns>
        object this[ string name ]
        {
            get;
            set;
        }

		/// <summary>
		/// Adds a new property to the set
		/// </summary>
		/// <param name="property">Property object</param>
		void Add( IDynamicProperty property );

        /// <summary>
        /// Adds a new named property to the set
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">New property value</param>
        void Add( string name, object value );

        /// <summary>
        /// Removes an existing property from the set
        /// </summary>
        /// <param name="name">Name of the property to remove</param>
        void Remove( string name );

		/// <summary>
		/// Returns true if the set contains a property with the specified name
		/// </summary>
		/// <param name="name">Property name</param>
		/// <returns>true if set contains named property</returns>
		bool Contains( string name );

        /// <summary>
        /// Gets the value of an existing property
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Property value. Returns null if the property could not be found</returns>
        object Get( string name );

        /// <summary>
        /// Sets the value of an existing property
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        void Set( string name, object value );
    }
}
