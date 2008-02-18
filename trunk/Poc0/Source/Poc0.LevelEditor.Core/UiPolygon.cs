using System;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// A polygon defined by user input
	/// </summary>
	public class UiPolygon
	{
		/// <summary>
		/// A named polygonal brush
		/// </summary>
		/// <param name="name">Brush name</param>
		/// <param name="points">Brush polygon points</param>
		public UiPolygon( string name, Point2[] points ) :
			this( name, points, true )
		{
		}
		
		/// <summary>
		/// A named polygonal brush
		/// </summary>
		/// <param name="name">Brush name</param>
		/// <param name="points">Brush polygon points</param>
		/// <param name="checkWinding">If true, the constructor re-orders the points to enforce anti-clockwise winding</param>
		public UiPolygon( string name, Point2[] points, bool checkWinding )
		{
			m_Name = name;
			m_Points = points;

			if ( !checkWinding )
			{
				return;
			}

			//	Determine the winding of the polygon
			float internalAngles;
			float externalAngles;
			SumAngles( points, out internalAngles, out externalAngles );

			if ( externalAngles < internalAngles )
			{
				Array.Reverse( m_Points );
			}
		}

		/// <summary>
		/// Gets the name of this brush
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the points of this brush's polygon
		/// </summary>
		public Point2[] Points
		{
			get { return m_Points; }
		}
		
		/// <summary>
		/// Counts up the total internal and external angles in a polygon
		/// </summary>
		/// <param name="points">Polygon points</param>
		/// <param name="internalAngles">Sum of all internal angles</param>
		/// <param name="externalAngles">Sum of all external angles</param>
		public static void SumAngles( Point2[] points, out float internalAngles, out float externalAngles )
		{
			internalAngles = 0;
			externalAngles = 0;

			int lastPtIndex = points.Length - 1;
			int nextPtIndex = 1;
			for ( int ptIndex = 0; ptIndex < points.Length; ++ptIndex )
			{
				Point2 lastPt = points[ lastPtIndex ];
				Point2 curPt = points[ ptIndex ];
				Point2 nextPt = points[ nextPtIndex ];

				Vector2 vec0 = ( curPt - lastPt ).MakeNormal( );
				Vector2 vec1 = ( nextPt - curPt ).MakeNormal( );

				Vector2 perpVec0 = vec0.MakePerp( );
				float angle = Functions.Acos( vec0.Dot( vec1 ) );

				if ( perpVec0.Dot( vec1 ) >= 0 )
				{
					internalAngles += angle;
					externalAngles += Constants.TwoPi - angle;
				}
				else
				{
					internalAngles += Constants.TwoPi - angle;
					externalAngles += angle;
				}

				lastPtIndex = ptIndex;
				nextPtIndex = ( nextPtIndex + 1 ) % points.Length;
			}
		}

		private readonly string m_Name;
		private readonly Point2[] m_Points;
		
	}
}
