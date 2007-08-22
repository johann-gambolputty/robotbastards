namespace Rb.Core.Maths
{
	/// <summary>
	/// Interface for 3d shapes
	/// </summary>
	public interface IShape3
	{
		/// <summary>
		/// Returns true if this shape contains a given point
		/// </summary>
		bool Contains( Point3 pt );

		/// <summary>
		/// Gets the distance from this shape to a point
		/// </summary>
		float Distance( Point3 pt );

		/// <summary>
		/// Gets the distance from this shape to another shape
		/// </summary>
		float Distance( IShape3 shape );

	}
}
