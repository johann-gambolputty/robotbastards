using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Classes.Biomes.Models;
using System.Collections.Generic;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class BiomeDistributionDisplay : UserControl, IBiomeDistributionView
	{
		public BiomeDistributionDisplay( )
		{
			InitializeComponent( );
			DoubleBuffered = true;
		}

		private void BiomeDistributionDisplay_Paint( object sender, PaintEventArgs e )
		{
			if ( Distributions == null )
			{
				return;
			}

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

			int radius = Size.Height;
			Rectangle ellipseRect = new Rectangle( Size.Width - radius, 0, radius * 2, radius * 2 );
			e.Graphics.FillEllipse( Brushes.SteelBlue, ellipseRect );

			Color colour0 = Color.FromArgb( 128, Color.Orange );
			Color colour1 = Color.FromArgb( 128, Color.Orange );
			Rectangle transitionArea = new Rectangle( EndOfControlAreaX, 0, StartOfActiveAreaX - EndOfControlAreaX, Height );
			using ( LinearGradientBrush brush = new LinearGradientBrush( transitionArea, colour0, colour1, 0.0f ) )
			{
				transitionArea.X += 1;
				transitionArea.Width -= 1;
				e.Graphics.FillRectangle( brush, transitionArea );
			}
			//using ( SolidBrush brush = new SolidBrush( colour1 ) )
			//{
			//    e.Graphics.FillRectangle( brush, StartOfActiveAreaX, 0, Width - StartOfActiveAreaX, Height );
			//}
			foreach ( BiomeDistributionItemControl control in m_ItemControls )
			{
				int loY = GetBiomeLowLatitudeInPixels( control );
				int hiY = GetBiomeHighLatitudeInPixels( control );
				using ( GraphicsPath path = CreateControlPath( 5, 0, control.Location.Y, Size.Width, control.Height, loY, hiY ) )
				{	
					using ( PathGradientBrush fillBrush = new PathGradientBrush( path ) )
					{
						fillBrush.WrapMode = WrapMode.Tile;
						fillBrush.SurroundColors = new Color[] { Color.FromArgb( 128, Color.LightSteelBlue ) };
						fillBrush.CenterColor = Color.FromArgb( 128, Color.WhiteSmoke );
						fillBrush.FocusScales = new PointF( 0.95f, 0.85f );
						fillBrush.CenterPoint = new PointF( control.Location.X + control.Width / 2, control.Location.Y + control.Height / 2 );
						e.Graphics.FillPath( Enabled ? fillBrush : SystemBrushes.Control, path );
					}

					e.Graphics.DrawPath( m_BorderPen, path );
				}
			}

		}

		#region IBiomeDistributionView Members

		/// <summary>
		/// Gets/sets the displayed biome list distribution model
		/// </summary>
		public BiomeListLatitudeDistributionModel Distributions
		{
			get { return m_Distributions; }
			set
			{
				UnlinkCurrentModel( );
				m_Distributions = value;
				LinkCurrentModel( );
			}
		}

		#endregion

		#region Private Members

		private BiomeDistributionItemControl m_Upper;
		private BiomeDistributionItemControl m_Lower;
		private int m_LastY = 0;
		private Pen m_BorderPen = new Pen( Color.Black, 2 );
		private readonly List<BiomeDistributionItemControl> m_ItemControls = new List<BiomeDistributionItemControl>( );
		private BiomeListLatitudeDistributionModel m_Distributions;

		/// <summary>
		/// Creates a path surrounding this control
		/// </summary>
		private static GraphicsPath CreateControlPath( float radius, int x, int y, int w, int h, int loY, int hiY )
		{
			GraphicsPath path = new GraphicsPath( );
		//	float diameter = radius * 2;
			float midX = x + w / 2;
			float midY = y + h;
			float endX = x + w;

			//	Draw rounded box around control parts
			path.AddLine( midX, y, x + radius, y );				//	Top middle to top left
		//	path.AddArc( x, y, diameter, diameter, 180, 90 );	//	Top left corner
			path.AddLine( x, y + radius, x, midY - radius );	//	Top left to bottom left
		//	path.AddArc( x, midY, diameter, diameter, 90, 90 );	//	Bottom left corner
			path.AddLine( x + radius, midY, midX, midY );		//	Bottom left to bottom middle

			float offset = 15;
			float tWidth = 40;

			float tWidthOff = tWidth - offset;

			//	Spline from control box to biome latitude
			path.AddBezier
				(
					new PointF( midX, midY ),			//	Bottom middle 
					new PointF( midX + offset, midY ),	//	Bottom middle X + 5 
					new PointF( midX + tWidthOff, hiY ),//	High middle X + 10
					new PointF( midX + tWidth, hiY )	//	High middle X + 15
				);
			path.AddLine( midX + tWidth, hiY, endX, hiY );		//	High middle X + 15 to high right
			path.AddLine( endX, hiY, endX, loY );				//	High right to low right
			path.AddLine( endX, loY, midX + tWidth, loY );		//	Low right to low middle X + 15

			path.AddBezier
				(
					new PointF( midX + tWidth, loY ),	//	Low middle X + 15
					new PointF( midX + tWidthOff, loY ),//	Low middle X + 10
					new PointF( midX + offset, y ),		//	Bottom middle X + 5
					new PointF( midX, y )				//	Bottom middle
				);

			path.CloseFigure( );

			return path;
		}
		/// <summary>
		/// Unlinks the current model to the view
		/// </summary>
		private void UnlinkCurrentModel( )
		{
			if ( m_Distributions == null )
			{
				return;
			}
			m_Distributions.DistributionAdded -= OnDistributionChanged;
			m_Distributions.DistributionRemoved -= OnDistributionChanged;
		}

		/// <summary>
		/// Links the current model to the view
		/// </summary>
		private void LinkCurrentModel( )
		{
			if ( m_Distributions == null )
			{
				return;
			}
			m_Distributions.DistributionAdded += OnDistributionChanged;
			m_Distributions.DistributionRemoved += OnDistributionChanged;

			Rebuild( );
		}

		/// <summary>
		/// Removes all the controls in a list
		/// </summary>
		private void RemoveControls( )
		{
			foreach ( BiomeDistributionItemControl control in m_ItemControls )
			{
				Controls.Remove( control );
			}
			m_ItemControls.Clear( );
		}

		/// <summary>
		/// Rebuilds the display dynamic controls
		/// </summary>
		private void Rebuild( )
		{
			RemoveControls( );
			if ( Distributions == null || Distributions.Distributions.Count == 0 )
			{
				Invalidate( );
				return;
			}

			foreach ( BiomeLatitudeRangeDistribution distribution in Distributions.Distributions )
			{
				BiomeDistributionItemControl control = CreateControlForDistribution( distribution );
				m_ItemControls.Add( control );
				Controls.Add( control );
			}

			UpdateItemControlSizes( );
			Invalidate( );
		}

		/// <summary>
		/// Returns the X coordinate of the end of the control are
		/// </summary>
		private int EndOfControlAreaX
		{
			get { return DisplayRectangle.Width / 2; }
		}

		/// <summary>
		/// Returns the X coordinate of the end of the control are
		/// </summary>
		private int StartOfActiveAreaX
		{
			get { return EndOfControlAreaX + 40; }
		}

		/// <summary>
		/// Updates the sizes of the current set of controls
		/// </summary>
		private void UpdateItemControlSizes( )
		{
			int y = 0;
			int width = EndOfControlAreaX;
			int height = ( int )( DisplayRectangle.Height / ( float )Distributions.NumberOfDistributions );
			foreach ( BiomeDistributionItemControl control in m_ItemControls )
			{
				control.SetBounds( 0, y, width, height );
				y += height;
			}
	
		}

		/// <summary>
		/// Creates controls for a model
		/// </summary>
		private BiomeDistributionItemControl CreateControlForDistribution( BiomeLatitudeRangeDistribution distribution )
		{
			BiomeDistributionItemControl control = new BiomeDistributionItemControl( );
			control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			control.Distribution = distribution;
			control.MoveUp += OnMoveBiomeUp;
			control.MoveDown += OnMoveBiomeDown;
			return control;
		}

		/// <summary>
		/// Gets the height in pixels of a biome's low latitude line
		/// </summary>
		private int GetBiomeLowLatitudeInPixels( BiomeDistributionItemControl itemControl )
		{
			return ( int )( itemControl.Distribution.MinLatitude * DisplayRectangle.Height );
		}

		/// <summary>
		/// Gets the height in pixels of a biome's high latitude line
		/// </summary>
		private int GetBiomeHighLatitudeInPixels( BiomeDistributionItemControl itemControl )
		{
			return ( int )( itemControl.Distribution.MaxLatitude * DisplayRectangle.Height );
		}

		/// <summary>
		/// Selects distributions to move
		/// </summary>
		private bool SelectDistributionsToMove( int x, int y, out BiomeDistributionItemControl upper, out BiomeDistributionItemControl lower )
		{
			if ( x < StartOfActiveAreaX )
			{
				upper = null;
				lower = null;
				return false;
			}
			int lastIndex = m_ItemControls.Count - 1;
			for ( int controlIndex = 0; controlIndex < m_ItemControls.Count; ++controlIndex )
			{
				BiomeDistributionItemControl control = m_ItemControls[ controlIndex ];
				int lo = GetBiomeLowLatitudeInPixels( control );
				int hi = GetBiomeHighLatitudeInPixels( control );
				if ( ( Math.Abs( y - lo ) < 4 ) && ( controlIndex > 0 ) )
				{
					upper = m_ItemControls[ controlIndex - 1 ];
					lower = m_ItemControls[ controlIndex ];
					return false;
				}
				if ( ( Math.Abs( y - hi ) < 4 ) && ( controlIndex < lastIndex ) )
				{
					upper = m_ItemControls[ controlIndex ];
					lower = m_ItemControls[ controlIndex + 1 ];
					return false;
				}
			}
			upper = null;
			lower = null;
			return true;
		}

		/// <summary>
		/// Swaps two distributions
		/// </summary>
		private void SwapDistributionPositions( int index, int swapIndex )
		{
			if ( swapIndex < 0 || swapIndex >= m_ItemControls.Count )
			{
				return;
			}
			BiomeLatitudeRangeDistribution tmpDistribution = m_ItemControls[ index ].Distribution;
			m_ItemControls[ index ].Distribution = m_ItemControls[ swapIndex ].Distribution;
			m_ItemControls[ swapIndex ].Distribution = tmpDistribution;
		}


		#region Event Handlers

		/// <summary>
		/// Handles moving a biome up the distribution list
		/// </summary>
		private void OnMoveBiomeUp( BiomeDistributionItemControl control )
		{
			int index = m_ItemControls.IndexOf( control );
			System.Diagnostics.Debug.Assert( index != -1 );
			SwapDistributionPositions( index, index - 1 );
		}

		/// <summary>
		/// Handles moving a biome down the distribution list
		/// </summary>
		private void OnMoveBiomeDown( BiomeDistributionItemControl control )
		{
			int index = m_ItemControls.IndexOf( control );
			System.Diagnostics.Debug.Assert( index != -1 );
			SwapDistributionPositions( index, index + 1 );
		}

		/// <summary>
		/// Handles the biome list changing
		/// </summary>
		private void OnDistributionChanged( BiomeLatitudeRangeDistribution distribution )
		{
			Rebuild( );
		}

		/// <summary>
		/// Handles control resizing
		/// </summary>
		private void BiomeDistributionDisplay_Resize( object sender, EventArgs e )
		{
			UpdateItemControlSizes( );
			Invalidate( );
		}


		/// <summary>
		/// Handles control mouse leave event
		/// </summary>
		private void BiomeDistributionDisplay_MouseLeave( object sender, EventArgs e )
		{
			m_Upper = null;
			m_Lower = null;
			Cursor = Cursors.Arrow;
		}


		/// <summary>
		/// Handles control mouse move
		/// </summary>
		private void BiomeDistributionDisplay_MouseMove( object sender, MouseEventArgs e )
		{
			float diffY = ( e.Y - m_LastY ) / ( float )Height;
			bool invalidate = false;
			bool canMoveUpper = ( m_Upper != null ) && ( m_Upper.Distribution.MaxLatitude + diffY > m_Upper.Distribution.MinLatitude );
			bool canMoveLower = ( m_Lower != null ) && ( m_Lower.Distribution.MinLatitude + diffY < m_Lower.Distribution.MaxLatitude );
			if ( canMoveUpper && ( m_Lower == null || canMoveLower ) )
			{
				m_Upper.Distribution.MaxLatitude += diffY;
				invalidate = true;
			}
			if ( canMoveLower && ( m_Upper == null || canMoveUpper ) )
			{
				m_Lower.Distribution.MinLatitude += diffY;
				invalidate = true;
			}
			if ( invalidate )
			{
				Invalidate( );
			}
			BiomeDistributionItemControl upper, lower;
			if ( SelectDistributionsToMove( e.X, e.Y, out upper, out lower ) )
			{
				Cursor = Cursors.Arrow;
			}
			else
			{
				Cursor = Cursors.HSplit;
			}
			m_LastY = e.Y;
		}

		/// <summary>
		/// Handles control mouse down event
		/// </summary>
		private void BiomeDistributionDisplay_MouseDown( object sender, MouseEventArgs e )
		{
			if ( SelectDistributionsToMove( e.X, e.Y, out m_Upper, out m_Lower ) )
			{
				Cursor = Cursors.HSplit;
			}
			else
			{
				Cursor = Cursors.Arrow;
			}
		}

		/// <summary>
		/// Handles control mouse up event
		/// </summary>
		private void BiomeDistributionDisplay_MouseUp( object sender, MouseEventArgs e )
		{
			m_Upper = null;
			m_Lower = null;
			Cursor = Cursors.Arrow;
		}

		#endregion

		#endregion
	}
}
