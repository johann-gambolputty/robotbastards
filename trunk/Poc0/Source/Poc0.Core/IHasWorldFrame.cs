using System;
using System.Collections.Generic;
using System.Text;

namespace Poc0.Core
{
	/// <summary>
	/// For objects that have a world position and orientation
	/// </summary>
	public interface IHasWorldFrame
	{
		/// <summary>
		/// Gets the world frame for this object
		/// </summary>
		Frame WorldFrame
		{
			get;
		}
	}
}
