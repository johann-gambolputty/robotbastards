using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	public class LinePath : IPath
	{
		public LinePath( Point3[] points, bool closed )
		{
			m_Points = points;
			m_Closed = closed;
		}

		#region IPath members

		/// <summary>
		/// Returns true if a time t is at the beginning of the path
		/// </summary>
		public bool AtStart( float t, float tolerance )
		{
			int ptIndex = ( int )t;
			return ( ptIndex == 0 ) && ( GetPoint( t ).SqrDistanceTo( m_Points[ 0 ] ) < ( tolerance * tolerance ) );
		}

		/// <summary>
		/// Returns true if a time t is at the end of the path
		/// </summary>
		public bool AtEnd( float t, float tolerance )
		{
			int ptIndex = ( int )t;
			int lastPt = m_Points.Length - 1;
			if ( ptIndex == lastPt )
			{
				return true;
			}
			return ( ptIndex == ( lastPt - 1  ) ) && ( GetPoint( t ).SqrDistanceTo( m_Points[ lastPt ] ) < ( tolerance * tolerance ) );
		}

		/// <summary>
		/// Gets the position and tangent on the path at time t
		/// </summary>
		public void GetFrame( float t, out Point3 pt, out Vector3 dir )
		{
			int ptIndex = ( int )t;

			if ( ptIndex == m_Points.Length - 1 )
			{
				pt = m_Points[ ptIndex ];
				dir = ( m_Points[ ptIndex ] - m_Points[ ptIndex - 1 ] ).MakeNormal( );
				return;
			}

			float localT = t - ptIndex;

			Point3 start = m_Points[ ptIndex ];
			Point3 end = m_Points[ ptIndex + 1 ];
			pt = LineSegment3.GetPointOnLine( start, end, localT );
			dir = ( end - start ).MakeNormal( );
		}

		/// <summary>
		/// Gets the closest point to pt on the path
		/// </summary>
		public float GetClosestPoint( Point3 pt )
		{
			float closestDistance = float.MaxValue;
			float closestT = 0;

			for ( int pointIndex = 0; pointIndex < m_Points.Length - 1; ++pointIndex )
			{
				Point3 start = m_Points[ pointIndex ];
				Point3 end = m_Points[ pointIndex + 1 ];

				float t = LineSegment3.GetClosestTimeOnLine( start, end, pt );
				float distance = LineSegment3.GetPointOnLine( start, end, t ).DistanceTo( pt );
				if ( distance < closestDistance )
				{
					closestDistance = distance;
					closestT = ( pointIndex - 1 ) + t;
				}
			}

			return closestT;
		}

		/// <summary>
		/// Moves along the path a given distance
		/// </summary>
		/// <param name="t">Path time</param>
		/// <param name="distance">Distance along the path to move</param>
		/// <param name="reverse">Move backwards along the path</param>
		/// <returns>Returns the new path time</returns>
		public float Move( float t, float distance, bool reverse )
		{
			int ptIndex = ( int )t;
			float remainingDistance = distance;

			while ( remainingDistance > 0 )
			{
				Point3 start = m_Points[ ptIndex ];
				Point3 end = m_Points[ ptIndex + 1 ];

				float localT = t - ptIndex;
				Point3 pt = LineSegment3.GetPointOnLine( start, end, localT );

				if ( !reverse )
				{
					float distanceToEnd = pt.DistanceTo( end );

					if ( distanceToEnd > remainingDistance )
					{
						t += remainingDistance / start.DistanceTo( end );
						remainingDistance = 0;
					}
					else
					{
						remainingDistance -= distanceToEnd;

						t = ptIndex + 1;
						if ( ptIndex == m_Points.Length - 1 )
						{
							//	Reached the end... unless...
							if ( m_Closed )
							{
								t = 0;
							}
							else
							{
								remainingDistance = 0;
							}
						}
					}
				}
				else
				{
					float distanceToStart = pt.DistanceTo( start );
					
					if ( distanceToStart > remainingDistance )
					{
						t -= remainingDistance / start.DistanceTo( end );
						remainingDistance = 0;
					}
					else
					{
						remainingDistance -= distanceToStart;

						if ( ptIndex == 0 )
						{
							//	Reached the end... unless...
							if ( m_Closed )
							{
								t = m_Points.Length - 1;
							}
							else
							{
								remainingDistance = 0;
								t = 0;
							}
						}
						else
						{
							t = ptIndex - 1;
						}
					}
				}

			}
			return t;
		}

		#endregion

		#region Private members

		private readonly Point3[] m_Points;
		private readonly bool m_Closed;
		
		/// <summary>
		/// Gets a point on the path
		/// </summary>
		private Point3 GetPoint( float t )
		{
			int ptIndex = ( int )t;
			if ( ptIndex == m_Points.Length - 1 )
			{
				return m_Points[ ptIndex ];
			}
			return LineSegment3.GetPointOnLine( m_Points[ ptIndex ], m_Points[ ptIndex + 1 ], t - ptIndex );
		}

		#endregion
	}
}
