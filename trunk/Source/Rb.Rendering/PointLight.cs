using Rb.Core.Maths;

namespace Rb.Rendering
{
	/// <summary>
	/// A point light
	/// </summary>
	public class PointLight : Light
	{
		/// <summary>
		/// Light position
		/// </summary>
		public Point3	Position
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		/// <summary>
		/// Light inner radius
		/// </summary>
		public float	InnerRadius
		{
			get { return m_InnerRadius; }
			set { m_InnerRadius = value; }
		}

		/// <summary>
		/// Light outer radius
		/// </summary>
		public float	OuterRadius
		{
			get { return m_OuterRadius; }
			set { m_OuterRadius = value; }
		}

		/// <summary>
		/// Light attenuation
		/// </summary>
		public float	Attenuation
		{
			get { return m_Attenuation; }
			set { m_Attenuation = value; }
		}

		private Point3	m_Position = new Point3( );
		private float	m_InnerRadius;
		private float	m_OuterRadius;
		private float	m_Attenuation;
	}
}
