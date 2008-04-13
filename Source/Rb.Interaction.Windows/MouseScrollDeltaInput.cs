using System.Windows.Forms;

namespace Rb.Interaction.Windows
{
	public class MouseScrollDeltaInput : ScalarInput
	{

        /// <summary>
        /// Flag that determines if the input should be disabled (IsActive = false) after each update
        /// </summary>
        public override bool DeactivateOnUpdate
        {
            get { return true; }
        }

		/// <summary>
		/// Setup constructor
		/// </summary>
		public MouseScrollDeltaInput( InputContext context, MouseButtons button, float delta ) :
			base( context )
		{
			m_Button    = button;
		    m_Delta     = delta;
		    Value       = 0;

			( ( Control )context.Control ).MouseWheel += OnMouseWheel;
		}

		/// <summary>
		/// Triggers the command, if the appropriate mouse button is pressed
		/// </summary>
		private void OnMouseWheel( object sender, MouseEventArgs args )
		{
			if ( m_Button == MouseButtons.None )
			{
                IsActive = true;
			}
			else
			{
                IsActive = ( args.Button == m_Button );
			}

            if ( IsActive )
            {
				Value = args.Delta > 0 ? m_Delta : -m_Delta;
            }
		}

        private readonly MouseButtons m_Button;
		private readonly float m_Delta;
	}
}
