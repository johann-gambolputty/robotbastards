using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Waves;
using Poc1.Tools.Waves;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Poc1.Bob.Controls.Waves
{
	public partial class WaveAnimatorControl : UserControl, IWaveAnimatorView
	{
		public WaveAnimatorControl( )
		{
			InitializeComponent( );
		}

		#region Private Members

		private WaveAnimation m_Animation;
		private int m_CurrentFrame;
		private WaveAnimationParameters m_Model;

		#region Event Handlers

		private void regenerateButton_Click( object sender, EventArgs e )
		{
			if ( GenerateAnimation != null )
			{
				GenerateAnimation( );
			}
		}

		private void animationTimer_Tick( object sender, EventArgs e )
		{
			if ( enableAnimationCheckBox.Checked || m_Animation == null )
			{
				return;
			}
			animationPanel.BackgroundImage = m_Animation.Frames[ m_CurrentFrame ];
			m_CurrentFrame = ( m_CurrentFrame + 1 ) % m_Animation.Frames.Length;
		}

		#endregion

		#endregion

		#region IWaveAnimatorView Members

		/// <summary>
		/// Event raised when the generate button is clicked
		/// </summary>
		public event ActionDelegates.Action GenerateAnimation;

		/// <summary>
		/// Gets/sets the generation enabled flag
		/// </summary>
		public bool GenerationEnabled
		{
			get { return regenerateButton.Enabled; }
			set { regenerateButton.Enabled = value; }
		}

		/// <summary>
		/// Gets/sets the progress of the wave animation gneerator
		/// </summary>
		public float WaveAnimationGenerationProgress
		{
			set
			{
				int range = progressBar1.Maximum - progressBar1.Minimum;
				progressBar1.Value = ( int )Utils.Clamp( progressBar1.Minimum + value * range, progressBar1.Minimum, progressBar1.Maximum );
			}
		}

		/// <summary>
		/// Gets/sets the wave animation model
		/// </summary>
		public WaveAnimationParameters Model
		{
			get { return m_Model;  }
			set
			{
				m_Model = value;
				animationPropertyGrid.SelectedObject = value;
			}
		}

		/// <summary>
		/// Shows the specified wave animation
		/// </summary>
		/// <param name="animation">Wave animation to show</param>
		public void ShowAnimation( WaveAnimation animation )
		{
			regenerateButton.Enabled = true;
			if ( animation.Frames.Length == 0 )
			{
				animationPanel.BackgroundImage = null;
				return;
			}
			animationPanel.BackgroundImage = animation.Frames[ 0 ];
			m_CurrentFrame = 0;
			m_Animation = animation;
		}


		#endregion
	}
}