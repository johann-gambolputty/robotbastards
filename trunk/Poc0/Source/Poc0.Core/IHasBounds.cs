using System;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Maths;

namespace Poc0.Core
{
	/// <summary>
	/// For objects that have a bounding region
	/// </summary>
	public interface IHasBounds
	{
		/// <summary>
		/// Gets the boundary region for this object
		/// </summary>
		IShape3 Bounds
		{
			get;
		}
	}
}
