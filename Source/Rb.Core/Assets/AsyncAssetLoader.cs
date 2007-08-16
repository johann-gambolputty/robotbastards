using System;
using System.Threading;
using System.Collections.Generic;
using Rb.Core.Utils;
using Rb.Log;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Simple implementation of IAsyncAssetLoader
	/// </summary>
	public class AsyncAssetLoader : IAsyncAssetLoader, IDisposable
	{
		/// <summary>
		/// Kicks off the loader thread
		/// </summary>
		public AsyncAssetLoader( )
		{
			Monitor.Enter( m_Exit );
			Monitor.Enter( m_QueueNotEmpty );
			m_LoadThread = new Thread( LoadThread );
			m_LoadThread.Start( );
		}

		/// <summary>
		/// Queues up a load request
		/// </summary>
		/// <param name="location">Asset location</param>
		/// <param name="parameters">Load parameters</param>
		/// <param name="priority">Load priority</param>
		/// <returns>Asynchronous result</returns>
		public AsyncLoadResult QueueLoad( Location location, LoadParameters parameters, LoadPriority priority )
		{
			FullAsyncLoadResult result = new FullAsyncLoadResult( location, parameters );
			lock ( m_Locations )
			{
				m_Locations.Add( result );
			}
			return result;
		}

		private readonly object m_Exit = new object( );
		private readonly object m_QueueNotEmpty = new object( );
		private readonly Thread m_LoadThread;
		private readonly List< FullAsyncLoadResult > m_Locations = new List< FullAsyncLoadResult >( );

		/// <summary>
		/// Loading thread
		/// </summary>
		private void LoadThread( )
		{
			while ( true )
			{
				FullAsyncLoadResult currentResult = null;

				lock ( m_Locations )
				{
					//	TODO: AP: This causes the thread to spin endlessly - it needs to block after loading
					//	until there are more than 1 items in m_Locations
					if (m_Locations.Count > 0)
					{
						currentResult = m_Locations[ 0 ];
						m_Locations.RemoveAt( 0 );
					}
				}

				if ( currentResult != null )
				{
					currentResult.Load( );
				}

				if ( Monitor.TryEnter( m_Exit ) )
				{
					return;
				}
			}
		}

		/// <summary>
		/// Stores asset loading parameters
		/// </summary>
		private class FullAsyncLoadResult : AsyncLoadResult
		{
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="location">Asset location</param>
			/// <param name="parameters">Asset load parameters</param>
			public FullAsyncLoadResult( Location location, LoadParameters parameters )
			{
				m_LoadState = AssetManager.Instance.CreateLoadState( location, parameters );
			}

			/// <summary>
			/// Loads the asset
			/// </summary>
			public void Load( )
			{
				try
				{
					m_LoadState.Load( );
					OnComplete( m_LoadState.Asset );
				}
				catch ( Exception ex )
				{
					AssetsLog.Error( "Failed to async load asset {0}", m_LoadState.Location );
					ExceptionUtils.ToLog( AssetsLog.GetSource( Severity.Error ), ex );
				}
			}

			private readonly LoadState m_LoadState;
		}

		#region IDisposable Members

		public void Dispose( )
		{
			if ( m_LoadThread != null )
			{
				Monitor.Exit( m_Exit );
				m_LoadThread.Join( );
				m_LoadThread = null;
			}
		}

		#endregion
	}
}
