using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;

namespace Poc1.Tools.TerrainTextures
{
	public partial class TerrainTypeDistributionControl : UserControl
	{
		public TerrainTypeDistributionControl( )
		{
			InitializeComponent( );

			Color dark = Color.LightSteelBlue;
			Color light = Color.White;

			m_Blends = new ColorBlend( 4 );
			m_Blends.Colors[ 0 ] = dark; m_Blends.Positions[ 0 ] = 0;
			m_Blends.Colors[ 1 ] = light; m_Blends.Positions[ 1 ] = 0.1f;
			m_Blends.Colors[ 2 ] = light; m_Blends.Positions[ 2 ] = 0.9f;
			m_Blends.Colors[ 3 ] = dark; m_Blends.Positions[ 3 ] = 1;

			m_GridColour = dark;

			m_ControlPoints.Add( new ControlPoint( 0, 0 ) );
			m_ControlPoints.Add( new ControlPoint( 0.5f, 0.5f ) );
			m_ControlPoints.Add( new ControlPoint( 1, 1 ) );
		}

		private static int GraphX
		{
			get { return 2; }
		}

		private static int GraphY
		{
			get { return 2; }
		}

		private int GraphWidth
		{
			get { return Width - 4; }
		}

		private int GraphHeight
		{
			get { return Height - 4; }
		}

		private IDistribution m_Distribution = new LinearDistribution( );
		private readonly List<ControlPoint> m_ControlPoints = new List<ControlPoint>( );
		private readonly ColorBlend m_Blends;
		private readonly Color m_GridColour;
		private int m_SelectedCpIndex = -1;
		private int m_MovingCpIndex = -1;
		private Point m_LastMousePos;

		private const int HorizontalSubdivisions = 8;
		private const int VerticalSubdivisions = 8;

		private void TerrainTypeDistributionControl_Paint( object sender, PaintEventArgs e )
		{
			Rectangle bounds = new Rectangle( 0, 0, Width, Height );
			if ( Enabled )
			{
				using ( LinearGradientBrush fillBrush = new LinearGradientBrush( bounds, Color.Black, Color.White, 90 ) )
				{
					fillBrush.InterpolationColors = m_Blends;
					e.Graphics.FillRectangle( fillBrush, bounds );
				}
			}
			else
			{
				e.Graphics.FillRectangle( SystemBrushes.Control, bounds );
			}

			using ( Pen pen = new Pen( Enabled ? m_GridColour : Color.LightGray, 1.0f ) )
			{
				float x = GraphX;
				float incX = ( GraphWidth - 1 ) / ( ( float )HorizontalSubdivisions - 1 );
				for ( int col = 0; col < 8; ++col, x += incX )
				{
					e.Graphics.DrawLine( pen, x, GraphY, x, GraphHeight );
				}

				float y = GraphY;
				float incY = ( GraphHeight - 1 ) / ( ( float )VerticalSubdivisions - 1 );
				for ( int row = 0; row < 8; ++row, y += incY )
				{
					e.Graphics.DrawLine( pen, GraphX, y, GraphWidth, y );
				}
			}

			if ( !Enabled )
			{
				return;
			}

			float yOffset = GraphY;
			float graphHeight = GraphHeight;
			Pen graphPen = Pens.Red;
			float v0 = yOffset + graphHeight - m_Distribution.Sample( m_ControlPoints, 0 ) * graphHeight;
			float t = 0;
			float tInc = 1.0f / ( GraphWidth - 1 );
			for ( int sample = 0; sample < GraphWidth; ++sample, t += tInc )
			{
				float v1 = yOffset + graphHeight - m_Distribution.Sample( m_ControlPoints, t ) * graphHeight;
				e.Graphics.DrawLine( graphPen, sample + GraphX, v0, sample + GraphX + 1, v1 );
				v0 = v1;
			}

			for ( int cpIndex = 0; cpIndex < m_ControlPoints.Count; ++cpIndex )
			{
				Brush brush = Brushes.Blue;
				if ( cpIndex == m_SelectedCpIndex )
				{
					brush = Brushes.Red;
				}
				else if ( cpIndex == 0 || cpIndex == m_ControlPoints.Count - 1 )
				{
					brush = Brushes.Green;	
				}

				float x = GraphX + m_ControlPoints[ cpIndex ].Position * GraphWidth;
				float y = yOffset + graphHeight - m_ControlPoints[ cpIndex ].Value * graphHeight;

				e.Graphics.FillEllipse( brush, x - 3, y - 3, 6, 6 );
			}
		}

		private void TerrainTypeDistributionControl_MouseDown( object sender, MouseEventArgs e )
		{
			if ( e.Button == MouseButtons.Right )
			{
				float fX = ( e.X - GraphX ) / ( float )GraphWidth;
				float fY = ( ( GraphHeight - e.Y ) - GraphY ) / ( float )GraphHeight;

				for ( int cpIndex = 0; cpIndex < m_ControlPoints.Count; ++cpIndex )
				{
					if ( fX < m_ControlPoints[ cpIndex ].Position )
					{
						m_ControlPoints.Insert( cpIndex, new ControlPoint( fX, fY ) );
						Invalidate( );
						return;
					}
				}
				m_ControlPoints.Add( new ControlPoint( fX, fY ) );
				Invalidate( );
				return;
			}

			for ( int cpIndex = 0; cpIndex < m_ControlPoints.Count; ++cpIndex )
			{
				float cpX = GraphX + m_ControlPoints[ cpIndex ].Position * GraphWidth;
				float cpY = GraphY + GraphHeight - m_ControlPoints[ cpIndex ].Value * GraphHeight;

				float sqrDist = ( e.X - cpX ) * ( e.X - cpX ) + ( e.Y - cpY ) * ( e.Y - cpY );
				if ( sqrDist <= 9.0f )
				{
					m_SelectedCpIndex = m_MovingCpIndex = cpIndex;
					Invalidate( );
					return;
				}
			}
			m_SelectedCpIndex = -1;
			m_MovingCpIndex = -1;
		}

		private void TerrainTypeDistributionControl_MouseMove( object sender, MouseEventArgs e )
		{
			if ( m_MovingCpIndex == -1 )
			{
				m_LastMousePos = e.Location;
				return;
			}
			float deltaX = e.X - m_LastMousePos.X;
			float deltaY = e.Y - m_LastMousePos.Y;

			deltaX /= GraphWidth;
			deltaY /= GraphHeight;

			float min = ( m_MovingCpIndex == 0 ) ? 0 : ( m_ControlPoints[ m_MovingCpIndex - 1 ].Position );
			float max = ( m_MovingCpIndex == m_ControlPoints.Count - 1 ) ? 1 : ( m_ControlPoints[ m_MovingCpIndex + 1 ].Position );
			min += 0.001f;
			max -= 0.001f;

			float pos = m_ControlPoints[ m_MovingCpIndex ].Position + deltaX;
			pos = ( pos < min ? min : ( pos > max ) ? max : pos );

			m_ControlPoints[ m_MovingCpIndex ].Position = pos;
			m_ControlPoints[ m_MovingCpIndex ].Value -= deltaY;

			m_LastMousePos = e.Location;

			Invalidate( );
		}

		private void TerrainTypeDistributionControl_MouseUp( object sender, MouseEventArgs e )
		{
			m_MovingCpIndex = -1;
		}

		private void TerrainTypeDistributionControl_KeyUp( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Delete )
			{
				if ( m_SelectedCpIndex != -1 && m_ControlPoints.Count > 2 )
				{
					m_ControlPoints.RemoveAt( m_SelectedCpIndex );
					m_SelectedCpIndex = -1;
					Invalidate( );
				}
			}
		}
	}
}
