using System;
using System.Collections.Generic;
using Rb.Core.Utils;
using System.Collections;

namespace Rb.Core.Components
{
	/// <summary>
	/// Composite utility methods
	/// </summary>
	public static class CompositeUtils
	{
		/// <summary>
		/// Gets all components from a composite by type
		/// </summary>
		/// <param name="composite">Composite to search</param>
		/// <typeparam name="T">Type of component to retrieve</typeparam>
		/// <returns>Returns an enumerable object containing all components of type T in the composite</returns>
		public static IEnumerable<T> GetComponents<T>( IComposite composite )
		{
			foreach ( object component in composite.Components )
			{
				if ( component is T )
				{
					yield return ( T )component;
				}
			}
		}

		/// <summary>
		/// Gets all components from a composite by type
		/// </summary>
		/// <param name="composite">Composite to search</param>
		/// <param name="componentType">Type of component to retrieve</param>
		/// <returns>Returns an enumerable object containing all components of type T in the composite</returns>
		public static IEnumerable GetComponents( IComposite composite, Type componentType )
		{
			foreach ( IComponent component in composite.Components )
			{
				if ( componentType.IsInstanceOfType( component ) )
				{
					yield return component;
				}
			}
		}

		/// <summary>
		/// Gets the first component in a composite of the specified type
		/// </summary>
		/// <param name="composite">Composite to search</param>
		/// <typeparam name="T">Type of component to retrieve</typeparam>
		/// <returns>Returns an object of type T, or null if no component of type T exists in this composite</returns>
		public static T GetComponent<T>( IComposite composite )
		{
			return ( T )GetComponent( composite, typeof( T ) );
		}

		/// <summary>
		/// Gets the first component in a composite of the specified type
		/// </summary>
		/// <param name="composite">Composite to search</param>
		/// <param name="componentType">Type of component to retrieve</param>
		/// <returns>Returns an object of type T, or null if no component of type T exists in this composite</returns>
		public static object GetComponent( IComposite composite, Type componentType )
		{
			Arguments.CheckNotNull( componentType, "componentType" );
			foreach ( IComponent component in composite.Components )
			{
				if ( componentType.IsInstanceOfType( component ) )
				{
					return component;
				}
			}
			return null;
		}

		/// <summary>
		/// Removes all components derived from, or implementing, the specified type
		/// </summary>
		/// <param name="composite">Composite to search</param>
		public static void RemoveAllOfType<T>( IComposite composite )
		{
			RemoveAllOfType( composite, typeof( T ) );
		}

		/// <summary>
		/// Removes all components derived from, or implementing, the specified type
		/// </summary>
		/// <param name="composite">Composite to search</param>
		/// <param name="type">Component type to remove</param>
		public static void RemoveAllOfType( IComposite composite, Type type )
		{
			IEnumerable allComponents = new ArrayList( composite.Components );
			foreach ( object component in allComponents )
			{
				if ( type.IsInstanceOfType( component ) )
				{
					composite.Remove( component );
				}
			}
		}
	}
}
