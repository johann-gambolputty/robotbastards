using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;
using Rb.Common.Controls.Forms.Graphs;

namespace Rb.Common.Controls.Forms
{
	public partial class NumberBarControl : UserControl
	{
		public NumberBarControl( )
		{
			InitializeComponent( );

			( ( Bitmap )stepUpButton.BackgroundImage).MakeTransparent( Color.Magenta );
			( ( Bitmap )stepDownButton.BackgroundImage).MakeTransparent( Color.Magenta );

			Color mainColour = Color.LightSteelBlue;
			Color shadeColour = Color.SteelBlue;

			m_Blends = new ColorBlend( 3 );
			m_Blends.Colors[ 0 ] = shadeColour; m_Blends.Positions[ 0 ] = 0;
			m_Blends.Colors[ 1 ] = mainColour;	m_Blends.Positions[ 1 ] = 0.2f;
			m_Blends.Colors[ 2 ] = mainColour;	m_Blends.Positions[ 2 ] = 1;
		//	m_Blends.Colors[ 3 ] = Color.Black; m_Blends.Positions[ 3 ] = 1;
			
			m_MovingBlends = new ColorBlend( 3 );
			m_MovingBlends.Colors[ 0 ] = mainColour;	m_MovingBlends.Positions[ 0 ] = 0;
			m_MovingBlends.Colors[ 1 ] = shadeColour;	m_MovingBlends.Positions[ 1 ] = 0.2f;
			m_MovingBlends.Colors[ 2 ] = shadeColour;	m_MovingBlends.Positions[ 2 ] = 1;
		//	m_MovingBlends.Colors[ 3 ] = Color.Black;	m_MovingBlends.Positions[ 3 ] = 1;

		}

		/// <summary>
		/// Gets/sets the colour of the bar
		/// </summary>
		public Color BarColour
		{
			get { return m_Blends.Colors[ 1 ]; }
			set
			{
				m_Blends.Colors[ 1 ] = value;
				m_Blends.Colors[ 2 ] = value;
				m_MovingBlends.Colors[ 0 ] = value;
			//	m_MovingBlends.Colors[ 3 ] = value;
			}
		}
		
		/// <summary>
		/// Gets/sets the shade colour of the bar
		/// </summary>
		public Color BarShadeColour
		{
			get { return m_Blends.Colors[ 0 ]; }
			set
			{
				m_Blends.Colors[ 0 ] = value;
			//	m_Blends.Colors[ 3 ] = value;
				m_MovingBlends.Colors[ 1 ] = value;
				m_MovingBlends.Colors[ 2 ] = value;
			}
		}

		/// <summary>
		/// Gets/sets the step value
		/// </summary>
		public decimal Step
		{
			get { return m_Step; }
			set
			{
				bool changed = ( m_Step != value );
				m_Step = value;
				if ( changed )
				{
					Invalidate( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the minimum value
		/// </summary>
		public decimal MinimumValue
		{
			get { return m_MinimumValue; }
			set
			{ 
				bool changed = ( m_MinimumValue != value );
				m_MinimumValue = value;
				if ( changed )
				{
					Invalidate( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the maximum value
		/// </summary>
		public decimal MaximumValue
		{
			get { return m_MaximumValue; }
			set
			{
				bool changed = ( m_MaximumValue != value );
				m_MaximumValue = value;
				if ( changed )
				{
					Invalidate( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the bar value
		/// </summary>
		public decimal Value
		{
			get { return m_Value; }
			set
			{
				decimal clampedValue = value < MinimumValue ? MinimumValue : ( value > MaximumValue ? MaximumValue : value );
				bool changed = ( m_Value != clampedValue );
				m_Value = clampedValue;
				if ( changed )
				{
					valueLabel.Text = decimal.Round( m_Value, ValuePrecision ).ToString( );
					Invalidate( true );
				}
			}
		}

		/// <summary>
		/// Gets/sets the maximum number of decimal places used to show the value
		/// </summary>
		public int ValuePrecision
		{
			get { return m_ValuePrecision; }
			set { m_ValuePrecision = value; }
		}

		/// <summary>
		/// Gets/sets the font used to render tick marks
		/// </summary>
		public Font TickFont
		{
			get { return m_TickFont; }
			set { m_TickFont = value; }
		}

		#region Private Members

		private Font m_TickFont = new Font( "Courier New", 6 );
		private int m_ValuePrecision = 2;
		private readonly ColorBlend m_Blends;
		private readonly ColorBlend m_MovingBlends;
		private decimal m_Step = 1;
		private decimal m_Value = 50;
		private decimal m_MinimumValue = 0;
		private decimal m_MaximumValue = 100;
		private bool m_Moving;
		private float m_LastPos;
		
		#endregion

		#region Event Handlers

		private float GetNormalizedDataPosition( float x )
		{
			float result = ( x / valueLabel.Width );
			return result < 0 ? 0 : ( result > 1 ? 1 : result );
		}

		private void valueLabel_MouseDown( object sender, MouseEventArgs e )
		{
			m_LastPos = GetNormalizedDataPosition( e.X );
			m_Moving = true;
			Invalidate( true );
		}

		private void valueLabel_MouseUp( object sender, MouseEventArgs e )
		{
			m_Moving = false;
			Invalidate( true );
		}

		private void valueLabel_MouseMove( object sender, MouseEventArgs e )
		{
			if ( !m_Moving )
			{
				return;
			}
			float pos = GetNormalizedDataPosition( e.X );
			decimal diff = ( decimal )( pos - m_LastPos );
			m_LastPos = pos;
			Value += diff * ( MaximumValue - MinimumValue );
		}
		
		private void valueLabel_MouseLeave( object sender, System.EventArgs e )
		{
			m_Moving = false;
			Invalidate( true );
		}

		private void stepUpButton_Click( object sender, System.EventArgs e )
		{
			Value += m_Step;
		}

		private void stepDownButton_Click( object sender, System.EventArgs e )
		{
			Value -= m_Step;
		}

		#endregion

		private readonly ColorBlend m_BackBrush = CreateBackBrushBlends( );
		static ColorBlend CreateBackBrushBlends( )
		{
			Color mainColour = Color.LightSteelBlue;
			Color shadeColour = Color.WhiteSmoke;
			ColorBlend blends = new ColorBlend( 4 );
			blends.Colors[ 0 ] = mainColour;	blends.Positions[ 0 ] = 0;
			blends.Colors[ 1 ] = shadeColour;	blends.Positions[ 1 ] = 0.2f;
			blends.Colors[ 2 ] = shadeColour;	blends.Positions[ 2 ] = 0.8f;
			blends.Colors[ 3 ] = mainColour;	blends.Positions[ 3 ] = 1;

			return blends;
		}

		private void barPanel_Paint( object sender, PaintEventArgs e )
		{
			float t = ( float )( ( m_Value - m_MinimumValue ) / ( m_MaximumValue - m_MinimumValue ) );
			
			int width = ( int )( ( valueLabel.Width - 1 ) * t );

			Rectangle valueLabelRect = new Rectangle( valueLabel.Left, valueLabel.Top, valueLabel.Width, valueLabel.Height );

			using ( LinearGradientBrush backBrush = new LinearGradientBrush( new Rectangle( 0, 0, valueLabel.Width, valueLabel.Height ), Color.Black, Color.White, 90.0f ) )
			{
				backBrush.InterpolationColors = m_BackBrush;
				e.Graphics.FillRectangle( backBrush, valueLabelRect );
			}


			if ( width > 0 )
			{
				int x = valueLabel.Left;
				int y = valueLabel.Top + 1;
				int height = valueLabel.Height - 2;
				Rectangle barRect = new Rectangle( x, y, width, height );
				int radius = width < 4 ? width : 4;
				using ( GraphicsPath barPath = GraphGraphicsCanvas.CreateRoundedRectanglePath( barRect, radius ) )
				{
					using ( LinearGradientBrush barBrush = new LinearGradientBrush( new Rectangle( x, y, valueLabel.Width, height ), Color.Black, Color.White, 25 ) )
					{
						barBrush.InterpolationColors = m_Moving ? m_MovingBlends : m_Blends;
						e.Graphics.FillPath( barBrush, barPath );
						e.Graphics.DrawPath( Pens.Black, barPath );
					}
				}
			}

			int bottom = valueLabel.Height + 4;

			//e.Graphics.DrawLine( Pens.Black, valueLabel.Left, 0, valueLabel.Left, bottom );
			//e.Graphics.DrawLine( Pens.Black, valueLabel.Right, 0, valueLabel.Right, bottom );

			int mid = valueLabel.Left + valueLabel.Width / 2;
		//	e.Graphics.DrawLine( Pens.Black, mid, 0, mid, bottom );

			DrawCenteredString( e.Graphics, m_MinimumValue.ToString(), valueLabel.Left, bottom );
			DrawCenteredString( e.Graphics, ( m_MinimumValue + ( m_MaximumValue - m_MinimumValue ) / 2 ).ToString(), mid, bottom );
			DrawCenteredString( e.Graphics, m_MaximumValue.ToString(), valueLabel.Right, bottom );
		}

		private void DrawCenteredString( Graphics graphics, string str, float x, float y )
		{
			Font font = TickFont;
			float width = graphics.MeasureString( str, font ).Width;
			graphics.DrawString( str, font, Brushes.Black, x - width / 2, y );
		}
	}

}
