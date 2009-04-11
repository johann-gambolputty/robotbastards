using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Interfaces.Projects
{
	/// <summary>
	/// A group that can hold project types, or project type sub-groups
	/// </summary>
	public class ProjectGroup : ProjectGroupContainer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ProjectGroup( )
		{
		}

		/// <summary>
		/// Setup constructor with default icon
		/// </summary>
		/// <param name="name">Group name</param>
		/// <param name="description">Group description</param>
		/// <param name="contents">Pre-defined groups and templates</param>
		public ProjectGroup( string name, string description, params ProjectNode[] contents ) :
			this( name, description, null, contents )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Group name</param>
		/// <param name="description">Group description</param>
		/// <param name="icon">Group icon</param>
		/// <param name="contents">Pre-defined sub-groups and templates</param>
		public ProjectGroup( string name, string description, Icon icon, params ProjectNode[] contents ) :
			base( name, description, icon )
		{
			foreach ( ProjectNode templateBase in contents )
			{
				Arguments.CheckNotNull( templateBase, "contents" );
				if ( templateBase is ProjectGroupContainer )
				{
					SubGroups.Add( ( ProjectGroupContainer )templateBase );
				}
				else if ( templateBase is ProjectType )
				{
					m_Templates.Add( ( ProjectType )templateBase );
				}
				else
				{
					throw new ArgumentException( string.Format( "Unsupported template type ({0}) specified in group \"{1}\" contents", templateBase.GetType( ), name ), "contents" );
				}
			}
		}

		/// <summary>
		/// Gets the list of templates belonging to this group
		/// </summary>
		public ReadOnlyCollection<ProjectType> Templates
		{
			get { return m_Templates.AsReadOnly( ); }
		}

		/// <summary>
		/// Adds a new template to the group
		/// </summary>
		/// <param name="template">ProjectType to add</param>
		/// <exception cref="ArgumentNullException">Thrown if template is null</exception>
		public void AddTemplate( ProjectType template )
		{
			Arguments.CheckNotNull( template, "template" );
			m_Templates.Add( template );
		}

		/// <summary>
		/// Removes the specified project type
		/// </summary>
		/// <param name="template">ProjectType to remove</param>
		public void RemoveTemplate( ProjectType template )
		{
			Arguments.CheckNotNull( template, "template" );
			m_Templates.Remove( template );
		}

		#region Private Members

		private readonly List<ProjectType> m_Templates = new List<ProjectType>( );

		#endregion
	}
}
