using Rb.Core.Utils;

namespace Poc1.Bob.Core.Interfaces.Templates
{
	/// <summary>
	/// Describes a view supported by a template
	/// </summary>
	public class TemplateViewInfo
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="group">View group</param>
		/// <param name="name">View name</param>
		/// <param name="create">View creation delegate</param>
		public TemplateViewInfo( string group, string name, ActionDelegates.Action create )
		{
			m_Group = group;
			m_Name = name;
			m_Create = create;
		}

		#region Private Members

		private readonly string m_Group;
		private readonly string m_Name;
		private readonly ActionDelegates.Action m_Create;

		#endregion
	}
}
