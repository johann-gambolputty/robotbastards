using Poc1.Core.Interfaces.Rendering.Cameras;
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
		/// Returns true if far objects are being rendered according to the current pass
		/// </summary>
		bool RenderFarObjects
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
