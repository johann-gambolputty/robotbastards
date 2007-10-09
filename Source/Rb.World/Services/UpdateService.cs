using System.Collections.Generic;
using Rb.Core.Utils;

namespace Rb.World.Services
{
	/// <summary>
	/// Simple implementation of IUpdateService
	/// </summary>
	public class UpdateService : IUpdateService
	{
		#region IUpdateService Members

		/// <summary>
		/// Adds a named clock to the service
		/// </summary>
		/// <param name="name">Clock name</param>
		/// <param name="clock">Clock object</param>
		public void AddClock( string name, Clock clock )
		{
			m_Clocks.Add( name, clock );
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

		#endregion
	}
}
