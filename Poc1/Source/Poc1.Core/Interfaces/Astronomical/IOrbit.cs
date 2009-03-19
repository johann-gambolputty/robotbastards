using System;

namespace Poc1.Core.Interfaces.Astronomical
{
	/// <summary>
	/// Orbit interface
	/// </summary>
	public interface IOrbit
	{
		/// <summary>
		/// Gets the orbital centre
		/// </summary>
		IUniObject Centre
		{
			get;
		}

		/// <summary>
		/// Gets the time it takes to make a complete orbit
		/// </summary>
		TimeSpan Period
		{
			get;
		}

		/// <summary>
		/// Gets the position on this orbit at the specified time
		/// </summary>
		UniPoint3 GetPositionAtTime( double t );
	}

}
