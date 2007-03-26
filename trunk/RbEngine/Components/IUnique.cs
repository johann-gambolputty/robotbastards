using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Uniquely identifies this object
	/// </summary>
	public interface IUnique
	{
		/// <summary>
		/// Access to the unique identifier of this object
		/// </summary>
		ObjectId Id
		{
			get;
			set;
		}
	}
}
