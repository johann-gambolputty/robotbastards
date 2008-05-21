using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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
		}

		public void Detach( IGraphControl control )
		{
			System.Diagnostics.Debug.Assert( control == m_GraphControl );
			m_GraphControl.BaseControl.MouseDown -= MouseDown;
			m_GraphControl = null;
		}

		#endregion

		#region Private Members

		private IGraphControl m_GraphControl;
		private readonly IPiecewiseGraph m_Graph;
		private int m_SelectedCpIndex = -1;
		private int m_MovingCpIndex = -1;

		private List<ControlPoint> ControlPoints
		{
			get { return m_Graph.ControlPoints; }
		}

		#endregion

		#region Control Event Handlers

		private void MouseDown( object sender, MouseEventArgs e )
		{
			if ( e.Button == MouseButtons.Right )
			{
				if ( m_Graph.FixedControlPointCount )
				{
					return;
				}

				PointF graphPt = m_GraphControl.ClientToGraph( e.Location );

				for ( int cpIndex = 0; cpIndex < ControlPoints.Count; ++cpIndex )
				{
					if ( graphPt.X < ControlPoints[ cpIndex ].Position )
					{
						m_Graph.ControlPoints.Insert( cpIndex, new ControlPoint( graphPt.X, graphPt.Y ) );
					//	OnDistributionChanged( );
						return;
					}
				}
				m_Graph.ControlPoints.Add( new ControlPoint( graphPt.X, graphPt.Y ) );
			//	OnDistributionChanged( );
				return;
			}

			for ( int cpIndex = 0; cpIndex < ControlPoints.Count; ++cpIndex )
			{
				PointF cp = m_GraphControl.GraphToClient( ControlPoints[ cpIndex ].Position, ControlPoints[ cpIndex ].Value );
				float sqrDist = ( e.X - cp.X ) * ( e.X - cp.X ) + ( e.Y - cp.Y ) * ( e.Y - cp.Y );
				if ( sqrDist <= 9.0f )
				{
					m_SelectedCpIndex = m_MovingCpIndex = cpIndex;
				//	OnDistributionChanged( );
					return;
				}
			}
			m_SelectedCpIndex = -1;
			m_MovingCpIndex = -1;
		}

		#endregion

	}
}
