using System.Windows.Forms;

namespace Rb.Interaction.Windows
{
    /// <summary>
    /// Mouse input template
    /// </summary>
    public class MouseCursorInputTemplate : InputTemplate
    {
		/// <summary>
		/// Default constructor. Bindings to this input will trigger when the mouse moves with no buttons pressed
		/// </summary>
		public MouseCursorInputTemplate( )
		{
			m_Button = MouseButtons.None;
		}

		/// <summary>
		/// Setup constructor. Bindings to this input will trigger when the mouse moves with a particular button pressed
		/// </summary>
		/// <param name="button">Button to check for along with movement</param>
		public MouseCursorInputTemplate( MouseButtons button )
		{
			m_Button = button;
		}

        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public override IInput CreateInput( InputContext context )
        {
            return new MouseCursorInput( context, m_Button );
        }


		private MouseButtons m_Button;
    }
}
