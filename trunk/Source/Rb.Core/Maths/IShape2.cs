namespace Rb.Core.Maths
{
	/// <summary>
	/// Interface for 2d shapes
	/// </summary>
	public interface IShape2
	{
		/// <summary>
		/// Returns true if this shape contains a given point
		/// </summary>
		bool Contains( Point2 pt );

		/// <summary>
		/// Gets the distance from this shape to a point
		/// </summary>
		float Distance( Point2 pt );

		/// <summary>
		/// Gets the distance from this shape to another shape
		/// </summary>
		float Distance( IShape2 shape );

		/// <summary>
		/// Returns true if this shape and another shape overlap
		/// </summary>
		bool Overlaps( IShape2 shape );
	}
}
