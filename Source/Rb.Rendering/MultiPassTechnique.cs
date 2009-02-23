using System;
using System.Collections.Generic;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Stores a collection of IPass objects. For each pass, the technique applies it, then renders geometry using a callback
	/// </summary>
	[Serializable]
	public class MultiPassTechnique : AbstractTechnique
    {
        #region	Setup

        /// <summary>
		/// Default constructor
		/// </summary>
		public MultiPassTechnique( )
		{
		}

		/// <summary>
		/// Name setup constructor
		/// </summary>
		public MultiPassTechnique( string name ) : base( name )
		{
		}

		/// <summary>
		/// Sets up this technique
		/// </summary>
		/// <param name="name">Name of this technique</param>
		/// <param name="passes">Parameter array of passes to Add() to the technique</param>
		public MultiPassTechnique( string name, params IPass[] passes ) : base( name )
		{
			m_Passes.AddRange( passes );
		}

		/// <summary>
		/// Adds a new render pass to this technique
		/// </summary>
		/// <param name="pass"> The pass to add </param>
		public void Add( IPass pass )
		{
			Arguments.CheckNotNull( pass, "pass" );
			m_Passes.Add( pass );
		}

		#endregion

		#region	ITechnique Members

		/// <summary>
		/// Applies this technique. Applies a pass, then invokes the render delegate to render stuff, for each pass
        /// </summary>
        /// <param name="context">Rendering context</param>
        /// <param name="renderable">Object to render</param>
		public override void Apply( IRenderContext context, IRenderable renderable )
		{
			Begin( );

			if ( m_Passes.Count > 0 )
			{
				foreach ( IPass pass in m_Passes )
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
		/// Applies this technique. Applies a pass, then invokes the render delegate to render stuff, for each pass
        /// </summary>
        /// <param name="context">Rendering context</param>
		/// <param name="render">Rendering delegate</param>
		public override void Apply( IRenderContext context, RenderDelegate render )
		{
			Begin( );

			if ( m_Passes.Count > 0 )
			{
				foreach ( IPass pass in m_Passes )
				{
					pass.Begin( );

					render( context );

					pass.End( );
				}
			}
			else
			{
                render( context );
			}

			End( );
		}

		/// <summary>
		/// Called by Apply() before any passes are applied
		/// </summary>
		protected virtual void Begin( )
		{
			if ( Effect != null )
			{
				Effect.Begin( );
			}
		}

		/// <summary>
		/// Called by Apply() after all passes are applied
		/// </summary>
		protected virtual void End( )
		{
			if ( Effect != null )
			{
				Effect.End( );
			}
		}

		#endregion

		#region	Private stuff

		private readonly List<IPass> m_Passes = new List<IPass>( );

		#endregion
	}
}
