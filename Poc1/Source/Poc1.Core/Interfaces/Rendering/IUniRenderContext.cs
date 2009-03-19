using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Rendering
{
	/// <summary>
	/// Universe rendering context
	/// </summary>
	public interface IUniRenderContext : IRenderContext
	{
		/// <summary>
		/// Gets the current camera
		/// </summary>
		IUniCamera Camera
		{
			get;
		}

		/// <summary>
		/// Gets the current pass type
		/// </summary>
		UniRenderPass CurrentPass
		{
			get;
		}
	}
}
