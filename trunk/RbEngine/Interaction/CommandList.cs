using System;
using System.Collections;
using System.Reflection;

namespace RbEngine.Interaction
{
	/// <summary>
	/// A list of commands
	/// </summary>
	public class CommandList : Components.Node, Components.INamedObject, Components.IXmlLoader
	{
		#region	Command events

		/// <summary>
		/// Event, fired for any command that has just become active
		/// </summary>
		public event CommandEventDelegate	CommandActivated;

		/// <summary>
		/// Event, fired for every active command, every frame
		/// </summary>
		public event CommandEventDelegate	CommandActive;

		/// <summary>
		/// Invokes the Activated event. Called by Command.Update()
		/// </summary>
		public void OnCommandActivated( CommandMessage message )
		{
			if ( CommandActivated != null )
			{
				CommandActivated( message );
			}
		}

		/// <summary>
		/// Invokes the Active event. Called by Command.Update()
		/// </summary>
		public void OnCommandActive( CommandMessage message )
		{
			if ( CommandActive != null )
			{
				CommandActive( message );
			}
		}

		#endregion

		/// <summary>
		/// Access the command list (synonym for Components.Node.Children)
		/// </summary>
		public ICollection	Commands
		{
			get
			{
				return Children;
			}
		}

		/// <summary>
		/// Adds this command list to the command list manager
		/// </summary>
		public CommandList( string name )
		{
			m_Name = name;
			CommandListManager.Inst.Add( this );
		}

		/// <summary>
		/// Binds all commands to the specified scene view
		/// </summary>
		/// <param name="view">View to bind to</param>
		public void	BindToView( Scene.SceneView view )
		{
			foreach ( Command curCommand in Commands )
			{
				curCommand.BindToView( view );
			}
		}

		/// <summary>
		/// Unbinds all commands from the specified scene view
		/// </summary>
		/// <param name="view">View to unbind from</param>
		public void UnbindFromView( Scene.SceneView view )
		{
			foreach ( Command curCommand in Commands )
			{
				curCommand.UnbindFromView( view );
			}
		}

		/// <summary>
		/// Gets a command by ID
		/// </summary>
		public Command GetCommandById( int id )
		{
			foreach ( Command curCommand in Commands )
			{
				if ( curCommand.Id == id )
				{
					return curCommand;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets a command by name
		/// </summary>
		public Command GetCommandByName( string name )
		{
			foreach ( Command curCommand in Commands )
			{
				if ( curCommand.Name == name )
				{
					return curCommand;
				}
			}
			return null;
		}
		/// <summary>
		/// Adds a command
		/// </summary>
		/// <param name="cmd">Command to add</param>
		public void AddCommand( Command cmd )
		{
			AddChild( cmd );
		}

		/// <summary>
		/// Creates this command list from an enum
		/// </summary>
		public void AddEnumCommands( Type enumType )
		{
			Output.WriteLineCall( Output.InputInfo, "Creating command list from enum \"{0}\"", enumType.Name );

			string[]	commandNames	= Enum.GetNames( enumType );
			int[]		commandValues	= ( int[] )Enum.GetValues( enumType );

			for ( int commandIndex = 0; commandIndex < commandNames.Length; ++commandIndex )
			{
				string name			= commandNames[ commandIndex ];
				string description	= string.Empty;

				MemberInfo[]	info	= enumType.GetMember( name );
				object[]		attribs	= info[ 0 ].GetCustomAttributes( typeof( CommandEnumDescriptionAttribute ), false );
				if ( attribs.Length > 0 )
				{
					description = ( ( CommandEnumDescriptionAttribute )attribs[ 0 ] ).Description;
				}

				CommandInputInterpreter interpreter = null;
				attribs = info[ 0 ].GetCustomAttributes( typeof( CommandEnumInputInterpreterAttribute ), false );
				if ( attribs.Length > 0 )
				{
					Type interpreterType = ( ( CommandEnumInputInterpreterAttribute )attribs[ 0 ] ).InterpreterType;
					interpreter = ( CommandInputInterpreter )System.Activator.CreateInstance( interpreterType );
				}

				Output.WriteLineCall( Output.InputInfo, "Adding enum command \"{0}\"", name );

				Command newCommand		= new Command( name, description, ( ushort )commandValues[ commandIndex ] );
				newCommand.Interpreter	= interpreter;

				AddCommand( newCommand );
			}
		}

		/// <summary>
		/// Updates the list of commands
		/// </summary>
		public void	Update( )
		{
			foreach ( Command curCommand in Commands )
			{
				curCommand.Update( this );
			}
		}

		#region INamedObject Members

		/// <summary>
		/// Access to the name of this object
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		#endregion

		#region IXmlLoader Members

		/// <summary>
		/// Parses the generating element of this object
		/// </summary>
		public void ParseGeneratingElement( System.Xml.XmlElement element )
		{
		}

		/// <summary>
		/// Parses an element in the definition of this object
		/// </summary>
		public bool ParseElement( System.Xml.XmlElement element )
		{
			if ( element.Name != "inputs" )
			{
				return false;
			}

			//	Find the command
			string	commandName	= element.GetAttribute( "command" );
			Command	cmd			= GetCommandByName( commandName );
			if ( cmd == null )
			{
				throw new RbXmlException( element, "Command \"{0}\" does not exist", commandName );
			}

			//	Run through all the child nodes of the "inputs" element
			foreach ( System.Xml.XmlNode childNode in element.ChildNodes )
			{
				//	Skip non-elements
				System.Xml.XmlElement childElement = childNode as System.Xml.XmlElement;
				if ( childElement == null )
				{
					continue;
				}

				//	Create a CommandInput subclass instance from the "input" element's "type" attribute
				string			inputType	= childElement.GetAttribute( "type" );
				CommandInput	input		= ( CommandInput )AppDomainUtils.CreateInstance( inputType );
				if ( input == null )
				{
					throw new RbXmlException( childElement, "Failed to create input type \"{0}\" for command \"{1}\"", inputType, commandName );
				}

				cmd.AddInput( input );

				//	If the new input object implements IXmlLoader, then let it parse the generating element, and the child elements
				Components.IXmlLoader inputLoader = input as Components.IXmlLoader;
				if ( inputLoader != null )
				{
					inputLoader.ParseGeneratingElement( childElement );
					foreach ( System.Xml.XmlNode inputNode in childElement.ChildNodes )
					{
						System.Xml.XmlElement inputElement = inputNode as System.Xml.XmlElement;
						if ( inputElement != null )
						{
							inputLoader.ParseElement( inputElement );
						}
					}
				}

			}

			return true;
		}

		#endregion

		private string	m_Name;
	}
}
