using System.Drawing;
using System.Drawing.Drawing2D;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Renderers
{
	/// <summary>
	/// Renders an <see cref="IGraphX2dSource"/> as a series of bars
	/// </summary>
	public class GraphX2dSamplesBarRenderer : IGraph2dRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public GraphX2dSamplesBarRenderer( )
		{
			Colour = Color.Red;
		}

		#region IGraph2dRenderer Members

		/// <summary>
		/// Gets/sets the principle colour of the graph
		/// </summary>
		public Color Colour
		{
			get { return m_Colour; }
			set
			{
				m_Colour = value;
				Color dark = GraphUtils.ScaleColour( m_Colour, 0.5f );
				m_Blends = new ColorBlend( 3 );
				m_Blends.Colors[ 0 ] = value; m_Blends.Positions[ 0 ] = 0;
				m_Blends.Colors[ 1 ] = value; m_Blends.Positions[ 1 ] = 0.90f;
				m_Blends.Colors[ 2 ] = dark;  m_Blends.Positions[ 2 ] = 1;
			}
		}

		/// <summary>
		/// Renders graph data
		/// </summary>
		/// <param name="graphics">Graphics object to render into</param>
		/// <param name="transform">Graph transform</param>
		/// <param name="data">Data to render</param>
		/// <param name="cursorDataPt">Position of the mouse cursor in data space</param>
		/// <param name="enabled">The enabled state of the graph control</param>
		public void Render( IGraphCanvas graphics, GraphTransform transform, IGraph2dSource data, PointF cursorDataPt, bool enabled )
		{
			IGraphX2dSampleSource sampleSource = ( IGraphX2dSampleSource )data;

			int sample = ( int )( transform.DataBounds.Left / sampleSource.XStepPerSample );
			Color darkColour = GraphUtils.ScaleColour( m_Colour, 0.2f );
			float hStep = sampleSource.XStepPerSample / 2;
			float dataX = ( sample * sampleSource.XStepPerSample ) + hStep;
			while ( dataX < transform.DataBounds.Right )
			{
				float value = sampleSource.Evaluate( dataX );
				dataX += sampleSource.XStepPerSample;

				Point tl = transform.DataToScreen( new PointF( dataX - hStep, value ) );
				Point br = transform.DataToScreen( new PointF( dataX + hStep, 0 ) );
				if ( ( br.Y - tl.Y ) == 0 )
				{
					continue;
				}

				Rectangle barArea = new Rectangle( tl.X - 1, tl.Y, ( br.X - tl.X ) + 1, br.Y - tl.Y );
				using ( LinearGradientBrush brush = new LinearGradientBrush( barArea, m_Colour, darkColour, 90.0f ) )
				{
					brush.InterpolationColors = m_Blends;
					graphics.FillRectangle( brush, barArea );
				}
			}
		}

		#endregion

		#region Private Members

		private ColorBlend m_Blends;
		private Color m_Colour;

		#endregion
	}
}
