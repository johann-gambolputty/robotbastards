using System;
using System.Collections;
using System.Reflection;

namespace RbEngine.Components
{
	/// <summary>
	/// Attribute, used for marking up a method as a message handler
	/// </summary>
	public class ComponentMessageHandlerAttribute : Attribute
	{
	}

	/// <summary>
	/// Creates a mapping from message types to message handlers
	/// </summary>
	public class ComponentMessageMap
	{
		/// <summary>
		/// Constructor. Creates the mapping for all ComponentMessageHandlerAttribute tagged methods in the specified type
		/// </summary>
		/// <param name="createForType"> Type to create the map for </param>
		public ComponentMessageMap( Type createForType )
		{
			//	TODO: Add proper binding flags
			foreach ( System.Reflection.MethodInfo curMethod in createForType.GetMethods( ) )
			{
				if ( curMethod.GetCustomAttributes( typeof( ComponentMessageHandlerAttribute ), false ).Length != 0 )
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

	/// <summary>
	/// Test
	/// </summary>
	public abstract class Message
	{
		/// <summary>
		/// Reliable delivery means ordered and acknowledged
		/// </summary>
		public bool				ReliableDelivery
		{
			get { return m_Reliable;	}
			set { m_Reliable = value;	}
		}

		/// <summary>
		/// Unreliable delivery means no duplicate messages only
		/// </summary>
		public bool				UnreliableDelivery
		{
			get { return !m_Reliable;	}
			set { m_Reliable = !value;	}
		}

		/// <summary>
		/// Gets the type identifier of this message
		/// </summary>
		public abstract ushort	TypeId
		{
			get;
		}

		/// <summary>
		/// Creates a message from a type ID
		/// </summary>
		/// <param name="typeId"> Message type identifier </param>
		/// <returns> Returns a new message of the specified type </returns>
		public static Message	CreateFromTypeId( ushort typeId )
		{
			return null;
		}

		/// <summary>
		/// Writes a message to the specified output stream
		/// </summary>
		/// <param name="output">Output stream</param>
		/// <remarks>
		/// This implementation writes the message type ID to the stream
		/// </remarks>
		public virtual void		Write( System.IO.BinaryWriter output )
		{
			output.Write( TypeId );
		}

		/// <summary>
		/// Reads a message from the specified stream
		/// </summary>
		/// <param name="input"> Input stream </param>
		protected virtual void	Read( System.IO.BinaryReader input )
		{
		}

		/// <summary>
		/// Reads a message from the specified stream
		/// </summary>
		/// <param name="input">Input stream</param>
		/// <returns>Returns the new message</returns>
		public static Message	ReadMessage( System.IO.BinaryReader input )
		{
			Message msg = CreateFromTypeId( input.ReadUInt16( ) );
			msg.Read( input );
			return msg;
		}

		private bool	m_Reliable = true;
	}

	/// <summary>
	/// Handy base class that implements IInstanceable, IParentObject and INamedObject (TODO)
	/// </summary>
	public class Component : IInstanceable, IParentObject
	{
		#region	Messaging

		//	TODO: Add nice reflection based thing
		//
		//	public static MessageMap GetMessageMap( )
		//	{
		//		return ...;
		//	}
		//	[ComponentMessageHandler()]
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
		[ ComponentMessageHandler( ) ]
		public void OnDamage( Message msg )
		{
		}

		/// <summary>
		/// Test
		/// </summary>
		public void	HandleMessage( Object msg )
		{
		}

		#endregion

		#region	IParentObject Members

		/// <summary>
		/// Adds a child to the child object list
		/// </summary>
		/// <param name="childObject"> Child object </param>
		public void			AddChild( Object childObject )
		{
			m_Children.Add( childObject );
		}

		/// <summary>
		/// Visits all children, calling visitor() for each
		/// </summary>
		/// <param name="visitor">Visitor function</param>
		public void VisitChildren( ChildVisitorDelegate visitor )
		{
			for ( int childIndex = 0; childIndex < m_Children.Count; ++childIndex )
			{
				if ( !visitor( m_Children[ childIndex ] ) )
				{
					return;
				}
			}
		}

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

		#region IInstanceable Members

		/// <summary>
		/// Creates a new instance of the derived type of this component, then tries to create instances of all the child objects
		/// </summary>
		/// <returns> Returns the new instance </returns>
		public object CreateInstance( )
		{
			Component clone = ( Component )Activator.CreateInstance( GetType( ) );
			InstanceChildren( clone );
			return clone;
		}

		#endregion

		/// <summary>
		/// Helper method for CreateInstance() - instances all child objects belonging to this component
		/// </summary>
		/// <param name="instance"> The instance to add child instances to</param>
		/// <remarks>
		/// If a child object of this component implements IInstanceable, then IInstanceable.CreateInstance() is called to instance it. If a child object
		/// of this component implements ICloneable, then ICloneable.Clone() is called to instance it. If a child object implements neither
		/// of these interfaces, a reference to the existing child object is added to the instance component
		/// </remarks>
		protected void InstanceChildren( Component instance )
		{
			foreach ( Object curChild in m_Children )
			{
				if ( curChild is IInstanceable )
				{
					instance.AddChild( ( ( IInstanceable )curChild ).CreateInstance( ) );
				}
				else if ( curChild is ICloneable )
				{
					instance.AddChild( ( ( ICloneable )curChild ).Clone( ) );
				}
				else
				{
					Output.WriteLineCall( Output.ComponentWarning, "Could not instance child node of type \"{0}\"", curChild.GetType( ).Name );
					instance.AddChild( curChild );
				}
			}
		}

		private ArrayList	m_Children = new ArrayList( );
	}
}
