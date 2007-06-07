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

			public Guid Id
			{
				get { return m_Id; }
				set { m_Id = value; }
			}

            public int Value
            {
                set
                {
                    Assert.AreEqual( value, 10 );
                    Assert.AreEqual( Definition++, 1 );
                }
            }

			private Guid m_Id = Guid.Empty;
        }

        [Test]
        public void TestSimple( )
        {
            string content =
                @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                @"<rb>" +
                @"  <object type=""Rb.ComponentXmlLoader.Tests.TestLoader+Root"">" +
                @"      <int value=""10"" property=""Value""/>" +
                @"  </object>" +
                @"</rb>" +
                "";

            Loader loader = new Loader( );

            byte[] contentBytes = System.Text.Encoding.ASCII.GetBytes( content );

            Definition = 0;
            loader.Load( new System.IO.MemoryStream( contentBytes ), new StackFrame( 0, true ).GetFileName( ) );
        }
    }
}
