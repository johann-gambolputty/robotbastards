using System;
using System.Xml;
using NUnit.Framework;
using Rb.Core.Assets;

namespace Rb.Core.Tests
{
	[TestFixture]
	public class TestAssets
	{
		private readonly LocationManagers m_LocationManagers = new LocationManagers( );

		[TestFixtureSetUp]
		public void Startup( )
		{
			m_LocationManagers.Add( new FileLocationManager( ) );
		}

		/// <summary>
		/// Tests the <see cref="AssetCache"/>
		/// </summary>
		[Test]
		public void TestAssetCache( )
		{
			AssetCache cache = new AssetCache( );
			int key = new Location( m_LocationManagers, "blarg" ).Key;

			//	Add an asset to the cache
			object asset = new object( );
			cache.Add( key, asset );

			//	Make sure that it can be retrived from its key
			Assert.AreEqual( asset, cache.Find( key ) );

			//	Collect garbage (will clean up asset)
			asset = null;
			GC.Collect( );

			//	Make sure that the asset is no longer in the cache
			Assert.AreEqual( cache.Find( key ), asset );
		}
		
		private const string XmlContent =
				@"<?xml version=""1.0"" encoding=""utf-8""?>" +
				@"<rb>" +
				@"  <object/>" +
				@"</rb>" +
				"";

		/// <summary>
		/// Checks the output of an <see cref="XmlAssetLoader"/> that was loaded from XmlContent
		/// </summary>
		private static void CheckDocument( XmlNode doc )
		{
			Assert.AreEqual( ( ( XmlDeclaration )doc.ChildNodes[ 0 ] ).Version, "1.0" );
			Assert.AreEqual( ( ( XmlDeclaration )doc.ChildNodes[ 0 ] ).Encoding, "utf-8" );

			Assert.AreEqual( doc.ChildNodes[ 1 ].Name, "rb" );
			Assert.AreEqual( doc.ChildNodes[ 1 ].ChildNodes[ 0 ].Name, "object" );
		}

		/// <summary>
		/// Tests loading a simple XML asset from a <see cref="StreamSource"/>
		/// </summary>
		[Test]
		public void TestLoad( )
		{
			AssetManager assets = new AssetManager( );
			assets.AddLoader( new XmlAssetLoader( 0 ) );

			XmlDocument doc = ( XmlDocument )assets.Load( new StreamSource( XmlContent, "test.xml" ) );
			CheckDocument( doc );
		}

		/// <summary>
		/// Tests loading a simple XML asset from a <see cref="StreamSource"/>, using asynchronous loading
		/// </summary>
		[Test]
		public void TestAsyncLoad( )
		{
			AssetManager assets = new AssetManager( );
			assets.AddLoader( new XmlAssetLoader( 10 ) );

			using ( AsyncAssetLoader loader = new AsyncAssetLoader( ) )
			{
				int success = 0;

				int numLoads = 8;
				AsyncLoadResult[] results = new AsyncLoadResult[ numLoads ];

				for ( int loadCount = 0; loadCount < numLoads; ++loadCount )
				{
					AsyncLoadResult result = loader.QueueLoad( assets, new StreamSource( XmlContent, "test.xml" ), null, LoadPriority.High );
					result.AddLoadCompleteCallback(
						delegate( object asset )
							{
								CheckDocument( ( XmlNode )asset );
								++success;
							},
						false );

					results[ loadCount ] = result;
				}

				for ( int loadCount = 0; loadCount < numLoads; ++loadCount )
				{
					results[ loadCount ].WaitUntilComplete( new TimeSpan( 0, 0, 2 ) );
				}

				Assert.AreEqual( success, numLoads );
			}
		}

		/// <summary>
		/// Simple XML asset loader
		/// </summary>
		private class XmlAssetLoader : AssetLoader
		{
			public XmlAssetLoader( int sleepAfterLoad )
			{
				m_SleepAfterLoad = sleepAfterLoad;
			}

			/// <summary>
			/// Loads an asset
			/// </summary>
			/// <param name="source">Source of the asset</param>
			/// <param name="parameters">Load parameters</param>
			/// <returns>Loaded asset</returns>
			public override object Load( ISource source, LoadParameters parameters )
			{
				XmlDocument doc = new XmlDocument( );
				doc.Load( source.Open( ) );

				if ( m_SleepAfterLoad > 0 )
				{
					System.Threading.Thread.Sleep( m_SleepAfterLoad );
				}
				return doc;
			}

			/// <summary>
			/// Returns true if this loader can load the asset at the specified location
			/// </summary>
			/// <param name="source">Asset source</param>
			/// <returns>Returns the </returns>
			public override bool CanLoad( ISource source )
			{
				return source.HasExtension( "xml" );
			}

			private readonly int m_SleepAfterLoad;
		}

	}
}
