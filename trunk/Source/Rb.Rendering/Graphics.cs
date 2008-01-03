using System;

namespace Rb.Rendering
{
	/// <summary>
	/// Graphics singletons
	/// </summary>
	public class Graphics
	{
		#region Singletons

		/// <summary>
		/// Gets the render object factory
		/// </summary>
		public static RenderFactory Factory
		{
			get { return ms_Factory;  }
		}

		/// <summary>
		/// Gets the draw object, for drawing simple shapes
		/// </summary>
		public static Draw Draw
		{
			get { return ms_Draw; }
		}

		/// <summary>
		/// Gets the renderer object, for changing the state of the rendering pipeline
		/// </summary>
		public static Renderer Renderer
		{
			get { return ms_Renderer; }
		}

		/// <summary>
		/// Gets the shader parameter bindings
		/// </summary>
		public static ShaderParameterBindings ShaderParameterBindings
		{
			get { return ms_ShaderBindings; }
		}

		#endregion

		#region Internal members

		/// <summary>
		/// Initialises the graphics singletons
		/// </summary>
		/// <param name="factory"></param>
		internal static void Initialise( RenderFactory factory )
		{
			ms_Factory = factory;
			ms_Draw = factory.NewDraw( );
			ms_Renderer = factory.NewRenderer( );
			ms_ShaderBindings = factory.NewShaderParameterBindings( );
		}

		#endregion

		#region Private members

		private static RenderFactory ms_Factory;
		private static Draw ms_Draw;
		private static Renderer ms_Renderer;
		private static ShaderParameterBindings ms_ShaderBindings;

		#endregion
	}
}
