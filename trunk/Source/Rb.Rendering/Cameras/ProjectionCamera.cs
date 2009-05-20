using Rb.Core.Maths;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Rb.Rendering.Cameras
{
	/// <summary>
	/// Base class for 3d cameras
	/// </summary>
	public abstract class ProjectionCamera : Camera3, IProjectionCamera
	{
		/// <summary>
		/// Returns the inverse of the camera's view.projection matrices
		/// </summary>
		public InvariantMatrix44 InverseCameraMatrix
		{
			get { return m_InvProjView; }
		}

		/// <summary>
		/// Gets the projection matrix
		/// </summary>
		public InvariantMatrix44 ProjectionMatrix
		{
			get { return m_ProjectionMatrix; }
			set { m_ProjectionMatrix.Copy( value ); }
		}

		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public override void Begin( )
		{
			IRenderer renderer = Graphics.Renderer;
			int width = renderer.ViewportWidth;
			int height = renderer.ViewportHeight;

			float aspectRatio = ( height == 0 ) ? 1.0f : width / ( float )height;

			Graphics.Renderer.PushTransform( TransformType.ViewToScreen );
			Graphics.Renderer.SetPerspectiveProjectionTransform( m_PerspectiveFov, aspectRatio, m_PerspectiveZNear, m_PerspectiveZFar );

			ProjectionMatrix = Graphics.Renderer.GetTransform( TransformType.ViewToScreen );
			//	TODO: AP: Projection matrix should be updated by projection property sets

			m_InvProjView.StoreMultiply( m_ProjectionMatrix, ViewMatrix );
			m_InvProjView.Invert( );

			base.Begin( );
		}

		/// <summary>
		/// Pops camera transforms
		/// </summary>
		public override void End( )
		{
			Graphics.Renderer.PopTransform( TransformType.ViewToScreen );
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

		private Matrix44 m_ProjectionMatrix	= new Matrix44( );
		private Matrix44 m_InvProjView		= new Matrix44( );
		private float m_PerspectiveFov		= 45.0f;
		private float m_PerspectiveZNear	= 1.0f;
		private float m_PerspectiveZFar		= 1000.0f;

		#endregion
	}
}
