using System.Collections.Generic;
using Rb.Core.Utils;

namespace Rb.World.Services
{
	/// <summary>
	/// Maintains a dictionary of clocks that objects can subscribe to
	/// </summary>
	public interface IUpdateService
	{
		/// <summary>
		/// Adds a named clock to the service
		/// </summary>
		/// <param name="name">Clock name</param>
		/// <param name="clock">Clock object</param>
		void AddClock( string name, Clock clock );

		/// <summary>
		/// Gets a named clock
		/// </summary>
		/// <param name="name">Clock name</param>
		/// <returns>Returns the named clock</returns>
		/// <exception cref="KeyNotFoundException">Thrown if no clock with the specified name exists</exception>
		Clock this[ string name ]
		{
			get;
		}
	}
}
