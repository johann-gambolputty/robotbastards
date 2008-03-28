
using Rb.Rendering.Interfaces;

namespace Rb.Rendering.Base.Cameras
{
	/// <summary>
	/// Base class for 3d cameras
	/// </summary>
	public abstract class ProjectionCamera : Camera3
	{

		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public override void Begin( )
		{
			IRenderer renderer = Graphics.Renderer;
			int width = renderer.ViewportWidth;
			int height = renderer.ViewportHeight;

			float aspectRatio = ( height == 0 ) ? 1.0f : width / ( float )height;

			Graphics.Renderer.PushTransform( Transform.ViewToScreen );
			Graphics.Renderer.SetPerspectiveProjectionTransform( m_PerspectiveFov, aspectRatio, m_PerspectiveZNear, m_PerspectiveZFar );

			m_ProjectionMatrix = Graphics.Renderer.GetTransform( Transform.ViewToScreen );
			//	TODO: AP: Projection matrix should be updated by projection property sets

			base.Begin( );
		}

		/// <summary>
		/// Pops camera transforms
		/// </summary>
		public override void End( )
		{
			Graphics.Renderer.PopTransform( Transform.ViewToScreen );
			base.End( );
		}

		#region	Public properties

		/// <summary>
		/// Gets or sets the field of view
		/// </summary>
		public float PerspectiveFovDegrees
		{
			get { return m_PerspectiveFov; }
			set { m_PerspectiveFov = value; }
		}

		/// <summary>
		/// The z value of the near clipping plane. The greater the better, really (means more z buffer precision)
		/// </summary>
		public float PerspectiveZNear
		{
			get { return m_PerspectiveZNear; }
			set { m_PerspectiveZNear = value; }
		}

		/// <summary>
		/// The z value of the far clipping plane
		/// </summary>
		public float PerspectiveZFar
		{
			get { return m_PerspectiveZFar; }
			set { m_PerspectiveZFar = value; }
		}

		#endregion

		#region	Private stuff

		private float m_PerspectiveFov		= 45.0f;
		private float m_PerspectiveZNear	= 5.0f;
		private float m_PerspectiveZFar		= 1000.0f;

		#endregion
	}
}