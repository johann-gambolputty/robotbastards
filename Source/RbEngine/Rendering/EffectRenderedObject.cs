using System;

namespace RbEngine.Rendering
{
	/*
	 * Overriding technique implementation
	 * 
	 * Use hash code of technique name
	 * 
	 * Can work in the RenderTechnique.Begin() call. 
	 * 
	 */

	/// <summary>
	/// Wraps up an IRender object with a RenderEffect
	/// </summary>
	public class EffectRenderedObject : IRender, Components.IXmlLoader, Scene.ISceneObject
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
		public AppliedTechnique	Technique
		{
			get
			{
				return m_Technique;
			}
		}

		/// <summary>
		/// Sets the selected technique by name
		/// </summary>
		public string				AppliedTechniqueName
		{
			set
			{
				m_Technique.SelectTechnique( value );
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
		public void Render( )
		{
			m_Technique.Apply( new TechniqueRenderDelegate( m_RenderedObject.Render ) );
		}

		#endregion

		#region IXmlLoader Members

		/// <summary>
		/// Parses the element that generated this object
		/// </summary>
		public void ParseGeneratingElement( System.Xml.XmlElement element )
		{
		}

		/// <summary>
		/// Parses an element
		/// </summary>
		public bool ParseElement( System.Xml.XmlElement element )
		{
			if ( element.Name == "technique" )
			{
				m_Technique.SelectTechnique( element.GetAttribute( "name" ) );
				return true;
			}

			return false;
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Adds this object to the scene rendering manager
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			db.Rendering.AddObject( this );
		}

		/// <summary>
		/// Removes this object to the scene rendering manager
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
			db.Rendering.RemoveObject( this );
		}

		#endregion
		
		private IRender				m_RenderedObject;
		private AppliedTechnique	m_Technique = new AppliedTechnique( );
	}
}
