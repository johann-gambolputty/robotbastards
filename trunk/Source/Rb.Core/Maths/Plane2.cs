using System;

namespace Rb.Core.Maths
{
	public enum PlaneClassification
	{
		Behind,
		On,
		InFront
	}

	/// <summary>
	/// 2d plane
	/// </summary>
	[Serializable]
	public class Plane2
	{
		/// <summary>
		/// Default constructor. Plane normal points up the x axis
		/// </summary>
		public Plane2( )
		{
			m_Normal = new Vector2( 1, 0 );
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="normal">Plane normal</param>
		/// <param name="distance">Plane distance</param>
		public Plane2( Vector2 normal, float distance )
		{
			m_Normal = normal;
			m_Distance = distance;
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="src">Source plane</param>
		public Plane2( Plane2 src )
		{
			m_Normal = src.m_Normal;
			m_Distance = src.m_Distance;
		}

		/// <summary>
		/// Builds this plane from a line segment
		/// </summary>
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		public Plane2( Point2 start, Point2 end )
		{
			m_Normal = ( end - start ).MakePerpNormal( );
			m_Distance = -m_Normal.Dot( start );
		}

		/// <summary>
		/// Sets/gets the plane normal
		/// </summary>
		public Vector2 Normal
		{
			get { return m_Normal; }
			set { m_Normal = value; }
		}

		/// <summary>
		/// Sets/gets the plane distance
		/// </summary>
		public float Distance
		{
			get { return m_Distance; }
			set { m_Distance = value; }
		}

		/// <summary>
		/// Gets the signed distance to a point
		/// </summary>
		/// <param name="pt">Calculates distance to this point</param>
		/// <returns>Returns +ve distance to pt if pt is in front of this plane, -ve distance if pt is behind this plane</returns>
		public float GetSignedDistanceTo( Point2 pt )
		{
			return ( pt.X * m_Normal.X ) + ( pt.Y * m_Normal.Y ) + m_Distance;
		}

		/// <summary>
		/// Classifies a point
		/// </summary>
		/// <param name="pt">Point to classify</param>
		/// <param name="tolerance">"On" plane tolerance</param>
		/// <returns>Returns the classification of the specified point with respect to this plane</returns>
		public PlaneClassification ClassifyPoint( Point2 pt, float tolerance )
		{
			float signedDist = GetSignedDistanceTo( pt );
			return signedDist < -tolerance ? PlaneClassification.Behind : ( signedDist > tolerance ? PlaneClassification.InFront : PlaneClassification.On );
		}

		private Vector2 m_Normal;
		private float m_Distance;
	}
}
