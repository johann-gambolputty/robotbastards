using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// 3D object pick information
	/// </summary>
	public class PickInfoRay3 : PickInfoCursor
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="cursorX">Cursor X position</param>
		/// <param name="cursorY">Cursor Y position</param>
		/// <param name="pickRay">Pick ray generated from cursor coordinates</param>
		/// <param name="intersection">Intersection of pick ray with the world</param>
		public PickInfoRay3( int cursorX, int cursorY, Ray3 pickRay, Line3Intersection intersection ) :
			base( cursorX, cursorY )
		{
			m_PickRay = pickRay;
			m_Intersection = intersection;
		}

		/// <summary>
		/// Ray generated from cursor position. Objects intersecting this ray are picked
		/// </summary>
		public Ray3 PickRay
		{
			get { return m_PickRay; }
		}

		/// <summary>
		/// Gets the line intersection of the ray
		/// </summary>
		public Line3Intersection Intersection
		{
			get { return m_Intersection; }
		}

		private readonly Ray3 m_PickRay;
		private readonly Line3Intersection m_Intersection;
	}
}
