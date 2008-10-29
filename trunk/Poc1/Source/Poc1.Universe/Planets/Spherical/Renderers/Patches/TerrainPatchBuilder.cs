using System.Threading;
using Rb.Core.Threading;

namespace Poc1.Universe.Planets.Spherical.Renderers.Patches
{
	/// <summary>
	/// Queues up <see cref="TerrainPatchBuildItem"/> work items into a thread pool
	/// </summary>
	public class TerrainPatchBuilder
	{
		/// <summary>
		/// Gets the number of build items pending
		/// </summary>
		public static int PendingBuildItems
		{
			get { return s_PendingBuildItems; }
		}

		/// <summary>
		/// Queues up a build item
		/// </summary>
		/// <param name="buildItem">Build item to queue</param>
		public static void QueueWork( TerrainPatchBuildItem buildItem )
		{
			Interlocked.Increment( ref s_PendingBuildItems );
			ExtendedThreadPool.Instance.Enqueue
				(
					delegate
						{
							buildItem.StartBuild( );
							s_Marshaller.PostAction( buildItem.FinishBuild );
							Interlocked.Decrement( ref s_PendingBuildItems );
						}
				);
		}

		private static int s_PendingBuildItems;
		private static DelegateMarshaller s_Marshaller = new DelegateMarshaller( );
	}
}
