namespace Goo.Core.Mvc.Generic
{
	/// <summary>
	/// Controller factory that creates controllers with typed initialization contexts
	/// </summary>
	/// <typeparam name="TInitializationContext"></typeparam>
	public interface IControllerFactory<TInitializationContext> : IControllerFactory
		where TInitializationContext : ControllerInitializationContext
	{
		/// <summary>
		/// Creates a controller and its view, using typed initialization context
		/// </summary>
		IController<TInitializationContext> Create( TInitializationContext initContext );
	}
}
