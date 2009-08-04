using System;
using log4net;
using Rb.Core.Utils;

namespace Goo.Core.Mvc
{
	/// <summary>
	/// Base class for controllers
	/// </summary>
	public class ControllerBase : IController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">View controlled by this controller</param>
		public ControllerBase( IView view )
		{
			Arguments.CheckNotNull( view, "view" );
			m_View = view;
			m_Log = LogManager.GetLogger( GetType( ) );
		}

		/// <summary>
		/// Gets the controller logger
		/// </summary>
		public ILog Log
		{
			get { return m_Log; }
		}

		#region IController Members

		/// <summary>
		/// Gets the factory that created this controller
		/// </summary>
		public IControllerFactory Factory
		{
			get { return m_Factory; }
			set
			{
				Arguments.CheckNotNull( value, "value" );
				m_Factory = value;
			}
		}

		/// <summary>
		/// Gets the view controlled by this controller
		/// </summary>
		public IView View
		{
			get { return m_View; }
		}


		/// <summary>
		/// Initializes this controller
		/// </summary>
		/// <param name="context">Initialization context</param>
		public void Initialize( ControllerInitializationContext context )
		{
			ValidateInitializationContext( context );
			if ( m_Factory == null )
			{
				throw new InvalidOperationException( "Factory must be set prior to initialization" );
			}
			m_InitContext = context;
			PostInitialize( context );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Validates the initialization context passed to Initialize()
		/// </summary>
		protected virtual void ValidateInitializationContext( ControllerInitializationContext context )
		{
			Arguments.CheckNotNull( context, "context" );
		}

		/// <summary>
		/// Post initialization - provides customatization of initialization to derived classes
		/// </summary>
		protected virtual void PostInitialize( ControllerInitializationContext initContext )
		{
		}

		#endregion

		#region Private Members

		private readonly ILog m_Log;
		private readonly IView m_View;
		private IControllerFactory m_Factory;
		private ControllerInitializationContext m_InitContext;

		#endregion
	}
}
