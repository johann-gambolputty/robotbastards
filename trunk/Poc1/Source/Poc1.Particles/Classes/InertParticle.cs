
using Rb.Core.Maths;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Simple inert particle
	/// </summary>
	public class InertParticle
	{
		public Point3 Position
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		public int Age
		{
			get { return m_Age; }
			set { m_Age = value; }
		}

		#region Private Members

		private Point3	m_Position;
		private int		m_Age;

		#endregion
	}
}
