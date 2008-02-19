using System;
using System.Collections.Generic;

namespace Rb.Core.Components
{
    /// <summary>
    /// Interface for collections that associate guid keys with objects
    /// </summary>
    public interface IObjectMap : IDictionary< Guid, object >
	{
		/// <summary>
		/// Adds any object to the map, assigning a new ID
		/// </summary>
		/// <param name="obj">Unique object</param>
		/// <returns>Returns the ID of the added object</returns>
		Guid Add( object obj );

    	/// <summary>
    	/// Adds an IUnique object to the map
    	/// </summary>
    	/// <param name="obj">Unique object</param>
    	void Add( IUnique obj );

		/// <summary>
		/// Removes an IUnique object from the map
		/// </summary>
		/// <param name="obj">Unique object</param>
		void Remove( IUnique obj );

		/// <summary>
		/// Removes an object from the map
		/// </summary>
		void Remove( object obj );

		/// <summary>
		/// Gets an object of a given type, and key, from the map
		/// </summary>
		object Get( Type objectType, Guid key );

		/// <summary>
		/// Gets an object of a given type, and key, from the map
		/// </summary>
		object Get< T >( Guid key );

        /// <summary>
        /// Gets all objects of a given type from the map
        /// </summary>
        IEnumerable< T > GetAllOfType< T >( );
    }
}
