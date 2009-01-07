using System;
using System.Collections.Generic;
using Rb.Common.Controls.Graphs.Classes.Renderers;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Sources
{
	/// <summary>
	/// Abstract base class for piecewise linear graphs
	/// </summary>
	public abstract class Graph2dSourcePiecewiseLinear : Graph2dSourceAbstract
	{
		#region ControlPoint Class

		/// <summary>
		/// Line segment control point
		/// </summary>
		public struct ControlPoint
		{
			/// <summary>
			/// Control point setup
			/// </summary>
			public ControlPoint( float x, float y )
			{
				m_X = x;
				m_Y = y;
			}

			/// <summary>
			/// Gets/sets the X component of this control point
			/// </summary>
			public float X
			{
				get { return m_X; }
				set { m_X = value; }
			}

			/// <summary>
			/// Gets/sets the Y component of this control point
			/// </summary>
			public float Y
			{
				get { return m_Y; }
				set { m_Y = value; }
			}

			#region Private Members

			private float m_X;
			private float m_Y;

			#endregion
		}

		#endregion

		/// <summary>
		/// Adds a control point to the graph
		/// </summary>
		/// <param name="pt">Control point to add</param>
		/// <returns>Returns the index of the n</returns>
		public virtual int AddControlPoint( ControlPoint pt )
		{
			m_ControlPoints.Add( pt );
			return NumberOfControlPoints - 1;
		}

		/// <summary>
		/// Removes a control point from the graph
		/// </summary>
		/// <param name="index">Index of the control point</param>
		public virtual void RemoveControlPointAt( int index )
		{
			m_ControlPoints.RemoveAt( index );
		}

		/// <summary>
		/// Inserts a control point into the grahp
		/// </summary>
		/// <param name="index">Insert index</param>
		/// <param name="pt">Control point to insert</param>
		public virtual void InsertControlPoint( int index, ControlPoint pt )
		{
			m_ControlPoints.Insert( index, pt );
		}

		/// <summary>
		/// Gets an indexed control point
		/// </summary>
		/// <param name="index">Control point index</param>
		/// <returns>Control point at the specified index</returns>
		public ControlPoint GetControlPoint( int index )
		{
			return m_ControlPoints[ index ];
		}
		
		/// <summary>
		/// Sets an indexed control point
		/// </summary>
		/// <param name="index">Control point index</param>
		/// <param name="cp">Control point data</param>
		public void SetControlPoint( int index, ControlPoint cp )
		{
			m_ControlPoints[ index ] = cp;
		}

		/// <summary>
		/// Gets the 2 control point indices of the line closest to the specified point (x,y)
		/// </summary>
		public bool GetLineNear( float x, float y, out int cp0, out int cp1, float tolerance )
		{
			cp0 = -1;
			cp1 = -1;
			if ( m_ControlPoints.Count < 2 )
			{
				return false;
			}
			cp0 = 0;
			float sqrDistToClosestLine = Math.Abs( y - m_ControlPoints[ 0 ].Y );
			sqrDistToClosestLine *= sqrDistToClosestLine;
			for ( int cpIndex = 1; cpIndex < m_ControlPoints.Count; ++cpIndex )
			{
				float lineX = m_ControlPoints[ cpIndex ].X - m_ControlPoints[ cpIndex - 1 ].X;
				float lineY = m_ControlPoints[ cpIndex ].Y - m_ControlPoints[ cpIndex - 1 ].Y;

				float diffX = x - m_ControlPoints[ cpIndex - 1 ].X;
				float diffY = y - m_ControlPoints[ cpIndex - 1 ].Y;
				float sqrLength = ( lineX * lineX ) + ( lineY * lineY );
				float t = ( ( diffX * lineX ) + ( diffY * lineY ) ) / sqrLength;
				t = t < 0 ? 0 : ( t > 1 ? 1 : t );

				float ptX = m_ControlPoints[ cpIndex - 1 ].X + lineX * t;
				float ptY = m_ControlPoints[ cpIndex - 1 ].Y + lineY * t;

				float xToPt = x - ptX;
				float yToPt = y - ptY;
				float sqrDist = ( xToPt * xToPt ) + ( yToPt * yToPt );
				if ( sqrDist < sqrDistToClosestLine )
				{
					cp0 = cpIndex - 1;
					cp1 = cpIndex;
					sqrDistToClosestLine = sqrDist;
				}
			}
			float sqrDistToLastLine = Math.Abs( y - m_ControlPoints[ m_ControlPoints.Count - 1 ].Y );
			sqrDistToLastLine *= sqrDistToLastLine;
			if ( sqrDistToLastLine < sqrDistToClosestLine )
			{
				cp0 = m_ControlPoints.Count - 1;
				cp1 = -1;
				sqrDistToClosestLine = sqrDistToLastLine;
			}

			return sqrDistToClosestLine < ( tolerance * tolerance );
		}

		/// <summary>
		/// Returns the index of the first control point within a given distance of a specified position
		/// </summary>
		public int IndexOfControlPointNear( float x, float y, float tolerance )
		{
			float sqrTol = tolerance * tolerance;

			for ( int cpIndex = 0; cpIndex < m_ControlPoints.Count; ++cpIndex )
			{
				float xDiff = m_ControlPoints[ cpIndex ].X - x;
				float yDiff = m_ControlPoints[ cpIndex ].Y - y;
				float sqrDist = ( xDiff * xDiff ) + ( yDiff * yDiff );
				if ( sqrDist < sqrTol )
				{
					return cpIndex;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the number of control points in the graph
		/// </summary>
		public int NumberOfControlPoints
		{
			get { return m_ControlPoints.Count; }
		}

		/// <summary>
		/// Gets the control point list
		/// </summary>
		public IEnumerable<ControlPoint> ControlPoints
		{
			get { return m_ControlPoints; }
		}

		/// <summary>
		/// Creates an instance of the default renderer type for this graph
		/// </summary>
		public override IGraph2dRenderer CreateRenderer( )
		{
			IGraph2dRenderer renderer = new Graph2dRendererList( new Graph2dPiecewiseLinearRenderer( ), new Graph2dControlPointRenderer( ) );
			return renderer;
		}

		#region Protected Members

		/// <summary>
		/// Gets the graph control points
		/// </summary>
		protected List<ControlPoint> ControlPointList
		{
			get { return m_ControlPoints; }
		}

		#endregion

		#region Private Members

		private List<ControlPoint> m_ControlPoints = new List<ControlPoint>( );

		#endregion
	}
}
