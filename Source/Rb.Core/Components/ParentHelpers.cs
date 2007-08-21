
namespace Rb.Core.Components
{
	/// <summary>
	/// IParent helper functions
	/// </summary>
	public static class ParentHelpers
	{
		#region IParent Helpers

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
