
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
			ms_Factory = factory;
			ms_Renderer = factory.CreateRenderer( );
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
		/// Gets the graphics factory
		/// </summary>
		public static IGraphicsFactory Factory
		{
			get { return ms_Factory; }
		}

		#region Private members

		private static IDraw ms_Draw;
		private static IRenderer ms_Renderer;
		private static IGraphicsFactory ms_Factory;

		#endregion

	}
}
