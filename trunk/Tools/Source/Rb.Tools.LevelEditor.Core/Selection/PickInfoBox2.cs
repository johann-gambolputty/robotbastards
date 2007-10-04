
using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// 2D pick box
	/// </summary>
	public class PickInfoBox2 : IPickInfo
	{
		/// <summary>
		/// Builds this box
		/// </summary>
		/// <param name="topLeft">Box top left corner</param>
		/// <param name="bottomRight">Box bottom right corner</param>
		public PickInfoBox2( Point2 topLeft, Point2 bottomRight )
		{
			m_Rect = new Rectangle( topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y );
		}

		/// <summary>
		/// Gets the pick rectangle
		/// </summary>
		public Rectangle Rectangle
		{
			get { return m_Rect; }
		}

		private readonly Rectangle m_Rect;
	}
}
