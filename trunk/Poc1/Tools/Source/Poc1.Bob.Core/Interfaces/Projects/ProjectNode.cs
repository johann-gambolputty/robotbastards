using System.Drawing;

namespace Poc1.Bob.Core.Interfaces.Projects
{
	/// <summary>
	/// Base project type class
	/// </summary>
	public abstract class ProjectNode
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ProjectNode( )
		{	
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public ProjectNode( string name, string description, Icon icon )
		{
			m_Name = name;
			m_Description = description;
			m_Icon = icon;
		}

		/// <summary>
		/// Gets/sets the name of the project type
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// Gets/sets the description of the project type
		/// </summary>
		public string Description
		{
			get { return m_Description; }
			set { m_Description = value; }
		}

		/// <summary>
		/// Gets/sets the project type icon
		/// </summary>
		/// <remarks>
		/// If the icon is null, the project type views (<see cref="ITemplateSelectorView"/>)
		/// will use a default icon.
		/// </remarks>
		public Icon Icon
		{
			get { return m_Icon; }
			set { m_Icon = value; }
		}

		#region Private Members

		private string m_Name;
		private string m_Description;
		private Icon m_Icon;

		#endregion

	}
}
