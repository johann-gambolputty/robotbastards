using Rb.Core.Utils;

namespace Goo.Core.Mvc
{
	/// <summary>
	/// Implementation of <see cref="IControllerFactory"/> that uses a delegate
	/// to create the controller
	/// </summary>
	public class DelegateControllerFactory : IControllerFactory
	{
		/// <summary>
		/// Delegate type used to create new controllers
		/// </summary>
		public delegate IController CreateControllerDelegate( ControllerInitializationContext initContext );

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="create">Delegate used to create the controller</param>
		public DelegateControllerFactory( CreateControllerDelegate create )
		{
			Arguments.CheckNotNull( create, "create" );
			m_Create = create;
		}

		#region Private Members

		private readonly CreateControllerDelegate m_Create;

		#endregion

		/// <summary>
		/// Creates a controller and its view
		/// </summary>
		public IController Create( ControllerInitializationContext initContext )
		{
			IController controller = m_Create( initContext );
			controller.Factory = this;
			controller.Initialize( initContext );
			return controller;
		}
	}
}
