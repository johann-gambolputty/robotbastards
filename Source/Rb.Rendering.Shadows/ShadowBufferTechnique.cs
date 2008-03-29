using System;
using System.Collections.Generic;
using Rb.Assets;
using Rb.Assets.Base;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Lights;
using Rb.Rendering.Textures;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Lights;
using Rb.Rendering.Shadows.Properties;
using Rb.World;
using Rb.World.Services;

namespace Rb.Rendering.Shadows
{
	/// <summary>
	/// Builds up to 4 depth textures for lights
	/// </summary>
	/// <remarks>
	/// This will do nothing, until a LightGroup is associated with the technique, using the ShadowLights property.
	/// Any child techniques this technique has will be applied after the shadow buffer build step.
	/// A good wiki on shadow mapping is here: http://www.ogre3d.org/wiki/index.php/Custom_Shadow_Mapping (where
	/// the projection ratio formula came from)
	/// </remarks>
	public class ShadowBufferTechnique : Technique, IDisposable
	{
		#region	Technique properties

		/// <summary>
		/// Maximum number of shadow casting lights
		/// </summary>
		public const int MaxLights = 4;

		/// <summary>
		/// Sets the distance of the near z plane
		/// </summary>
		public float NearZ
		{
			get { return m_NearZ; }
			set { m_NearZ = value; }
		}

		/// <summary>
		/// Sets the distance of the far z plane
		/// </summary>
		public float FarZ
		{
			get { return m_FarZ; }
			set { m_FarZ = value; }
		}

		/// <summary>
		/// Gets the technique that the shadow buffer render technique uses to override rendered objects' techniques
		/// </summary>
		public static ITechnique OverrideTechnique
		{
			get { return ms_OverrideTechnique; }
		}

		/// <summary>
		/// The light group this technique uses to get shadow lights from
		/// </summary>
		public LightGroup ShadowLights
		{
			get { return m_ShadowLights; }
		}

		/// <summary>
		/// Dumps the shadow buffer to a texture
		/// </summary>
		public static bool DumpShadowBuffer
		{
			get { return ms_DumpLights; }
			set { ms_DumpLights = value; }
		}

		#endregion

		#region	Construction

		/// <summary>
		/// Sets up the builder
		/// </summary>
		/// <param name="resX">Shadow map resolution width</param>
		/// <param name="resY">Shadow map resolution height</param>
		/// <param name="bitDepth">Shadow map depth pixel resolution</param>
		/// <param name="useDepthTexture">If true, then depth textures are used. Otherwise, colour texturing is used</param>
		/// <exception cref="ApplicationException">Thrown if internal render target creation is not successful</exception>
		public ShadowBufferTechnique( int resX, int resY, int bitDepth, bool useDepthTexture )
		{
			Name = GetType( ).Name;
			m_DepthTextureUsed = useDepthTexture;

			//	Create a technique that is used to override standard scene rendering techniques (for shadow buffer generation)
			if ( ms_OverrideTechnique == null )
			{
				byte[] shaderBytes = m_DepthTextureUsed ? Resources.DefaultShadowMapDepthTexture : Resources.DefaultShadowMapTexture;
				StreamSource source = new StreamSource( shaderBytes, m_DepthTextureUsed ? "shadowMapDepthTexture.cgfx" : "shadowMapTexture.cgfx" );
				Effect effect = ( Effect )AssetManager.Instance.Load( source );
				ms_OverrideTechnique = effect.Techniques[ "DefaultTechnique" ];
			}

			//	Create render targets for up to MaxLights lights
			try
			{
				for ( int lightIndex = 0; lightIndex < MaxLights; ++lightIndex )
				{
					IRenderTarget target = Graphics.Factory.CreateRenderTarget( );

					//	TODO: Remove hardcoded render target format
					target.Create( resX, resY, m_DepthTextureUsed ? TextureFormat.Undefined : TextureFormat.R8G8B8, bitDepth, 0, m_DepthTextureUsed );

					m_RenderTargets[ lightIndex ] = target;
				}
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( "Failed to create shadow buffer render technique (render target creation failed)", exception );
			}

			//	Create a shader parameter binding to the shadow matrix
			m_InvShadowNearZBinding	= Graphics.EffectDataSources.CreateValueDataSourceForNamedParameter<float>( "InvShadowNearZ" );
			m_InvShadowZRatio		= Graphics.EffectDataSources.CreateValueDataSourceForNamedParameter<float>( "InvShadowZRatio" );
			m_ShadowMatrixBinding	= Graphics.EffectDataSources.CreateValueDataSourceForNamedParameter<Matrix44[]>( "ShadowMatrix" );
			m_ShadowMatrixBinding.Value = new Matrix44[ MaxLights ];
		}

		#endregion

		#region	Technique Overrides

		/// <summary>
		/// Applies this technique
		/// </summary>
		public override void Apply( IRenderContext context, IRenderable renderable )
		{
			BuildLightGroup( ( Scene )renderable );

			//	Make the shadow depth buffers
			int numBuffers = 0;
			if ( ShadowLights.NumLights > 0 )
			{
				numBuffers = MakeBuffers( renderable, context, ShadowLights.Lights );
			}

			//	Apply all shadow depth buffer textures
			for ( int bufferIndex = 0; bufferIndex < numBuffers; ++bufferIndex )
			{
				Graphics.Renderer.BindTexture( GetDepthTexture( bufferIndex ) );
			}

			//	Apply all child techniques
			if ( Children.Count > 0 )
			{
				foreach ( Object childObj in Children )
				{
					if ( childObj is ITechnique )
					{
						( ( ITechnique )childObj ).Apply( context, renderable );
					}
				}
			}
			else
			{
				renderable.Render( context );
			}

			//	Unbind all the shadow depth buffer textures
			for ( int bufferIndex = 0; bufferIndex < numBuffers; ++bufferIndex )
			{
				Graphics.Renderer.UnbindTexture( GetDepthTexture( bufferIndex ) );
			}
		}

		/// <summary>
		/// Gets the depth texture associated with a given buffer index
		/// </summary>
		private ITexture2d GetDepthTexture( int index )
		{
			return m_DepthTextureUsed ? m_RenderTargets[ index ].DepthTexture : m_RenderTargets[ index ].Texture;
		}

		#endregion

		#region Private stuff

		private static ITechnique							ms_OverrideTechnique;
		private static bool									ms_DumpLights = false;

		private readonly LightGroup							m_ShadowLights = new LightGroup( );
		private readonly IRenderTarget[]					m_RenderTargets = new IRenderTarget[ MaxLights ];
		private float 										m_NearZ = 1.0f;
		private float 										m_FarZ = 30.0f;
		private readonly IEffectValueDataSource<Matrix44[]> m_ShadowMatrixBinding;
		private readonly IEffectValueDataSource<float>		m_InvShadowNearZBinding;
		private readonly IEffectValueDataSource<float>		m_InvShadowZRatio;
		private readonly bool								m_DepthTextureUsed;

		private void BuildLightGroup( Scene scene )
		{
            //  TODO: AP: Shouldn't have to do this each frame
			ILightingService service = scene.GetService< ILightingService >( );
            List< ILight > lights = new List< ILight >( );

            foreach ( ILight light in service.Lights )
            {
                //  TODO: AP: Proper volume/coverage determination
                if ( ( light.CastsShadows ) && ( light is ISpotLight ) )
                {
                    lights.Add( light );
                }
            }
            ShadowLights.Lights = lights.ToArray( );
		}

		/// <summary>
		/// Makes the shadow buffers
		/// </summary>
		private int MakeBuffers( IRenderable renderable, IRenderContext context, ILight[] lights )
		{
			Graphics.Renderer.PushTransform( Transform.LocalToWorld );
			Graphics.Renderer.PushTransform( Transform.WorldToView );
			Graphics.Renderer.PushTransform( Transform.ViewToScreen );

			Graphics.Renderer.SetTransform( Transform.LocalToWorld, Matrix44.Identity );

			//  Set the global technique to the override technique (this forces all objects to be rendered using the
			//  override technique, unlesss they support a valid substitute technique), and render away...
			context.PushGlobalTechnique( ms_OverrideTechnique );

			//	Set near and far Z plane bindings
			float nearZ = m_NearZ;
			float farZ = m_FarZ;
			m_InvShadowNearZBinding.Value = ( 1.0f / nearZ );
			m_InvShadowZRatio.Value = ( 1.0f / ( 1.0f / nearZ - 1.0f / farZ ) );

			int numLights = lights.Length > MaxLights ? MaxLights : lights.Length;
			int numBuffers = 0;
			for ( int lightIndex = 0; lightIndex < numLights; ++lightIndex )
			{
				IRenderTarget curTarget = m_RenderTargets[ numBuffers++ ];
				ISpotLight curLight = ( ISpotLight )lights[ lightIndex ];

				//	TODO: Need proper Y axis
				int width = curTarget.Width;
				int height = curTarget.Height;
				float aspectRatio = ( height == 0 ) ? 1.0f : ( width / ( float )height );

				Graphics.Renderer.SetLookAtTransform( curLight.Position + curLight.Direction, curLight.Position, Vector3.YAxis );
				Graphics.Renderer.SetPerspectiveProjectionTransform( curLight.ArcDegrees, aspectRatio, m_NearZ, m_FarZ );

				//	Set the current MVP matrix as the shadow transform. This is for after, when the scene is rendered properly
				Matrix44 shadowMat = Graphics.Renderer.GetTransform( Transform.ViewToScreen ) * Graphics.Renderer.GetTransform( Transform.WorldToView );
				m_ShadowMatrixBinding.Value[ lightIndex ] = shadowMat;

				//	Set up the render target for the light
				curTarget.Begin( );

				if ( !m_DepthTextureUsed )
				{
					Graphics.Renderer.ClearColour( System.Drawing.Color.Black );
				}
				Graphics.Renderer.ClearDepth( 1.0f );

				renderable.Render( context );

				//	Stop using the current render target
				curTarget.End( );


				//	Save the depth buffer
				if ( ms_DumpLights )
				{
					string path = string.Format( "ShadowBuffer{0}.png", numBuffers - 1 );
					path = System.IO.Path.Combine( System.IO.Directory.GetCurrentDirectory( ), path );
					GraphicsLog.Verbose( "Dumping shadow buffer image to \"{0}\"...", path );
					if ( m_DepthTextureUsed )
					{
						//	curTarget.SaveDepthBuffer( path );
						TextureUtils.Save( curTarget.DepthTexture, path );
					}
					else
					{
						TextureUtils.Save( curTarget.Texture, path );
					}
					GraphicsLog.Verbose( "...Done" );
				}
			}

			ms_DumpLights = false;

			context.PopGlobalTechnique( );

			Graphics.Renderer.PopTransform( Transform.LocalToWorld );
			Graphics.Renderer.PopTransform( Transform.WorldToView );
			Graphics.Renderer.PopTransform( Transform.ViewToScreen );

			return numBuffers;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Cleans up technique resources
		/// </summary>
		public void Dispose( )
		{
			for ( int targetIndex = 0; targetIndex < m_RenderTargets.Length; ++targetIndex )
			{
				if ( m_RenderTargets[ targetIndex ] != null )
				{
					m_RenderTargets[ targetIndex ].Dispose( );
					m_RenderTargets[ targetIndex ] = null;
				}
			}
		}

		#endregion
	}
}
