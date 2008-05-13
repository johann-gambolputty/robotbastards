using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;

namespace Poc1.Tools.TerrainTextures
{
	public partial class TerrainTypeDistributionControl : UserControl
	{
		public event EventHandler DistributionChanged;

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
		}

		public IDistribution Distribution
		{
			get { return m_Distribution; }
			set
			{
				m_Distribution = value;
				OnDistributionChanged( );
			}
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

		private IList<ControlPoint> ControlPoints
		{
			get { return Distribution.ControlPoints; }
		}

		private IDistribution m_Distribution = new LinearDistribution( );
		private readonly ColorBlend m_Blends;
		private readonly Color m_GridColour;
		private int m_SelectedCpIndex = -1;
		private int m_MovingCpIndex = -1;
		private Point m_LastMousePos;
		private bool m_MoveAllControlPoints;

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
			float v0 = yOffset + graphHeight - Distribution.Sample( 0 ) * graphHeight;
			float t = 0;
			float tInc = 1.0f / ( GraphWidth - 1 );
			for ( int sample = 0; sample < GraphWidth; ++sample, t += tInc )
			{
				float v1 = yOffset + graphHeight - Distribution.Sample( t ) * graphHeight;
				e.Graphics.DrawLine( graphPen, sample + GraphX, v0, sample + GraphX + 1, v1 );
				v0 = v1;
			}

			for ( int cpIndex = 0; cpIndex < ControlPoints.Count; ++cpIndex )
			{
				Brush brush = Brushes.Blue;
				if ( cpIndex == m_SelectedCpIndex )
				{
					brush = Brushes.Red;
				}
				else if ( cpIndex == 0 || cpIndex == ControlPoints.Count - 1 )
				{
					brush = Brushes.Green;	
				}

				float x = GraphX + ControlPoints[ cpIndex ].Position * GraphWidth;
				float y = yOffset + graphHeight - ControlPoints[ cpIndex ].Value * graphHeight;

				e.Graphics.FillEllipse( brush, x - 3, y - 3, 6, 6 );
			}
		}

		private void TerrainTypeDistributionControl_MouseDown( object sender, MouseEventArgs e )
		{
			if ( e.Button == MouseButtons.Right )
			{
				float fX = ( e.X - GraphX ) / ( float )GraphWidth;
				float fY = ( ( GraphHeight - e.Y ) - GraphY ) / ( float )GraphHeight;

				for ( int cpIndex = 0; cpIndex < ControlPoints.Count; ++cpIndex )
				{
					if ( fX < ControlPoints[ cpIndex ].Position )
					{
						ControlPoints.Insert( cpIndex, new ControlPoint( fX, fY ) );
						OnDistributionChanged( );
						return;
					}
				}
				ControlPoints.Add( new ControlPoint( fX, fY ) );
				OnDistributionChanged( );
				return;
			}

			for ( int cpIndex = 0; cpIndex < ControlPoints.Count; ++cpIndex )
			{
				float cpX = GraphX + ControlPoints[ cpIndex ].Position * GraphWidth;
				float cpY = GraphY + GraphHeight - ControlPoints[ cpIndex ].Value * GraphHeight;

				float sqrDist = ( e.X - cpX ) * ( e.X - cpX ) + ( e.Y - cpY ) * ( e.Y - cpY );
				if ( sqrDist <= 9.0f )
				{
					m_SelectedCpIndex = m_MovingCpIndex = cpIndex;
					OnDistributionChanged( );
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

			if ( m_MoveAllControlPoints )
			{
				if ( m_MovingCpIndex != 0 && m_MovingCpIndex != ControlPoints.Count - 1 )
				{
					for ( int cpIndex = 1; cpIndex < ControlPoints.Count - 1; ++cpIndex )
					{
						float min = ControlPoints[ cpIndex - 1 ].Position + 0.0001f;
						float max = ControlPoints[ cpIndex + 1 ].Position - 0.0001f;

						float pos = ControlPoints[ cpIndex ].Position + deltaX;
						pos = ( pos < min ? min : ( pos > max ) ? max : pos );
						ControlPoints[ cpIndex ].Position = pos;
						ControlPoints[ cpIndex ].Value -= deltaY;
					}
				}
			}
			else
			{
				float min = ( m_MovingCpIndex == 0 ) ? 0 : ( ControlPoints[ m_MovingCpIndex - 1 ].Position + 0.0001f );
				float max = ( m_MovingCpIndex == ControlPoints.Count - 1 ) ? 1 : ( ControlPoints[ m_MovingCpIndex + 1 ].Position - 0.0001f );

				float pos = ControlPoints[ m_MovingCpIndex ].Position + deltaX;
				pos = ( pos < min ? min : ( pos > max ) ? max : pos );

				ControlPoints[ m_MovingCpIndex ].Position = pos;
				ControlPoints[ m_MovingCpIndex ].Value -= deltaY;
			}

			m_LastMousePos = e.Location;

			OnDistributionChanged( );
		}

		private void OnDistributionChanged( )
		{
			if ( DistributionChanged != null )
			{
				DistributionChanged( this, null );
			}
			Invalidate( );
		}

		private void TerrainTypeDistributionControl_MouseUp( object sender, MouseEventArgs e )
		{
			m_MovingCpIndex = -1;
		}

		private void TerrainTypeDistributionControl_KeyUp( object sender, KeyEventArgs e )
		{
			m_MoveAllControlPoints = false;
			if ( e.KeyCode == Keys.Delete )
			{
				if ( m_SelectedCpIndex != -1 && ControlPoints.Count > 2 )
				{
					ControlPoints.RemoveAt( m_SelectedCpIndex );
					m_SelectedCpIndex = -1;
					OnDistributionChanged( );
				}
			}
		}

		private void TerrainTypeDistributionControl_KeyDown( object sender, KeyEventArgs e )
		{
			m_MoveAllControlPoints = ( e.KeyCode == Keys.ShiftKey );
		}
	}
}
