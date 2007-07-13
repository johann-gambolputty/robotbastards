using System;
using Rb.Core.Maths;

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

		public Vector3 XAxis
		{
			get { return ActiveCamera3.XAxis; }
		}

		public Vector3 YAxis
		{
			get { return ActiveCamera3.YAxis; }
		}

		public Vector3 ZAxis
		{
			get { return ActiveCamera3.ZAxis; }
		}

		public Point3 Position
		{
			get { return ActiveCamera3.Position; }
		}

		#endregion
	}
}
