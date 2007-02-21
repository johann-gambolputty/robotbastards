using System;
using System.Reflection;

using MessageTypeId = System.UInt16;


//	TODO: Must check that Message.IdFromType() will give consistent results on client and server (with different assemblies loaded in different orders)

namespace RbEngine.Components
{
	/// <summary>
	/// Tracks Message-derived types, providing a conversion from message type identifiers to Type instances
	/// </summary>
	public class MessageTypeManager
	{
		/// <summary>
		/// Singleton
		/// </summary>
		public static MessageTypeManager	Inst
		{
			get
			{
				return ms_Singleton;
			}
		}

		/// <summary>
		/// Scans an assembly for classes that implement the Message abstract class
		/// </summary>
		/// <param name="assembly">Assembly to scan</param>
		public void ScanAssembly( Assembly assembly )
		{
			//	Run through all the types in the assembly
			foreach ( Type curType in assembly.GetTypes( ) )
			{
				if ( curType.IsSubclassOf( typeof( Message ) ) )
				{
					AddMessageType( curType );
				}
			}
		}

		/// <summary>
		/// Adds a new Message-derived type to the manager
		/// </summary>
		/// <param name="messageType">Message derived class Type</param>
		public void AddMessageType( Type messageType )
		{
			MessageTypeId typeId = Message.IdFromType( messageType );

			if ( m_MessageTable.ContainsKey( typeId ) )
			{
				Output.Fail( Output.ComponentError, "Type \"{0}\" has a type id ({1}) that clashes with type \"{2}\"", messageType.Name, typeId, ( ( Type )m_MessageTable[ typeId ] ).Name );
			}
			else
			{
				m_MessageTable[ typeId ] = messageType;
				Output.WriteLineCall( Output.ComponentInfo, "Tracked message type \"{0}\", id \"{1}\"", messageType.Name, typeId );
			}
		}

		/// <summary>
		/// Gets a message type from a message type identifier
		/// </summary>
		/// <param name="id">Message type identifier</param>
		/// <returns>Returns the associated type</returns>
		public Type	GetMessageTypeFromId( MessageTypeId id )
		{
			return ( Type )m_MessageTable[ id ];
		}

		/// <summary>
		/// Message ID to type table
		/// </summary>
		private System.Collections.Hashtable	m_MessageTable	= new System.Collections.Hashtable( );

		/// <summary>
		/// Singleton
		/// </summary>
		private static MessageTypeManager		ms_Singleton	= new MessageTypeManager( );

		/// <summary>
		/// Scans all existing assemblies for classes implementing Message. Adds a delegate to the assembly load event, to scan any subsequently loaded assemblies
		/// </summary>
		private MessageTypeManager( )
		{
			//	Scan the current assemblies
			foreach ( Assembly curAssembly in AppDomain.CurrentDomain.GetAssemblies( ) )
			{
				ScanAssembly( curAssembly );
			}

			//	Track any new assemblies that are loaded
			AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler( OnAssemblyLoad );
		}

		/// <summary>
		/// Scans loaded assemblies
		/// </summary>
		private void OnAssemblyLoad( object sender, AssemblyLoadEventArgs args )
		{
			ScanAssembly( args.LoadedAssembly );
		}
	}
}
