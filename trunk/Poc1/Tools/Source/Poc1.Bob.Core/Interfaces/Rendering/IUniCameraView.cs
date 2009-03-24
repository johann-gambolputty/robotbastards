using Poc1.Core.Interfaces.Rendering.Cameras;

namespace Poc1.Bob.Core.Interfaces.Rendering
{
	/// <summary>
	/// Represents the view from a single camera
	/// </summary>
	public interface IUniCameraView : ICameraView
	{
		/// <summary>
		/// Returns the <see cref="ICameraView.Camera"/> property as an <see cref="IUniCamera"/>
		/// </summary>
		IUniCamera UniCamera
		{
			get;
		}
	}
}
