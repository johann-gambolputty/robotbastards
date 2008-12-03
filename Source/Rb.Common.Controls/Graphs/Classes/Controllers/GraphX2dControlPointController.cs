using Rb.Common.Controls.Graphs.Classes.Sources;
using Rb.Common.Controls.Graphs.Interfaces;
using Rb.Core.Maths;

namespace Rb.Common.Controls.Graphs.Classes.Controllers
{
	/// <summary>
	/// Moves control points in a piecewise linear graph
	/// </summary>
	public class GraphX2dControlPointController : Graph2dControllerBase
	{
		/// <summary>
		/// Screen selection distance
		/// </summary>
		public const float ScreenSelectionTolerance = 3.0f;

		/// <summary>
		/// Called by the control owner when the right mouse button is pressed
		/// </summary>
		/// <param name="data">Graph data</param>
		/// <param name="transform">Graph view transform</param>
		/// <param name="dataX">Mouse X position in data space</param>
		/// <param name="dataY">Mouse Y position in data space</param>
		/// <param name="down">True if the button was pressed, false if the button was released</param>
		/// <returns>Returns true if the event was handled, and the control owner should perform no further processing</returns>
		public override bool OnMouseRightButton( IGraph2dSource data, GraphTransform transform, float dataX, float dataY, bool down )
		{
			if ( !data.Selected || data.Disabled || !down )
			{
				return false;
			}

			GraphX2dSourceFunction1dAdapter pwlData = ( GraphX2dSourceFunction1dAdapter )data;
			PiecewiseLinearFunction1d pwlFunc = ( PiecewiseLinearFunction1d )pwlData.Function;
			int deleteIndex = pwlFunc.FindControlPoint( new Point2( dataX, dataY ), transform.ScreenToDataYScale * ScreenSelectionTolerance );
			if ( deleteIndex != -1 )
			{
				pwlFunc.RemoveControlPoint( deleteIndex );
				return false;
			}
			int insertAfterIndex = 0;
			for (; insertAfterIndex < pwlFunc.NumControlPoints; ++insertAfterIndex )
			{
				if ( dataX < pwlFunc[ insertAfterIndex ].X )
				{
					break;
				}
			}

			pwlFunc.InsertControlPoint( insertAfterIndex, new Point2( dataX, dataY ) );

			return true;
		}


		/// <summary>
		/// Called by the control owner when the left mouse button is pressed
		/// </summary>
		/// <param name="data">Graph data</param>
		/// <param name="transform">Graph view transform</param>
		/// <param name="dataX">Mouse X position in data space</param>
		/// <param name="dataY">Mouse Y position in data space</param>
		/// <param name="down">True if the button was pressed, false if the button was released</param>
		/// <returns>Returns true if the event was handled, and the control owner should perform no further processing</returns>
		public override bool OnMouseLeftButton( IGraph2dSource data, GraphTransform transform, float dataX, float dataY, bool down )
		{
			if ( !down || data.Disabled )
			{
				m_MovingIndices = null;
				return false;
			}

			GraphX2dSourceFunction1dAdapter pwlData = ( GraphX2dSourceFunction1dAdapter )data;
			PiecewiseLinearFunction1d pwlFunc = ( PiecewiseLinearFunction1d )pwlData.Function;

			float selectionDistance = transform.ScreenToDataYScale * ScreenSelectionTolerance;
			int closestCpIndex = pwlFunc.FindControlPoint( new Point2(  dataX, dataY ), selectionDistance );
			if ( closestCpIndex != -1 )
			{
				m_MovingIndices = new int[] { closestCpIndex };
				return true;
			}

			int closestCp0;
			int closestCp1;
			if ( GetLineNear( pwlFunc, new Point2( dataX, dataY ), out closestCp0, out closestCp1, selectionDistance ) )
			{
				m_MovingIndices = new int[] { closestCp0, closestCp1 };
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Called by the control owner when the mouse is moved
		/// </summary>
		/// <param name="data">Graph data</param>
		/// <param name="transform">Graph view transform</param>
		/// <param name="lastDataX">Previous mouse X position in data space</param>
		/// <param name="lastDataY">Previous mouse Y position in data space</param>
		/// <param name="dataX">Mouse X position in data space</param>
		/// <param name="dataY">Mouse Y position in data space</param>
		/// <returns>Returns true if the event was handled, and the control owner should perform no further processing</returns>
		public override bool OnMouseMove( IGraph2dSource data, GraphTransform transform, float lastDataX, float lastDataY, float dataX, float dataY )
		{
			if ( data.Disabled || m_MovingIndices == null )
			{
				return false;
			}
			
			GraphX2dSourceFunction1dAdapter pwlData = ( GraphX2dSourceFunction1dAdapter )data;
			PiecewiseLinearFunction1d pwlFunc = ( PiecewiseLinearFunction1d )pwlData.Function;

			int lastCpIndex = pwlFunc.NumControlPoints - 1;
			foreach ( int cpIndex in m_MovingIndices )
			{
				if ( cpIndex == -1 )
				{
					continue;
				}
				Point2 cp = pwlFunc[ cpIndex ];
				float deltaX = dataX - lastDataX;
				float deltaY = dataY - lastDataY;

				//	Clamp new control point (x,y) to the bounds of the data source
				float x = Rb.Core.Maths.Utils.Clamp( cp.X + deltaX, data.MinimumX, data.MaximumX );
				float y = Rb.Core.Maths.Utils.Clamp( cp.Y + deltaY, data.MinimumY, data.MaximumY );

				//	Clamp new control point x to the two neighbouring control points
				if ( ( cpIndex > 0 ) && ( x < pwlFunc[ cpIndex - 1 ].X ) )
				{
					x = pwlFunc[ cpIndex - 1 ].X;
				}
				else if ( ( cpIndex < lastCpIndex ) && ( x > pwlFunc[ cpIndex + 1 ].X ) )
				{
					x = pwlFunc[ cpIndex + 1 ].X;
				}

				pwlFunc[ cpIndex ] = new Point2( x, y );
			}

			return true;
		}

		#region Private Members

		private int[] m_MovingIndices;

		/// <summary>
		/// Gets the 2 control point indices of the line closest to the specified point (x,y)
		/// </summary>
		public bool GetLineNear( PiecewiseLinearFunction1d pwlFunc, Point2 pt, out int cp0, out int cp1, float tolerance )
		{
			cp0 = -1;
			cp1 = -1;
			if ( pwlFunc.NumControlPoints < 2 )
			{
				return false;
			}
			float sqrTol = tolerance * tolerance;
			LineSegment2 seg = new LineSegment2( );
			for ( int cpIndex = 1; cpIndex < pwlFunc.NumControlPoints; ++cpIndex )
			{
				seg.Start = pwlFunc[ cpIndex - 1 ];
				seg.End = pwlFunc[ cpIndex ];
				if ( seg.GetSqrDistanceToPoint( pt ) < sqrTol )
				{
					cp0 = cpIndex - 1;
					cp1 = cpIndex;
					return true;
				}
			}
			return false;
		}

		#endregion
	}
}
