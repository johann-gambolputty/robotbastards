using System;

namespace RbEngine.Components
{

	/// <summary>
	/// Delegate for the INamedObject.NameChanged event
	/// </summary>
	public delegate void NameChangedDelegate( Object obj );

	/// <summary>
	/// Interface for named objects
	/// </summary>
	public interface INamedObject
	{
		/// <summary>
		/// The name of this object
		/// </summary>
		/// <remarks>
		/// Implementors should call the NameChanged event when the name is set
		/// </remarks>
		string Name { get; set; }

		/// <summary>
		/// Event, invoked when the name is changed
		/// </summary>
		event NameChangedDelegate NameChanged;
	}
}
