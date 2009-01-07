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
		/// Helper class for creating a template in the TemplateGroup constructor
		/// </summary>
		public class TemplateInfo : TemplateBase
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="name">Template name</param>
			/// <param name="description">Template description</param>
			public TemplateInfo( string name, string description )
			{
				Name = name;
				Description = description;
			}
		}

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
				else if ( templateBase is TemplateInfo )
				{
					AddTemplate( templateBase.Name, templateBase.Description, templateBase.Icon );
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
		/// <returns>Returns the new template</returns>
		/// <exception cref="InvalidOperationException">Thrown if project type creation fails</exception>
		public Template AddTemplate( string name, string description, Icon icon )
		{
			Template template = CreateTemplate( );
			if ( template == null )
			{
				throw new InvalidOperationException( string.Format( "CreateTemplate for template group \"{0}\" returned a null", Name ) );
			}

			template.Name = name;
			template.Description = description;
			template.Icon = icon;
			m_Templates.Add( template );
			return template;
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

		#region Protected Members

		/// <summary>
		/// Creates a new project type
		/// </summary>
		protected virtual Template CreateTemplate( )
		{
			return new Template( this );
		}

		#endregion

		#region Private Members

		private readonly List<Template> m_Templates = new List<Template>( );

		#endregion
	}
}
