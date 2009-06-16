using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using Rb.Assets;
using Poc1.Tools.Waves;

namespace Poc1.PlanarReflectionTest
{
	/// <summary>
	/// Reflective plane
	/// </summary>
	public class ReflectionPlane : AbstractRenderable<ReflectionsRenderContext>
	{
		/// <summary>
		/// Default constructor. Loads rendering assets
		/// </summary>
		public ReflectionPlane( )
		{
			m_Technique = new TechniqueSelector( "PlanarReflectionTest/reflectionTest.cgfx", true, "DefaultTechnique" );
			using ( WaveAnimation animation = ( WaveAnimation )AssetManager.Instance.Load( "PlanarReflectionTest/SimpleWater.waves.bin" ) )
			{
				m_WaveAnimation = new AnimatedTexture2d( animation.ToTextures( true ), 5.0f );
			}
		}

		/// <summary>
		/// Renders the plane
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( ReflectionsRenderContext context )
		{
			if ( context.RenderingReflections )
			{
				return;
			}

			UpdateGeometry( );

			m_WaveAnimation.UpdateAnimation( context.RenderTime );

			m_Technique.Effect.Parameters[ "ReflectionsTexture" ].Set( context.ReflectionsRenderTarget.Texture );
			m_Technique.Effect.Parameters[ "OceanTexture0" ].Set( m_WaveAnimation.SourceTexture );
			m_Technique.Effect.Parameters[ "OceanTexture1" ].Set( m_WaveAnimation.DestinationTexture );
			m_Technique.Effect.Parameters[ "OceanTextureT" ].Set( m_WaveAnimation.LocalT ); 
			
			context.ApplyTechnique( m_Technique, RenderPlane );
		}

		/// <summary>
		/// Creates a reflection matrix for the plane
		/// </summary>
		public Matrix44 CreateReflectionMatrix( )
		{
			return Matrix44.MakeReflectionMatrix( m_Centre, m_YAxis );
		}

		#region Private Members

		private readonly AnimatedTexture2d m_WaveAnimation;
		private readonly TechniqueSelector m_Technique;
		private float m_Size = 80;
		private float m_ZRotation = 0 * Constants.DegreesToRadians;
		private float m_YOffset = -10;
		private Vector3 m_YAxis = Vector3.YAxis;
		private Point3 m_Centre = Point3.Origin;
		private Point3 m_MinXMinZ;
		private Point3 m_MaxXMinZ;
		private Point3 m_MaxXMaxZ;
		private Point3 m_MinXMaxZ;

		/// <summary>
		/// Updates rendering geometry
		/// </summary>
		private void UpdateGeometry( )
		{
			InvariantMatrix44 planeMatrix = InvariantMatrix44.MakeRotationAroundZAxisMatrix( m_ZRotation ) *
				InvariantMatrix44.MakeTranslationMatrix( 0, m_YOffset, 0 );

			m_Centre = planeMatrix * Point3.Origin;

			float hSize = m_Size / 2;
			Vector3 offsetX = planeMatrix.XAxis * hSize;
			Vector3 offsetZ = planeMatrix.ZAxis * hSize;
			m_MinXMinZ = m_Centre - offsetX - offsetZ;
			m_MaxXMinZ = m_Centre + offsetX - offsetZ;
			m_MaxXMaxZ = m_Centre + offsetX + offsetZ;
			m_MinXMaxZ = m_Centre - offsetX + offsetZ;
			m_YAxis = planeMatrix.YAxis;
		}

		/// <summary>
		/// Renders the plane
		/// </summary>
		private void RenderPlane( IRenderContext context )
		{
			Graphics.Draw.BeginPrimitiveList( PrimitiveType.QuadList );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 0 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, m_MinXMinZ );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 0 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, m_MaxXMinZ );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 1 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, m_MaxXMaxZ );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 1 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, m_MinXMaxZ );

			Graphics.Draw.EndPrimitiveList( );
		}

		#endregion
	}
}
