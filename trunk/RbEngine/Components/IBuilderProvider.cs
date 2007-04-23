using System;

namespace RbEngine.Components
{
	/// <summary>
	/// An interface for objects that support the IBuilder interface
	/// </summary>
	public interface IBuilderProvider
	{
		/// <summary>
		/// Gets the associated builder object
		/// </summary>
		IBuilder	Builder
		{
			get;
		}
	}
}
