using System;
using NUnit.Framework;

using Rb.Network;
using Rb.Core.Components;


namespace Rb.Network.Tests
{
	[TestFixture]
	public class TestConnections
	{
		[Test]
		public void TestTcpClientConnectionListener0( )
		{
			const string connect = "127.0.0.1";
			const int port = 3000;

			Connections connections = new Connections( );

			TcpConnectionListener listener = new TcpConnectionListener( connect, port );
			listener.Listen( connections );

			listener.Dispose( );
		}

		[Test]
		public void TestTcpClientConnectionListener1( )
		{
			const string connect = "127.0.0.1";
			const int port = 3001;

			Connections connections = new Connections( );

			TcpConnectionListener listener = new TcpConnectionListener( connect, port );
			listener.Listen( connections );

			TcpSocketConnection connection = new TcpSocketConnection( connect, port );
			connection.OpenConnection( );

			connection.Disconnect( );

			connections.DisconnectAll( );

			listener.Dispose( );
		}

		[Serializable]
		private class TestMessageContent
		{
			public int  m_Value;

			public TestMessageContent( int value )
			{
				m_Value = value;
			}
		}

		[Serializable]
		private class TestMessage : Message
		{
		    public TestMessageContent m_Content;
            
            public TestMessage( int value ) { m_Content = new TestMessageContent( value ); }
		}

		private static void MessageChecker( IConnection connection, Message msg )
		{
			Assert.AreEqual( ( ( TestMessage )msg ).m_Content.m_Value, 10 );
		}

		[Test]
		public void TestTcpClientConnectionListener2( )
		{
			const string connect = "127.0.0.1";
			const int port = 3002;

			Connections connections = new Connections( );

			TcpConnectionListener listener = new TcpConnectionListener( connect, port );
			listener.Listen( connections );

			TcpSocketConnection connection = new TcpSocketConnection( connect, port );
			connection.OpenConnection( );

			connection.DeliverMessage( new TestMessage( 10 ) );
			connection.DeliverMessage( new TestMessage( 10 ) );

			while ( connections.ConnectionCount == 0 );

			foreach ( IConnection curConnection in connections )
			{
				curConnection.ReceivedMessage += MessageChecker;
				curConnection.ReceiveMessages( );
			}

			connection.Disconnect( );

			listener.Dispose( );
		}

        [Serializable]
        private class DualRefMessage : Message
        {
            public TestMessage[] m_Messages = new TestMessage[ 2 ];

            public DualRefMessage( TestMessage msg0,  TestMessage msg1 )
            {
                m_Messages[ 0 ] = msg0;
                m_Messages[ 1 ] = msg1;
            }
        }

		[Test]
		public void TestTcpClientConnectionListener3( )
		{
			const string connect = "127.0.0.1";
			const int port = 3003;

			Connections connections = new Connections( );

			TcpConnectionListener listener = new TcpConnectionListener( connect, port );
			listener.Listen( connections );

			TcpSocketConnection connection = new TcpSocketConnection( connect, port );
			connection.OpenConnection( );

			while ( connections.ConnectionCount == 0 );

			listener.Dispose( );

            TestMessage payload0 = new TestMessage( 10 );
            //TestMessage payload1 = new TestMessage( 11 );
		    TestMessage payload1 = payload0;
            Message msg = new DualRefMessage( payload0, payload1 );
            connection.DeliverMessage( msg );

			connections.GetConnection( 0 ).Disconnect( );
			connections.ReceiveMessages( );

			connection.Disconnect( );
		}
	}
}
