using System;

namespace Rb.World
{
	/// <summary>
	/// Simple implementation of IHost
	/// </summary>
	public class Host : IHost
	{
		/// <summary>
		/// Host setup constructor (ID is automatically generated)
		/// </summary>
		/// <param name="type">Host type</param>
		public Host( HostType type )
		{
			m_HostType = type;
			m_Id = Guid.NewGuid( );
		}

		/// <summary>
		/// Host setup constructor
		/// </summary>
		/// <param name="type">Host type</param>
		/// <param name="id">Host identifier</param>
		public Host( HostType type, Guid id )
		{
			m_HostType = type;
			m_Id = id;
		}

		#region IHost Members

		/// <summary>
		/// Host type
		/// </summary>
		public HostType HostType
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion

		#region IUnique Members

		/// <summary>
		/// Host identifier
		/// </summary>
		public Guid Id
		{
			get { return m_Id; }
			set { m_Id = value; }
		}

		#endregion

		private HostType	m_HostType;
		private Guid		m_Id;
	}
}
