
namespace Rb.Rendering.Contracts
{
	/// <summary>
	/// A static class containing implementations of the key rendering interfaces
	/// </summary>
	public static class Graphics
	{
		/// <summary>
		/// Creates graphics objects from the specified factory
		/// </summary>
		public static void Initialize( IGraphicsFactory factory )
		{
			ms_Renderer = factory.CreateRenderer( );
			ms_RenderStateBuilder = factory.CreateRenderStateBuilder( );
			ms_Draw = factory.CreateDraw( );
		}

		/// <summary>
		/// Gets the current draw object
		/// </summary>
		public static IDraw Draw
		{
			get { return ms_Draw; }
		}

		/// <summary>
		/// Gets the current renderer
		/// </summary>
		public static IRenderer Renderer
		{
			get { return ms_Renderer; }
		}

		/// <summary>
		/// Gets the current render state builder
		/// </summary>
		public static IRenderStateBuilder RenderStateBuilder
		{
			get { return ms_RenderStateBuilder; }
		}

		#region Private members

		private static IDraw ms_Draw;
		private static IRenderer ms_Renderer;
		private static IRenderStateBuilder ms_RenderStateBuilder;

		#endregion

	}
}
