using System.Drawing;
using System.Windows.Forms;
using Rb.Core.Maths;

namespace Rb.NiceControls.Graph
{
	public class PiecewiseGraphInputHandler : GraphInputHandler
	{
		/// <summary>
		/// Sets the function to be controller by this handler
		/// </summary>
		public PiecewiseGraphInputHandler( PiecewiseLinearFunction1d function ) :
			base( function )
		{
		}

		/// <summary>
		/// Gets the piecewise function controlled by this handler
		/// </summary>
		public new PiecewiseLinearFunction1d Function
		{
			get { return ( PiecewiseLinearFunction1d )base.Function; }
		}

		#region IGraphInputHandler Members

		public override void Attach( IGraphControl control )
		{
			m_GraphControl = control;
			m_GraphControl.BaseControl.MouseDown += MouseDown;
			m_GraphControl.BaseControl.MouseMove += MouseMove;
			m_GraphControl.BaseControl.MouseUp += MouseUp;
		}

		public override void Detach( IGraphControl control )
		{
			System.Diagnostics.Debug.Assert( control == m_GraphControl );
			m_GraphControl.BaseControl.MouseDown -= MouseDown;
			m_GraphControl.BaseControl.MouseMove -= MouseMove;
			m_GraphControl.BaseControl.MouseUp -= MouseUp;
			m_GraphControl = null;
		}

		public override void Render( System.Drawing.Rectangle bounds, Graphics graphics )
		{
			base.Render( bounds, graphics );

			float yOffset = bounds.Bottom;
			float graphHeight = bounds.Height;
			for ( int cpIndex = 0; cpIndex < Function.NumControlPoints; ++cpIndex )
			{
				Brush brush = Brushes.Blue;
				//if ( cpIndex == m_SelectedCpIndex )
				//{
				//    brush = Brushes.Red;
				//}
				//else if ( cpIndex == 0 || cpIndex == ControlPoints.Count - 1 )
				//{
				//    brush = Brushes.Green;	
				//}

				float x = bounds.Left + Function[ cpIndex ].X * bounds.Width;
				float y = yOffset - Function[ cpIndex ].Y * graphHeight;
				graphics.FillEllipse( brush, x - 3, y - 3, 6, 6 );
			}
		}

		#endregion

		#region Private Members

		private IGraphControl m_GraphControl;
		private Point m_LastMousePos;
		private int m_MovingCpIndex = -1;

		#endregion

		#region Control Event Handlers

		private void InsertControlPoint( int index, PointF pt )
		{
			Function.InsertControlPoint( index, new Point2( pt.X, pt.Y ) );
			m_GraphControl.OnGraphChanged( );
		}

		private void AddNewControlPoint( Point controlLocation )
		{
			PointF graphPt = m_GraphControl.ClientToGraph( controlLocation );

			int cpIndex = 0;
			for ( ; cpIndex < Function.NumControlPoints; ++cpIndex )
			{
				if ( graphPt.X < Function[ cpIndex ].X )
				{
					break;
				}
			}
			InsertControlPoint( cpIndex, graphPt );
		}

		private int GetControlPointIndexAt( Point controlLocation )
		{
			for ( int cpIndex = 0; cpIndex < Function.NumControlPoints; ++cpIndex )
			{
				PointF cp = m_GraphControl.GraphToClient( Function[ cpIndex ].X, Function[ cpIndex ].Y );
				float sqrDist = ( controlLocation.X - cp.X ) * ( controlLocation.X - cp.X ) + ( controlLocation.Y - cp.Y ) * ( controlLocation.Y - cp.Y );
				if ( sqrDist <= 9.0f )
				{
					return cpIndex;
				}
			}
			return -1;
		}

		private void SelectControlPoint( Point controlLocation )
		{
			int cpIndex = GetControlPointIndexAt( controlLocation );
			m_MovingCpIndex = cpIndex;
		}

		private void MouseMove( object sender, MouseEventArgs e )
		{
			if ( m_MovingCpIndex == -1 )
			{
				m_LastMousePos = e.Location;
				return;
			}
			PointF lastGraphPos = m_GraphControl.ClientToGraph( m_LastMousePos );
			PointF curGraphPos = m_GraphControl.ClientToGraph( e.Location );

			float deltaX = curGraphPos.X - lastGraphPos.X;
			float deltaY = curGraphPos.Y - lastGraphPos.Y;

			float minPos = m_MovingCpIndex == 0 ? 0 : Function[ m_MovingCpIndex - 1 ].X + 0.01f;
			float pos = Function[ m_MovingCpIndex ].X;
			float maxPos = m_MovingCpIndex == Function.NumControlPoints - 1 ? 1 : Function[ m_MovingCpIndex + 1 ].X - 0.01f;

			Point2 cp = new Point2( );
			cp.X = Utils.Clamp( pos + deltaX, minPos, maxPos );
			cp.Y = Function[ m_MovingCpIndex ].Y + deltaY;

			Function[ m_MovingCpIndex ] = cp;

			m_LastMousePos = e.Location;
			m_GraphControl.OnGraphChanged( );
		}

		private void MouseDown( object sender, MouseEventArgs e )
		{
			if ( e.Button == MouseButtons.Right )
			{
				AddNewControlPoint( e.Location );
				return;
			}
			if ( e.Button == MouseButtons.Left )
			{
				SelectControlPoint( e.Location );
				return;
			}

		}

		private void MouseUp( object sender, MouseEventArgs e )
		{
			m_MovingCpIndex = -1;
		}

		#endregion

	}
}
