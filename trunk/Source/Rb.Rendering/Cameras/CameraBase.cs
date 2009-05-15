using System;
using Rb.Core.Components;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Rb.Rendering.Cameras
{

	/// <summary>
	/// Base class for cameras
	/// </summary>
	public abstract class CameraBase : Part, ICamera
	{
		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public virtual void Begin( )
		{
			Graphics.Renderer.PushCamera( this );
		}

		/// <summary>
		/// Should remove camera transforms from the current renderer
		/// </summary>
		public virtual void End( )
		{
			if ( Graphics.Renderer.Camera != this )
			{
				throw new InvalidOperationException( string.Format( "Unexpected current camera in renderer (was \"{0}\", expected \"{1}\"", Graphics.Renderer.Camera, this ) );
			}
			Graphics.Renderer.PopCamera( );
		}
	}
}
