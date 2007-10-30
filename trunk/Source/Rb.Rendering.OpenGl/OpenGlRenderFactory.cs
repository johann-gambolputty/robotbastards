
namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Implementation of RenderFactory
	/// </summary>
	public abstract class OpenGlRenderFactory : RenderFactory
	{
		/// <summary>
		/// Gets the name of the API supported by this render factory
		/// </summary>
		public override string ApiName
		{
			get { return "OpenGl"; }
		}

	    /// <summary>
		/// Creates a new RenderState object
		/// </summary>
		public override RenderState NewRenderState( )
		{
			return new OpenGlRenderState( );
		}

		/// <summary>
		/// Creates a new Material object
		/// </summary>
		public override Material NewMaterial( )
		{
			return new OpenGlMaterial( );
		}

		/// <summary>
		/// Creates a new Texture2d object
		/// </summary>
		/// <returns></returns>
		public override ITexture2d NewTexture2d( )
		{
			return new OpenGlTexture2d( );
		}

		/// <summary>
		/// Creates a new TextureSampler2d object
		/// </summary>
		public override TextureSampler2d NewTextureSampler2d( )
		{
			return new OpenGlTextureSampler2d( );
		}

		/// <summary>
		/// Creates a new RenderFont object
		/// </summary>
		public override RenderFont NewFont( )
		{
			return new OpenGlRenderFont( );
		}

		/// <summary>
		/// Creates a new RenderTarget object
		/// </summary>
		public override RenderTarget NewRenderTarget( )
		{
			return new OpenGlRenderTarget( );
		}

		/// <summary>
		/// Creates a new IVertexBuffer object
		/// </summary>
		public override IVertexBuffer NewVertexBuffer( VertexBufferData bufferData )
		{
			return new OpenGlVertexBuffer( bufferData );
		}


		/// <summary>
		/// Creates a new Renderer object
		/// </summary>
		protected override Renderer NewRenderer( )
		{
			return new OpenGlRenderer( );
		}

		/// <summary>
		/// Creates a new ShapeRenderer object
		/// </summary>
		protected override ShapeRenderer NewShapeRenderer( )
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

		/// <summary>
		/// Creates a new Draw object
		/// </summary>
		protected override Draw NewDraw( )
		{
			return new OpenGlDraw( );
		}
	}
}
