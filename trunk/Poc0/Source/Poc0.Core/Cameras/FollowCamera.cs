using Poc0.Core.Objects;
using Rb.Rendering.Base.Cameras;

namespace Poc0.Core.Cameras
{
	/// <summary>
	/// Follows a given entity around
	/// </summary>
	public class FollowCamera : SphereCamera
	{
		/// <summary>
		/// Sets up the follow camera. There is no initial target (camera will point at origin)
		/// </summary>
		public FollowCamera( )
		{
		}

		/// <summary>
		/// Sets up the follow camera
		/// </summary>
		/// <param name="target">Initial target</param>
		public FollowCamera( IPlaceable target )
		{
			m_Target = target;
		}

		/// <summary>
		/// The follow camera target
		/// </summary>
		public IPlaceable Target
		{
			get { return m_Target; }
			set { m_Target = value; }
		}

		/// <summary>
		/// Gets/sets the offset to the S component of the sphere camera
		/// </summary>
		/// <remarks>
		/// The follow camera naturally always faces the back of the target - this property provides
		/// an offset (so SOffset = pi will always face the front of the target)
		/// </remarks>
		public float SOffset
		{
			get { return m_SOffset; }
			set { m_SOffset = value; }
		}


		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public override void Begin( )
		{
			if ( m_Target != null )
			{
				LookAt = m_Target.Position;
			//	S = Functions.Atan2( -m_Target.Frame.ZAxis.Z, -m_Target.Frame.ZAxis.X ) + m_SOffset;
			}
			base.Begin( );
		}

		private IPlaceable m_Target;
		private float m_SOffset;
	}
}
