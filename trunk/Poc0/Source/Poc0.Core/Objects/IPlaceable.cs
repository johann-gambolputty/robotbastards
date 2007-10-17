using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Delegate, used by <see cref="IHasPosition.PositionChanged"/>
	/// </summary>
	/// <param name="obj">Positioned object</param>
	/// <param name="lastPos">Objects position prior to change</param>
	/// <param name="newPos">Objects new position</param>
	public delegate void PositionChangedDelegate( object obj, Point3 lastPos, Point3 newPos );

	/// <summary>
	/// Placeable objects can be positioned
	/// </summary>
	/// <remarks>
	/// For objects that move smoothly between positions, use <see cref="IMoveable"/>
	/// </remarks>
	public interface IPlaceable
	{
		/// <summary>
		/// Event, raised when Position is changed
		/// </summary>
		event PositionChangedDelegate PositionChanged;

		/// <summary>
		/// Objects current position
		/// </summary>
		Point3 Position
		{
			get; set;
		}
	}
}
