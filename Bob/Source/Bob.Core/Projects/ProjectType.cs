using Rb.Core.Utils;
using Rb.Interaction.Classes;

namespace Bob.Core.Projects
{
	/// <summary>
	/// Project type
	/// </summary>
	public class ProjectType
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="category">Project type category name</param>
		/// <param name="name">Project type name</param>
		/// <param name="description">Project type description</param>
		/// <param name="commandLists">Project type command lists</param>
		/// <exception cref="System.ArgumentNullException">Name or commandLists is null</exception>
		/// <exception cref="System.ArgumentException">Name is empty</exception>
		public ProjectType( string category, string name, string description, params ProjectCommandList[] commandLists )
		{
			Arguments.CheckNotNullOrEmpty( name, "name" );
			Arguments.CheckNotNull( commandLists, "commandLists" );
			m_Name = name;
			m_Category = category ?? "";
			m_Description = description ?? "";
			m_CommandLists = commandLists;
		}

		/// <summary>
		/// Gets the category name of this project type
		/// </summary>
		public string Category
		{
			get { return m_Category; }
		}

		/// <summary>
		/// Gets the name of this project type
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the description of this project type
		/// </summary>
		public string Description
		{
			get { return m_Description; }
		}

		/// <summary>
		/// Gets the command lists associated with this project type
		/// </summary>
		public CommandList[] CommandLists
		{
			get { return m_CommandLists; }
		}

		#region Private Members

		private readonly string m_Category;
		private readonly string m_Name;
		private readonly string m_Description;
		private readonly ProjectCommandList[] m_CommandLists;

		#endregion

	}

	public class ExampleProjectType : ProjectType
	{
		public static class FileCommands
		{
			public static readonly ProjectCommandList	Commands;
			public static readonly ProjectCommand		Open;
			public static readonly ProjectCommand		Close;
			
			static FileCommands( )
			{
				Commands = new ProjectCommandList( "file", "File", CommandRegistry.Instance );
				Open = Commands.NewCommand( null, "open", "&Open", "Opens a file" );
				Close = Commands.NewCommand( null, "close", "&Close", "Closes the current file" );
			}
		}

		public static class ViewCommands
		{
			public static readonly ProjectCommandList	Commands;
			public static readonly ProjectCommand		Output;

			static ViewCommands( )
			{
				Commands = new ProjectCommandList( "view", "View", CommandRegistry.Instance );
				Output = Commands.NewCommand( null, "view", "&Output", "Shows/hides the output window" );
			}
		}

		public ExampleProjectType( ) :
			base( "", "Example", "Example project type", FileCommands.Commands, ViewCommands.Commands )
		{
		}
	}
}
