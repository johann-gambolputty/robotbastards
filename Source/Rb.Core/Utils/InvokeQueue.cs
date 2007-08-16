using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Utils
{
	/// <summary>
	/// An invoke queue is a list of delegates that can be invoked
	/// </summary>
	/// <remarks>
	/// The main purpose of this is to provide a platform independent version of Dispatcher.
	/// See <see cref="Rb.Core.Assets.AsyncLoadResult"/> for an example of usage.
	/// </remarks>
	public class InvokeQueue
	{
		/// <summary>
		/// Gets the InvokeQueue associated with the current thread
		/// </summary>
		public static InvokeQueue Instance
		{
			get { return ms_Singleton; }
		}

		/// <summary>
		/// Adds a delegate to the queue
		/// </summary>
		public void Add( Delegate del )
		{
			lock ( m_Calls )
			{
				m_Calls.Add( new CallData( del, new object[ 0 ] ) );
			}
		}

		/// <summary>
		/// Adds a delegate to the queue
		/// </summary>
		public void Add( Delegate del, params object[] parameters )
		{
			lock ( m_Calls )
			{
				m_Calls.Add( new CallData( del, parameters ) );
			}
		}

		/// <summary>
		/// Invokes all the delegates in the queue. Clears the queue
		/// </summary>
		public void Invoke( )
		{
			lock ( m_Calls )
			{
				foreach ( CallData call in m_Calls )
				{
					call.Invoke( );
				}
				m_Calls.Clear( );
			}
		}

		/// <summary>
		/// Stores data about a delegate call. Created by <see cref="InvokeQueue.Add"/>
		/// </summary>
		private class CallData
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="del"></param>
			/// <param name="parameters"></param>
			public CallData( Delegate del, object[] parameters )
			{
				m_Delegate = del;
				m_Parameters = parameters;
			}

			/// <summary>
			/// Calls the stored delegate
			/// </summary>
			public void Invoke( )
			{
				m_Delegate.DynamicInvoke( m_Parameters );
			}

			private readonly Delegate m_Delegate;
			private readonly object[] m_Parameters;
		}

		private readonly List< CallData > m_Calls = new List< CallData >( );

		[ThreadStatic]
		private static readonly InvokeQueue ms_Singleton = new InvokeQueue( );
	}
}
