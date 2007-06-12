using System;
using System.Collections;
using System.Reflection;

namespace RbEngine.Components
{
	/// <summary>
	/// Attribute, used for marking up a method as a message handler
	/// </summary>
	public class MessageHandlerAttribute : Attribute
	{
	}

	/// <summary>
	/// Creates a mapping from message types to message handlers
	/// </summary>
	public class ComponentMessageMap
	{
		/// <summary>
		/// Constructor. Creates the mapping for all MessageHandlerAttribute tagged methods in the specified type
		/// </summary>
		/// <param name="createForType"> Type to create the map for </param>
		public ComponentMessageMap( Type createForType )
		{
			//	TODO: Add proper binding flags
			foreach ( System.Reflection.MethodInfo curMethod in createForType.GetMethods( ) )
			{
				if ( curMethod.GetCustomAttributes( typeof( MessageHandlerAttribute ), false ).Length != 0 )
				{
					//	It's a thing! Use the thing!
					ParameterInfo[] parameters = curMethod.GetParameters( );
					System.Diagnostics.Trace.Assert( parameters.Length == 1, String.Format( "Method \"{0}\" was flagged as a component message handler, but did not have the correct number of parameters", curMethod.Name ) );

					//	What's the type of the message?
                    Type messageType = parameters[ 0 ].GetType( );
					System.Diagnostics.Debug.WriteLine( String.Format( "Adding handler for class \"{0}\", for message type \"{1}\", using method \"{2}\"", createForType.Name, messageType.Name, curMethod.Name ) );

					//	TODO: Create a method by emitting bytecode that calls the appropriate method?
				}
			}
		}
	}

	//	NOTE: Component accessibility:
	//		By type: GetComponent( typeof( ISomeInterfaceOrOther ) );	//	Returns the first component that implements the specified interface
	//		By name: GetComponent( "badger" );							//	Returns the first INamedObject component with the specified name
	//		By guid: GetComponent( 0x12345 );							//	Returns the first component with the specified GUID
	//
	//	Component linkages could be specified in data:
	//	<object type="MyComponent" name="obj">
	//		<object type="ComponentType0" name="child0"/>
	//		<object type="ComponentType1" name="child1">
	//			// name can be qualified by path. Path can be prefixed by symbolic name (e.g. "$parent", "$engine", "$scene"). Following are nested names, or guid, of child components
	//			<reference name="$parent.child0" property="OtherThing"/>
	//		</object>
	//	</object>

	/// <summary>
	/// Handy base class that derives from Node, and implements IMessageHandler, IMessageHub and IUnique
	/// </summary>
	public class Component : Node, IMessageHandler, IMessageHub, IUnique
	{
		#region	Messaging tests

		//	TODO: Add nice reflection based thing
		//
		//	public static MessageMap GetMessageMap( )
		//	{
		//		return ...;
		//	}
		//	[MessageHandler()]
		//	public void OnDamage( DamageMessage msg )
		//	{
		//		
		//	}
		//
		//
		//	public void HandleMessage( Object msg )
		//	{
		//		GetMessageMap( ).GetHandler( msg.GetType( ) )( msg );
		//	}
		//

		/// <summary>
		/// Test
		/// </summary>
		public static ComponentMessageMap ms_Messages = new ComponentMessageMap( typeof( Component ) );

		/// <summary>
		/// Test
		/// </summary>
		public virtual ComponentMessageMap GetMessageMap( )
		{
			return ms_Messages;
		}

		/// <summary>
		/// Test
		/// </summary>
		[ MessageHandler( ) ]
		public void OnDamage( Message msg )
		{
		}

		#endregion

		#region	IMessageHandler Members

		/// <summary>
		/// Adds the message to a recipient chain, if there is one
		/// </summary>
		public virtual void	HandleMessage( Message msg )
		{
			if ( m_RecipientChains == null )
			{
				return;
			}

			Type baseType = typeof( Object );
			for ( Type messageType = msg.GetType( ); messageType != baseType; messageType = messageType.BaseType )
			{	
				MessageRecipientChain chain = ( MessageRecipientChain )m_RecipientChains[ messageType ];
				if ( chain != null )
				{
					chain.Deliver( msg );
					return;
				}
			}
		}
		#endregion

		#region	IMessageHub Members

		/// <summary>
		/// Adds a recipient for messages of a given type
		/// </summary>
		/// <param name="messageType">Base class of messages that the recipient is interested in</param>
		/// <param name="recipient">Recipient call</param>
		/// <param name="order">Recipient order value</param>
		public virtual void AddRecipient( Type messageType, MessageRecipientDelegate recipient, int order )
		{
			if ( m_RecipientChains == null )
			{
				m_RecipientChains = new System.Collections.Hashtable( );
			}

			MessageRecipientChain chain = ( MessageRecipientChain )m_RecipientChains[ messageType ];
			if ( chain == null )
			{
				chain = new MessageRecipientChain( );
				m_RecipientChains[ messageType ] = chain;
			}
			chain.AddRecipient( recipient, order );
		}

		/// <summary>
		/// Removes a recipient from a given type of message chain
		/// </summary>
		public virtual void RemoveRecipient( Type messageType, object recipient )
		{
			MessageRecipientChain chain = ( MessageRecipientChain )m_RecipientChains[ messageType ];
			if ( chain != null )
			{
				chain.RemoveRecipient( recipient );
			}
		}

		#endregion

		#region	IUnique Members

		/// <summary>
		/// Access to the unique ID of this component
		/// </summary>
		public ObjectId	Id
		{
			get
			{
				return m_Id;
			}
			set
			{
				m_Id = value;
			}
		}

		#endregion

		#region	Child list

		/// <summary>
		/// Gets a child that is derived from, or implements, a given type
		/// </summary>
		/// <param name="childType"> Type to look for </param>
		/// <returns> Returns null if no child of the specified type could be found. Otherwise, returns the first child of the given type</returns>
		public Object		GetChild( Type childType )
		{
			foreach ( Object curObj in m_Children )
			{
				if ( childType.IsInstanceOfType( curObj ) )
				{
					return curObj;
				}
			}

			return null;
		}

		#endregion

		private ObjectId						m_Id				= new ObjectId( );
		private System.Collections.Hashtable	m_RecipientChains	= null;
	}
}
