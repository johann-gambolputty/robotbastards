using Rb.Core.Components;
using Rb.Rendering.Cameras;

namespace Rb.TestApp
{
	/// <summary>
	/// Base class for camera controllers
	/// </summary>
	public class CameraController : Component, ICameraController
	{
		/// <summary>
		/// Access to the camera being controlled
		/// </summary>
		public virtual CameraBase Camera
		{
			get { return m_Camera;  }
			set { m_Camera = value; }
		}

		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		public override void AddedToParent( object parent )
		{
			base.AddedToParent( parent );

			//	If the parent is a camera object, and the camera hasn't been set yet, then
			//	set the parent as the controlled camera
			if ( ( m_Camera == null ) && ( parent is CameraBase ) )
			{
				Camera = ( CameraBase )parent;
			}
		}

		#region ICameraController Members

		/// <summary>
		/// Enables or disables this controller
		/// </summary>
		public bool Enabled
		{
			get { return m_Enabled; }
			set { m_Enabled = value; }
		}

		#endregion

		#region Private stuff

		private bool		m_Enabled = true;
		private CameraBase	m_Camera;

		#endregion
	}
}
