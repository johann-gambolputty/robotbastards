

using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core.EditModes
{
	/// <summary>
	/// Performs a CSG operation
	/// </summary>
	public class UserBrushEditMode : DefinePolygonEditMode
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="operation">CSG operation to perform with the brush</param>
		public UserBrushEditMode( LevelGeometry.Csg operation )
		{
			m_Operation = operation;
		}

		/// <summary>
		/// Called when the base edit mode finishes defining a polygon
		/// </summary>
		/// <param name="points">Polygon points</param>
		protected override void OnPolygonClosed( Point2[] points )
		{
			GeometryBrush brush = new GeometryBrush( "", points );
			EditModeContext.Instance.Scene.LevelGeometry.Combine( m_Operation, brush );
		}

		private readonly LevelGeometry.Csg m_Operation;
	}
}