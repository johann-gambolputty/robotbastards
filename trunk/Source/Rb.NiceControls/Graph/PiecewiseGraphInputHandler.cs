using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Rb.Core.Maths;

namespace Rb.NiceControls.Graph
{
	public class PiecewiseGraphInputHandler : IGraphInputHandler
	{

		public PiecewiseGraphInputHandler( IPiecewiseGraph graph )
		{
			m_Graph = graph;
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

		#endregion

		#region Private Members

		private IGraphControl m_GraphControl;
		private readonly IPiecewiseGraph m_Graph;
		private Point m_LastMousePos;
		private int m_SelectedCpIndex = -1;
		private int m_MovingCpIndex = -1;

		private List<ControlPoint> ControlPoints
		{
			get { return m_Graph.ControlPoints; }
		}

		#endregion

		#region Control Event Handlers

		private void InvalidateControl( )
		{
			m_GraphControl.BaseControl.Invalidate( );
		}

		private void InsertControlPoint( int index, PointF pt )
		{
			m_Graph.ControlPoints.Insert( index, new ControlPoint( pt.X, pt.Y ) );
			InvalidateControl( );
		}

		private void AddNewControlPoint( Point controlLocation )
		{
			if ( m_Graph.FixedControlPointCount )
			{
				return;
			}

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

			ControlPoints[ m_MovingCpIndex ].Position = Utils.Clamp( pos + deltaX, minPos, maxPos );
			ControlPoints[ m_MovingCpIndex ].Value += deltaY;

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
