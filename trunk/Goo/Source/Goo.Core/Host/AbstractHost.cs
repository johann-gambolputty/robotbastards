using System.Collections.Generic;
using Goo.Core.Environment;
using Goo.Core.Units;
using log4net;
using Rb.Core.Utils;

namespace Goo.Core.Host
{
	/// <summary>
	/// Abstract base class for hosts
	/// </summary>
	public abstract class AbstractHost : IHost
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public AbstractHost( )
		{
			m_Environment = new Environment.Environment( this );
			m_Log = LogManager.GetLogger( GetType( ) );
		}

		/// <summary>
		/// Gets this object's log
		/// </summary>
		public ILog Log
		{
			get { return m_Log; }
		}

		#region IHost Members

		/// <summary>
		/// Gets the main application environment
		/// </summary>
		public IEnvironment Environment
		{
			get { return m_Environment; }
		}

		/// <summary>
		/// Loads a unit into the host
		/// </summary>
		/// <param name="unit">Unit to load</param>
		public void LoadUnit( IUnit unit )
		{
			Arguments.CheckNotNull( unit, "unit" );
			Log.Info( "Loading unit " + unit);
			unit.Initialize( m_Environment );
			m_Units.Add( unit );
		}

		/// <summary>
		/// Unloads a unit from the host
		/// </summary>
		/// <param name="unit">Unit to unload</param>
		public void UnloadUnit( IUnit unit )
		{
			Arguments.CheckNotNull( unit, "unit" );
			Log.Info( "Unloading unit " + unit );
			unit.Shutdown( m_Environment );
			m_Units.Remove( unit );
		}

		/// <summary>
		/// Runs the host
		/// </summary>
		public abstract void Run( params IUnit[] startupUnits );

		/// <summary>
		/// Closes the host
		/// </summary>
		public void Close( )
		{
			foreach ( IUnit unit in m_Units.ToArray( ) )
			{
				UnloadUnit( unit );
			}
			CloseHost( );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Specific handling for closing the host
		/// </summary>
		protected abstract void CloseHost( );
			 
		#endregion


		#region Private Members

		private readonly ILog m_Log;
		private readonly IEnvironment m_Environment;
		private readonly List<IUnit> m_Units = new List<IUnit>( );

		#endregion
	}
}
