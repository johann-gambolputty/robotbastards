using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Components
{
    /// <summary>
    /// Interface for collections that associate guid keys with objects
    /// </summary>
    public interface IObjectMap : IDictionary< Guid, object >
	{
		/// <summary>
		/// Gets an object of a given type, and key, from the map
		/// </summary>
		object Get( Type objectType, Guid key );

		/// <summary>
		/// Gets an object of a given type, and key, from the map
		/// </summary>
		object Get< T >( Guid key );
    }
}
