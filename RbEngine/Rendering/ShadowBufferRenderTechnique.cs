using System;
using System.Collections;
using RbEngine.Maths;

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
		/// Override technique
		/// </summary>
		private static RenderTechnique	ms_OverrideTechnique;

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
				RenderEffect effect = ( RenderEffect )Resources.ResourceManager.Inst.Load( "shadowRenderEffect.cgfx" );
				ms_OverrideTechnique = effect.FindTechnique( "DefaultTechnique" );
			}

			try
			{
				for ( int lightIndex = 0; lightIndex < MaxLights; ++lightIndex )
				{
					RenderTarget target = RenderFactory.Inst.NewRenderTarget( );

					//	TODO: Remove hardcoded render target format
					//	target.Create( 512, 512, System.Drawing.Imaging.PixelFormat.Undefined, 24, 0 );
					target.Create( 512, 512, System.Drawing.Imaging.PixelFormat.Format32bppArgb, 24, 0 );

					m_RenderTargets[ lightIndex ] = target;
				}
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( "Failed to create shadow buffer render technique (render target creation failed)", exception );
			}
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
			Begin( );

			Renderer.Inst.PushTransform( Transform.LocalToView );
			Renderer.Inst.PushTransform( Transform.ViewToScreen );

			for ( int lightIndex = 0; lightIndex < m_Lights.Count; ++lightIndex )
			{
				RenderTarget	curTarget	= m_RenderTargets[ lightIndex ];
				SpotLight		curLight	= ( SpotLight )m_Lights[ lightIndex ];

				//	Set up the transform for the current light

				//	TODO: Need proper Y axis
				int		width		= curTarget.Width;
				int		height		= curTarget.Height;
				float	aspectRatio	= ( height == 0 ) ? 1.0f : ( ( float )width / ( float )height );

				Renderer.Inst.SetLookAtTransform( curLight.Position + curLight.Direction, curLight.Position, Vector3.YAxis * -1.0f );	//	NOTE: negative y axis used
				Renderer.Inst.SetPerspectiveProjectionTransform( curLight.ArcDegrees, aspectRatio, 5.0f, 100.0f );	//	TODO: Arbitrary z range

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

				//	Save the depth buffer
				if ( ms_DumpLights )
				{
					curTarget.SaveDepthBuffer( string.Format( "LightView{0}.png", lightIndex ) );
					ms_DumpLights = false;
				}
				//	Stop using the current render target
				curTarget.End( );

			}

			Renderer.Inst.PopTransform( Transform.LocalToView );
			Renderer.Inst.PopTransform( Transform.ViewToScreen );

			End( );
		}

		private static bool		ms_DumpLights	= false;

		private ArrayList		m_Lights		= new ArrayList( );
		private RenderTarget[]	m_RenderTargets	= new RenderTarget[ MaxLights ];
	//	private Camera[]		m_LightCameras	= new Cameras[ MaxLights ];
	}
}
