using System;
using System.Collections.Generic;
using Rb.Core.Utils;

namespace Rb.World.Services
{
	/// <summary>
	/// Simple implementation of IUpdateService
	/// </summary>
	[Serializable]
	public class UpdateService : IUpdateService, ISceneObject
	{
		#region IUpdateService Members

		/// <summary>
		/// Adds a named clock to the service
		/// </summary>
		/// <param name="clock">Clock object</param>
		public void AddClock( Clock clock )
		{
			m_Clocks.Add( clock.Name, clock );
		}
		
		/// <summary>
		/// Starts all clocks in the service
		/// </summary>
		public void Start( )
		{
			foreach ( Clock clock in m_Clocks.Values )
			{
				clock.Pause = false;
			}
		}

		/// <summary>
		/// Stops all clocks in the service
		/// </summary>
		public void Stop( )
		{
			foreach ( Clock clock in m_Clocks.Values )
			{
				clock.Pause = true;
			}
		}

		/// <summary>
		/// Gets a named clock
		/// </summary>
		/// <param name="name">Clock name</param>
		/// <returns>Returns the named clock</returns>
		/// <exception cref="KeyNotFoundException">Thrown if no clock with the specified name exists</exception>
		public Clock this[ string name ]
		{
			get { return m_Clocks[ name ]; }
		}

		#endregion

		#region Private members

		private readonly Dictionary< string, Clock > m_Clocks = new Dictionary< string, Clock >( );

		/// <summary>
		/// Called when the scene that this object is attached to, is disposed of
		/// </summary>
		private void SceneDisposing( object sender, EventArgs args )
		{
			Stop( );
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene scene )
		{
			scene.Disposing += SceneDisposing;
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene( Scene scene )
		{
			scene.Disposing -= SceneDisposing;
		}

		#endregion
	}
}
