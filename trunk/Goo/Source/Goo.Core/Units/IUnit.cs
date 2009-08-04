using Goo.Core.Environment;

namespace Goo.Core.Units
{
	/// <summary>
	/// A unit is a modular part of a Goo application that provides controllers, commands and so forth
	/// </summary>
	public interface IUnit
	{
		/// <summary>
		/// Initializes this unit
		/// </summary>
		/// <param name="env">Environment</param>
		void Initialize( IEnvironment env );

		/// <summary>
		/// Shuts this unit down
		/// </summary>
		/// <param name="env">Environment</param>
		void Shutdown( IEnvironment env );
	}
}
