using System.Collections.Generic;
using Goo.Core.Commands;
using log4net;
using Rb.Core.Utils;

namespace Goo.Core.Environment
{
	/// <summary>
	/// Default implementation of <see cref="ICommandExecutorRegistry"/>
	/// </summary>
	public class CommandExecutorRegistry : ICommandExecutorRegistry
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public CommandExecutorRegistry( )
		{
			m_Log = LogManager.GetLogger( GetType( ) );
		}

		#region ICommandExecutorRegistry Members

		/// <summary>
		/// Returns all the executors in this registry
		/// </summary>
		public ICommandExecutor[] Executors
		{
			get { return m_Executors.ToArray( ); }
		}

		/// <summary>
		/// Registers a command executor
		/// </summary>
		public void RegisterExecutor( ICommandExecutor executor )
		{
			Arguments.CheckNotNull( executor, "executor" );
			if ( m_Executors.Contains( executor ) )
			{
				m_Log.WarnFormat( "Command executor registry already contains an executor \"{0}\" - ignoring add", executor );
			}
			else
			{
				m_Executors.Add( executor );
			}
		}

		/// <summary>
		/// Unregisters a command executor
		/// </summary>
		public void UnregisterExecutor( ICommandExecutor executor )
		{
			Arguments.CheckNotNull( executor, "executor" );
			if ( !m_Executors.Contains( executor ) )
			{
				m_Log.WarnFormat( "Command executor registry doesn't contain an executor \"{0}\" - ignoring remove", executor );
			}
			else
			{
				m_Executors.Remove( executor );
			}
		}

		#endregion

		#region Private Members

		private readonly ILog m_Log;
		private readonly List<ICommandExecutor> m_Executors = new List<ICommandExecutor>( );

		#endregion
	}
}
