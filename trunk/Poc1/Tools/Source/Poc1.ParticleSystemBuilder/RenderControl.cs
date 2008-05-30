using System;
using System.Windows.Forms;

namespace Poc1.ParticleSystemBuilder
{

	public partial class RenderControl : UserControl
	{
		public RenderControl( RenderMethod method )
		{
			m_RenderMethod = method;
			InitializeComponent( );
		}

		public Action<RenderControl> RenderMethodChanged;

		public RenderMethod RenderMethod
		{
			get { return m_RenderMethod; }
		}

		private readonly RenderMethod m_RenderMethod;

		private void stationaryRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			renderPanel.Controls.Clear( );
			m_RenderMethod.Method = RenderMethodType.Stationary;
		}

		private void circleRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			renderPanel.Controls.Clear( );
			m_RenderMethod.Method = RenderMethodType.Circle;
		}

		private void figureOfEightRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			renderPanel.Controls.Clear( );
			m_RenderMethod.Method = RenderMethodType.FigureOfEight;
		}

		private void followMouseRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			renderPanel.Controls.Clear( );
			m_RenderMethod.Method = RenderMethodType.FollowTheMouse;
		}
		
	}
}
