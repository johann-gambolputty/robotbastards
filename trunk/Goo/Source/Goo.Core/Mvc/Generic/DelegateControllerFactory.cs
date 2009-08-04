
namespace Goo.Core.Mvc.Generic
{
	/// <summary>
	/// Controller factory that uses a delegate to create an <see cref="IController"/> with a typed initialization context
	/// </summary>
	/// <typeparam name="TInitializationContext">Controller initialization context</typeparam>
	public class DelegateControllerFactory<TInitializationContext> : IControllerFactory<TInitializationContext>
		where TInitializationContext : ControllerInitializationContext
	{
		/// <summary>
		/// Controller creation delegate
		/// </summary>
		/// <param name="initContext">Controller initialization context</param>
		/// <returns>Returns the new initialized contoller</returns>
		public delegate IController<TInitializationContext> CreateControllerDelegate( TInitializationContext initContext );

		/// <summary>
		/// Setup constructor
		/// </summary>
		public DelegateControllerFactory( CreateControllerDelegate createController )
		{
			m_CreateController = createController;
		}

		#region IControllerFactory<TInitializationContext> Members

		/// <summary>
		/// Creates a controller and its view, using typed initialization context
		/// </summary>
		public IController<TInitializationContext> Create( TInitializationContext initContext )
		{
			return m_CreateController( initContext );
		}

		#endregion

		#region IControllerFactory Members

		/// <summary>
		/// Creates a controller and its view
		/// </summary>
		public IController Create( ControllerInitializationContext initContext )
		{
			return Create( ( TInitializationContext )initContext );
		}

		#endregion

		#region Private Members

		private readonly CreateControllerDelegate m_CreateController;

		#endregion
	}
}
