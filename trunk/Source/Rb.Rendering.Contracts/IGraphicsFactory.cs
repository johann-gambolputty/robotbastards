using Rb.Rendering.Contracts.Objects;

namespace Rb.Rendering.Contracts
{
	/// <summary>
	/// Contract for creating graphics objects
	/// </summary>
	public interface IGraphicsFactory
	{
		#region Systems

		/// <summary>
		/// Creates an implementation of the IRenderer interface
		/// </summary>
		IRenderer CreateRenderer( );

		/// <summary>
		/// Creates an implementation of the IDraw interface
		/// </summary>
		IDraw CreateDraw( );

		#endregion

		#region Platform

		/// <summary>
		/// Creates a platform independent object used for setting up rendering displays
		/// </summary>
		IDisplaySetup CreateDisplaySetup( );

		#endregion

		#region Objects

		/// <summary>
		/// Creates a render target
		/// </summary>
		IRenderTarget CreateRenderTarget( );

		/// <summary>
		/// Creates a 2d texture
		/// </summary>
		ITexture2d CreateTexture2d( );

		/// <summary>
		/// Creates a 2d texture sampler
		/// </summary>
		ITexture2dSampler CreateTexture2dSampler( );

		/// <summary>
		/// Creates a new material
		/// </summary>
		IMaterial CreateMaterial( );

		/// <summary>
		/// Creates a render state
		/// </summary>
		IRenderTarget CreateRenderState( );

		/// <summary>
		/// Creates a font
		/// </summary>
		/// <param name="data">Data used to initialize the font</param>
		IFont CreateFont( FontData data );

		/// <summary>
		/// Creates a vertex buffer
		/// </summary>
		/// <param name="data">Data used to initialize the vertex buffer</param>
		IVertexBuffer CreateVertexBuffer( VertexBufferData data );

		/// <summary>
		/// Creates an index buffer
		/// </summary>
		/// <param name="data">Data used to initialize the index buffer</param>
		IIndexBuffer CreateIndexBuffer( IndexBufferData data );

		#endregion
	}
}
