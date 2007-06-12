using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Rendering appliance
	/// </summary>
	public interface IAppliance
	{
		/// <summary>
		/// Starts applying this object
		/// </summary>
		void	Begin( );

		/// <summary>
		/// Stops applying this object
		/// </summary>
		void	End( );
	}
}
