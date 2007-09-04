
namespace Rb.Core.Components
{
	/// <summary>
	/// IParent helper functions
	/// </summary>
	public static class ParentHelpers
	{
		#region IParent Helpers

		/// <summary>
		/// Returns parent, if parent is of type T, or the first child of a given type in parent
		/// </summary>
		public static T GetType< T >( object obj ) where T : class
		{
			if ( obj is T )
			{
				return ( T )obj;
			}
			return GetChildOfType< T >( obj as IParent );
		}

		/// <summary>
		/// Returns the first child of a given type in the specified IParent object
		/// </summary>
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
