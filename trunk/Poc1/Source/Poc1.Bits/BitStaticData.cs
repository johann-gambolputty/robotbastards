
namespace Poc1.Bits
{
	/// <summary>
	/// Bit shared data
	/// </summary>
	public class BitStaticData
	{
		/// <summary>
		/// Setup constructo
		/// </summary>
		/// <param name="id">Bit type ID</param>
		/// <param name="name">Bit type name</param>
		public BitStaticData( int id, string name )
		{
			m_Name = name;
			m_Id = id;
		}

		/// <summary>
		/// Gets the unique, zero-based ID of this bit type
		/// </summary>
		public int Id
		{
			get { return m_Id; }
		}

		/// <summary>
		/// Gets the name of this bit type
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		private readonly int m_Id;
		private readonly string m_Name;
	}
}
