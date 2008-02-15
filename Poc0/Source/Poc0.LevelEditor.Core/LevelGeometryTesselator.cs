using System.Collections.Generic;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Er... tesselates level geometry?
	/// </summary>
	internal class LevelGeometryTesselator
	{
		/// <summary>
		/// Tesselator output polygon type
		/// </summary>
		public class Polygon
		{
			public Polygon( int[] indices )
			{
				m_Indices = indices;
			}

			public int[] Indices
			{
				get { return m_Indices; }
			}

			public LevelPolygon LevelPolygon
			{
				get { return m_LevelPoly; }
				set { m_LevelPoly = value; }
			}

			private LevelPolygon m_LevelPoly;
			private readonly int[] m_Indices;
		}

		/// <summary>
		/// Creates an initial bounding polygon
		/// </summary>
		public Polygon CreateBoundingPolygon( float minX, float minY, float maxX, float maxY )
		{
			return new Polygon
			(
				new int[]
					{
						AddPoint( new Point2( minX, minY ) ),
						AddPoint( new Point2( maxX, minY ) ),
						AddPoint( new Point2( maxX, maxY ) ),
						AddPoint( new Point2( minX, maxY ) )
					}
			);
		}

		public List<Point2> Points
		{
			get { return m_Points; }
		}

		/// <summary>
		/// Splits a source polygon along a plane, returning the sub-poly behind the plane in "behind"
		/// and the sub-poly in-front of the plane in "inFront"
		/// </summary>
		public void Split( Plane2 plane, Polygon source, out Polygon behind, out Polygon inFront )
		{
			float tolerance = 0.001f;
			int firstDefiniteClassIndex = -1;
			PlaneClassification[] classifications = new PlaneClassification[ source.Indices.Length ];
			for ( int index = 0; index < source.Indices.Length; ++index )
			{
				classifications[ index ] = plane.ClassifyPoint( m_Points[ source.Indices[ index ] ], tolerance );
				if ( ( firstDefiniteClassIndex == -1 ) && ( classifications[ index ] != PlaneClassification.On ) )
				{
					firstDefiniteClassIndex = index;
				}
			}
			if ( firstDefiniteClassIndex > 1 )
			{
				//	The first two points are on the split plane. This means that we
				//	can easily classify the polygon as either behind or in front depending on the first
				//	definite class
				if ( classifications[ firstDefiniteClassIndex ] == PlaneClassification.Behind )
				{
					behind = new Polygon( source.Indices );
					inFront = null;
				}
				else
				{
					behind = null;
					inFront = new Polygon( source.Indices );
				}
				return;
			}

			int lastPtIndex = firstDefiniteClassIndex;
			int curPtIndex = ( lastPtIndex + 1 ) % source.Indices.Length;
			Point2 lastPt = m_Points[ source.Indices[ lastPtIndex ] ];
			PlaneClassification lastClass = classifications[ lastPtIndex ];

			List<int> behindPoints = new List<int>( source.Indices.Length + 2 );
			List<int> inFrontPoints = new List<int>( source.Indices.Length + 2 );

			for ( int count = 0; count < source.Indices.Length; ++count )
			{
				Point2 curPt = m_Points[ source.Indices[ curPtIndex ] ];
				PlaneClassification curClass = classifications[ curPtIndex ];
				if ( curClass == PlaneClassification.On )
				{
					curClass = lastClass;
				}

				if ( curClass != lastClass )
				{
					//	Split line on plane
					Line2Intersection intersection = Intersections2.GetLinePlaneIntersection( lastPt, curPt, plane );
					int newPtIndex = AddPoint( intersection.IntersectionPosition );
					behindPoints.Add( newPtIndex );
					inFrontPoints.Add( newPtIndex );
				}

				if ( curClass == PlaneClassification.Behind )
				{
					behindPoints.Add( source.Indices[ curPtIndex ] );
				}
				else
				{
					inFrontPoints.Add( source.Indices[ curPtIndex ] );
				}

				lastClass = curClass;
				lastPt = curPt;

				curPtIndex = ( curPtIndex + 1 ) % source.Indices.Length;
			}

			behind = ( behindPoints.Count == 0 ) ? null : new Polygon( behindPoints.ToArray( ) );
			inFront = ( inFrontPoints.Count == 0 ) ? null : new Polygon( inFrontPoints.ToArray( ) );
		}

		#region Private members

		private readonly List< Point2 > m_Points = new List< Point2 >( );

		/// <summary>
		/// Adds a point to the point list and returns its index
		/// </summary>
		private int AddPoint( Point2 pt )
		{
			int index = m_Points.Count;
			m_Points.Add( pt );
			return index;
		}

		#endregion
	}
}
