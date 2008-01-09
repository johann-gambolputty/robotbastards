using System;
using Rb.Core.Assets;
using Rb.Core.Maths;
using Rb.Rendering.Lights;
using Rb.Rendering.Textures;

namespace Rb.Rendering
{
	/// <summary>
	/// Builds up to 4 depth textures for lights
	/// </summary>
	/// <remarks>
	/// This will do nothing, until a LightGroup is associated with the technique, using the ShadowLights property.
	/// Any child techniques this technique has will be applied after the shadow buffer build step.
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
		/// Temporary bodge to switch between depth texture and normal texture for shadow map source
		/// </summary>
		public static bool DepthTextureMethod = true;

		#endregion

		#region	Construction

		/// <summary>
		/// Sets up the builder
		/// </summary>
		/// <exception cref="ApplicationException">Thrown if internal render target creation is not successful</exception>
		public ShadowBufferTechnique( int resX, int resY )
		{
		    Name = GetType( ).Name;

			//	Create a technique that is used to override standard scene rendering techniques (for shadow buffer generation)
			if ( ms_OverrideTechnique == null )
			{
				//	TODO: This should be an embedded resource, directly compiled from a hardcoded string, or something
				byte[] shaderBytes = DepthTextureMethod ? Properties.Resources.shadowMapDepthTexture : Properties.Resources.shadowMapTexture;
				StreamSource source = new StreamSource( shaderBytes, DepthTextureMethod ? "shadowMapDepthTexture.cgfx" : "shadowMapTexture.cgfx" );
				Effect effect = ( Effect )AssetManager.Instance.Load( source );
				ms_OverrideTechnique = effect.FindTechnique( "DefaultTechnique" );
			}

			//	Create render targets for up to MaxLights lights
			try
			{
				for ( int lightIndex = 0; lightIndex < MaxLights; ++lightIndex )
				{
					RenderTarget target = Graphics.Factory.NewRenderTarget( );

					//	TODO: Remove hardcoded render target format
					target.Create( resX, resY, DepthTextureMethod ? TextureFormat.Undefined : TextureFormat.R8G8B8, 16, 0, DepthTextureMethod );

					m_RenderTargets[ lightIndex ] = target;
				}
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( "Failed to create shadow buffer render technique (render target creation failed)", exception );
			}

			//	Create a shader parameter binding to the shadow matrix
			m_ShadowMatrixBinding	= Graphics.ShaderParameterBindings.CreateBinding( "ShadowMatrix", ShaderParameterCustomBinding.ValueType.Matrix, MaxLights );
			m_InvShadowNearZBinding = Graphics.ShaderParameterBindings.CreateBinding( "InvShadowNearZ", ShaderParameterCustomBinding.ValueType.Float32 );
			m_InvShadowZRatio		= Graphics.ShaderParameterBindings.CreateBinding( "InvShadowZRatio", ShaderParameterCustomBinding.ValueType.Float32 );
		}

		#endregion

        #region	Technique Overrides

		/// <summary>
		/// Applies this technique
		/// </summary>
		public override void Apply( IRenderContext context, IRenderable renderable )
		{
			//	Make the shadow depth buffers
			int numBuffers = 0;
			if ( ShadowLights.NumLights > 0 )
			{
				numBuffers = MakeBuffers( renderable, context, ShadowLights );
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
            return DepthTextureMethod ? m_RenderTargets[ index ].DepthTexture : m_RenderTargets[ index ].Texture;
        }

		#endregion

        #region Private stuff

        private static ITechnique               		ms_OverrideTechnique;
        private static bool                     		ms_DumpLights = false;

        private readonly LightGroup                     m_ShadowLights = new LightGroup( );
        private readonly RenderTarget[]                 m_RenderTargets = new RenderTarget[ MaxLights ];
        private float                           		m_NearZ = 1.0f;
        private float                           		m_FarZ = 300.0f;
		private readonly ShaderParameterCustomBinding 	m_ShadowMatrixBinding;
		private readonly ShaderParameterCustomBinding	m_InvShadowNearZBinding;
		private readonly ShaderParameterCustomBinding 	m_InvShadowZRatio;

		private RenderTarget m_DumpTarget;
		private static bool ms_DumpLights2;

        /// <summary>
        /// Makes the shadow buffers
        /// </summary>
        private int MakeBuffers( IRenderable renderable, IRenderContext context, LightGroup lights )
        {
            Graphics.Renderer.PushTransform( Transform.LocalToWorld );
            Graphics.Renderer.PushTransform( Transform.WorldToView );
            Graphics.Renderer.PushTransform( Transform.ViewToScreen );

            Graphics.Renderer.SetTransform( Transform.LocalToWorld, Matrix44.Identity );

            //
            //	Ideal:
            //		- Run through currently active lights
            //		- Query the scene renderables for any intersecting the current light's cone
            //		- Render all objects in that list to current render target
            //		- as before
            //
            //	Issues:
            //		- Needs access to the light manager
            //		- Naive inside-cone query will return the entire environment. Query needs to be aware of environment subsections (e.g. BSP nodes)
            //

            //  Set the global technique to the override technique (this forces all objects to be rendered using the
            //  override technique, unlesss they support a valid substitute technique), and render away...
            context.PushGlobalTechnique( ms_OverrideTechnique );

            //	Set near and far Z plane bindings
            //	NOTE: AP: This could be done once in setup - kept here for now so I can change them on the fly
			m_InvShadowNearZBinding.Set( 1.0f / m_NearZ );
			m_InvShadowZRatio.Set( 1.0f / ( 1.0f / m_FarZ ) );

            int numLights = lights.NumLights > MaxLights ? MaxLights : lights.NumLights;
            int numBuffers = 0;
            for ( int lightIndex = 0; lightIndex < numLights; ++lightIndex )
            {
                RenderTarget curTarget = m_RenderTargets[ numBuffers++ ];
                ISpotLight curLight = ( ISpotLight )lights[ lightIndex ];

                //	TODO: Need proper Y axis
                int width = curTarget.Width;
                int height = curTarget.Height;
                float aspectRatio = ( height == 0 ) ? 1.0f : ( width / ( float )height );

				Graphics.Renderer.SetLookAtTransform( curLight.Position + curLight.Direction, curLight.Position, Vector3.YAxis );
				Graphics.Renderer.SetPerspectiveProjectionTransform( curLight.ArcDegrees, aspectRatio, m_NearZ, m_FarZ );

				if ( ms_DumpLights2 )
				{
					if ( m_DumpTarget == null )
					{
						m_DumpTarget = Graphics.Factory.NewRenderTarget( );
						m_DumpTarget.Create( 512, 512, TextureFormat.A8R8G8B8, 32, 0, false );
					}
					m_DumpTarget.Begin( );
					renderable.Render( context );
					m_DumpTarget.End( );

					string path = string.Format( "ShadowDump.png" );
					path = System.IO.Path.Combine( System.IO.Directory.GetCurrentDirectory( ), path );
					GraphicsLog.Verbose( "Dumping shadow view to \"{0}\"...", path );

					TextureUtils.Save( m_DumpTarget.Texture, path );
					ms_DumpLights2 = false;
				}

                //	Set the current MVP matrix as the shadow transform. This is for after, when the scene is rendered properly
                Matrix44 shadowMat = Graphics.Renderer.GetTransform( Transform.ViewToScreen ) * Graphics.Renderer.GetTransform( Transform.WorldToView );
                m_ShadowMatrixBinding.SetAt( lightIndex, shadowMat );

                //	Set up the render target for the light
                curTarget.Begin( );
                Graphics.Renderer.ClearColour( System.Drawing.Color.Black );  //  NOTE: AP: Unecessary if depth texture is being used
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
                    if ( DepthTextureMethod )
                    {
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
