using System;
using Rb.World;

namespace Rb.TestApp
{
	public class HostSetup
	{
		public RemoteHostAddress ServerAddress
		{
			get { return m_ServerAddress; }
			set { m_ServerAddress = value; }
		}

		public int Port
		{
			get { return m_Port; }
			set { m_Port = value; }
		}

		public string SceneFile
		{
			get { return m_SceneFile; }
			set { m_SceneFile = value; }
		}

		public string InputFile
		{
			get { return m_InputFile; }
			set { m_InputFile = value; }
		}

		public Guid HostGuid
		{
			set { m_HostGuid = value; }
			get { return m_HostGuid;  }
		}

		public HostType HostType
		{
			set { m_HostType = value; }
			get { return m_HostType; }
		}

		private RemoteHostAddress	m_ServerAddress;
		private int					m_Port;
		private string				m_SceneFile;
		private string				m_InputFile;
		private Guid				m_HostGuid;
		private HostType			m_HostType;
	}
}
