using System;
using Poc1.Core.Classes.Profiling;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
using Poc1.Core.Interfaces.Rendering;
using Poc1.Tools.Waves;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renders a reflective ocean
	/// </summary>
	public class SpherePlanetReflectiveOceanRenderer : SpherePlanetEnvironmentRenderer, IPlanetReflectiveOceanRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetReflectiveOceanRenderer( ) :
			this( "Effects/Planets/sphereReflectiveOcean.cgfx", "Ocean/SimpleWater.waves.bin" )
		{
			
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetReflectiveOceanRenderer( string effectPath, string waterAnimationPath )
		{
			Arguments.CheckNotNullOrEmpty( effectPath, "effectPath" );
			Arguments.CheckNotNull( waterAnimationPath, "waterAnimationPath" );

			m_Effect = new EffectAssetHandle( effectPath, true );
			m_Technique = new TechniqueSelector( m_Effect, "DefaultTechnique" );

			using ( WaveAnimation animation = ( WaveAnimation )AssetManager.Instance.Load( waterAnimationPath ) )
			{
				m_WaveAnimation = new AnimatedTexture2d( animation.ToTextures( true ), 5.0f );
			}
		}


		#region IRenderable<IUniRenderContext> Members

		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
		//	if ( context.CurrentPass != UniRenderPass.CloseObjects )
			if ( context.RenderFarObjects )
			{
				return;
			}

			//	Get ocean model
			IPlanetOceanModel model = Planet.Model.GetModel<IPlanetOceanModel>( );
			if ( model == null )
			{
				return;
			}

			//	Create ocean geometry
			Units.Metres expectedGeometryRadius = Planet.Model.Radius.ToMetres + model.SeaLevel;
			if ( ( m_FarGeometry == null ) ||  ( expectedGeometryRadius != m_GeometryRadius ) )
			{
				m_FarGeometry = CreateGeoSphere( expectedGeometryRadius, 40 );
			}

			//	Render ocean
			GameProfiles.Game.Rendering.PlanetRendering.OceanRendering.Begin( );

			m_WaveAnimation.UpdateAnimation( context.RenderTime );

			SetupOceanTexturesInEffect( m_Technique.Effect );
			context.ApplyTechnique( m_Technique, m_FarGeometry );

			GameProfiles.Game.Rendering.PlanetRendering.OceanRendering.End( );
		}

		#endregion

		#region Private Members

		private IRenderable m_FarGeometry;
		private Units.Metres m_GeometryRadius;
		private TechniqueSelector m_Technique;
		private readonly EffectAssetHandle m_Effect;
		private AnimatedTexture2d m_WaveAnimation;

		/// <summary>
		/// Sets the ocean textures in an effect
		/// </summary>
		private void SetupOceanTexturesInEffect( IEffect effect )
		{
			effect.Parameters[ "OceanTexture0" ].Set( m_WaveAnimation.SourceTexture );
			effect.Parameters[ "OceanTexture1" ].Set( m_WaveAnimation.DestinationTexture );
			effect.Parameters[ "OceanTextureT" ].Set( m_WaveAnimation.LocalT );
		}

		/// <summary>
		/// Creates a geo sphere
		/// </summary>
		private IRenderable CreateGeoSphere( Units.Metres radius, int subdivisions )
		{
			m_GeometryRadius = radius;
			Vector3[] sideNormals = new Vector3[ 6 ]
				{
					new Vector3( 1, 0, 0 ), new Vector3( 0, 1, 0 ), new Vector3( 0, 0, 1 ),
					new Vector3( -1, 0, 0 ), new Vector3( 0, -1, 0 ), new Vector3( 0, 0, -1 )
				};
			VertexBufferFormat format = new VertexBufferFormat( );
			format.Add<float>( VertexFieldSemantic.Position, 3 );
			format.Add<float>( VertexFieldSemantic.Normal, 3 );
			format.Add<float>( VertexFieldSemantic.Texture0, 2 );

			float renderRadius = radius.ToRenderUnits;
			float modelSideLength = 10.0f;
			float uvMul = 1.0f / subdivisions;

			VertexBufferBuilder vbBuilder = new VertexBufferBuilder( format );
			for ( int sideNormalIndex = 0; sideNormalIndex < sideNormals.Length; ++sideNormalIndex )
			{
				Vector3 sideNormal = sideNormals[ sideNormalIndex ];
				Vector3 xAxis = sideNormals[ ( sideNormalIndex + 1 ) % sideNormals.Length ];
				Vector3 yAxis = sideNormals[ ( sideNormalIndex + 2 ) % sideNormals.Length ];
				Vector3 xStride = xAxis * modelSideLength;
				Vector3 yStride = yAxis * modelSideLength;
				Vector3 xStep = xStride / subdivisions;
				Vector3 yStep = yStride / subdivisions;

				Point3 mid = Point3.Origin + sideNormal * modelSideLength / 2;

				Point3 topLeft = mid - ( xStride / 2 ) - ( yStride / 2 );
				Point3 sidePos = topLeft;
				for ( int y = 0; y < subdivisions; ++y )
				{
					Point3 curPos = sidePos;
					float v = y * uvMul;
					float nV = ( y + 1 ) * uvMul;

					for ( int x = 0; x < subdivisions; ++x )
					{
						Vector3 sphereNormal = curPos.ToVector3( ).MakeNormal( );
						Vector3 sphereNxNormal = ( curPos + xStep ).ToVector3( ).MakeNormal( );
						Vector3 sphereNyNormal = ( curPos + yStep ).ToVector3( ).MakeNormal( );
						Vector3 sphereNxNyNormal = ( curPos + xStep + yStep ).ToVector3( ).MakeNormal( );

						Point3 spherePos = ( sphereNormal * renderRadius ).ToPoint3( );
						Point3 sphereNxPos = ( sphereNxNormal * renderRadius ).ToPoint3( );
						Point3 sphereNyPos = ( sphereNyNormal * renderRadius ).ToPoint3( );
						Point3 sphereNxNyPos = ( sphereNxNyNormal * renderRadius ).ToPoint3( );

						float u = x * uvMul;
						float nU = ( x + 1 ) * uvMul;

						vbBuilder.Add( VertexFieldSemantic.Position, spherePos );
						vbBuilder.Add( VertexFieldSemantic.Normal, sphereNormal );
						vbBuilder.Add( VertexFieldSemantic.Texture0, u, v );

						vbBuilder.Add( VertexFieldSemantic.Position, sphereNxPos );
						vbBuilder.Add( VertexFieldSemantic.Normal, sphereNxNormal );
						vbBuilder.Add( VertexFieldSemantic.Texture0, nU, v );

						vbBuilder.Add( VertexFieldSemantic.Position, sphereNyPos );
						vbBuilder.Add( VertexFieldSemantic.Normal, sphereNyNormal );
						vbBuilder.Add( VertexFieldSemantic.Texture0, u, nV );


						vbBuilder.Add( VertexFieldSemantic.Position, sphereNxPos );
						vbBuilder.Add( VertexFieldSemantic.Normal, sphereNxNormal );
						vbBuilder.Add( VertexFieldSemantic.Texture0, nU, v );

						vbBuilder.Add( VertexFieldSemantic.Position, sphereNxNyPos );
						vbBuilder.Add( VertexFieldSemantic.Normal, sphereNxNyNormal );
						vbBuilder.Add( VertexFieldSemantic.Texture0, nU, nV );

						vbBuilder.Add( VertexFieldSemantic.Position, sphereNyPos );
						vbBuilder.Add( VertexFieldSemantic.Normal, sphereNyNormal );
						vbBuilder.Add( VertexFieldSemantic.Texture0, u, nV );

						curPos += xStep;
					}
					sidePos += yStep;
				}
			}

			return new VertexBufferRenderer( vbBuilder.Build( ), PrimitiveType.TriList );
		}

		#endregion

		#region IPlanetReflectiveOceanRenderer Members


		/// <summary>
		/// Gets the tangent plane at the specified position
		/// </summary>
		public Plane3 GetTangentPlaneUnderPoint( UniPoint3 pos, out Point3 pointOnPlane )
		{
			Vector3 normal = Planet.Transform.Position.VectorTo( pos ).MakeNormal( );
			Vector3 vec = normal * ( Planet.Model.Radius + m_GeometryRadius ).ToRenderUnits;
			pointOnPlane = vec.ToPoint3( );
			return new Plane3( pointOnPlane, normal );
		}

		/// <summary>
		/// Gets the tangent space matrix at the specified position
		/// </summary>
		public Matrix44 GetTangentSpaceUnderPoint( UniPoint3 pos )
		{
			throw new NotImplementedException( );
		}

		#endregion
	}
}
