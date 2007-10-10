using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Details about the intersection of a ray with an object
	/// </summary>
	[Serializable]
	public class Line3Intersection : ILineIntersection
	{
		#region Public properties

		/// <summary>
		/// The point that the ray intersected the object. Always valid
		/// </summary>
		public Point3 IntersectionPosition
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		/// <summary>
		/// The normal on the object surface at the point that the ray intersected. Always valid
		/// </summary>
		public Vector3 IntersectionNormal
		{
			get { return m_Normal; }
			set { m_Normal = value; }
		}

		#endregion

		#region ILineIntersection members

		/// <summary>
		/// The object that the ray intersected. Can be null even if intersection was successful
		/// </summary>
		public object IntersectedObject
		{
			get { return m_Object; }
			set { m_Object = value; }
		}

		/// <summary>
		/// Distance along the ray at which the intersection occurred
		/// </summary>
		/// <remarks>
		/// The IntersectionPosition can also be determined by calling Ray3.GetPointOnRay( Distance )
		/// </remarks>
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
		public Line3Intersection( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="pos"> Ray intersection position </param>
		/// <param name="normal"> Ray intersection normal </param>
		/// <param name="distance"> Ray intersection distance </param>
		public Line3Intersection( Point3 pos, Vector3 normal, float distance )
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
		public Line3Intersection( Point3 pos, Vector3 normal, float distance, object obj )
		{
			m_Position = pos;
			m_Normal = normal;
			m_Distance = distance;
			m_Object = obj;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Converts this object to a string
		/// </summary>
		/// <returns>String representation</returns>
		public override string ToString( )
		{
			return string.Format( "X: {0:F2} Y: {1:F2} Z: {2:F2}", m_Position.X, m_Position.Y, m_Position.Z );
		}

		#endregion

		#region Private members

		private object	m_Object;
		private Point3	m_Position;
		private Vector3	m_Normal;
		private float	m_Distance;

		#endregion
	}
}
