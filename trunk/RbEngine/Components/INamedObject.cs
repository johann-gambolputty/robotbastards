using System;

namespace RbEngine.Components
{

	/// <summary>
	/// Interface for named objects
	/// </summary>
	public interface INamedObject
	{
		/// <summary>
		/// The name of this object
		/// </summary>
		string Name
		{
			get;
			set;
		}
	}
}
