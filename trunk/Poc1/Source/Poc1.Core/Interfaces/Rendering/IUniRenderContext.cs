using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
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

		/// <summary>
		/// Gets the current planetary atmosphere renderer that the camera is inside. Returns null if the camera
		/// not inside any planetary atmospheres
		/// </summary>
		/// <remarks>
		/// Used to set up rendering effects for objects seen through atmospheres.
		/// </remarks>
		IPlanetAtmosphereRenderer InAtmosphereRenderer
		{
			get;
		}

		/// <summary>
		/// Gets a specified render target
		/// </summary>
		/// <param name="target">Target to retrieve</param>
		/// <returns>Returns the specified render target. If it's not supported, null is returned.</returns>
		IRenderTarget GetRenderTarget( UniRenderTargets target );

		/// <summary>
		/// Sets a render target
		/// </summary>
		/// <param name="targetType">Render target type</param>
		/// <param name="target">Render target. Can be null</param>
		void SetRenderTarget( UniRenderTargets targetType, IRenderTarget target );
	}
}
