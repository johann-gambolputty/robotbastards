using System.Collections.Generic;
using System.Drawing;

namespace Rb.NiceControls.Graph
{
	public class LineGraph : IPiecewiseGraph
	{
		public LineGraph( )
		{
			m_ControlPoints.Add( new ControlPoint( 0, 0 ) );
			m_ControlPoints.Add( new ControlPoint( 1, 1 ) );
		}

		#region IPiecewiseGraph Members

		/// <summary>
		/// Returns true if control points cannot be added or removed from the graph
		/// </summary>
		public bool FixedControlPointCount
		{
			get { return false; }
		}

		/// <summary>
		/// The control points defining the function
		/// </summary>
		public List<ControlPoint> ControlPoints
		{
			get { return m_ControlPoints; }
		}

		#endregion

		#region IGraph Members

		/// <summary>
		/// Returns the name of the graph function
		/// </summary>
		public string Name
		{
			get { return "Line"; }
		}

		/// <summary>
		/// Creates an input handler for this graph
		/// </summary>
		public IGraphInputHandler CreateInputHandler( )
		{
			return new PiecewiseGraphInputHandler( this );
		}

		/// <summary>
		/// Samples the graph
		/// </summary>
		public float Sample( float t )
		{
			if ( ControlPoints.Count == 0 )
			{
				return 0;
			}
			if ( t <= ControlPoints[ 0 ].Position )
			{
				return ControlPoints[ 0 ].Value;
			}
			for ( int i = 0; i < ControlPoints.Count - 1; ++i )
			{
				if ( t <= ControlPoints[ i + 1 ].Position )
				{
					float minLocalT = ControlPoints[ i ].Position;
					float maxLocalT = ControlPoints[ i + 1 ].Position;

					float localT = ( t - minLocalT ) / ( maxLocalT - minLocalT );
					return ( ControlPoints[ i ].Value * ( 1 - localT ) + ControlPoints[ i + 1 ].Value * localT );
				}
			}
			return ControlPoints[ ControlPoints.Count - 1 ].Value;
		}

		/// <summary>
		/// Renders the graph
		/// </summary>
		public void Render( Rectangle bounds, Graphics graphics )
		{
			float yOffset = bounds.Bottom;
			float graphHeight = bounds.Height;
			using ( Pen graphPen = new Pen( Color.Red, 1.5f ) )
			{
				float y0 = yOffset - Sample( 0 ) * graphHeight;
				float t = 0;
				float tInc = 1.0f / ( bounds.Width - 1 );
				for ( int sample = 0; sample < bounds.Width; ++sample, t += tInc )
				{
					float y1 = yOffset - Sample( t ) * graphHeight;
					float x0 = sample + bounds.Left;
					float x1 = x0 + 1;
					graphics.DrawLine( graphPen, x0, y0, x1, y1 );
				//	graphics.FillEllipse( Brushes.Azure, x0 - 1, y0 - 1, 2, 2 );
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

		private readonly List<ControlPoint> m_ControlPoints = new List<ControlPoint>( );

		#endregion
	}
}
