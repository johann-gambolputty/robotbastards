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
		public const int	MaxLights = 4;

		/// <summary>
		/// Sets up the technique
		/// </summary>
		public ShadowBufferRenderTechnique( )
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
				//	Set up the transform for the current light
				SpotLight curLight = ( SpotLight )m_Lights[ lightIndex ];

				//	TODO: Need proper Y axis
				int		width		= m_RenderTargets[ lightIndex ].Width;
				int		height		= m_RenderTargets[ lightIndex ].Height;
				float	aspectRatio	= ( height == 0 ) ? 1.0f : ( ( float )width / ( float )height );

				Renderer.Inst.SetLookAtTransform( curLight.Position + curLight.Direction, curLight.Position, Vector3.YAxis );
				Renderer.Inst.SetPerspectiveProjectionTransform( curLight.ArcDegrees, aspectRatio, 0.001f, 100.0f );	//	TODO: Arbitrary z range

				//	Set up the render target for the light
				m_RenderTargets[ lightIndex ].Begin( );

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

				//	Stop using the current render target
				m_RenderTargets[ lightIndex ].End( );

				if ( ms_DumpLights )
				{
					m_RenderTargets[ lightIndex ].Texture.Save( string.Format( "LightView{0}.png", lightIndex ) );
					ms_DumpLights = false;
				}
			}

			Renderer.Inst.PopTransform( Transform.LocalToView );
			Renderer.Inst.PopTransform( Transform.ViewToScreen );

			End( );
		}

		private static bool		ms_DumpLights	= true;

		private ArrayList		m_Lights		= new ArrayList( );
		private RenderTarget[]	m_RenderTargets	= new RenderTarget[ MaxLights ];
	//	private Camera[]		m_LightCameras	= new Cameras[ MaxLights ];
	}
}
