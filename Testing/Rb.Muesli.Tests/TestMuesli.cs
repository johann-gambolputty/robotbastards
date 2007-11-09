using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;

namespace Rb.Muesli.Tests
{
    [TestFixture]
    public class TestMuesli
    {
        private static string BuildRandomString( Random rnd, IDictionary< string, string > uniqueMap )
        {
            int length = rnd.Next( 5, 20 );

            StringBuilder sb = new StringBuilder( length + 1 );
            string result;
            do
            {
                for ( int index = 0; index < length; ++index )
                {
                    sb.Append( ( char )rnd.Next( 'a', 'z' ) );
                }
                result = sb.ToString( );
            } while ( uniqueMap.ContainsKey( result ) );
            uniqueMap[ result ] = result;
            return result;
        }

        [Test]
        public void EnsureSerializationInfoWorksAsExpected( )
        {
            //  Muesli assumes that SerializationInfo stores members in order added, not in some crazy map

            Random rnd = new Random( );
            Dictionary< string, string > uniqueMap = new Dictionary< string, string >( );
            string[] dummyNames = new string[ 100 ];
            for ( int nameIndex = 0; nameIndex < dummyNames.Length; ++nameIndex )
            {
                dummyNames[ nameIndex ] = BuildRandomString( rnd, uniqueMap );
            }

            SerializationInfo info = new SerializationInfo( typeof( TestMuesli ), new FormatterConverter( ) );
            for ( int nameIndex = 0; nameIndex < dummyNames.Length; ++nameIndex )
            {
                info.AddValue( dummyNames[ nameIndex ], null );
            }

            {
                int nameIndex = 0;
                SerializationInfoEnumerator e = info.GetEnumerator();
                while ( e.MoveNext( ) )
                {
                    Assert.AreEqual( dummyNames[ nameIndex++ ], e.Name );
                }
            }
         }

        [Test]
        public void TestSimpleIntegerArrayIo()
        {
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

            for ( int i = 0; i < 10; ++i )
            {
                output.Write( i );
            }

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            for ( int i = 0; i < 10; ++i )
            {
                int value;
                input.Read( out value );
                Assert.AreEqual( value, i );
            }
        }

        [Test]
        public void Test1dArrayIo( )
        {
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

            Primitives[] outputObject = new Primitives[ 3 ];
            outputObject[ 0 ] = new Primitives( );
			outputObject[ 1 ] = null;
			outputObject[ 2 ] = new Primitives( );
            output.Write( ( object )outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

			Assert.AreEqual( outputObject, inputObject );
        }

		[Test]
		public void TestListIo( )
        {
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

            List< Primitives > outputObject = new List< Primitives >( );

            outputObject.Add( new Primitives( ) );
            outputObject.Add( new Primitives( ) );
            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

			//	TODO: AP: For some reason, Assert.AreEqual() isn't work on lists... I could have sworn it was last night :(
			//Assert.AreEqual( inputObject, outputObject );

			List< Primitives > inputList = ( List< Primitives > )inputObject;
			Assert.AreEqual( outputObject.Count, inputList.Count );
			for ( int i = 0; i < outputObject.Count; ++i )
			{
				Assert.AreEqual( inputList[ i ], outputObject[ i ] );
			}
        }

		[Test]
		public void TestDictionaryIo( )
		{
			MemoryStream stream = new MemoryStream( );
			BinaryOutput output = new BinaryOutput( stream );

			Dictionary< string, Primitives > outputObject = new Dictionary< string, Primitives >( );

			outputObject[ "jam" ] = new Primitives( );
			outputObject[ "pie" ] = new Primitives( );
			output.Write( outputObject );

			output.Finish( );

			stream.Seek( 0, SeekOrigin.Begin );
			BinaryInput input = new BinaryInput( stream );

			object inputObject;
			input.Read( out inputObject );
			input.Finish( );
			Dictionary< string, Primitives > inputDictionary = ( Dictionary< string, Primitives > )inputObject;

			//	TODO: AP: Ditto here - Assert.AreEqual() isn't working anymore on the dictionaries :(
			//Assert.AreEqual( outputObject, inputObject );
			Assert.AreEqual( inputDictionary.Count, outputObject.Count );

			foreach ( string key in outputObject.Keys )
			{
				Assert.AreEqual( inputDictionary[ key ], outputObject[ key ] );
			}
		}
		
		[Test]
		public void TestDictionaryIo2( )
		{
			MemoryStream stream = new MemoryStream( );
			BinaryOutput output = new BinaryOutput( stream );

			Dictionary< Type, object > outputObject = new Dictionary< Type, object >( );

			outputObject[ typeof( Primitives ) ] = new Primitives( );
			output.Write( outputObject );

			output.Finish( );

			stream.Seek( 0, SeekOrigin.Begin );
			BinaryInput input = new BinaryInput( stream );

			object inputObject;
			input.Read( out inputObject );
			input.Finish( );
			Dictionary< Type, object > inputDictionary = ( Dictionary< Type, object > )inputObject;

			//	TODO: AP: Ditto here - Assert.AreEqual() isn't working anymore on the dictionaries :(
			//Assert.AreEqual( outputObject, inputObject );
			Assert.AreEqual( inputDictionary.Count, outputObject.Count );

			foreach ( Type key in outputObject.Keys )
			{
			    Assert.AreEqual( inputDictionary[ key ], outputObject[ key ] );
			}
		}

		[Test]
		public void TestGenericIo( )
		{
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

			Test< string > outputObject = new Test< string >( "pie" );
            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            Assert.AreEqual( outputObject, inputObject );
		}

        [Test]
        public void TestSimpleObjectIo( )
        {
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

            Primitives outputObject = new Primitives( );
            outputObject.m_Value12 = "badgers";
            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            Assert.AreEqual( outputObject, inputObject );
        }


        [Test]
        public void TestNestedObjectIo()
        {
            MemoryStream stream = new MemoryStream( );
            BinaryOutput output = new BinaryOutput( stream );

            Wrapper outputObject = new Wrapper( );
            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            Assert.AreEqual( outputObject, inputObject );
        }

		[Serializable]
		private class MyNestedDictionary : NestedDictionary
		{
			
		}

		[Test]
		public void TestNestedDictionaryIo( )
		{
            MemoryStream stream = new MemoryStream( );
            BinaryOutput output = new BinaryOutput( stream );

			NestedDictionary outputObject = new MyNestedDictionary();
			output.Write( outputObject );
			output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );
			input.Finish( );
			( ( NestedDictionary )inputObject ).Check( );

		}

        [Test]
        public void TestRingIo()
        {
            MemoryStream stream = new MemoryStream( );
            BinaryOutput output = new BinaryOutput( stream );

            RingTest outputObject = new RingTest( );
            outputObject.m_Next = new RingTest( );
            outputObject.m_Next.m_Next = outputObject;

            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            RingTest curOutput = outputObject;
            RingTest curInput = ( RingTest )inputObject;

            do
            {
                Assert.AreEqual( curOutput.m_Value, curInput.m_Value );
                curOutput = curOutput.m_Next;
                curInput = curInput.m_Next;
            } while ( curOutput != outputObject );
        }

		[Test]
		public void TestSerializationEvents( )
		{
			MemoryStream stream = new MemoryStream( );
			IFormatter formatter = new BinaryFormatter( );

			formatter.Serialize( stream, new EventsTest( ) );
			stream.Seek( 0, SeekOrigin.Begin );
			EventsTest test = ( EventsTest )formatter.Deserialize( stream );
			EventsTest.CheckCalled( EventType.Serializing, true );
			EventsTest.CheckCalled( EventType.Serialized, true );
			EventsTest.CheckCalled( EventType.Deserializing, true );
			EventsTest.CheckCalled( EventType.Deserialized, true );
			EventsTest.CheckCalled( EventType.DeserializationCallback, true );
		}
		
		[Test]
		public void TestEventSerialization( )
		{
            MemoryStream stream = new MemoryStream( );
            BinaryOutput output = new BinaryOutput( stream );

			EventHandlers outputObject = new DerivedEventHandlers( );
			output.Write( outputObject );
			output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );
			input.Finish( );
			
			( ( EventHandlers )inputObject ).Invoke( );
		}

		#region Test Classes

		[Serializable]
		public class EventHandlers
		{
			public EventHandlers( )
			{
				SimpleEvent += SimpleEventHandler;
				GenericEvent += GenericEventHandler;
			}

			public void Invoke( )
			{
				SimpleEvent( this, null );
				GenericEvent( this, null );
			}

			public class MyEventArgs : EventArgs
			{
			}

			public event EventHandler SimpleEvent;
			public event EventHandler< MyEventArgs > GenericEvent;

			private static void SimpleEventHandler( object send, EventArgs args )
			{
			}
			
			private static void GenericEventHandler( object send, MyEventArgs args )
			{
			}
		}

		[Serializable]
		public class DerivedEventHandlers : EventHandlers
		{
		}

		/// <summary>
		/// Serialization event enumeration
		/// </summary>
		public enum EventType
		{
			Serializing,
			Serialized,
			Deserializing,
			Deserialized,
			DeserializationCallback,
			Count
		}

		/// <summary>
		/// Tests generic nested values
		/// </summary>
		/// <typeparam name="T"></typeparam>
		[Serializable]
		public struct Test<T> where T : IEquatable<T>
		{
			private readonly T m_Value;
			public Test(T val) { m_Value = val; }
			public override bool Equals(object obj)
			{
				Test<T> rhs = (Test<T>)obj;
				return m_Value.Equals(rhs.m_Value);
			}
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		/// <summary>
		/// Tests circular references
		/// </summary>
		[Serializable]
        private class RingTest
        {
            private static int ms_ValueInit = 0;
            public readonly int m_Value = ms_ValueInit++;
            public RingTest m_Next;
		}
		
		/// <summary>
		/// Tests nested generic dictionaries
		/// </summary>
		[Serializable]
		private class NestedDictionary
		{
			public NestedDictionary( )
			{
				m_Dictionary[ "key0" ] = 0;
				m_Dictionary[ "key1" ] = 1;
				m_Dictionary[ "key2" ] = 2;
				m_Dictionary[ "key3" ] = 3;
			}

			public void Check( )
			{
				Assert.AreEqual( m_Dictionary[ "key0" ], 0 );
				Assert.AreEqual( m_Dictionary[ "key1" ], 1 );
				Assert.AreEqual( m_Dictionary[ "key2" ], 2 );
				Assert.AreEqual( m_Dictionary[ "key3" ], 3 );
			}

			private readonly Dictionary< string, int > m_Dictionary = new Dictionary< string, int >( );
		}
		
		/// <summary>
		/// Makes sure that (de)serialization events are called in the right order
		/// </summary>
		[Serializable]
		public class EventsTest : IDeserializationCallback
		{
			private static readonly bool[] m_Called = new bool[ ( int )EventType.Count ];

			public static void CheckCalled( EventType eventType, bool called )
			{
				Assert.AreEqual( m_Called[ ( int )eventType ], called );
			}

			[OnSerializing]
			public void OnSerializing( StreamingContext context )
			{
				CheckCalled( EventType.Serialized, false );
				m_Called[ ( int )EventType.Serializing ] = true;
			}

			[OnSerialized]
			public void OnSerialized( StreamingContext context )
			{
				CheckCalled( EventType.Serializing, true );
				m_Called[ ( int )EventType.Serialized ] = true;
			}

			[OnDeserializing]
			public void OnDeserializing( StreamingContext context )
			{
				CheckCalled( EventType.Deserialized, false );
				m_Called[ ( int )EventType.Deserializing ] = true;
			}

			[OnDeserialized]
			public void OnDeserialized( StreamingContext context )
			{
				CheckCalled( EventType.Deserializing, true );
				m_Called[ ( int )EventType.Deserialized ] = true;
			}

			#region IDeserializationCallback Members

			public void OnDeserialization( object sender )
			{
				CheckCalled( EventType.Deserializing, true );
				CheckCalled( EventType.Deserialized, true );
				m_Called[ ( int )EventType.DeserializationCallback ] = true;
			}

			#endregion
		}

		#endregion
    }
}
