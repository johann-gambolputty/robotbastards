using System;
using System.Collections;

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
				target.Create( 600, 480, System.Drawing.Imaging.PixelFormat.Format32bppArgb, 24, 0 );

				m_RenderTargets[ lightIndex ] = target;
			}
		}

		/// <summary>
		/// Adds a light to the render technique
		/// </summary>
		public void AddLight( Light light )
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
		public void RemoveLight( Light light )
		{
			m_Lights.Remove( light );
		}

		/// <summary>
		/// Applies this technique
		/// </summary>
		public override void Apply( RenderDelegate render )
		{
			Begin( );

			foreach ( Light curLight in m_Lights )
			{
				if ( m_Passes.Count > 0 )
				{
					foreach ( RenderPass pass in m_Passes )
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
			}

			End( );
		}

		private ArrayList		m_Lights		= new ArrayList( );
		private RenderTarget	m_RenderTargets	= new RenderTarget[ MaxLights ];
	}
}
