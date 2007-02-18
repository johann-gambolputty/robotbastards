using System;
using RbEngine.Rendering;

namespace RbOpenGlRendering
{
	/// <summary>
	/// Implementation of RenderFactory
	/// </summary>
	public class OpenGlRenderFactory : RenderFactory
	{

		/// <summary>
		/// Creates a new composite object, from a given base type
		/// </summary>
		/// <param name="baseType">Base type of the composite</param>
		/// <returns>Returns the new composite object. Returns null if the specified composite type is not supported</returns>
		/// <remarks>
		/// Composites are objects that combine render states, vertex buffers, textures and other rendering objects. Examples
		/// are meshes, particle systems, heightfields, and so on.
		/// </remarks>
		/// <example>
		/// <code lang="CSharp">
		/// Mesh newMesh = ( Mesh )RenderFactory.Inst.NewComposite( typeof( Mesh ) );
		/// </code>
		/// </example>
		public override Object				NewComposite( Type baseType )
		{
			return null;
		}


		/// <summary>
		/// Creates a new RenderState object
		/// </summary>
		public override RenderState			NewRenderState( )
		{
			return new OpenGlRenderState( );
		}

		/// <summary>
		/// Creates a new Material object
		/// </summary>
		public override Material			NewMaterial( )
		{
			return new OpenGlMaterial( );
		}

		/// <summary>
		/// Creates a new Texture2d object
		/// </summary>
		/// <returns></returns>
		public override Texture2d			NewTexture2d( )
		{
			return new OpenGlTexture2d( );
		}

		/// <summary>
		/// Creates a new ApplyTexture2d object
		/// </summary>
		public override TextureSampler2d	NewTextureSampler2d( )
		{
			return new OpenGlTextureSampler2d( );
		}

		/// <summary>
		/// Creates a new Renderer object
		/// </summary>
		protected override Renderer			NewRenderer( )
		{
			return new OpenGlRenderer( );
		}

		/// <summary>
		/// Creates a new ShapeRenderer object
		/// </summary>
		protected override ShapeRenderer	NewShapeRenderer( )
		{
			return null;
		}
	}
}
