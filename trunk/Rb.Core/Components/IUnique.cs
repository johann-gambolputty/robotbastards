using System;

namespace Rb.Core.Components
{

	/// <summary>
	/// Objects implementing this interface can be uniquely identified
	/// </summary>
	public interface IUnique
	{
		/// <summary>
		/// Access to the unique ID of this object
		/// </summary>
		Guid Id { get; set; }
	};

};