using System;
using System.Collections.Generic;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renders rings around a planet
	/// </summary>
	public class SpherePlanetRingRenderer : IPlanetRingRenderer
	{
		/// <summary>
		/// Default constructor. Loads rendering resources
		/// </summary>
		public SpherePlanetRingRenderer( )
		{
			ITexture2d texture = ( ITexture2d )AssetManager.Instance.Load( "Rings\\ring1.jpg" );
			m_Texture = Graphics.Factory.CreateTexture2dSampler( );
			m_Texture.Texture = texture;

			m_State = Graphics.Factory.CreateRenderState( );
			m_State.Enable2dTextures = true;
		}

		#region IPlanetEnvironmentRenderer Members

		/// <summary>
		/// Gets/sets the planet that the rings are centered on
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set
			{
				m_Planet = ( ISpherePlanet )value;
				if ( m_Planet.PlanetModel.RingModel == null )
				{
					throw new InvalidOperationException( "Ring renderer requires that a valid ring model" );
				}
				m_Planet.PlanetModel.RingModel.ModelChanged += OnRingModelChanged;
			}
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Close in rendering of the planet's rings. Does not render anything.
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_Vertices == null )
			{
				RebuildRingGeometry( );
			}
			m_Texture.Begin( );
			m_State.Begin( );
			m_Vertices.Draw( PrimitiveType.TriStrip );
			m_State.End( );
			m_Texture.End( );
		}

		#endregion

		#region Private Members

		private IRenderState m_State;
		private ITexture2dSampler m_Texture;
		private ISpherePlanet m_Planet;
		private IVertexBuffer m_Vertices;

		/// <summary>
		/// Ring vertex structure
		/// </summary>
		private struct RingVertex
		{
			[VertexField( VertexFieldSemantic.Position )]
			Point3 Position;

			[VertexField( VertexFieldSemantic.Texture0 )]
			Point2 TexCoord;

			public RingVertex( float x, float y, float z, float u, float v )
			{
				Position = new Point3( x, y, z );
				TexCoord = new Point2( u, v );
			}
		}

		/// <summary>
		/// Rebuilds the ring goemetry
		/// </summary>
		private void RebuildRingGeometry( )
		{
			int subdivisionCount = 256;

			List<RingVertex> vertices = new List<RingVertex>( );

			float innerRadius = ( float )m_Planet.SpherePlanetModel.SphereRingModel.InnerRadius.ToAstroRenderUnits;
			float outerRadius = ( float )( m_Planet.SpherePlanetModel.SphereRingModel.InnerRadius + m_Planet.PlanetModel.RingModel.Width ).ToAstroRenderUnits;
			float angle = 0;
			float angleInc = Constants.TwoPi / ( subdivisionCount - 1 );
			bool toggle = false;
			for ( int subdivision = 0; subdivision < subdivisionCount; ++subdivision )
			{
				float x = Functions.Sin( angle );
				float y = 0;
				float z = Functions.Cos( angle );

				vertices.Add( new RingVertex( x * innerRadius, y * innerRadius, z * innerRadius, toggle ? 0 : 1, 0 ) );
				vertices.Add( new RingVertex( x * outerRadius, y * outerRadius, z * outerRadius, toggle ? 0 : 1, 1 ) );

				toggle = !toggle;
				angle += angleInc;
			}

			m_Vertices = Graphics.Factory.CreateVertexBuffer( );
			m_Vertices.Create( vertices.ToArray( ) );
		}

		/// <summary>
		/// Handles the ring model ModelChanged event
		/// </summary>
		private void OnRingModelChanged( object sender, EventArgs args )
		{
			if ( m_Vertices != null )
			{
				m_Vertices.Dispose( );
				m_Vertices = null;
			}
		}

		#endregion

	}
}
