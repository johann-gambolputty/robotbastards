using System;

namespace Poc1.Bits
{
	/// <summary>
	/// Base interface for parts and connectors
	/// </summary>
	public interface IBit : ICloneable
	{
		/// <summary>
		/// Gets shared data about this bit
		/// </summary>
		BitStaticData BitStaticData
		{
			get;
		}
	}
}
