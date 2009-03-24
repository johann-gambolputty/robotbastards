using System;
using Poc1.Core.Interfaces.Astronomical;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Rendering;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets
{
	/// <summary>
	/// Abstract planet implementation
	/// </summary>
	public class AbstractPlanet : AbstractAstronomicalBody, IPlanet
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="modelFactory">Factory used to create this planet's model</param>
		/// <param name="rendererFactory">Factory used to create this planet's renderer</param>
		public AbstractPlanet( IPlanetModelFactory modelFactory, IPlanetRendererFactory rendererFactory )
		{
			Arguments.CheckNotNull( modelFactory, "modelFactory" );
			Arguments.CheckNotNull( rendererFactory, "rendererFactory" );

			m_Model = modelFactory.Create( this );
			m_Renderer = rendererFactory.Create( this );
		}

		#region IPlanet Members

		/// <summary>
		/// Gets the planet model
		/// </summary>
		public IPlanetModel Model
		{
			get { return m_Model; }
		}

		/// <summary>
		/// Gets the planet renderer
		/// </summary>
		public IPlanetRenderer Renderer
		{
			get { return m_Renderer; }
		}

		#endregion

		#region IRenderable<IUniRenderContext> Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IUniRenderContext context )
		{
			m_Renderer.Render( context );
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
				m_Renderer.Render( context );
				return;
			}
			throw new NotSupportedException( "Rendering from standard render context is not supported - use an IUniRenderContext" );
		}

		#endregion

		#region Private Members

		private readonly IPlanetModel m_Model;
		private readonly IPlanetRenderer m_Renderer;

		#endregion
	}
}
