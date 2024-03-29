using System.Collections.Generic;
using System.Drawing;

namespace Poc1.Bob.Core.Interfaces.Projects
{
	/// <summary>
	/// Container that can hold project type sub-groups
	/// </summary>
	public class ProjectGroupContainer : ProjectNode
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ProjectGroupContainer( )
		{	
		}

		/// <summary>
		/// Setup constructor with default icon
		/// </summary>
		/// <param name="name">Group name</param>
		/// <param name="description">Group description</param>
		/// <param name="subGroups">Pre-defined groups</param>
		public ProjectGroupContainer( string name, string description, params ProjectGroupContainer[] subGroups ) :
			this( name, description, null, subGroups )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Group name</param>
		/// <param name="description">Group description</param>
		/// <param name="icon">Group icon</param>
		/// <param name="subGroups">Pre-defined sub-groups</param>
		public ProjectGroupContainer( string name, string description, Icon icon, params ProjectGroupContainer[] subGroups )
		{
			Name = name;
			Description = description;
			Icon = icon;

			foreach ( ProjectGroupContainer subGroup in subGroups )
			{
				if ( subGroup != null )
				{
					SubGroups.Add( subGroup );
				}
			}
		}

		/// <summary>
		/// Gets the list of sub-groups belonging to this group
		/// </summary>
		public IList<ProjectGroupContainer> SubGroups
		{
			get { return m_SubGroups; }
		}

		#region Private Members

		private readonly List<ProjectGroupContainer> m_SubGroups = new List<ProjectGroupContainer>( );

		#endregion
	}
}
