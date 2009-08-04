using Rb.Core.Utils;

namespace Goo.Core.Mvc.Generic
{
	/// <summary>
	/// Base class for controllers with typed initialization contexts
	/// </summary>
	/// <typeparam name="TInitializationContext">Initialization context type</typeparam>
	public class ControllerBase<TInitializationContext> : ControllerBase, IController<TInitializationContext>
		where TInitializationContext : ControllerInitializationContext
	{
		/// <summary>
		/// Controller setup constructor
		/// </summary>
		/// <param name="view">Controlled view</param>
		public ControllerBase( IView view ) :
			base( view )
		{
		}

		#region Protected Members

		/// <summary>
		/// Validates the initialization initContext passed to Initialize()
		/// </summary>
		protected override void ValidateInitializationContext( ControllerInitializationContext initContext )
		{
			Arguments.CheckedNonNullCast<TInitializationContext>( initContext, "initContext" );
		}

		/// <summary>
		/// Initializes this controller
		/// </summary>
		/// <param name="initContext">Initialization initContext</param>
		protected override sealed void PostInitialize( ControllerInitializationContext initContext )
		{
			PostInitialize( ( TInitializationContext )initContext );
		}

		/// <summary>
		/// Initializes this controller
		/// </summary>
		/// <param name="initContext">Initialization context</param>
		protected virtual void PostInitialize( TInitializationContext initContext )
		{
		}

		#endregion

		#region IController<TInitializationContext> Members

		/// <summary>
		/// Initializes this controller
		/// </summary>
		/// <param name="context">Initialization context</param>
		public void Initialize( TInitializationContext context )
		{
			base.Initialize( context );
		}

		#endregion
	}
}
