using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Interface that messages implement when they contain target sequence values
	/// </summary>
	public interface ISequenceMessage
	{
		/// <summary>
		/// Sequence value
		/// </summary>
		uint Sequence
		{
			get;
		}
	}
}
