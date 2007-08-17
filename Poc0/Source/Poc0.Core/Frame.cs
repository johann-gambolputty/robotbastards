using System;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Maths;

namespace Poc0.Core
{
	/// <summary>
	/// Position and orientation for an object in tile world
	/// </summary>
	public class Frame
	{
		/// <summary>
		/// Gets and sets the position of the object
		/// </summary>
		public Point3 Position
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		/// <summary>
		/// Gets and sets the rotation of the object (in radians)
		/// </summary>
		public float Rotation
		{
			get { return m_Rotation; }
			set { m_Rotation = value; }
		}

		private Point3 m_Position = new Point3( 0, 0, 0 );
		private float m_Rotation = 0;
	}
}
