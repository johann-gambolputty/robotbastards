
namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// Level geometry object
	/// </summary>
	public interface ILevelGeometryObject
	{
		/// <summary>
		/// Adds this object to a level geometry instance
		/// </summary>
		void AddToLevel( LevelGeometry level );

		/// <summary>
		/// Removes this object from a level geometry instance
		/// </summary>
		void RemoveFromLevel( LevelGeometry level );

	}

}
