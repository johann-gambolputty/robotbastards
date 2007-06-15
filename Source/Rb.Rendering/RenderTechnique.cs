using System;
using Rb.Core.Components;

namespace Rb.Rendering
{
	/// <summary>
	/// Stores a collection of RenderPass objects. For each pass, the technique applies it, then renders geometry using a callback
	/// </summary>
	public class RenderTechnique : Node, ITechnique
	{
		#region	Setup

		/// <summary>
		/// Sets the name of this technique to an empty string
		/// </summary>
		public RenderTechnique( )
		{
			m_Name = "";
		}

		/// <summary>
		/// Sets up the name of this technique
		/// </summary>
		public RenderTechnique( string name )
		{
			m_Name = name;
		}

		/// <summary>
		/// Sets up this technique
		/// </summary>
		/// <param name="name">Name of this technique</param>
		/// <param name="passes">Parameter array of passes to Add() to the technique</param>
		public RenderTechnique( string name, params RenderPass[] passes )
		{
			m_Name = name;

			for ( int passIndex = 0; passIndex < passes.Length; ++passIndex )
			{
				Add( passes[ passIndex ] );
			}
		}

		/// <summary>
		/// Adds a new render pass to this technique
		/// </summary>
		/// <param name="pass"> The pass to add </param>
		public void Add( RenderPass pass )
		{
			AddChild( pass );
		}

		/// <summary>
		/// The effect that owns this technique
		/// </summary>
		public RenderEffect Effect
		{
			get { return m_Effect; }
			set { m_Effect = value; }
		}

		#endregion

		#region	ITechnique Members

        /// <summary>
        /// Returns true if this technique is a reasonable substitute for the specified technique
        /// </summary>
        /// <param name="technique">Technique to substitute</param>
        /// <returns>true if this technique can substitute the specified technique</returns>
        public virtual bool IsSubstituteFor( ITechnique technique )
        {
            return ( Name == technique.Name );
        }

		/// <summary>
		/// Applies this technique. Applies a pass, then invokes the render delegate to render stuff, for each pass
        /// </summary>
        /// <param name="context">Rendering context</param>
        /// <param name="renderable">Object to render</param>
		public virtual void Apply( IRenderContext context, IRenderable renderable )
		{
			Begin( );

			if ( Children.Count > 0 )
			{
				foreach ( RenderPass pass in Children )
				{
					pass.Begin( );

					renderable.Render( context );

					pass.End( );
				}
			}
			else
			{
                renderable.Render( context );
			}

			End( );
		}

		/// <summary>
		/// Called by Apply() before any passes are applied
		/// </summary>
		protected virtual void Begin( )
		{
			if ( m_Effect != null )
			{
				m_Effect.Begin( );
			}
		}

		/// <summary>
		/// Called by Apply() after all passes are applied
		/// </summary>
		protected virtual void End( )
		{
			if ( m_Effect != null )
			{
				m_Effect.End( );
			}
		}

		#endregion

		#region INamed Members

		/// <summary>
		/// Access to the name of this technique
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		#endregion

		#region	Private stuff

		private RenderEffect	m_Effect;
		private string			m_Name;

		#endregion
	}
}
