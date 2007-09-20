using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using Rb.Core.Utils;

namespace Rb.Interaction
{
	/// <summary>
	/// Stores mappings between <see cref="InputTemplate"/> objects and <see cref="Command"/> objects
	/// </summary>
	public class CommandInputTemplateMap : IXmlSerializable
	{
		/// <summary>
		/// Runs through all the commands that have been mapped to inputs
		/// </summary>
		public IEnumerable< Command > Commands
		{
			get
			{
				foreach ( CommandInputTemplates inputTemplates in m_Map )
				{
					yield return inputTemplates.Command;
				}
			}
		}

		/// <summary>
		/// Instances all the templates in the map, for the specified context, and adds them to a <see cref="CommandUser"/>
		/// </summary>
		/// <param name="context">Input context</param>
		/// <param name="user">User to bind inputs to commands</param>
		public void BindToInput( InputContext context, CommandUser user )
		{
			foreach ( CommandInputTemplates inputTemplates in m_Map )
			{
				foreach ( InputTemplate template in inputTemplates )
				{
					user.Bind( inputTemplates.Command, template.CreateInput( context ) );
				}
			}
		}

		#region IXmlSerializable Members

		/// <summary>
		/// No schema provided
		/// </summary>
		/// <returns>Returns null</returns>
		public System.Xml.Schema.XmlSchema GetSchema( )
		{
			return null;
		}

		/// <summary>
		/// Reads this object from an XML reader
		/// </summary>
		/// <param name="reader">XML reader</param>
		public void ReadXml( XmlReader reader )
		{
			if ( reader.IsEmptyElement )
			{
				reader.Read( );
				return;
			}

			m_Map.Clear( );

			reader.ReadStartElement( );

			while ( reader.NodeType != XmlNodeType.EndElement )
			{
				if ( reader.NodeType != XmlNodeType.Element )
				{
					reader.Read( );
					continue;
				}
				
				if ( reader.Name == "commandList" )
				{
					ReadCommandList( reader );
				}
				else
				{
					throw new ApplicationException( string.Format( "Unexpected element <{0}>", reader.Name ) );
				}
			}

			reader.ReadEndElement( );
		}

		/// <summary>
		/// Writes this object to XML
		/// </summary>
		/// <param name="writer">XML writer</param>
		public void WriteXml( XmlWriter writer )
		{
			//	TODO: AP: At some point, I guess...
			throw new ApplicationException( "WriteXml() unsupported" );
		}

        /// <summary>
        /// Reads an input template from XML
        /// </summary>
        public static InputTemplate ReadInputTemplate( XmlReader reader )
        {
            if ( reader.Name != "input" )
            {
                throw new ApplicationException("Expected <input> element");
            }

			string type = reader.GetAttribute( "type" );
			string assembly = reader.GetAttribute( "assembly" );
			if ( type == null )
			{
				throw new ApplicationException( "Expected \"type\" attribute in <input> element" );
			}
			Type inputType;
			if ( assembly == null )
			{
				inputType = AppDomainUtils.FindType( type );
			}
			else
			{
				inputType = AppDomain.CurrentDomain.Load( assembly ).GetType( type );
			}
			if ( inputType == null )
			{
				throw new ApplicationException( string.Format( "Failed to find input type \"{0}\"", type ) );
			}
			InputTemplate template = ( InputTemplate )Activator.CreateInstance( inputType );
			IXmlSerializable templateReader = template as IXmlSerializable;
			if ( templateReader != null )
			{
				templateReader.ReadXml( reader );
            }



            return template;
        }

		#endregion

		#region Private stuff

		private readonly List< CommandInputTemplates > m_Map = new List< CommandInputTemplates >( );

		/// <summary>
		/// Associates a command with some input templates
		/// </summary>
		private class CommandInputTemplates : List< InputTemplate >
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="cmd">Command</param>
			public CommandInputTemplates( Command cmd )
			{
				m_Command = cmd;
			}

			/// <summary>
			/// Gets the command
			/// </summary>
			public Command Command
			{
				get { return m_Command;  }
			}

			private readonly Command m_Command;
		}

		#endregion

		#region Private XML helpers

		/// <summary>
		/// Reads a CommandInputTemplates object from XML
		/// </summary>
		private static CommandInputTemplates ReadCommandInputs( XmlReader reader, CommandList commands )
		{
			//	Determine the command
			string commandName = reader.GetAttribute( "name" );
			if ( commandName == null )
			{
				throw new ApplicationException( string.Format( "<{0}> element should have \"command\" attribute", reader.Name ) );
			}

			Command cmd = commands.FindByName( commandName );
			if ( cmd == null )
			{
				throw new ApplicationException( string.Format( "Could not find command \"{0}\" in command list \"{1}\"", commandName, commands.Name ) );
			}

			CommandInputTemplates result = new CommandInputTemplates( cmd );

			if ( reader.IsEmptyElement )
			{
				reader.Read( );
				return result;
			}

			reader.ReadStartElement( );

			while ( reader.NodeType != XmlNodeType.EndElement )
			{
				if ( reader.NodeType != XmlNodeType.Element )
				{
					reader.Read( );
					continue;
				}
                result.Add( ReadInputTemplate( reader ) );
			}

			reader.ReadEndElement( );

			return result;
		}

		/// <summary>
		/// Reads a set of templates associated with a command list 
		/// </summary>
		private void ReadCommandList( XmlReader reader )
		{
			CommandList commandList;
			string commandListName = reader.GetAttribute( "name" );
			if ( commandListName == null )
			{
				string commandEnumName = reader.GetAttribute( "enum" );
				if ( commandEnumName == null )
				{
					throw new ApplicationException( "<commandList> requires a \"name\" or an \"enum\" attribute" );
				}
				string commandEnumAssemblyName = reader.GetAttribute( "assembly" );
				Type enumType = AppDomain.CurrentDomain.Load( commandEnumAssemblyName ).GetType( commandEnumName );
				if ( enumType == null )
				{
					throw new ApplicationException( string.Format( "Could not find enum type \"{0}\"", commandEnumName ) );
				}
				commandList = CommandList.FromEnum( enumType );
			}
			else
			{
				commandList = CommandListManager.Instance.Get( commandListName );
				if ( commandList == null )
				{
					throw new ApplicationException( string.Format( "Could not find command list named \"{0}\"", commandListName ) );
				}
			}

			if ( reader.IsEmptyElement )
			{
				return;
			}
			reader.ReadStartElement( );

			while ( reader.NodeType != XmlNodeType.EndElement )
			{
				if ( reader.NodeType != XmlNodeType.Element )
				{
					reader.Read( );
					continue;
				}
				if ( reader.Name == "command" )
				{
					m_Map.Add( ReadCommandInputs( reader, commandList ) );
				}
				else
				{
					throw new ApplicationException( string.Format( "Unexpected element <{0}>", reader.Name ) );
				}
			}

			reader.ReadEndElement( );
		}

		#endregion
	}
}
