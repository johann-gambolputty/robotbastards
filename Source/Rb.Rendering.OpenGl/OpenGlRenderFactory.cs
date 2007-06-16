using System;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Implementation of RenderFactory
	/// </summary>
	public abstract class OpenGlRenderFactory : Rb.Rendering.RenderFactory
	{
		/// <summary>
		/// Creates a new RenderState object
		/// </summary>
		public override Rb.Rendering.RenderState NewRenderState( )
		{
			return new OpenGlRenderState( );
		}

		/// <summary>
		/// Creates a new Material object
		/// </summary>
		public override Rb.Rendering.Material NewMaterial( )
		{
			return new OpenGlMaterial( );
		}

		/// <summary>
		/// Creates a new Texture2d object
		/// </summary>
		/// <returns></returns>
		public override Rb.Rendering.Texture2d NewTexture2d( )
		{
			return new OpenGlTexture2d( );
		}

		/// <summary>
		/// Creates a new TextureSampler2d object
		/// </summary>
		public override Rb.Rendering.TextureSampler2d NewTextureSampler2d( )
		{
			return new OpenGlTextureSampler2d( );
		}

		/// <summary>
		/// Creates a new RenderFont object
		/// </summary>
		public override Rb.Rendering.RenderFont NewFont( )
		{
			return new OpenGlRenderFont( );
		}

		/// <summary>
		/// Creates a new RenderTarget object
		/// </summary>
		public override Rb.Rendering.RenderTarget NewRenderTarget( )
		{
			return new OpenGlRenderTarget( );
		}

		/// <summary>
		/// Creates a new Renderer object
		/// </summary>
		protected override Rb.Rendering.Renderer NewRenderer( )
		{
			return new OpenGlRenderer( );
		}

		/// <summary>
		/// Creates a new ShapeRenderer object
		/// </summary>
		protected override Rb.Rendering.ShapeRenderer NewShapeRenderer( )
		{
			return new OpenGlShapeRenderer( );
		}
		
		/// <summary>
		/// Creates a new ShaderParameterBindings object
		/// </summary>
		protected override ShaderParameterBindings NewShaderParameterBindings( )
		{
			return new Cg.CgShaderParameterBindings( );
		}
	}
}
