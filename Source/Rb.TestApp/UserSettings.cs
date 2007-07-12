using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Rb.TestApp
{
	/// <summary>
	/// Stores the address of a remote host
	/// </summary>
	public class RemoteHostAddress
	{
		public const string	LocalAddress = "127.0.0.1";
		public const int	DefaultPort = 30000;

		/// <summary>
		/// Default constructor
		/// </summary>
		public RemoteHostAddress( )
		{
			m_Address = LocalAddress;
			m_Port = DefaultPort;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public RemoteHostAddress( string address, int port )
		{
			m_Address = address;
			m_Port = port;
		}

		/// <summary>
		/// The IP address
		/// </summary>
		[XmlAttribute( "address" )]
		public string Address
		{
			get { return m_Address; }
			set { m_Address = value; }
		}

		/// <summary>
		/// The port
		/// </summary>
		[XmlAttribute( "port" )]
		public int Port
		{
			get { return m_Port; }
			set { m_Port = value;  }
		}

		/// <summary>
		/// Converts this object to a string
		/// </summary>
		public override string ToString( )
		{
			return m_Address + " : " + m_Port;
		}

		private string	m_Address;
		private int		m_Port;
	}

	/// <summary>
	/// User settings
	/// </summary>
	[XmlRoot( "userSettings" )]
	public class UserSettings
	{
		/// <summary>
		/// Saves user settings to a file
		/// </summary>
		public void Save( )
		{
			TextWriter writer = new StreamWriter( GetFilename( ) );

			try
			{
				XmlSerializer serializer = new XmlSerializer( typeof( UserSettings ) );

				serializer.Serialize( writer, this );
			}
			finally
			{
				writer.Close( );
			}
		}

		/// <summary>
		/// Returns a default setup for the UserSettings
		/// </summary>
		public static UserSettings DefaultSettings( )
		{
			UserSettings settings = new UserSettings( );

			settings.ServerIpHistory.Add( new RemoteHostAddress( ) );
			settings.SceneFileHistory.Add( "scene0.components.xml" );
			settings.InputFileHistory.Add( "testCommandInputs0.components.xml" );
			settings.ViewerFileHistory.Add( "viewerSetup0.components.xml" );

			return settings;
		}

		/// <summary>
		/// Loads user settings from a file
		/// </summary>
		public static UserSettings Load( )
		{
			string filename = GetFilename( );
			if ( !File.Exists( filename ) )
			{
				return DefaultSettings( );
			}

			UserSettings settings;
			TextReader reader = new StreamReader( filename );
			try
			{
				XmlSerializer serializer = new XmlSerializer( typeof( UserSettings ) );
				settings = ( UserSettings )serializer.Deserialize( reader );

				//	TODO: AP: argh...
				if ( settings.ViewerFileHistory.Count == 0 )
				{
					settings.ViewerFileHistory.Add( "viewerSetup0.components.xml" );
				}
			}
			finally
			{
				reader.Close( );
			}

			return settings;
		}

		/// <summary>
		/// Returns the filename that the user settings file is saved as
		/// </summary>
		public static string GetFilename( )
		{
			return "../Settings For " + Environment.UserName + ".xml";
		}

		/// <summary>
		/// List of previously used server IPs
		/// </summary>
		[XmlElement( "serverIpHistory" )]
		public List< RemoteHostAddress > ServerIpHistory
		{
			get { return m_ServerIpHistory; }
			set { m_ServerIpHistory = value; }
		}

		/// <summary>
		/// List of previously used scene files
		/// </summary>
		[XmlElement( "sceneFileHistory" )]
		public List< string > SceneFileHistory
		{
			get { return m_SceneFileHistory; }
			set { m_SceneFileHistory = value; }
		}

		/// <summary>
		/// List of previously used input files
		/// </summary>
		[XmlElement( "inputFileHistory" )]
		public List< string > InputFileHistory
		{
			get { return m_InputFileHistory; }
			set { m_InputFileHistory = value; }
		}
		
		/// <summary>
		/// List of previously used input files
		/// </summary>
		[XmlElement( "viewerFileHistory" )]
		public List< string > ViewerFileHistory
		{
			get { return m_ViewerFileHistory; }
			set { m_ViewerFileHistory = value; }
		}

		#region Private stuff

		private List< RemoteHostAddress >	m_ServerIpHistory		= new List< RemoteHostAddress >( );
		private List< string > 				m_SceneFileHistory		= new List< string >( );
		private List< string > 				m_InputFileHistory		= new List< string >( );
		private List< string > 				m_ViewerFileHistory 	= new List< string >( );

		#endregion
	}
}
