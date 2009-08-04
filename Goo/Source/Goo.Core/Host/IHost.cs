
using Goo.Core.Environment;
using Goo.Core.Units;

namespace Goo.Core.Host
{
	/// <summary>
	/// Host interface
	/// </summary>
	public interface IHost
	{
		/// <summary>
		/// Gets the main application environment
		/// </summary>
		IEnvironment Environment
		{
			get;
		}

		/// <summary>
		/// Loads a unit into the host
		/// </summary>
		/// <param name="unit">Unit to load</param>
		void LoadUnit( IUnit unit );

		/// <summary>
		/// Unloads a unit from the host
		/// </summary>
		/// <param name="unit">Unit to unload</param>
		void UnloadUnit( IUnit unit );

		/// <summary>
		/// Runs the host
		/// </summary>
		void Run( params IUnit[] startupUnits );

		/// <summary>
		/// Closes the host
		/// </summary>
		void Close( );
	}
}
