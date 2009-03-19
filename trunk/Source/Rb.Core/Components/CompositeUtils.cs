using System;
using System.Collections.Generic;
using Rb.Core.Utils;
using System.Collections;
using Rb.Core.Components.Generic;

namespace Rb.Core.Components
{
	/// <summary>
	/// Composite utility methods
	/// </summary>
	public static class CompositeUtils
	{
		/// <summary>
		/// Untyped component visitor delegate
		/// </summary>
		public delegate void VisitorDelegate( object component );
		
		/// <summary>
		/// Typed component visitor delegate
		/// </summary>
		public delegate void VisitorDelegate<T>( T component );

		/// <summary>
		/// Visits all components in a composite
		/// </summary>
		public static void Visit( IComposite composite, VisitorDelegate visitor )
		{
			foreach ( object component in composite.Components )
			{
				visitor( component );
			}
		}

		/// <summary>
		/// Visits a typed composite, and all components
		/// </summary>
		public static void Visit<T>( IComposite<T> composite, VisitorDelegate<T> visitor )
		{
			foreach ( T component in composite.Components )
			{
				visitor( component );
			}
		}


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
			foreach ( object component in composite.Components )
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
			foreach ( object component in composite.Components )
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
