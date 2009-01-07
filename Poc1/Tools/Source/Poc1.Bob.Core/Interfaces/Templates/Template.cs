using System;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Interfaces.Templates
{
	/// <summary>
	/// Project type
	/// </summary>
	public class Template : TemplateBase
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="group">Parent group</param>
		/// <exception cref="ArgumentNullException">Thrown if group is null</exception>
		public Template( TemplateGroup group )
		{
			Arguments.CheckNotNull( group, "group" );
			m_Group = group;
		}

		/// <summary>
		/// Gets the group that this project type belongs to
		/// </summary>
		public TemplateGroup Group
		{
			get { return m_Group; }
		}

		#region Private Members

		private readonly TemplateGroup m_Group;

		#endregion
	}
}
