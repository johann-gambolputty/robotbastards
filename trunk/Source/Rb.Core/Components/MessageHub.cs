using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using Rb.Core.Utils;

namespace Rb.Core.Components
{
	/// <summary>
	/// Handy implementation of IMessageHub
	/// </summary>
	public class MessageHub : IMessageHub
	{
		#region IMessageHub Members

		/// <summary>
		/// Adds a recipient for messages of, or derived from, a given type
		/// </summary>
		/// <param name="messageType">Message type</param>
		/// <param name="recipient">Delegate to call when a message of the designated type arrives</param>
		/// <param name="order">Recipient order <see cref="MessageRecipientOrder"/></param>
		public void AddRecipient( Type messageType, MessageRecipientDelegate recipient, int order )
		{
			bool foundExactMatch = false;
			foreach ( MessageRecipientChain chain in m_Chains )
			{
				if ( DoMessageTypesMatch( messageType, chain.MessageType ) )
				{
					foundExactMatch |= messageType == chain.MessageType;
					chain.AddRecipient( recipient, order );
				}
			}
			if ( !foundExactMatch )
			{
				MessageRecipientChain chain = new MessageRecipientChain( messageType );
				chain.AddRecipient( recipient, order );
                m_Chains.Add( chain );
			}
		}

		/// <summary>
		///	Removes a recipient from the message hub
		/// </summary>
		/// <param name="messageType">Type of message</param>
		/// <param name="obj">Object to remove</param>
		public void RemoveRecipient( Type messageType, object obj )
		{
			foreach ( MessageRecipientChain chain in m_Chains )
			{
				if ( DoMessageTypesMatch( messageType, chain.MessageType ) )
				{
					chain.RemoveRecipient( obj );
				}
			}
		}

		/// <summary>
		/// Sends a message to all recipients of that message type
		/// </summary>
		public void DeliverMessageToRecipients( Message msg )
		{
			Type messageType = msg.GetType( );
			foreach ( MessageRecipientChain chain in m_Chains )
			{
				if ( DoMessageTypesMatch( chain.MessageType, messageType ) )
				{
					chain.Deliver( msg );
				}
			}
		}
		
		#endregion

        #region IMessageHub helpers

        /// <summary>
        /// Searches an object for [Dispatch]-attributed methods. If one takes the supplied message type (exact match only)
        /// the method is added as a recipient to the message hub
        /// </summary>
        /// <param name="hub">Message hub to add recipient to</param>
        /// <param name="messageType">Message type that recipient must handle</param>
        /// <param name="obj">Target object containing [Dispatch]-attributed methods</param>
        /// <param name="order">Recipient order</param>
        public static void AddDispatchRecipient( IMessageHub hub, Type messageType, object obj, int order )
        {
            MethodInfo[] methods = obj.GetType( ).GetMethods( );
            foreach ( MethodInfo method in methods )
            {
                if ( method.GetCustomAttributes( typeof( DispatchAttribute ), false ).Length == 1 )
                {
                    ParameterInfo[] parameters = method.GetParameters( );
                    if ( ( parameters.Length == 1 ) && ( parameters[ 0 ].ParameterType == messageType ) )
                    {
                        AddRecipient( hub, messageType, obj, method, order );
                        return;
                    }
                }
            }
            string err = string.Format( "Can't create dispatch message recipient - no dispatch method in type \"{0}\" takes one parameter of type \"{1}\"", obj.GetType( ), messageType );
            throw new ApplicationException( err );
        }

        /// <summary>
        /// Searches an object for methods with a given name. If one takes the supplied message type (exact match only)
        /// the method is added as a recipient to the message hub
        /// </summary>
        /// <param name="hub">Message hub to add recipient to</param>
        /// <param name="messageType">Message type that recipient must handle</param>
        /// <param name="obj">Target object containing named methods</param>
        /// <param name="methodName">Method name to search for</param>
        /// <param name="order">Recipient order</param>
        public static void AddNamedRecipient( IMessageHub hub, Type messageType, object obj, string methodName, int order )
        {
            MethodInfo[] methods = obj.GetType( ).GetMethods( );
            foreach ( MethodInfo method in methods )
            {
                if ( method.Name == methodName )
                {
                    ParameterInfo[] parameters = method.GetParameters( );
                    if ( ( parameters.Length == 1 ) && ( parameters[ 0 ].ParameterType == messageType ) )
                    {
                        AddRecipient( hub, messageType, obj, method, order );
                        return;
                    }
                }
            }
            string err = string.Format( "Can't create named message recipient - no method named \"{0}\" in type \"{1}\" takes one parameter of type \"{2}\"", methodName, obj.GetType( ), messageType );
            throw new ApplicationException( err );
        }

        /// <summary>
        /// Adds a supplied method as a recipient to a message hub
        /// </summary>
        /// <param name="hub">Message hub to add recipient to</param>
        /// <param name="messageType">Message type that recipient must handle</param>
        /// <param name="obj">Target object containing method</param>
        /// <param name="method">Message handling method</param>
        /// <param name="order">Recipient order</param>
        public static void AddRecipient( IMessageHub hub, Type messageType, object obj, MethodInfo method, int order )
        {
			if ( method.ReturnType != typeof( MessageRecipientResult ) )
			{
				throw new ArgumentException( string.Format( "Method \"{0}\" cannot be added to a message hub - it does not have a return type of \"MessageRecipientResult\"", method.Name ) );
			}

            //  TODO: AP: Assumes parameter type is correct
            DynamicMethod recipientMethod;
            if ( method.IsStatic )
            {
                recipientMethod = new DynamicMethod( "CallRecipient", typeof( MessageRecipientResult ), new Type[] { typeof( Message ) }, obj.GetType( ), true );
            }
            else
            {
                recipientMethod = new DynamicMethod( "CallRecipient", typeof( MessageRecipientResult ), new Type[] { obj.GetType( ), typeof( Message ) }, obj.GetType( ), true );   
            }
            ILGenerator generator = recipientMethod.GetILGenerator( );
            
            if ( method.IsStatic )
            {
                generator.Emit( OpCodes.Ldarg_0 );                  //  Load argument 0
                generator.Emit( OpCodes.Castclass, messageType );   //  Cast argument 0 to the message type
                generator.Emit( OpCodes.Call, method );             //  Call static method
            }
            else
            {
                generator.Emit( OpCodes.Ldarg_0 );                  //  Load argument 0 (this)
                generator.Emit( OpCodes.Ldarg_1 );                  //  Load argument 1
                generator.Emit( OpCodes.Castclass, messageType );   //  Cast argument 1 to the message type
                if ( method.IsVirtual || method.IsAbstract )
                {
                    generator.Emit( OpCodes.Callvirt, method );     //  Call virtual method
                }
                else
                {
                    generator.Emit( OpCodes.Call, method );         //  Call method
                }
            }

            generator.Emit( OpCodes.Ret );							//  Return instruction

            MessageRecipientDelegate recipientDelegate;
            if ( method.IsStatic )
            {
                recipientDelegate = ( MessageRecipientDelegate )recipientMethod.CreateDelegate( typeof( MessageRecipientDelegate ) );
            }
            else
            {
                recipientDelegate = ( MessageRecipientDelegate )recipientMethod.CreateDelegate( typeof( MessageRecipientDelegate ), obj );
            }

            hub.AddRecipient( messageType, recipientDelegate, order );
        }

        #endregion

        #region Private stuff

        private List< MessageRecipientChain > m_Chains = new List< MessageRecipientChain >( );
		
		/// <summary>
		/// Returns true if 2 classes are equal, or subclass is derived from baseClass
		/// </summary>
		private static bool DoMessageTypesMatch( Type baseClass, Type subClass )
		{
			return ( baseClass == subClass ) || ( subClass.IsSubclassOf( baseClass ) );
		}
		
		#endregion

	}
}
