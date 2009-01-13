
using Poc1.Bob.Core.Interfaces.Templates;
using Rb.Core.Sets.Interfaces;

namespace Poc1.Bob.Core.Classes.Templates
{
	/// <summary>
	/// Template instance context
	/// </summary>
	public class TemplateInstanceContext : IObjectSetService
	{
		/// <summary>
		/// Gets/sets the currently selected template instance
		/// </summary>
		public TemplateInstance Instance
		{
			get { return m_Instance; }
			set { m_Instance = value; }
		}

		#region Private Members

		private TemplateInstance m_Instance;

		#endregion
	}
}
