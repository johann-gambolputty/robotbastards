
namespace Poc1.Bob.Core.Interfaces.Projects
{
	/// <summary>
	/// Template instance
	/// </summary>
	public class Project
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="template">Template that created this instance</param>
		/// <param name="name">Template instance name</param>
		/// <param name="data">Template instance data</param>
		/// <param name="location">Template location (if loaded from a file)</param>
		public Project( ProjectType template, string name, object data, string location )
		{
			m_Template = template;
			m_Name = name;
			m_Data = data;
			m_Location = location;
		}

		/// <summary>
		/// Gets the template that created this instance
		/// </summary>
		public ProjectType Template
		{
			get { return m_Template; }
		}

		/// <summary>
		/// Gets the name of this instance
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the location of this instance
		/// </summary>
		public string Location
		{
			get { return m_Location; }
		}

		/// <summary>
		/// Gets the template instance data
		/// </summary>
		public object Data
		{
			get { return m_Data; }
		}

		#region Private Members

		private readonly ProjectType m_Template;
		private readonly string m_Name;
		private readonly string m_Location;
		private readonly object m_Data;

		#endregion
	}

	/// <summary>
	/// Template instance with typed data
	/// </summary>
	/// <typeparam name="T">Template instance data type</typeparam>
	public class TemplateInstance<T> : Project
		where T : class
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="template">Template that created this instance</param>
		/// <param name="name">Template instance name</param>
		/// <param name="data">Template instance data</param>
		/// <param name="location">Template location (if loaded from a file)</param>
		public TemplateInstance( ProjectType template, string name, T data, string location ) :
			base( template, name, data, location )
		{
		}

		/// <summary>
		/// Gets the typed template instance data
		/// </summary>
		public new T Data
		{
			get { return ( T )base.Data; }
		}

	}
}
