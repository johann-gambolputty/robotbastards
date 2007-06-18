using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering
{
    //  TODO: AP: Implement ITechnique interface? (would require very little change, but is it useful?)

    public class TechniqueSelector
    {
        public TechniqueSelector( )
        {
        }

        public TechniqueSelector( IEffect effect )
        {
            Effect = effect;
        }

        public TechniqueSelector( ITechnique technique )
        {
            Technique = technique;
        }

        public void Select( string name )
        {
            Technique = Effect.GetTechnique( name );
        }

        public IEffect Effect
        {
            get { return m_Effect; }
            set
            {
                m_Effect = value;
                m_Technique = null;
            }
        }

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

        private IEffect m_Effect;
        private ITechnique m_Technique;
    }
}
