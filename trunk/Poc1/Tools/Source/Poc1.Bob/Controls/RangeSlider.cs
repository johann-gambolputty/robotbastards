using System;
using System.Windows.Forms;

namespace Poc1.Bob.Controls
{
	public partial class RangeSlider : UserControl
	{
		/// <summary>
		/// Event raised when the slider value changes
		/// </summary>
		public event EventHandler ValueChanged;

		/// <summary>
		/// Default constructor
		/// </summary>
		public RangeSlider( )
		{
			InitializeComponent( );
		}

		/// <summary>
		/// Gets/sets the slider enabled flag
		/// </summary>
		public bool SliderEnabled
		{
			get { return valueScrollBar.Enabled; }
			set { valueScrollBar.Enabled = value; }
		}

		/// <summary>
		/// Gets/sets the slider minimum value
		/// </summary>
		public decimal MinValue
		{
			get { return minValueUpDown.Value; }
			set
			{
				minValueUpDown.Value = value;
				if ( value > maxValueUpDown.Value )
				{
				    MaxValue = value;
				}
			}
		}

		/// <summary>
		/// Gets/sets the slider maximum value
		/// </summary>
		public decimal MaxValue
		{
			get { return maxValueUpDown.Value; }
			set
			{
				maxValueUpDown.Value = value;
				if ( value < minValueUpDown.Value )
				{
					MinValue = value;
				}
			}
		}

		/// <summary>
		/// Gets the current value of the slider
		/// </summary>
		public decimal Value
		{
			get
			{
				decimal t = ( valueScrollBar.Value - valueScrollBar.Minimum ) / ( decimal )( valueScrollBar.Maximum - valueScrollBar.Minimum );
				return MinValue + ( MaxValue - MinValue ) * t;
			}
			set
			{
				if ( MaxValue - MinValue == 0 )
				{
					return;
				}
				decimal t = ( Value - MinValue ) / ( MaxValue - MinValue );
				valueScrollBar.Value = ( int )( valueScrollBar.Minimum + ( valueScrollBar.Maximum - valueScrollBar.Minimum ) * t );
			}
		}

		#region Private Members

		/// <summary>
		/// Raises the ValueChanged event
		/// </summary>
		private void OnValueChanged( )
		{
			valueLabel.Text = Value.ToString( "F2" );
			if ( ValueChanged != null )
			{
				ValueChanged( this, EventArgs.Empty );
			}
		}

		#region Event Handlers

		private void valueScrollBar_Scroll( object sender, ScrollEventArgs e )
		{
			OnValueChanged( );
		}

		private void minValueUpDown_ValueChanged( object sender, EventArgs e )
		{
			OnValueChanged( );
		}

		private void maxValueUpDown_ValueChanged( object sender, EventArgs e )
		{
			OnValueChanged( );
		}

		#endregion

		#endregion
	}
}
