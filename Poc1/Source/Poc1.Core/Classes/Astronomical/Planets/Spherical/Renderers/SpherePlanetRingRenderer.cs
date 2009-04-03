using System.Collections.Generic;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
using Poc1.Core.Interfaces.Rendering;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Planetary ring renderer for spherical planets
	/// </summary>
	/// <seealso cref="ISpherePlanetRingModel"/>
	public class SpherePlanetRingRenderer : SpherePlanetEnvironmentRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetRingRenderer( )
		{
			ITexture2d texture = ( ITexture2d )AssetManager.Instance.Load( "Rings\\ring1.jpg" );
			m_Texture = Graphics.Factory.CreateTexture2dSampler( );
			m_Texture.Texture = texture;

			m_State = Graphics.Factory.CreateRenderState( );
			m_State.Enable2dTextures = true;	
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
			if ( context.CurrentPass != UniRenderPass.FarObjects )
			{
				return;
			}
			if ( m_Vertices == null || HasRingModelChanged( ) )
			{
				if ( !RebuildRingGeometry( ) )
				{
					return;
				}
			}
			m_Texture.Begin( );
			m_State.Begin( );
			m_Vertices.Draw( PrimitiveType.TriStrip );
			m_State.End( );
			m_Texture.End( );
		}


		#region Private Members

		/// <summary>
		/// Destroys current vertex buffer
		/// </summary>
		private void DestroyVertices( )
		{
			if ( m_Vertices != null )
			{
				m_Vertices.Dispose( );
				m_Vertices = null;
			}
		}

		private IRenderState m_State;
		private ITexture2dSampler m_Texture;
		private IVertexBuffer m_Vertices;
		private Units.Metres m_BuildInnerRadius = new Units.Metres( 0 );
		private Units.Metres m_BuildWidth = new Units.Metres( 0 );

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
		/// Returns true if the model parameters used to create the ring renderable geometry have changed
		/// </summary>
		private bool HasRingModelChanged( )
		{
			ISpherePlanetRingModel model = Planet.Model.GetModel<ISpherePlanetRingModel>( );
			if ( model == null )
			{
				return false;
			}
			return ( m_BuildInnerRadius != model.InnerRadius ) || ( m_BuildWidth != model.Width );
		}

		/// <summary>
		/// Rebuilds the ring goemetry
		/// </summary>
		private bool RebuildRingGeometry( )
		{
			DestroyVertices( );
			ISpherePlanetRingModel model = Planet.Model.GetModel<ISpherePlanetRingModel>( );
			if ( model == null )
			{
				return false;
			}

			int subdivisionCount = 256;

			List<RingVertex> vertices = new List<RingVertex>( );

			m_BuildInnerRadius = model.InnerRadius;
			m_BuildWidth = model.Width;

			float innerRadius = ( float )model.InnerRadius.ToAstroRenderUnits;
			float outerRadius = ( float )( model.InnerRadius + model.Width ).ToAstroRenderUnits;
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

			return true;
		}

		#endregion
	}

}
