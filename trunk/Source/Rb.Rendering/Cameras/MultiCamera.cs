using System;
using System.Collections.Generic;
using Rb.Core.Maths;

namespace Rb.Rendering.Cameras
{
	/// <summary>
	/// Stores a bunch of cameras, with one active one
	/// </summary>
	public class MultiCamera : CameraBase
	{
		/// <summary>
		/// Returns the list of supported cameras
		/// </summary>
		public List<CameraBase> Cameras
		{
			get { return m_Cameras; }
		}

		/// <summary>
		/// Access to the active camera index
		/// </summary>
		public int ActiveCameraIndex
		{
			get { return m_ActiveCameraIndex; }
			set
			{
				m_ActiveCameraIndex = Utils.Clamp( value, 0, m_Cameras.Count - 1 );
			}
		}

		/// <summary>
		/// Access to the active camera
		/// </summary>
		public CameraBase ActiveCamera
		{
			get
			{
				return m_Cameras.Count == 0 ? null : m_Cameras[ m_ActiveCameraIndex ];
			}
			set
			{
				int index = m_Cameras.IndexOf( value );
				if ( index == -1 )
				{
					throw new ArgumentException( "Could not find camera in camera list" );
				}
				m_ActiveCameraIndex = index;
			}
		}

		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public override void Begin( )
		{
			if ( ActiveCamera != null )
			{
				ActiveCamera.Begin( );
			}
		}

		/// <summary>
		/// Should remove camera transforms from the current renderer
		/// </summary>
		public override void End( )
		{
			if ( ActiveCamera != null )
			{
				ActiveCamera.End( );
			}
		}

		#region Private stuff

		private readonly List<CameraBase> m_Cameras = new List<CameraBase>( );
		private int m_ActiveCameraIndex;

		#endregion
	}
}
