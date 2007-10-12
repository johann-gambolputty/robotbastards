
namespace Rb.Core.Components
{

	/// <summary>
	/// Handy helpers
	/// </summary>
	public static class ComponentHelpers
	{
		/// <summary>
		/// Returns an empty string if obj does not implement INamed. Otherwise, returns INamed.Name
		/// </summary>
		/// <param name="obj">Object to check</param>
		/// <returns>Object name</returns>
		public static string GetName( object obj )
		{
			INamed named = obj as INamed;
			return ( named == null ) ? obj.ToString( ) : named.Name;
		}
	}

}