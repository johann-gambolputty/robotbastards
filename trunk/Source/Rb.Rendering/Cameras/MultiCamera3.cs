using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Rb.Rendering.Cameras
{
	public class MultiCamera3 : MultiCamera, ICamera3
	{
		public ICamera3 ActiveCamera3
		{
			get { return ( ICamera3 )ActiveCamera; }
		}

		#region ICamera3 Members

		public Point3 Unproject( int x, int y, float depth )
		{
			return ActiveCamera3.Unproject( x, y, depth );
		}

		public Ray3 PickRay( int x, int y )
		{
			return ActiveCamera3.PickRay( x, y );
		}

		public Matrix44 Frame
		{
			get { return ActiveCamera3.Frame; }
		}

		public Vector3 XAxis
		{
			get { return ActiveCamera3.Frame.XAxis; }
		}

		public Vector3 YAxis
		{
			get { return ActiveCamera3.Frame.YAxis; }
		}

		public Vector3 ZAxis
		{
			get { return ActiveCamera3.Frame.ZAxis; }
		}

		public Point3 Position
		{
			get { return ActiveCamera3.Frame.Translation; }
		}

		#endregion
	}
}
