
namespace Rb.Rendering.Cameras
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
			Renderer renderer = Renderer.Inst;
			int width = renderer.ViewportWidth;
			int height = renderer.ViewportHeight;

			float aspectRatio = ( height == 0 ) ? 1.0f : ( float )width / ( float )height;
			Renderer.Inst.SetPerspectiveProjectionTransform( m_PerspectiveFov, aspectRatio, m_PerspectiveZNear, m_PerspectiveZFar );

			base.Begin( );
		}

		#region	Public properties

		/// <summary>
		/// Gets or sets the field of view
		/// </summary>
		public float			PerspectiveFovDegrees
		{
			get
			{
				return m_PerspectiveFov;
			}
			set
			{
				m_PerspectiveFov = value;
			}
		}

		/// <summary>
		/// The z value of the near clipping plane. The greater the better, really (means more z buffer precision)
		/// </summary>
		public float			PerspectiveZNear
		{
			get { return m_PerspectiveZNear; }
			set { m_PerspectiveZNear = value; }
		}

		/// <summary>
		/// The z value of the far clipping plane
		/// </summary>
		public float			PerspectiveZFar
		{
			get { return m_PerspectiveZFar; }
			set { m_PerspectiveZFar = value; }
		}

		#endregion

		#region	Private stuff

		private float			m_PerspectiveFov			= 45.0f;
		private float			m_PerspectiveZNear			= 5.0f;
		private float			m_PerspectiveZFar			= 1000.0f;

		#endregion
	}
}
