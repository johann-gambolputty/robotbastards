using System;
using Rb.Interaction.Classes;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.Bob.Core.Interfaces.Rendering
{
	/// <summary>
	/// Represents the view from a single camera
	/// </summary>
	public interface ICameraView
	{
		/// <summary>
		/// Event raised when the camera is ready to initialize rendering
		/// </summary>
		/// <remarks>
		/// Rendering resources like effects can only be initialized once a rendering context has been
		/// created by the underlying Display object used by this view.
		/// </remarks>
		event EventHandler InitializeRendering;

		/// <summary>
		/// Gets/sets the camera used by the view
		/// </summary>
		ICamera Camera
		{
			get; set;
		}

		/// <summary>
		/// Gets the input source for this view
		/// </summary>
		CommandInputSource InputSource
		{
			get;
		}

		/// <summary>
		/// Gets/sets the object rendered by this view
		/// </summary>
		IRenderable Renderable
		{
			get; set;
		}
	}
}
