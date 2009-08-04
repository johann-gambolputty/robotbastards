
using System.Collections.Generic;
using Goo.Core.Mvc;
using log4net;
using Rb.Core.Utils;

namespace Goo.Core.Environment
{
	/// <summary>
	/// Controller factory registry default implementation
	/// </summary>
	public class ControllerFactoryRegistry : IControllerFactoryRegistry
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ControllerFactoryRegistry( )
		{
			m_Log = LogManager.GetLogger( GetType( ) );
		}

		#region IControllerFactoryRegistry Members

		/// <summary>
		/// Gets all controller factories
		/// </summary>
		public IControllerFactory[] Factories
		{
			get { return m_Factories.ToArray( ); }
		}

		/// <summary>
		/// Registers a the specified controller factory
		/// </summary>
		/// <param name="controllerFactory">Controller factory</param>
		public void RegisterControllerFactory( IControllerFactory controllerFactory )
		{
			Arguments.CheckNotNull( controllerFactory, "controllerFactory" );
			if ( m_Factories.Contains( controllerFactory ) )
			{
				m_Log.WarnFormat( "Controller factory registry already contains a factory \"{0}\" - ignoring add", controllerFactory );
			}
			else
			{
				m_Factories.Remove( controllerFactory );
			}
		}

		/// <summary>
		/// Unregisters a command executor
		/// </summary>
		/// <summary>
		/// Unregisters the specified controller factory
		/// </summary>
		public void UnregisterControllerFactory( IControllerFactory controllerFactory )
		{
			Arguments.CheckNotNull( controllerFactory, "controllerFactory" );
			if ( !m_Factories.Contains( controllerFactory ) )
			{
				m_Log.WarnFormat( "Controller factory registry doesn't contain a factory \"{0}\" - ignoring remove", controllerFactory );
			}
			else
			{
				m_Factories.Remove( controllerFactory );
			}
		}

		#endregion

		#region Private Members

		private readonly ILog m_Log;
		private readonly List<IControllerFactory> m_Factories = new List<IControllerFactory>( );

		#endregion
	}
}
