

namespace Rb.Rendering
{
	/// <summary>
	/// Wraps up an IRenderable object with a RenderEffect
	/// </summary>
	public class EffectRenderedObject : IRenderable
	{
		/// <summary>
		/// The effect
		/// </summary>
		public IEffect Effect
		{
			get { return m_Technique.Effect; }
			set { m_Technique.Effect = value; }
		}

		/// <summary>
		/// The currently selected technique in Effect
		/// </summary>
		public ITechnique Technique
		{
			get { return m_Technique.Technique; }
			set { m_Technique.Technique = value; }
		}

		/// <summary>
		/// Sets the selected technique by name
		/// </summary>
		public string TechniqueName
		{
			set
			{
				m_Technique.Select( value );
			}
		}

		/// <summary>
		/// The object that will be rendered by the effect
		/// </summary>
		public IRenderable RenderedObject
		{
			get { return m_RenderedObject; }
			set { m_RenderedObject = value; }
		}

		#region IRenderable Members

		/// <summary>
		/// Renders the attached object in the specified context
		/// </summary>
		public void Render( IRenderContext context )
		{
			context.ApplyTechnique( m_Technique.Technique, m_RenderedObject );
		}

		#endregion

		private IRenderable m_RenderedObject;
		private readonly TechniqueSelector m_Technique = new TechniqueSelector( );
	}
}
