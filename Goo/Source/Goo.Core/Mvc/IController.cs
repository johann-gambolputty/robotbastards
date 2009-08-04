
namespace Goo.Core.Mvc
{
	/// <summary>
	/// Controller interface
	/// </summary>
	public interface IController
	{
		/// <summary>
		/// Gets the factory that created this controller
		/// </summary>
		IControllerFactory Factory
		{
			get; set;
		}

		/// <summary>
		/// Gets the view attached to this controller
		/// </summary>
		IView View
		{
			get;
		}

		/// <summary>
		/// Initializes this controller
		/// </summary>
		/// <param name="context">Initialization context</param>
		void Initialize( ControllerInitializationContext context );
	}

	/// <summary>
	/// Controller interface with typed initialization context
	/// </summary>
	/// <typeparam name="TInitializationContext">Controller initialization context type</typeparam>
	public interface IController<TInitializationContext> : IController
		where TInitializationContext : ControllerInitializationContext
	{
		/// <summary>
		/// Initializes this controller
		/// </summary>
		/// <param name="context">Initialization context</param>
		void Initialize( TInitializationContext context );
	}
}
