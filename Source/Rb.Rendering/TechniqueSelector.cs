
namespace Rb.Rendering
{
	/// <summary>
	/// Selects a technique from one or more effects
	/// </summary>
    public class TechniqueSelector
    {
		/// <summary>
		/// A possible technique to select
		/// </summary>
		public class Option
		{


			private string		m_Path;
			private string		m_TechniqueName;
			private IEffect		m_Effect;
			private ITechnique	m_Technique;
		}

		/// <summary>
		/// Default constructor
		/// </summary>
        public TechniqueSelector( )
        {
        }

		/// <summary>
		/// Sets the effect, from which a technique can be selected
		/// </summary>
        public TechniqueSelector( IEffect effect )
        {
            Effect = effect;
        }

		/// <summary>
		/// Sets the selected technique
		/// </summary>
        public TechniqueSelector( ITechnique technique )
        {
            Technique = technique;
        }

		/// <summary>
		/// Selects a named technique from the current effect
		/// </summary>
		/// <param name="name">Technique name</param>
        public void Select( string name )
        {
            Technique = Effect.GetTechnique( name );
        }
		
		/// <summary>
		/// Access to the effect that the technique is selected from
		/// </summary>
        public IEffect Effect
        {
            get { return m_Effect; }
            set
            {
                m_Effect = value;
                m_Technique = null;
            }
        }

		/// <summary>
		/// Access to the selected technique
		/// </summary>
        public ITechnique Technique
        {
            get { return m_Technique; }
            set
            {
                m_Technique = value;
                if ( m_Technique != null )
                {
                    m_Effect = m_Technique.Effect;
                }
            }
        }

		/// <summary>
		/// Applies the selected technique (<see cref="ITechnique.Apply"/>) to render the specified object
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="renderable">Object to render</param>
        public void Apply( IRenderContext context, IRenderable renderable )
        {
            if ( m_Technique != null )
            {
                m_Technique.Apply( context, renderable );
            }
            else
            {
                renderable.Render( context );
            }
        }

        private IEffect		m_Effect;
        private ITechnique	m_Technique;
    }
}
