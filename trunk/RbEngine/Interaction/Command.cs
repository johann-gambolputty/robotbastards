using System;
using System.Collections;

namespace RbEngine.Interaction
{

	/// <summary>
	/// Delegate, used by the Command.Activated, Command.Active, Command.Deactivated and Command.Inactive events
	/// </summary>
	public delegate void	CommandEventDelegate( CommandEventArgs args );

	/// <summary>
	/// User command
	/// </summary>
	public class Command
	{
		#region	Command events

		/// <summary>
		/// Event, fired on the frame that this command becomes active (e.g. the first frame a key is pressed)
		/// </summary>
		public event CommandEventDelegate	Activated;

		/// <summary>
		/// Events, fired every frame that the command is active (e.g. the first and subsequent frames that a key is pressed)
		/// </summary>
		public event CommandEventDelegate	Active;

		/// <summary>
		/// Event, fired on the frame that this command becomes inactive (e.g. the frame that a key is raised)
		/// </summary>
		public event CommandEventDelegate	Deactivated;

		/// <summary>
		/// Event, fired every frame that the command is inactive (e.g. every frame that a key is not pressed)
		/// </summary>
		public event CommandEventDelegate	Inactive;


		#endregion

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Command name</param>
		/// <param name="description">Command description</param>
		/// <param name="id">Identifier for the command</param>
		public Command( string name, string description, ushort id )
		{
			m_Name			= name;
			m_Description	= description;
			m_Id			= id;
		}

		/// <summary>
		/// Command identifier
		/// </summary>
		public ushort				Id
		{
			get
			{
				return m_Id;
			}
		}

		#region	Command input

		/// <summary>
		/// The list of CommandInput objects
		/// </summary>
		public ArrayList			Inputs
		{
			get
			{
				return m_Inputs;
			}
		}

		/// <summary>
		/// Adds an input binding to this command
		/// </summary>
		public virtual void			AddInput( CommandInput input )
		{
			m_Inputs.Add( input );
		}

		/// <summary>
		/// Binds this command to a given scene view
		/// </summary>
		/// <param name="view">View to bind to</param>
		public void					BindToView( Scene.SceneView view )
		{
			foreach ( CommandInput curInput in m_Inputs )
			{
				m_Bindings.Add( input.BindToView( view ) );
			}
		}

		/// <summary>
		/// Unbinds this command from a given scene view
		/// </summary>
		/// <param name="view">View to unbind from</param>
		public void					UnbindFromView( Scene.SceneView view )
		{
			for ( int bindingIndex = 0; bindingIndex < m_Bindings.Count; )
			{
				if ( ( ( CommandInputBinding )m_Bindings[ bindingIndex ] ).View == view )
				{
					m_Bindings.RemoveAt( bindingIndex );
				}
				else
				{
					++bindingIndex;
				}
			}
		}

		#endregion

		/// <summary>
		/// Updates this command
		/// </summary>
		public void					Update( )
		{
			bool wasActive = ( m_LastActiveUpdate == m_UpdateCount );
			++m_UpdateCount;

			foreach ( CommandInputBinding curBinding in m_Bindings )
			{
				if ( curBinding.Active )
				{
					//	Always invoke the Active event if the command is active
					if ( Active != null )
					{
						Active( curBinding.CreateEventArgs( ) );
					}
					//	Invoke the Activated event if the command has only just gone active
					if ( ( !wasActive ) && ( Activated != null ) )
					{
						Activated( curBinding.CreateEventArgs( ) );
					}
					m_LastActiveUpdate = m_UpdateCount;
				}
			}

			if ( m_LastActiveUpdate != m_UpdateCount )
			{
				//	Always invoke the Inactive event, if the command is inactive
				if ( Inactive != null )
				{
					Inactive( curBinding.CreateEventArgs( ) );
				}
				//	Invoke the Deactivated event if the command has only just gone inactive
				if ( ( wasActive ) && ( Deactivated != null ) )
				{
					Deactivated( curBinding.CreateEventArgs( ) );
				}
			}
		}

		#region	Private stuff

		private string				m_Name;
		private string				m_Description;
		private ArrayList			m_Inputs	= new ArrayList( );
		private ArrayList			m_Bindings	= new ArrayList( );
		private ushort				m_Id;
		private int					m_UpdateCount;
		private int					m_LastActiveUpdate;

		#endregion
	}
}
