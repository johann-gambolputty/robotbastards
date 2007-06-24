using System.Windows.Forms;


namespace Rb.Interaction.Windows
{
    /// <summary>
    /// Keyboard input template
    /// </summary>
    class KeyInputTemplate : InputTemplate
    {
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="key">Key to check for</param>
		public KeyInputTemplate( Keys key )
		{
			m_Key = key;
		}

		/// <summary>
		/// Default constructor. Assigns no key
		/// </summary>
        public KeyInputTemplate()
		{
			m_Key = Keys.None;
		}

        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public override IInput CreateInput( InputContext context )
        {
            return new KeyInput( context, m_Key );
        }

		private Keys m_Key;
    }
}
