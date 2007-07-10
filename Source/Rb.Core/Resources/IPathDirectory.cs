using System;

namespace Rb.Core.Resources
{
	/// <summary>
	/// Summary description for IPathDirectory.
	/// </summary>
	public interface IPathDirectory
	{
		/// <summary>
		/// The base directory where resources are be located
		/// </summary>
		string BaseDirectory
		{
			get;
			set;
		}
	}
}
