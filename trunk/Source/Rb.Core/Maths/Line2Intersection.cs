namespace Rb.Core.Maths
{
	/// <summary>
	/// 2D line intersection
	/// </summary>
	public class Line2Intersection
	{
		/// <summary>
		/// The point that the line intersected the object. Always valid
		/// </summary>
		public Point2 IntersectionPosition
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		/// <summary>
		/// The normal on the object surface at the point that the ray intersected. Always valid
		/// </summary>
		public Vector2 IntersectionNormal
		{
			get { return m_Normal; }
			set { m_Normal = value; }
		}

		/// <summary>
		/// Fraction along the line at which the intersection occurred
		/// </summary>
		public float Distance
		{
			get { return m_Distance; }
			set { m_Distance = value; }
		}

		private Point2 m_Position;
		private Vector2 m_Normal;
		private float m_Distance;
	}
}
