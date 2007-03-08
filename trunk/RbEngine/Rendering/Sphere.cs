using System;
using RbEngine.Maths;

namespace RbEngine.Rendering
{
	/// <summary>
	/// A simple sphere that can be rendered
	/// </summary>
	public class Sphere : IRender
	{
		/// <summary>
		/// Radius of the sphere
		/// </summary>
		public float	Radius
		{
			get
			{
				return m_Radius;
			}
			set
			{
				m_Radius = value;
			}
		}

		/// <summary>
		/// Centre of the sphere
		/// </summary>
		public Point3	Centre
		{
			get
			{
				return m_Centre;
			}
			set
			{
				m_Centre = value;
			}
		}

		#region IRender Members

		/// <summary>
		/// Renders the sphere
		/// </summary>
		public void Render( )
		{
			ShapeRenderer.Inst.DrawSphere( Centre, Radius );
		}

		#endregion

		private Point3	m_Centre	= Point3.Origin;
		private float	m_Radius	= 10.0f;
	}
}
