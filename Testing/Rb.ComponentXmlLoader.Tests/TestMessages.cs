
using NUnit.Framework;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Rb.ComponentXmlLoader.Tests
{
    [TestFixture]
    public class TestMessages
    {
        private class Message0 : Message
        {
            public MethodId HandledBy = MethodId.NoMethod;
        }

        private class Message1 : Message0
        {
        }

        private class Message2 : Message1
        {
        }

        private class Message3 : Message0
        {
        }

        private enum MethodId
        {
            NoMethod,
            TestComponent_Handle_Message0,
            TestComponent_Handle_Message1,
            TestComponent_Handle_Message2,
            TestComponent_Handle_Message3,
        }

        private class TestComponent : Component
        {
            [Dispatch]
            public MessageRecipientResult Handle( Message0 msg )
            {
                msg.HandledBy = MethodId.TestComponent_Handle_Message0;
                return MessageRecipientResult.DeliverToNext;
            }

            [Dispatch]
            public MessageRecipientResult Handle( Message1 msg )
            {
                msg.HandledBy = MethodId.TestComponent_Handle_Message1;
                return MessageRecipientResult.DeliverToNext;
            }

            [Dispatch]
            public static MessageRecipientResult Handle( Message3 msg )
            {
                msg.HandledBy = MethodId.TestComponent_Handle_Message3;
                return MessageRecipientResult.DeliverToNext;
            }
        }

        [Test]
        public void TestHandleMessage( )
        {
            IMessageHandler handler = new TestComponent( );

            TestSenderMessage( handler, new Message0( ), MethodId.TestComponent_Handle_Message0 );
            TestSenderMessage( handler, new Message1( ), MethodId.TestComponent_Handle_Message1 );
            TestSenderMessage( handler, new Message2( ), MethodId.TestComponent_Handle_Message1 );
            TestSenderMessage( handler, new Message3( ), MethodId.TestComponent_Handle_Message3 );
        }

        private static void CheckExpectedHandler(Message0 msg, MethodId expectedHandler)
        {
            Assert.AreEqual( msg.HandledBy, expectedHandler );
        }

        private static void TestSenderMessage( IMessageHandler handler, Message0 msg, MethodId expectedHandler )
        {
            handler.HandleMessage( msg );
            CheckExpectedHandler( msg, expectedHandler );
        }

        private static void TestHubMessage( IMessageHub hub, Message0 msg, MethodId expectedHandler )
        {
            hub.DeliverMessageToRecipients( msg );
            CheckExpectedHandler( msg, expectedHandler );
        }

        [Test]
        public void TestMessageHub( )
        {
            Component hub = new Component( );
            TestComponent handler = new TestComponent( );

            MessageHub.AddDispatchRecipient( hub, typeof( Message0 ), handler, 0 );
            MessageHub.AddDispatchRecipient( hub, typeof( Message1 ), handler, 0 );
            //  No dispatch method takes parameter of type Message2
            MessageHub.AddDispatchRecipient( hub, typeof( Message3 ), handler, 0 );

            TestHubMessage( hub, new Message0( ), MethodId.TestComponent_Handle_Message0 );
            TestHubMessage( hub, new Message1( ), MethodId.TestComponent_Handle_Message1 );
            TestHubMessage( hub, new Message2( ), MethodId.TestComponent_Handle_Message1 );
            TestHubMessage( hub, new Message3( ), MethodId.TestComponent_Handle_Message3 );
        }

    }
}
