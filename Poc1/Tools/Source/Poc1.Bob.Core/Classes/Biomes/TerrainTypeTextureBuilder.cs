using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics=Rb.Rendering.Graphics;

namespace Poc1.Bob.Core.Classes.Biomes
{
	/// <summary>
	/// Singleton class used for building terrain type lookup textures asynchronously
	/// </summary>
	public class TerrainTypeTextureBuilder : IDisposable
	{
		/// <summary>
		/// Delegate used by the texture build finished events
		/// </summary>
		/// <param name="texture">New texture</param>
		public delegate void TextureBuiltDelegate( ITexture2d texture );

		/// <summary>
		/// Event raised on the UI thread when lookup texture has been built
		/// </summary>
		public event TextureBuiltDelegate LookupTextureReady;

		/// <summary>
		/// Event raised on the UI thread when pack texture has been built
		/// </summary>
		public event TextureBuiltDelegate PackTextureReady;

		/// <summary>
		/// Default constructor. Creates a background thread to process new texture requests
		/// </summary>
		public TerrainTypeTextureBuilder( TerrainTypeList terrainTypes )
		{
			Arguments.CheckNotNull( terrainTypes , "terrainTypes");
			m_Worker = new BackgroundWorker( );
			m_Worker.DoWork += ProcessRequest;
			m_Worker.RunWorkerCompleted += RequestComplete;
			m_LookupTexture = RbGraphics.Factory.CreateTexture2d( );
			m_PackTexture = RbGraphics.Factory.CreateTexture2d( );
			
			TerrainTypes = terrainTypes;
		}

		/// <summary>
		/// Shuts down the worker thread
		/// </summary>
		~TerrainTypeTextureBuilder( )
		{
			if ( m_Worker == null )
			{
				return;
			}
			//	TODO: AP: Need to shut down this background thread explicitly, as it
			//	handles unmanaged resources
			while ( m_Worker.IsBusy )
			{
				Thread.Sleep( 1 );
				Application.DoEvents( );
			}
			m_Worker.Dispose( );
			m_Worker = null;
		}

		/// <summary>
		/// Gets the lookup texture
		/// </summary>
		public ITexture2d LookupTexture
		{
			get { return m_LookupTexture; }
		}

		/// <summary>
		/// Gets the pack texture
		/// </summary>
		public ITexture2d PackTexture
		{
			get { return m_PackTexture; }
		}

		/// <summary>
		/// Rebuilds the lookup and pack textures
		/// </summary>
		public void Rebuild( bool rebuildLookupTexture, bool rebuildPackTexture )
		{
			if ( rebuildLookupTexture )
			{
				++m_LookupTextureRebuildRequests;
			}
			if ( rebuildPackTexture )
			{
				++m_PackTextureRebuildRequests;
			}
			if ( !m_Worker.IsBusy )
			{
				m_Worker.RunWorkerAsync( );
			}
		}

		/// <summary>
		/// Gets/sets the terrain type set used generate the textures
		/// </summary>
		public TerrainTypeList TerrainTypes
		{
			get { return m_TerrainTypes; }
			set
			{
				Arguments.CheckNotNull( value, "value" );
				if ( m_TerrainTypes != null )
				{
					m_TerrainTypes.TerrainTypeAdded -= OnTerrainTypeCountChanged;
					m_TerrainTypes.TerrainTypeRemoved -= OnTerrainTypeCountChanged;
				}
				m_TerrainTypes = value;
				m_TerrainTypes.TerrainTypeAdded += OnTerrainTypeCountChanged;
				m_TerrainTypes.TerrainTypeRemoved += OnTerrainTypeCountChanged;
				Rebuild( true, true );
			}
		}


		#region Private Members

		private int m_LookupTextureRebuildRequests;
		private int m_PackTextureRebuildRequests;
		private TerrainTypeList m_TerrainTypes;
		private BackgroundWorker m_Worker;
		private readonly ITexture2d m_LookupTexture;
		private readonly ITexture2d m_PackTexture;
		private Bitmap m_LookupBitmap;
		private Bitmap[] m_PackBitmaps;

		private void OnTerrainTypeCountChanged( TerrainType type )
		{
			Rebuild( false, true );
		}

		/// <summary>
		/// Process a work request from the worker thread
		/// </summary>
		private void ProcessRequest( object sender, DoWorkEventArgs args )
		{
			if ( m_LookupTextureRebuildRequests > 0 )
			{
				m_LookupBitmap = m_TerrainTypes.CreateDistributionBitmap( );
			}
			if ( args.Cancel )
			{
				return;
			}
			if ( m_PackTextureRebuildRequests > 0 )
			{
				m_PackBitmaps = m_TerrainTypes.CreateTerrainPackBitmapMipMaps( );
			}
		}

		/// <summary>
		/// Called (on the UI thread, most likely), when a work request has been completed
		/// </summary>
		private void RequestComplete( object sender, RunWorkerCompletedEventArgs args )
		{
			if ( m_LookupTextureRebuildRequests > 0 )
			{
				m_LookupTexture.Create( m_LookupBitmap, false );
				m_LookupBitmap.Dispose( );

				if ( LookupTextureReady != null )
				{
					LookupTextureReady( m_LookupTexture );
				}

				m_LookupBitmap = null;
				--m_LookupTextureRebuildRequests;
			}
			if ( m_PackTextureRebuildRequests > 0 )
			{
				//for ( int level = 0; level < m_PackBitmaps.Length; ++level )
				//{
				//    m_PackBitmaps[ level ].Save( "pack" + level + ".png" );
				//}

				m_PackTexture.Create( m_PackBitmaps );

				if ( PackTextureReady != null )
				{
					PackTextureReady( m_PackTexture );
				}

				foreach ( Bitmap packBitmapMipMap in m_PackBitmaps )
				{
					packBitmapMipMap.Dispose( );
				}

				m_PackBitmaps = null;
				--m_PackTextureRebuildRequests;
			}
			if ( ( m_LookupTextureRebuildRequests > 0 ) || ( m_PackTextureRebuildRequests > 0 ) )
			{
				m_Worker.RunWorkerAsync( );
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Shuts down the worker thread
		/// </summary>
		public void Dispose( )
		{
			if ( m_Worker.IsBusy )
			{
				m_Worker.CancelAsync( );
				while ( m_Worker.IsBusy )
				{
					Thread.Sleep( 10 );
				}
				m_Worker.Dispose( );
			}
		}

		#endregion
	}
}
