using System;
using System.Diagnostics;
using NUnit.Framework;

using Rb.Core.Components;
using Rb.ComponentXmlLoader;

namespace Rb.ComponentXmlLoader.Tests
{
    /// <summary>
    /// Test fixture for Rb.ComponentXmlLoader.Loader
    /// </summary>
    [TestFixture]
    public class TestLoader
    {
        public static int Definition = 0;

        public class Root : Node, IUnique
        {
            public Root( )
            {
                Assert.AreEqual( Definition++, 0 );
            }

			public object Call( )
			{
				return this;
			}

			public object Call( string val )
			{
				Assert.AreEqual( val, "hello" );
				return this;
			}

			public Guid Id
			{
				get { return m_Id; }
				set
				{
                    Assert.AreEqual( Definition++, 1 );
				    m_Id = value;
				}
			}

            public int Value
            {
                set
                {
                    Assert.AreEqual( value, 10 );
                    Assert.AreEqual( Definition++, 2 );
                }
            }

			private Guid m_Id = Guid.Empty;
        }
        
        private string GuidString = "{D7743458-D782-4f34-A9E0-082399F12A21}";

        [Test]
        public void TestSimple( )
        {
            Guid id = new Guid( GuidString );
            string content =
                @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                @"<rb>" +
                @"  <object type=""Rb.ComponentXmlLoader.Tests.TestLoader+Root"" id=""" + GuidString + @""">" +
                @"      <int value=""10"" property=""Value""/>" +
                @"  </object>" +
                @"</rb>" +
                "";

            Loader loader = new Loader( );

            byte[] contentBytes = System.Text.Encoding.ASCII.GetBytes( content );

            Definition = 0;

            ComponentLoadParameters parameters = new ComponentLoadParameters( );

			bool canCache;
			loader.Load( new System.IO.MemoryStream( contentBytes ), new StackFrame( 0, true ).GetFileName( ), out canCache, parameters );

            Assert.IsTrue( parameters.Objects.ContainsKey( id ) );

            object obj = parameters.Objects[ id ];
            Assert.IsInstanceOfType( typeof( Root ), obj );
        }

		[Test]
		public void TestCall( )
		{
			Guid id = new Guid( GuidString );
			string content =
				@"<?xml version=""1.0"" encoding=""utf-8""?>" +
				@"<rb>" +
				@"  <object type=""Rb.ComponentXmlLoader.Tests.TestLoader+Root"" id=""" + GuidString + @""">" +
				@"      <method objectId=""" + GuidString + @""" call=""Call""/>" +
				@"      <method objectId=""" + GuidString + @""" call=""Call"">" +
				@"			<parameters>" +
				@"				<string value=""hello""/>" +
				@"			</parameters>" +
				@"		</method>" +
				@"  </object>" +
				@"</rb>" +
				"";

			Loader loader = new Loader( );

			byte[] contentBytes = System.Text.Encoding.ASCII.GetBytes( content );

			Definition = 0;

			ComponentLoadParameters parameters = new ComponentLoadParameters( );

			bool canCache;
			loader.Load( new System.IO.MemoryStream( contentBytes ), new StackFrame( 0, true ).GetFileName( ), out canCache, parameters );

			Assert.IsTrue( parameters.Objects.ContainsKey( id ) );

			object obj = parameters.Objects[ id ];
			Assert.IsInstanceOfType( typeof( Root ), obj );
		}
    }
}
