using System;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Utils;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Asset load priority - determines priority in asynchronous asset loader
	/// </summary>
	public enum LoadPriority
	{
		High,
		Medium,
		Low
	}

	/// <summary>
	/// Asynchronous asset loader
	/// </summary>
	public interface IAsyncAssetLoader
	{
		/// <summary>
		/// Queues up a load request
		/// </summary>
		/// <param name="location">Asset location</param>
		/// <param name="parameters">Load parameters</param>
		/// <param name="priority">Load priority</param>
		/// <returns>Asynchronous result</returns>
		AsyncLoadResult QueueLoad( Location location, LoadParameters parameters, LoadPriority priority );
	}
}
