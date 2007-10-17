using Rb.Core.Components;

namespace Poc0.Core.Objects
{
	public class HealthChange : Message
	{
		public HealthChange( int amount, object source )
		{
			m_Amount = amount;
			m_Source = source;
		}

		public bool Healing
		{
			get { return m_Amount > 0; }
		}

		public object Source
		{
			get { return m_Source; }
		}

		public int Amount
		{
			get { return m_Amount; }
		}

		private readonly int	m_Amount;
		private readonly object	m_Source;
	}
}
