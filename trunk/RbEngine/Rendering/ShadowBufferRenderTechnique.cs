using System;
using System.Collections;
using RbEngine.Maths;

using Tao.OpenGl;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Render technique for shadows
	/// </summary>
	public class ShadowBufferRenderTechnique : RenderTechnique
	{
		/// <summary>
		/// Maximum number of shadow casting lights
		/// </summary>
		public const int				MaxLights = 4;

		/// <summary>
		/// Gets the technique that the shadow buffer render technique uses to override rendered objects' techniques
		/// </summary>
		public static RenderTechnique	OverrideTechnique
		{
			get
			{
				return ms_OverrideTechnique;
			}
		}

		/// <summary>
		/// Temporary bodge to switch between depth texture and normal texture for shadow map source
		/// </summary>
		public static bool	DepthTextureMethod = true;

		/// <summary>
		/// Sets up the technique
		/// </summary>
		/// <exception cref="ApplicationException">Thrown if internal render target creation is not successful</exception>
		public ShadowBufferRenderTechnique( )
		{

			if ( ms_OverrideTechnique == null )
			{
				/*
				ms_OverrideTechnique = 
					new RenderTechnique
					(
						"ShadowBufferRender",
						new RenderPass
						(
							RenderFactory.Inst.NewRenderState( )
								.DisableLighting( )
								.DisableCap( RenderStateFlag.Texture2d )
								.SetColour( System.Drawing.Color.White )
								.DisableCap( RenderStateFlag.DepthTest )
								.DisableCap( RenderStateFlag.CullFrontFaces | RenderStateFlag.CullBackFaces )
						)
					);
				*/
				//	TODO: This should be an embedded resource, directly compiled from a hardcoded string, or something
				RenderEffect effect = ( RenderEffect )Resources.ResourceManager.Inst.Load( DepthTextureMethod ? "shadowMapDepthTexture.cgfx" : "shadowMapTexture.cgfx" );
				ms_OverrideTechnique = effect.FindTechnique( "DefaultTechnique" );
			}

			try
			{
				for ( int lightIndex = 0; lightIndex < MaxLights; ++lightIndex )
				{
					RenderTarget target = RenderFactory.Inst.NewRenderTarget( );

					//	TODO: Remove hardcoded render target format
					target.Create( 512, 512, DepthTextureMethod ? TextureFormat.Undefined : TextureFormat.R8G8B8, 16, 0, DepthTextureMethod );

					m_RenderTargets[ lightIndex ] = target;
				}
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( "Failed to create shadow buffer render technique (render target creation failed)", exception );
			}
			
			//	Create a shader parameter binding to the shadow matrix
			m_ShadowMatrixBinding = ShaderParameterBindings.Inst.CreateBinding( "ShadowMatrix", ShaderParameterCustomBinding.ValueType.Matrix );

		}

		/// <summary>
		/// Adds a light to the render technique
		/// </summary>
		public void AddLight( SpotLight light )
		{
			if ( m_Lights.Count >= MaxLights )
			{
				throw new ApplicationException( "Tried to add too many lights to the shadow render technique" );
			}
			m_Lights.Add( light );
		}

		/// <summary>
		/// Removes a light from the render technique
		/// </summary>
		public void RemoveLight( SpotLight light )
		{
			m_Lights.Remove( light );
		}

		/// <summary>
		/// Applies this technique
		/// </summary>
		public override void Apply( RenderDelegate render )
		{
			//	No lights? Then just render...
			if ( m_Lights.Count == 0 )
			{
				render( );
				return;
			}

			Begin( );

			Renderer.Inst.PushTransform( Transform.LocalToWorld );
			Renderer.Inst.PushTransform( Transform.WorldToView );
			Renderer.Inst.PushTransform( Transform.ViewToScreen );

			Renderer.Inst.SetTransform( Transform.LocalToWorld, Matrix44.Identity );

			for ( int lightIndex = 0; lightIndex < m_Lights.Count; ++lightIndex )
			{
				RenderTarget	curTarget	= m_RenderTargets[ lightIndex ];
				SpotLight		curLight	= ( SpotLight )m_Lights[ lightIndex ];

				//	Set up the transform for the current light

				//	TODO: Need proper Y axis
				int		width		= curTarget.Width;
				int		height		= curTarget.Height;
				float	aspectRatio	= ( height == 0 ) ? 1.0f : ( ( float )width / ( float )height );

				Renderer.Inst.SetLookAtTransform( curLight.Position + curLight.Direction, curLight.Position, Vector3.YAxis );
				Renderer.Inst.SetPerspectiveProjectionTransform( curLight.ArcDegrees, aspectRatio, 2.0f, 500.0f );	//	TODO: Arbitrary z range

				//	Set the current MVP matrix as the shadow transform. This is for after, when the scene is rendered properly
				m_ShadowMatrixBinding.Set( Renderer.Inst.GetTransform( Transform.ViewToScreen ) * Renderer.Inst.GetTransform( Transform.WorldToView ) );

				//	Set up the render target for the light
				curTarget.Begin( );
				Rendering.Renderer.Inst.ClearColour( System.Drawing.Color.Black );
				Rendering.Renderer.Inst.ClearDepth( 1.0f );

				RenderTechnique.Override = ms_OverrideTechnique;

				//	Render the scene
				if ( ( Children != null ) && ( Children.Count > 0 ) )
				{
					foreach ( RenderPass pass in Children )
					{
						pass.Begin( );
						render( );
						pass.End( );
					}
				}
				else
				{
					render( );
				}

				RenderTechnique.Override = null;

				//	Stop using the current render target
				curTarget.End( );

				//	Save the depth buffer
				if ( ms_DumpLights )
				{
					if ( DepthTextureMethod )
					{
						curTarget.DepthTexture.Save( string.Format( "LightView{0}.png", lightIndex ) );
					}
					else
					{
						curTarget.Texture.Save( string.Format( "LightView{0}.png", lightIndex ) );
					}
					ms_DumpLights = false;
				}
			}

			Renderer.Inst.PopTransform( Transform.LocalToWorld );
			Renderer.Inst.PopTransform( Transform.WorldToView );
			Renderer.Inst.PopTransform( Transform.ViewToScreen );

			End( );

			//	Apply all light textures
			for ( int lightIndex = 0; lightIndex < m_Lights.Count; ++lightIndex )
			{
				Renderer.Inst.BindTexture( lightIndex, DepthTextureMethod ? m_RenderTargets[ lightIndex ].DepthTexture : m_RenderTargets[ lightIndex ].Texture );
			}

			//	Render the scene normally
			render( );

			//	Stops applying all light textures
			for ( int lightIndex = 0; lightIndex < m_Lights.Count; ++lightIndex )
			{
				Renderer.Inst.UnbindTexture( lightIndex );
			}
		}

		private static RenderTechnique			ms_OverrideTechnique;
		private static bool						ms_DumpLights	= true;

		private ArrayList						m_Lights		= new ArrayList( );
		private RenderTarget[]					m_RenderTargets	= new RenderTarget[ MaxLights ];
		private ShaderParameterCustomBinding	m_ShadowMatrixBinding;
	}
}
