
namespace Rb.Interaction
{
    /// <summary>
    /// A command
    /// </summary>
    public class Command
    {
        #region Setup

        /// <summary>
        /// Setup constructor
        /// </summary>
		/// <param name="commands">Command list owner</param>
		/// <param name="name">Command name</param>
		/// <param name="displayName">Command display name</param>
        /// <param name="description">Command description</param>
        /// <param name="id">Command unique identifier</param>
        public Command( CommandList commands, string name, string displayName, string description, byte id )
        {
			m_Commands		= commands;
            m_Name			= name;
			m_DisplayName	= displayName;
            m_Description	= description;
            m_Id			= id;
        }

        #endregion

        #region Public properties

		/// <summary>
		/// The command list that this command is part of
		/// </summary>
		public CommandList Commands
		{
			get { return m_Commands; }
		}

        /// <summary>
        /// Command name
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

		/// <summary>
		/// Command display name
		/// </summary>
    	public string DispayName
    	{
    		get { return m_DisplayName; }
    	}

        /// <summary>
        /// Command description
        /// </summary>
        public string Description
        {
            get { return m_Description;  }
        }

        /// <summary>
        /// Command identifier
        /// </summary>
        public byte Id
        {
            get { return m_Id; }
        }

        #endregion

        #region Private stuff

        private readonly string			m_Name;
		private readonly string			m_DisplayName;
		private readonly string			m_Description;
		private readonly byte			m_Id;
		private readonly CommandList	m_Commands;

        #endregion
    }
}
