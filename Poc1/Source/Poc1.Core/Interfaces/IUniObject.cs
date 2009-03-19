
using Poc1.Core.Classes;

namespace Poc1.Core.Interfaces
{
	/// <summary>
	/// Entity interface
	/// </summary>
	public interface IUniObject
	{
		/// <summary>
		/// Gets the transform of this object
		/// </summary>
		UniTransform Transform
		{
			get;
		}
	}
}
