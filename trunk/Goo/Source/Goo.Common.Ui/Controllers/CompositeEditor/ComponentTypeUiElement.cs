using System;
using System.Drawing;

namespace Goo.Common.Ui.Controllers.CompositeEditor
{
	public class ComponentTypeUiElement
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="componentType">Component type this represents</param>
		/// <param name="name">Type name</param>
		/// <param name="description">Type description</param>
		public ComponentTypeUiElement( Type componentType, string name, string description )
		{
			m_ComponentType = componentType;
			m_Name = name;
			m_Description = description;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="componentType">Component type this represents</param>
		/// <param name="name">Type name</param>
		/// <param name="description">Type description</param>
		/// <param name="image">Type image</param>
		public ComponentTypeUiElement( Type componentType, string name, string description, Image image )
		{
			m_ComponentType = componentType;
			m_Name = name;
			m_Description = description;
			m_Image = image;
		}

		public string Name
		{
			get { return m_Name; }
		}

		public string Description
		{
			get { return m_Description; }
		}

		public Image Image
		{
			get { return m_Image; }
		}

		public Type ComponentType
		{
			get { return m_ComponentType; }
		}

		#region Private Members

		private readonly Type m_ComponentType;
		private readonly string m_Name;
		private readonly string m_Description;
		private readonly Image m_Image;

		#endregion

	}
}
