using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics=Rb.Rendering.Graphics;

namespace Poc1.PlanetBuilder
{
	/// <summary>
	/// Singleton class used for building terrain type lookup textures asynchronously
	/// </summary>
	public class TerrainTypeTextureBuilder
	{
		/// <summary>
		/// Gets the singleton
		/// </summary>
		public static TerrainTypeTextureBuilder Instance
		{
			get { return s_Instance; }
		}

		public TerrainTypeTextureBuilder( )
		{
			m_Worker = new BackgroundWorker( );
			m_Worker.DoWork += ProcessRequest;
			m_Worker.RunWorkerCompleted += RequestComplete;
			m_LookupTexture = RbGraphics.Factory.CreateTexture2d( );
			m_PackTexture = RbGraphics.Factory.CreateTexture2d( );
			
			TerrainTypes = new TerrainTypeSet( );
		}

		~TerrainTypeTextureBuilder( )
		{
			while ( m_Worker.IsBusy )
			{
				Thread.Sleep( 0 );
				Application.DoEvents( );
			}
			m_Worker.Dispose( );
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
		public TerrainTypeSet TerrainTypes
		{
			get { return m_TerrainTypes; }
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value" );
				}
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
		private TerrainTypeSet m_TerrainTypes;
		private readonly BackgroundWorker m_Worker;
		private readonly ITexture2d m_LookupTexture;
		private readonly ITexture2d m_PackTexture;
		private Bitmap m_LookupBitmap;
		private Bitmap m_PackBitmap;
		private readonly static TerrainTypeTextureBuilder s_Instance = new TerrainTypeTextureBuilder( ); 

		private void OnTerrainTypeCountChanged( TerrainType type )
		{
			Rebuild( false, true );
		}

		private void ProcessRequest( object sender, DoWorkEventArgs args )
		{
			if ( m_LookupTextureRebuildRequests > 0 )
			{
				m_LookupBitmap = m_TerrainTypes.CreateDistributionBitmap( );
			}
			if ( m_PackTextureRebuildRequests > 0 )
			{
				m_PackBitmap = m_TerrainTypes.CreateTerrainPackBitmap( );
			}
		}

		private void RequestComplete( object sender, RunWorkerCompletedEventArgs args )
		{
			if ( m_LookupTextureRebuildRequests > 0 )
			{
				m_LookupTexture.Create( m_LookupBitmap, false );
				m_LookupBitmap.Dispose( );

				BuilderState.Instance.Planet.TerrainModel.TerrainTypesTexture = m_LookupTexture;

				m_LookupBitmap = null;
				--m_LookupTextureRebuildRequests;
			}
			if ( m_PackTextureRebuildRequests > 0 )
			{
				m_PackTexture.Create( m_PackBitmap, false );

				BuilderState.Instance.Planet.TerrainModel.TerrainPackTexture = m_PackTexture;

				m_PackBitmap.Dispose( );
				m_PackBitmap = null;
				--m_PackTextureRebuildRequests;
			}
			if ( ( m_LookupTextureRebuildRequests > 0 ) || ( m_PackTextureRebuildRequests > 0 ) )
			{
				m_Worker.RunWorkerAsync( );
			}
		}

		#endregion
	}
}
