using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// 2D line intersection
	/// </summary>
	[Serializable]
	public class Line2Intersection : ILineIntersection
	{
		#region Public properties

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

		#endregion

		#region ILineIntersection members

		/// <summary>
		/// Gets the object that was intersected
		/// </summary>
		public object IntersectedObject
		{
			get { return m_Object; }
			set { m_Object = value;}
		}

		/// <summary>
		/// Fraction along the line at which the intersection occurred
		/// </summary>
		public float Distance
		{
			get { return m_Distance; }
			set { m_Distance = value; }
		}

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor
		/// </summary>
		public Line2Intersection( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="pos"> Ray intersection position </param>
		/// <param name="normal"> Ray intersection normal </param>
		/// <param name="distance"> Ray intersection distance </param>
		public Line2Intersection( Point2 pos, Vector2 normal, float distance )
		{
			m_Position	= pos;
			m_Normal	= normal;
			m_Distance	= distance;
		}
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="pos"> Ray intersection position </param>
		/// <param name="normal"> Ray intersection normal </param>
		/// <param name="distance"> Ray intersection distance </param>
		/// <param name="obj">Intersected object</param>
		public Line2Intersection( Point2 pos, Vector2 normal, float distance, object obj )
		{
			m_Position	= pos;
			m_Normal	= normal;
			m_Distance	= distance;
			m_Object	= obj;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Converts this object to a string
		/// </summary>
		/// <returns>String representation</returns>
		public override string ToString( )
		{
			return string.Format( "X: {0} Y: {1}", m_Position.X, m_Position.Y );
		}

		#endregion

		#region Private members

		private object m_Object;
		private Point2 m_Position;
		private Vector2 m_Normal;
		private float m_Distance;

		#endregion
	}
}
