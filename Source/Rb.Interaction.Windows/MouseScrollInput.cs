using System;
using System.Windows.Forms;
using Rb.Core.Maths;

namespace Rb.Interaction.Windows
{
    /// <summary>
    /// Scroll wheel scalar input
    /// </summary>
    public class MouseScrollInput : ScalarInput
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
		public MouseScrollInput( InputContext context, MouseButtons button, float initial, float min, float max, float deltaPerDetent ) :
			base( context )
		{
			m_Button    = button;
		    Value       = initial;
		    m_Min       = min;
		    m_Max       = max;
		    m_Delta     = deltaPerDetent;

			( ( Control )context.Control ).MouseWheel += new MouseEventHandler( OnMouseWheel );
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
                Value = Utils.Clamp( Value + ( args.Delta > 0 ? m_Delta : -m_Delta ), m_Min, m_Max );
            }
		}

        private MouseButtons  m_Button;
        private float         m_Min;
        private float         m_Max;
        private float         m_Delta;
    }
}
