using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering
{
	/// <summary>
	/// Simple implementation of the IRenderContext interface
	/// </summary>
	class RenderContext : IRenderContext
	{
		#region IRenderContext Members

		/// <summary>
		/// Time that the render occurred in TinyTime clock ticks
		/// </summary>
		public long RenderTime
		{
			get { return m_RenderTime; }
			set { m_RenderTime = value;  }
		}

        /// <summary>
        /// Sets or gets the global technique
        /// </summary>
		public ITechnique GlobalTechnique
		{
            get
            {
                return m_GlobalTechniques.Count == 0 ? null : m_GlobalTechniques[ m_GlobalTechniques.Count - 1 ];
            }
		}


        /// <summary>
        /// Adds a technique to the global technique stack
        /// </summary>
        public void PushGlobalTechnique( ITechnique technique )
        {
            m_GlobalTechniques.Add( technique );
        }

        /// <summary>
        /// Pops a technique from the global technique stack
        /// </summary>
        public void PopGlobalTechnique( )
        {
            m_GlobalTechniques.RemoveAt( m_GlobalTechniques.Count - 1 );
        }


        /// <summary>
        /// Renders a renderable object using a given technique
        /// </summary>
		public void ApplyTechnique( ITechnique technique, IRenderable renderable )
		{
			if ( renderable == null )
			{
				return;
			}

			if ( GlobalTechnique != null )
			{
                if ( technique != null )
			    {
                    if ( technique.Effect != null )
                    {
                        //  GlobalTechnique and technique exist. technique is part of an effect group. Apply a substitute to GlobalTechnique
                        technique.Effect.SubstituteTechnique( GlobalTechnique ).Apply( this, renderable );
                    }
                    else if ( technique.IsSubstituteFor( GlobalTechnique ) )
                    {
                        //  GlobalTechnique and technique exist. technique is a substitute for GlobalTechnique. Apply technique
                        technique.Apply( this, renderable );
                    }
                    else
                    {
                        //  GlobalTechnique and technique exist. technique is not a substitute for GlobalTechnique. Apply GlobalTechnique
                        GlobalTechnique.Apply( this, renderable );
                    }
			    }
                else
                {
                    //  GlobalTechnique exists, technique was null. Apply GlobalTechnique
                    GlobalTechnique.Apply( this, renderable );
                }
			}
			else if ( technique != null )
			{
                //  GlobalTechnique doesn't exist, technique does. Apply technique
                technique.Apply( this, renderable );
			}
            else
			{
                //  Neither GlobalTechnique or technique exist. Render object directly
			    renderable.Render( this );
			}
		}

		#endregion

		#region Private stuff

		private long		        m_RenderTime = 1;
        private List< ITechnique >  m_GlobalTechniques = new List< ITechnique >( );

		#endregion
	}
}
