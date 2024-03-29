using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;
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
        
        private const string GuidString = "{D7743458-D782-4f34-A9E0-082399F12A21}";

		/// <summary>
		/// Creates a test asset source from a byte array
		/// </summary>
		private static ISource MemorySource( byte[] bytes )
		{
			return new StreamSource( new MemoryStream( bytes ), new StackFrame( 0, true ).GetFileName( ) );
		}

        [Test]
        public void TestEmpty()
        {
            try
            {
                byte[] contentBytes = System.Text.Encoding.ASCII.GetBytes( "" );

				new Loader( ).Load( MemorySource( contentBytes ), new ComponentLoadParameters( ) );

                Assert.Fail( "Expected empty test to fail and throw an ApplicationException" );
            }
            catch ( ApplicationException )
            {
            }
        }

        [Test]
        public void TestNoContent( )
        {
            try
            {
                byte[] contentBytes = System.Text.Encoding.ASCII.GetBytes( @"<?xml version=""1.0"" encoding=""utf-8""?><rb/>" );
				new Loader( ).Load( MemorySource( contentBytes ), new ComponentLoadParameters( ) );

                Assert.Fail( "Expected no content test to fail and throw an ApplicationException" );
            }
            catch ( ApplicationException )
            {
                
            }
        }

        [Test]
        public void TestDynamicProperties( )
        {
            string content =
                @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                @"<rb>" +
                @"  <object type=""Rb.Core.Components.Component"">" +
                @"      <int value=""10"" dynProperty=""intValue""/>" +
                @"      <string value=""badgers"" dynProperty=""strValue""/>" +
                @"  </object>" +
                @"</rb>" +
                "";

            byte[] contentBytes = System.Text.Encoding.ASCII.GetBytes( content );
			object result = new Loader( ).Load( MemorySource( contentBytes ), new ComponentLoadParameters( ) );
        
            Assert.AreEqual( ( ( ISupportsDynamicProperties )result ).Properties[ "intValue" ], 10 );
            Assert.AreEqual( ( ( ISupportsDynamicProperties )result ).Properties[ "strValue" ], "badgers" );
        }

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
			loader.Load( MemorySource( contentBytes ), parameters );

            Assert.IsTrue( parameters.Objects.ContainsKey( id ) );

            object obj = parameters.Objects[ id ];
            Assert.IsTrue( obj is Root );
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
			loader.Load( MemorySource( contentBytes ), parameters );

			Assert.IsTrue( parameters.Objects.ContainsKey( id ) );

			object obj = parameters.Objects[ id ];
			Assert.IsTrue( obj is Root );
		}
    }
}
