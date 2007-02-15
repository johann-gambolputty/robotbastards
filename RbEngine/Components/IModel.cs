using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Summary description for IModel.
	/// </summary>
	public interface IModel
	{
		/// <summary>
		/// Creates an instance of the data stored in this model
		/// </summary>
		object CreateInstance( );
	}
}
