using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Object editor interface for objects that can be placed in the world (support IPlaceable interface)
	/// </summary>
	public interface IPlaceableObjectEditor : IObjectEditor
	{
		/// <summary>
		/// Places this object at a line intersection
		/// </summary>
		/// <param name="intersection">Line intersection</param>
		void Place( ILineIntersection intersection );
	}
}