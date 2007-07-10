namespace Rb.TestApp
{
	/// <summary>
	/// Interface that supports a UI for resource providers 
	/// </summary>
	interface IResourceProviderUi
	{
		/// <summary>
		/// Returns the path of a resource avaiable in the provider
		/// </summary>
		string GetResourcePath( string filter );
	}
}
