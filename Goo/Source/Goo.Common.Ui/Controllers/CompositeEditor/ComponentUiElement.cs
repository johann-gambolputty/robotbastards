
namespace Goo.Common.Ui.Controllers.CompositeEditor
{
	public class ComponentUiElement
	{
		public ComponentUiElement( object component, string text, ComponentUiElement parentUiElement, ComponentTypeUiElement typeUiElement )
		{
			m_Component = component;
			m_Text = text;
			m_ParentUiElement = parentUiElement;
			m_TypeUiElement = typeUiElement;
		}

		#region Private Members

		private readonly object m_Component;
		private readonly string m_Text;
		private readonly ComponentUiElement m_ParentUiElement;
		private readonly ComponentTypeUiElement m_TypeUiElement;

		#endregion

		public string Text
		{
			get { return m_Text; }
		}

		public object Component
		{
			get { return m_Component; }
		}

		public ComponentTypeUiElement TypeUiElement
		{
			get { return m_TypeUiElement; }
		}

		public ComponentUiElement ParentUiElement
		{
			get { return m_ParentUiElement; }
		}
	}
}
