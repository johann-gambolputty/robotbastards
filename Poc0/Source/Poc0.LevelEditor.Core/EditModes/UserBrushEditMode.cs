

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
		public UserBrushEditMode( Csg.Operation operation )
		{
			m_Operation = operation;
		}

		/// <summary>
		/// Called when the base edit mode finishes defining a polygon
		/// </summary>
		/// <param name="points">Polygon points</param>
		protected override void OnPolygonClosed( Point2[] points )
		{
			CsgBrush brush = new CsgBrush( "", points );
			EditModeContext.Instance.Scene.LevelGeometry.Csg.Combine( m_Operation, brush );
		}

		private readonly Csg.Operation m_Operation;
	}
}