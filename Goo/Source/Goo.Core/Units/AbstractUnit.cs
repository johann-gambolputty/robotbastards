using System;
using Goo.Core.Commands;
using Goo.Core.Environment;
using Goo.Core.Mvc;

namespace Goo.Core.Units
{
	/// <summary>
	/// Abstract unit base class
	/// </summary>
	public abstract class AbstractUnit : IUnit
	{
		#region IUnit Members

		/// <summary>
		/// Initializes this unit
		/// </summary>
		/// <param name="env">Environment</param>
		public void Initialize( IEnvironment env )
		{
			if ( m_Initialized )
			{
				throw new InvalidOperationException( "Unit initialized more than once" );
			}
			m_Initialized = true;
			m_ControllerFactories = ControllerFactories;
			m_CommandExecutors = CommandExecutors;
			foreach ( IControllerFactory factory in m_ControllerFactories )
			{
				env.ControllerFactories.RegisterControllerFactory( factory );
			}
			foreach ( ICommandExecutor executor in m_CommandExecutors )
			{
				env.CommandExecutors.RegisterExecutor( executor );
			}
			PostInitialize( env );
		}

		/// <summary>
		/// Shuts this unit down
		/// </summary>
		/// <param name="env">Environment</param>
		public void Shutdown( IEnvironment env )
		{
			if ( !m_Initialized )
			{
				throw new InvalidOperationException( "Attempted to shut down the unit before initalizing it" );
			}
			foreach ( IControllerFactory factory in m_ControllerFactories )
			{
				env.ControllerFactories.UnregisterControllerFactory( factory );
			}
			foreach ( ICommandExecutor executor in m_CommandExecutors )
			{
				env.CommandExecutors.UnregisterExecutor( executor );
			}
			m_Initialized = false;
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Called after initialization
		/// </summary>
		/// <param name="env">Environment</param>
		protected virtual void PostInitialize( IEnvironment env )
		{
		}

		/// <summary>
		/// Gets the controller factories associated with this unit
		/// </summary>
		protected abstract IControllerFactory[] ControllerFactories
		{
			get;
		}

		/// <summary>
		/// Gets the command executors associated with this unit
		/// </summary>
		protected abstract ICommandExecutor[] CommandExecutors
		{
			get;
		}

		#endregion

		#region Private Members

		private bool m_Initialized;
		private IControllerFactory[] m_ControllerFactories;
		private ICommandExecutor[] m_CommandExecutors;

		#endregion
	}
}
