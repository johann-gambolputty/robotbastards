using Goo.Core.Commands;
using Goo.Core.Host;
using log4net;
using Rb.Core.Utils;
using Rb.Interaction.Classes;

namespace Goo.Core.Environment
{
	/// <summary>
	/// Default environment implementation
	/// </summary>
	public class Environment : ServiceProviderBase, IEnvironment
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="host">Host application</param>
		public Environment( IHost host )
		{
			Arguments.CheckNotNull( host, "host" );
			m_Host = host;
			m_Log = LogManager.GetLogger( GetType( ) );
		}

		#region IEnvironment Members

		/// <summary>
		/// Gets the application host
		/// </summary>
		public IHost Host
		{
			get { return m_Host; }
		}

		/// <summary>
		/// Gets the command executor registry
		/// </summary>
		public ICommandExecutorRegistry CommandExecutors
		{
			get { return m_CommandExecutors; }
		}

		/// <summary>
		/// Gets the controller factory registry
		/// </summary>
		public IControllerFactoryRegistry ControllerFactories
		{
			get { return m_ControllerFactories; }
		}

		#endregion

		#region Private Members

		private readonly ILog m_Log;
		private readonly IHost m_Host;
		private readonly ICommandExecutorRegistry m_CommandExecutors = new CommandExecutorRegistry( );
		private readonly IControllerFactoryRegistry m_ControllerFactories = new ControllerFactoryRegistry( );

		#endregion

		#region ICommandHost Members

		/// <summary>
		/// Executes a command
		/// </summary>
		/// <param name="command">Command to execute</param>
		/// <param name="parameters">Command parameters</param>
		public void Execute( Command command, CommandParameters parameters )
		{
			m_Log.InfoFormat( "Executing command \"{0}\"", command );
			foreach ( ICommandExecutor executor in m_CommandExecutors.Executors )
			{
				if ( executor.Execute( this, command, parameters ) == CommandExecutionResult.StopExecutingCommand )
				{
					m_Log.InfoFormat( "Executing of command \"{0}\" was stopped by executor \"{1}\"", command, executor );
					return;
				}
			}
		}

		#endregion
	}
}
