namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Index buffer interface
	/// </summary>
	/// <remarks>
	/// Index buffers are classified as passes - "beginning" an index buffer passes inedx data 
	/// to the rendering engine, "ending" an index buffer disables index buffering.
	/// Index buffers are created by passing an <see cref="IndexBufferData"/> object to
	/// <see cref="IGraphicsFactory.CreateIndexBuffer"/>.
	/// </remarks>
	public interface IIndexBuffer : IPass
	{
	}
}
