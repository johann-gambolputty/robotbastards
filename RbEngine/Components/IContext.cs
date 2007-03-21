using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Used by the IContext.AddedToContext event
	/// </summary>
	public delegate void AddedToContextDelegate( IContext context, Object obj );

	/// <summary>
	/// Used by the IContext.RemovedFromContext event
	/// </summary>
	public delegate void RemovedFromContextDelegate( IContext context, Object obj );

	/// <summary>
	/// Context for 
	/// </summary>
	public interface IContext
	{
		/// <summary>
		/// Event, invoked by AddToContext()
		/// </summary>
		event AddedToContextDelegate		AddedToContext;

		/// <summary>
		/// Event, invoked by RemoveFromContext()
		/// </summary>
		event RemovedFromContextDelegate	RemovedFromContext;

		/// <summary>
		/// Adds an object to this context
		/// </summary>
		void AddToContext( Object obj );

		/// <summary>
		/// Removes an object from this context
		/// </summary>
		void RemoveFromContext( Object obj );
	}
}
