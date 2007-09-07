
namespace Rb.Core.Components
{
	/// <summary>
	/// IParent helper functions
	/// </summary>
	public static class Parent
	{
		#region IParent Helpers

		/// <summary>
		/// Searches obj for an instance of type T. If one is found, it's returned. Otherwise, an instance of NewType
		/// is created, added to obj, and returned
		/// </summary>
		/// <typeparam name="T">Type to search for</typeparam>
		/// <typeparam name="NewType">Type to create, if instance of T is not found</typeparam>
		/// <param name="obj">Containing object</param>
		/// <returns>Returns instance of T</returns>
		public static T GetOrCreateType< T, NewType >( object obj )
			where T : class
			where NewType : T, new()
		{
			T result = GetType< T >( obj );
			if ( result != null )
			{
				return result;
			}
			result = new NewType( );
			( ( IParent )obj ).AddChild( result );
			return result;
		}

		/// <summary>
		/// Searches obj for an instance of type T
		/// </summary>
		/// <param name="obj">Containing object</param>
		/// <returns>Returns obj, if obj is of type T. If obj implements IParent, Returns null if </returns>
		public static T GetType< T >( object obj ) where T : class
		{
			if ( obj is T )
			{
				return ( T )obj;
			}
			return GetChildOfType< T >( obj as IParent );
		}

		/// <summary>
		/// Searches parent for a child object of type T
		/// </summary>
		/// <param name="parent">Containing object</param>
		/// <returns>Returns the first child of a type T in the parent object. Returns null if none can be found</returns>
		public static T GetChildOfType< T >( IParent parent ) where T : class
		{
			if ( parent == null )
			{
				return null;
			}
			foreach ( object obj in parent.Children )
			{
				T objT = obj as T;
				if ( objT != null )
				{
					return objT;
				}
			}
			return null;
		}

		#endregion
	}
}
