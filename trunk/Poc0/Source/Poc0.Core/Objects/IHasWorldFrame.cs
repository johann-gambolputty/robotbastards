using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	public delegate void PositionChangedDelegate( object obj, Point3 lastPos, Point3 newPos );

	//public delegate void OrientationChangedDelegate( object obj );

	/// <summary>
	/// Positionable object
	/// </summary>
	public interface IHasPosition
	{
		/// <summary>
		/// Event, raised when Position is changed
		/// </summary>
		event PositionChangedDelegate PositionChanged;

		/// <summary>
		/// Object's position
		/// </summary>
		Point3 Position
		{
			get; set;
		}
	}

	/// <summary>
	/// For objects that have a world position and orientation
	/// </summary>
	public interface IHasWorldFrame : IHasPosition
	{
		/// <summary>
		/// Gets the world frame for this object
		/// </summary>
		Matrix44 WorldFrame
		{
			get;
		}
	}
}
