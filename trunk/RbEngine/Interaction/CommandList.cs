using System;
using System.Collections;

namespace RbEngine.Interaction
{
	/// <summary>
	/// A list of commands
	/// </summary>
	public class CommandList : Components.Node, Components.INamedObject, Components.IXmlLoader
	{
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
		/// Updates the list of commands
		/// </summary>
		public void	Update( )
		{
			foreach ( Command curCommand in Commands )
			{
				curCommand.Update( );
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
				if ( NameChanged != null )
				{
					NameChanged( this );
				}
			}
		}

		/// <summary>
		/// Event, invoked when the name of this object changes
		/// </summary>
		public event Components.NameChangedDelegate NameChanged;

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
