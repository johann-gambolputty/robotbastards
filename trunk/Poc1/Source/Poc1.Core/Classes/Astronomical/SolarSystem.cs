using System;
using Poc1.Core.Classes.Astronomical.Stars;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical;
using Poc1.Core.Interfaces.Rendering;
using Rb.Core.Components.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical
{
	/// <summary>
	/// Solar system implementation
	/// </summary>
	public class SolarSystem : Composite<IUniObject>, ISolarSystem
	{
		/// <summary>
		/// Default constructor (star backdrop)
		/// </summary>
		public SolarSystem( ) :
			//this( new TexturedStarBox( ) )
			this( new ProceduralStarBox( ) )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="backdrop">Backdrop, rendered before anything else. Can be null</param>
		public SolarSystem( IRenderable backdrop )
		{
			m_Backdrop = backdrop;
		}

		#region IRenderable<IUniRenderContext> Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IUniRenderContext context )
		{
			if ( m_Backdrop != null )
			{
				m_Backdrop.Render( context );
			}
			foreach ( IUniObject uniObject in Components )
			{
				if ( uniObject is IRenderable )
				{
					( ( IRenderable )uniObject ).Render( context );
				}
			}
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( context is IUniRenderContext )
			{
				Render( ( IUniRenderContext )context );
				return;
			}
			throw new NotSupportedException( "Rendering from standard render context is not supported - use an IUniRenderContext" );
		}

		#endregion

		#region Private Members

		private IRenderable m_Backdrop;

		#endregion
	}
}
