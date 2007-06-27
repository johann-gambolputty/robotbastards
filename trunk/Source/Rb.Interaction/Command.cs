using System.Collections.Generic;

namespace Rb.Interaction
{
    /// <summary>
    /// Delegate used by <see cref="Command"/> events
    /// </summary>
    /// <param name="input">Input that activated the command</param>
    /// <param name="msg">Command details</param>
    public delegate void CommandEventDelegate( IInput input, CommandMessage msg );

    /// <summary>
    /// A command
    /// </summary>
    public class Command
    {
        #region Setup

        /// <summary>
        /// Setup constructor
        /// </summary>
		/// <param name="name">Command name</param>
		/// <param name="displayName">Command display name</param>
        /// <param name="description">Command description</param>
        /// <param name="id">Command unique identifier</param>
        public Command( string name, string displayName, string description, byte id )
        {
            m_Name			= name;
			m_DisplayName	= displayName;
            m_Description	= description;
            m_Id			= id;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event, fired when the command is activated (first frame of activation)
        /// </summary>
        public event CommandEventDelegate Activated;

        /// <summary>
        /// Event, fired when the command active
        /// </summary>
        public event CommandEventDelegate Active;

        #endregion

        #region Public properties

        /// <summary>
        /// Command name
        /// </summary>
        public string Name
        {
            get { return m_Name; }
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

        /// <summary>
        /// Command inputs
        /// </summary>
        public IList< IInput > Inputs
        {
            get { return m_Inputs; }
        }

        #endregion

        /// <summary>
        /// Command update
        /// </summary>
        public void Update( CommandList commands )
        {
			bool wasActive = ( m_LastActiveUpdate == m_UpdateCount );
			++m_UpdateCount;

            foreach ( IInput input in m_Inputs )
            {
                if ( input.IsActive )
				{
					CommandMessage message = input.CreateCommandMessage( this );

					//	Always invoke the Active event if the command is active
					if ( Active != null )
					{
						Active( input, message );
					}
					if ( commands != null )
					{
						commands.OnCommandActive( input, message );
					}

					//	Invoke the Activated event if the command has only just gone active
					if ( !wasActive )
					{
						if ( Activated != null )
						{
							Activated( input, message );
						}
						if ( commands != null )
						{
							commands.OnCommandActivated( input, message );
						}
					}
					m_LastActiveUpdate = m_UpdateCount;
					break;
				}
            }

            //  Disable all inputs
            foreach ( IInput input in m_Inputs )
            {
                if ( input.DeactivateOnUpdate )
                {
                    input.IsActive = false;
                }
            }
        }

        #region Private stuff

        private string          m_Name;
		private string			m_DisplayName;
        private string          m_Description;
        private byte            m_Id;

        private List< IInput >  m_Inputs = new List< IInput >( );
		private uint			m_UpdateCount;
		private uint			m_LastActiveUpdate;

        #endregion
    }
}
