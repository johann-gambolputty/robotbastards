namespace Poc1.ParticleSystemBuilder
{
	public enum RenderMethodType
	{
		Stationary,
		Circle,
		FigureOfEight,
		FollowTheMouse
	}

	public class RenderMethod
	{
		public const float MaxSpeed = 0.5f;

		public RenderMethodType Method
		{
			get { return m_Method; }
			set { m_Method = value; }
		}

		public float Radius
		{
			get { return m_Radius; }
			set { m_Radius = value; }
		}

		public float PeriodTime
		{
			get { return m_PeriodTime; }
			set { m_PeriodTime = value; }
		}

		#region Private Members

		private RenderMethodType m_Method;
		private float m_Radius = 2.0f;
		private float m_PeriodTime = 2.0f;

		#endregion
	}
}
