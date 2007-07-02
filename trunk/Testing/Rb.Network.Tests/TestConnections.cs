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
			public int m_Value;

			public TestMessageContent( int value )
			{
				m_Value = value;
			}
		}

		[Serializable]
		private class TestMessage : Message
		{
			public TestMessageContent m_Content = new TestMessageContent( 10 );
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

			connection.DeliverMessage( new TestMessage( ) );
			connection.DeliverMessage( new TestMessage( ) );

			while ( connections.ConnectionCount == 0 );

			foreach ( IConnection curConnection in connections )
			{
				curConnection.ReceivedMessage += MessageChecker;
				curConnection.ReceiveMessages( );
			}

			connection.Disconnect( );

			listener.Dispose( );
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

			connection.DeliverMessage( new TestMessage( ) );

			connections.GetConnection( 0 ).Disconnect( );
			connections.ReceiveMessages( );

			connection.Disconnect( );
		}
	}
}
