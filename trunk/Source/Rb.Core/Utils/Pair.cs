namespace Rb.Core.Utils
{
	/// <summary>
	/// Two paired values
	/// </summary>
	public class Pair<T0, T1>
	{
		/// <summary>
		/// Default constructor - both values set to their type's default values
		/// </summary>
		public Pair( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="value0">Value 0</param>
		/// <param name="value1">Value 1</param>
		public Pair( T0 value0, T1 value1 )
		{
			Value0 = value0;
			Value1 = value1;
		}

		/// <summary>
		/// Gets/sets the first value in the pair
		/// </summary>
		public T0 Value0
		{
			get { return m_Value0; }
			set { m_Value0 = value; }
		}

		/// <summary>
		/// Gets/sets the second value in the pair
		/// </summary>
		public T1 Value1
		{
			get { return m_Value1; }
			set { m_Value1 = value; }
		}

		#region Private Members

		private T0 m_Value0;
		private T1 m_Value1;

		#endregion

	}
}
