
namespace Rb.World
{
	/// <summary>
	/// Delegate for <see cref="IRayLayerIntersector.LayersChanged"/> events
	/// </summary>
	/// <param name="sender">Sender object</param>
	/// <param name="oldLayers">Old layer flags</param>
	/// <param name="newLayers">New layer flags</param>
	public delegate void RayLayersChangedDelegate( object sender, ulong oldLayers, ulong newLayers );

	/// <summary>
	/// Interface for objects that support ray cast layers
	/// </summary>
	public interface IRayLayerIntersector
	{
		/// <summary>
		/// Event, raised when <see cref="Layers"/> is set
		/// </summary>
		event RayLayersChangedDelegate LayersChanged;

		/// <summary>
		/// Gets/set raycast layers
		/// </summary>
		ulong Layers
		{
			get; set;
		}
	}
}
