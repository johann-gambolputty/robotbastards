using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Cameras;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// 2D camera
	/// </summary>
	public class TileCamera2d : CameraBase
	{
		/// <summary>
		/// The camera origin
		/// </summary>
		public Point2 Origin
		{
			set { m_Origin = value; }
			get { return m_Origin; }
		}

		/// <summary>
		/// The scale factor
		/// </summary>
		public float Scale
		{
			set { m_Scale = value; }
			get { return m_Scale; }
		}
		
		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public override void Begin( )
		{
			Renderer.Instance.Push2d( );
			Renderer.Instance.PushTransform( Transform.LocalToWorld, Matrix44.Identity );
			Renderer.Instance.Translate( Transform.LocalToWorld, m_Origin.X, m_Origin.Y, 0 );
			Renderer.Instance.Scale( Transform.LocalToWorld, m_Scale, m_Scale, 1 );
			base.Begin( );
		}

		/// <summary>
		/// Should remove camera transforms from the current renderer
		/// </summary>
		public override void End( )
		{
			base.End( );
			Renderer.Instance.PopTransform( Transform.LocalToWorld );
			Renderer.Instance.Pop2d( );
		}

		private Point2	m_Origin	= Point2.Origin;
		private float	m_Scale		= 1.0f;
	}
}
