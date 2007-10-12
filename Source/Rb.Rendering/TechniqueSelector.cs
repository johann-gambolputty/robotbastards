
using System;

namespace Rb.Rendering
{
	/// <summary>
	/// Selects a technique from one or more effects. Standard implementation of <see cref="ITechniqueSelector"/>
	/// </summary>
	public class TechniqueSelector : ITechniqueSelector, ITechnique
    {
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
		/// <exception cref="ArgumentException">Thrown if name does not correspond to a technique in the current effect</exception>
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
		/// Applies the selected technique (<see cref="ITechnique.Apply(IRenderContext, IRenderable)"/>) to render the specified object
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
		
		/// <summary>
		/// Applies the selected technique (<see cref="ITechnique.Apply(IRenderContext, RenderDelegate)"/>) to render the specified object
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="render">Render delegate</param>
        public void Apply( IRenderContext context, RenderDelegate render )
        {
            if ( m_Technique != null )
            {
                m_Technique.Apply( context, render );
            }
            else
            {
                render( context );
            }
        }

		/// <summary>
		/// Returns true if this technique is a reasonable substitute for the specified technique
		/// </summary>
		/// <param name="technique">Technique to substitute</param>
		/// <returns>true if this technique can substitute the specified technique</returns>
		public bool IsSubstituteFor( ITechnique technique )
		{
			return ( Technique != null ) && ( Technique.Name == technique.Name );
		}

        private IEffect		m_Effect;
        private ITechnique	m_Technique;

		#region INamed Members

		//	TODO: AP: Probably get rid of INamed support in ITechnique
		public string Name
		{
			get { throw new NotSupportedException( ); }
			set { throw new NotSupportedException( ); }
		}

		#endregion
	}
}
