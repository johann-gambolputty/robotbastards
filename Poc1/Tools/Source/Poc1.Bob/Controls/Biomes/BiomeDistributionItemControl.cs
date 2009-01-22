using System.Drawing;
using System.Windows.Forms;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Properties;
using System;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class BiomeDistributionItemControl : UserControl
	{
		/// <summary>
		/// Event raised when the user clicks the up button
		/// </summary>
		public event Action<BiomeDistributionItemControl> MoveUp;

		/// <summary>
		/// Event raised when the user clicks the down button
		/// </summary>
		public event Action<BiomeDistributionItemControl> MoveDown;

		public BiomeDistributionItemControl( )
		{
			InitializeComponent( );
			SetStyle( ControlStyles.SupportsTransparentBackColor, true );
			BackColor = Color.Transparent;
			DoubleBuffered = true;
			upButton.Image = s_UpArrow;
			downButton.Image = s_DownArrow;
		}

		/// <summary>
		/// Gets/sets the biome displayed by this control
		/// </summary>
		public BiomeLatitudeRangeDistribution Distribution
		{
			get { return m_Distribution;  }
			set
			{
				m_Distribution = value;
				biomeName.Text = m_Distribution == null ? "" : m_Distribution.Biome.Name;
			}
		}

		private readonly static Bitmap s_UpArrow;
		private readonly static Bitmap s_DownArrow;
		private BiomeLatitudeRangeDistribution m_Distribution;

		static BiomeDistributionItemControl( )
		{
			s_UpArrow = Resources.MoveUp;
			s_DownArrow = Resources.MoveDown;

			s_UpArrow.MakeTransparent( Color.Magenta );
			s_DownArrow.MakeTransparent( Color.Magenta );
		}

		private void downButton_Click( object sender, System.EventArgs e )
		{
			if ( MoveDown != null )
			{
				MoveDown( this );
			}
		}

		private void upButton_Click( object sender, System.EventArgs e )
		{
			if ( MoveUp != null )
			{
				MoveUp( this );
			}
		}

	}
}
