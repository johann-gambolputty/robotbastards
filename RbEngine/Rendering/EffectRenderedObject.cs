using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Wraps up an IRender object with a RenderEffect
	/// </summary>
	public class EffectRenderedObject : IRender, Components.IXmlLoader
	{
		/// <summary>
		/// The effect
		/// </summary>
		public RenderEffect			Effect
		{
			get
			{
				return m_Technique.Effect;
			}
			set
			{
				m_Technique.Effect = value;
			}
		}

		/// <summary>
		/// The currently selected technique in Effect
		/// </summary>
		public SelectedTechnique	Technique
		{
			get
			{
				return m_Technique;
			}
		}

		/// <summary>
		/// The object that will be rendered by the effect
		/// </summary>
		public IRender				RenderedObject
		{
			get
			{
				return m_RenderedObject;
			}
			set
			{
				m_RenderedObject = value;
				m_Technique.RenderCallback = new RenderTechnique.RenderDelegate( m_RenderedObject.Render );
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public EffectRenderedObject( )
		{
		}

		#region IRender Members

		/// <summary>
		/// Applies the currently selected technique, then renders the attached object
		/// </summary>
		public void Render()
		{
			m_Technique.Apply( );
		}

		#endregion

		#region IXmlLoader Members

		/// <summary>
		/// Parses the element that generated this object
		/// </summary>
		public bool ParseGeneratingElement( System.Xml.XmlElement element )
		{
			return true;
		}

		/// <summary>
		/// Parses an element
		/// </summary>
		public bool ParseElement( System.Xml.XmlElement element )
		{
			if ( element.Name == "technique" )
			{
				m_Technique.SelectTechnique( element.GetAttribute( "name" ) );
			}

			return true;
		}

		#endregion

		private IRender				m_RenderedObject;
		private SelectedTechnique	m_Technique = new SelectedTechnique( );
	}
}
