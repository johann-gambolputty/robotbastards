using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Rb.Core.Maths;

namespace Rb.NiceControls.Graph
{
	public class PiecewiseGraphInputHandler : IGraphInputHandler
	{
		public PiecewiseGraphInputHandler( PiecewiseLinearFunction1d function )
		{
			if ( function == null )
			{
				throw new ArgumentNullException( "function" );
			}
			m_Function = function;
		}

		#region IGraphInputHandler Members

		public void Attach( IGraphControl control )
		{
			m_GraphControl = control;
			m_GraphControl.BaseControl.MouseDown += MouseDown;
			m_GraphControl.BaseControl.MouseMove += MouseMove;
			m_GraphControl.BaseControl.MouseUp += MouseUp;
		}

		public void Detach( IGraphControl control )
		{
			System.Diagnostics.Debug.Assert( control == m_GraphControl );
			m_GraphControl.BaseControl.MouseDown -= MouseDown;
			m_GraphControl.BaseControl.MouseMove -= MouseMove;
			m_GraphControl.BaseControl.MouseUp -= MouseUp;
			m_GraphControl = null;
		}

		public void Render( System.Drawing.Rectangle bounds, Graphics graphics )
		{
			float yOffset = bounds.Bottom;
			float graphHeight = bounds.Height;
			using ( Pen graphPen = new Pen( Color.Red, 1.5f ) )
			{
				float y0 = yOffset - m_Function.GetValue( 0 ) * graphHeight;
				float t = 0;
				float tInc = 1.0f / ( bounds.Width - 1 );
				for ( int sample = 0; sample < bounds.Width; ++sample, t += tInc )
				{
					float y1 = yOffset - m_Function.GetValue( t ) * graphHeight;
					float x0 = sample + bounds.Left;
					float x1 = x0 + 1;
					graphics.DrawLine( graphPen, x0, y0, x1, y1 );
					y0 = y1;
				}
			}


			for ( int cpIndex = 0; cpIndex < ControlPoints.Count; ++cpIndex )
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

				float x = bounds.Left + ControlPoints[ cpIndex ].Position * bounds.Width;
				float y = yOffset - ControlPoints[ cpIndex ].Value * graphHeight;
				graphics.FillEllipse( brush, x - 3, y - 3, 6, 6 );
			}
		}

		#endregion

		#region Private Members

		private readonly PiecewiseLinearFunction1d m_Function;
		private IGraphControl m_GraphControl;
		private Point m_LastMousePos;
		private int m_SelectedCpIndex = -1;
		private int m_MovingCpIndex = -1;

		private List<PiecewiseLinearFunction1d.ControlPoint> ControlPoints
		{
			get { return m_Function.ControlPoints; }
		}

		#endregion

		#region Control Event Handlers

		private void InvalidateControl( )
		{
			m_GraphControl.BaseControl.Invalidate( );
		}

		private void InsertControlPoint( int index, PointF pt )
		{
			m_Function.ControlPoints.Insert( index, new PiecewiseLinearFunction1d.ControlPoint( pt.X, pt.Y ) );
			InvalidateControl( );
		}

		private void AddNewControlPoint( Point controlLocation )
		{
			PointF graphPt = m_GraphControl.ClientToGraph( controlLocation );

			int cpIndex = 0;
			for ( ; cpIndex < ControlPoints.Count; ++cpIndex )
			{
				if ( graphPt.X < ControlPoints[ cpIndex ].Position )
				{
					break;
				}
			}
			InsertControlPoint( cpIndex, graphPt );
			//	OnDistributionChanged( );	
		}

		private int GetControlPointIndexAt( Point controlLocation )
		{
			for ( int cpIndex = 0; cpIndex < ControlPoints.Count; ++cpIndex )
			{
				PointF cp = m_GraphControl.GraphToClient( ControlPoints[ cpIndex ].Position, ControlPoints[ cpIndex ].Value );
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
			m_SelectedCpIndex = cpIndex;
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

			float minPos = m_MovingCpIndex == 0 ? 0 : ControlPoints[ m_MovingCpIndex - 1 ].Position + 0.01f;
			float pos = ControlPoints[ m_MovingCpIndex ].Position;
			float maxPos = m_MovingCpIndex == ControlPoints.Count - 1 ? 1 : ControlPoints[ m_MovingCpIndex + 1 ].Position - 0.01f;

			PiecewiseLinearFunction1d.ControlPoint cp = new PiecewiseLinearFunction1d.ControlPoint( );
			cp.Position = Utils.Clamp( pos + deltaX, minPos, maxPos );
			cp.Value = ControlPoints[ m_MovingCpIndex ].Value + deltaY;

			ControlPoints[ m_MovingCpIndex ] = cp;

			m_LastMousePos = e.Location;
			InvalidateControl( );
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
