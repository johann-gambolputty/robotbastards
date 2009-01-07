using System.Collections.Generic;
using System.Drawing;

namespace Poc1.Bob.Core.Interfaces.Templates
{
	/// <summary>
	/// Container that can hold project type sub-groups
	/// </summary>
	public class TemplateGroupContainer : TemplateBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public TemplateGroupContainer( )
		{	
		}

		/// <summary>
		/// Setup constructor with default icon
		/// </summary>
		/// <param name="name">Group name</param>
		/// <param name="description">Group description</param>
		/// <param name="subGroups">Pre-defined groups</param>
		public TemplateGroupContainer( string name, string description, params TemplateGroupContainer[] subGroups ) :
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
		public TemplateGroupContainer( string name, string description, Icon icon, params TemplateGroupContainer[] subGroups )
		{
			Name = name;
			Description = description;
			Icon = icon;

			foreach ( TemplateGroupContainer subGroup in subGroups )
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
		public IList<TemplateGroupContainer> SubGroups
		{
			get { return m_SubGroups; }
		}

		#region Private Members

		private readonly List<TemplateGroupContainer> m_SubGroups = new List<TemplateGroupContainer>( );

		#endregion
	}
}
