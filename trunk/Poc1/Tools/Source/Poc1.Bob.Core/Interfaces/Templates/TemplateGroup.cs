using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Interfaces.Templates
{
	/// <summary>
	/// A group that can hold project types, or project type sub-groups
	/// </summary>
	public class TemplateGroup : TemplateGroupContainer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public TemplateGroup( )
		{
		}

		/// <summary>
		/// Setup constructor with default icon
		/// </summary>
		/// <param name="name">Group name</param>
		/// <param name="description">Group description</param>
		/// <param name="contents">Pre-defined groups and templates</param>
		public TemplateGroup( string name, string description, params TemplateBase[] contents ) :
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
		public TemplateGroup( string name, string description, Icon icon, params TemplateBase[] contents ) :
			base( name, description, icon )
		{
			foreach ( TemplateBase templateBase in contents )
			{
				Arguments.CheckNotNull( templateBase, "contents" );
				if ( templateBase is TemplateGroupContainer )
				{
					SubGroups.Add( ( TemplateGroupContainer )templateBase );
				}
				else if ( templateBase is Template )
				{
					m_Templates.Add( ( Template )templateBase );
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
		public ReadOnlyCollection<Template> Templates
		{
			get { return m_Templates.AsReadOnly( ); }
		}

		/// <summary>
		/// Adds a new template to the group
		/// </summary>
		/// <param name="template">Template to add</param>
		/// <exception cref="ArgumentNullException">Thrown if template is null</exception>
		public void AddTemplate( Template template )
		{
			Arguments.CheckNotNull( template, "template" );
			m_Templates.Add( template );
		}

		/// <summary>
		/// Removes the specified project type
		/// </summary>
		/// <param name="template">Template to remove</param>
		public void RemoveTemplate( Template template )
		{
			Arguments.CheckNotNull( template, "template" );
			m_Templates.Remove( template );
		}

		#region Private Members

		private readonly List<Template> m_Templates = new List<Template>( );

		#endregion
	}
}
