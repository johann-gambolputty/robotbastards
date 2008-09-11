
using System.Threading;

namespace Poc1.Universe.Planets.Spherical.Renderers.Patches
{
	/// <summary>
	/// Queues up <see cref="TerrainPatchBuildItem"/> work items into a thread pool
	/// </summary>
	public class TerrainPatchBuilder
	{
		/// <summary>
		/// Queues up a build item
		/// </summary>
		/// <param name="buildItem">Build item to queue</param>
		public static void QueueWork( TerrainPatchBuildItem buildItem )
		{
			WaitCallback callback = delegate { buildItem.Build( ); };
			ThreadPool.QueueUserWorkItem( callback );
		}


	}
}
