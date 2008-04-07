using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Interfaces
{
	/// <summary>
	/// Contract for creating graphics objects
	/// </summary>
	public interface IGraphicsFactory
	{
		/// <summary>
		/// Gets the name of the API that is being used by the implementation of this factory
		/// </summary>
		/// <remarks>
		/// This name is used to determine if a given custom rendering assembly can be loaded.
		/// If an assembly contain 
		/// </remarks>
		string ApiName
		{
			get;
		}

		#region Systems

		/// <summary>
		/// Creates an implementation of the IRenderer interface
		/// </summary>
		IRenderer CreateRenderer( );

		/// <summary>
		/// Creates an implementation of the IEffectDataSources interface
		/// </summary>
		IEffectDataSources CreateEffectDataSources( );

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
		IRenderState CreateRenderState( );

		/// <summary>
		/// Creates a font
		/// </summary>
		/// <param name="data">Data used to initialize the font</param>
		IFont CreateFont( FontData data );

		/// <summary>
		/// Creates a vertex buffer
		/// </summary>
		/// <param name="format">Format of vertices in the vertex buffer</param>
		/// <param name="numVertices">Number of vertices to allocate in the buffer</param>
		IVertexBuffer CreateVertexBuffer( VertexBufferFormat format, int numVertices );

		/// <summary>
		/// Creates a vertex buffer
		/// </summary>
		IVertexBuffer CreateVertexBuffer( VertexBufferData data );

		/// <summary>
		/// Creates an index buffer
		/// </summary>
		/// <param name="data">Data used to initialize the index buffer</param>
		IIndexBuffer CreateIndexBuffer( IndexBufferData data );

		/// <summary>
		/// Creates an index buffer
		/// </summary>
		/// <param name="format">Index buffer format</param>
		/// <param name="numIndices">Number of indices in the buffer</param>
		IIndexBuffer CreateIndexBuffer( IndexBufferFormat format, int numIndices );

		#endregion

		#region Custom Types

		/// <summary>
		/// Gets the custom type library
		/// </summary>
		LibraryBuilder CustomTypes
		{
			get;
		}

		#endregion
	}
}
