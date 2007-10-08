using System;
using System.Threading;
using System.Collections.Generic;

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
			m_LoadThread = new Thread( LoadThread );
			m_LoadThread.IsBackground = true;
			m_LoadThread.Start( );
		}

		/// <summary>
		/// Kills the loader thread (calls <see cref="Dispose"/>)
		/// </summary>
		~AsyncAssetLoader( )
		{
			Dispose( );
		}
		
		/// <summary>
		/// Queues up a load request. Uses the <see cref="AssetManager"/> singleton
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Load parameters</param>
		/// <param name="priority">Load priority</param>
		/// <returns>Asynchronous result</returns>
		public AsyncLoadResult QueueLoad( ISource source, LoadParameters parameters, LoadPriority priority )
		{
			return QueueLoad( AssetManager.Instance, source, parameters, priority );
		}
		
		/// <summary>
		/// Queues up a load request
		/// </summary>
		/// <param name="manager">Asset manager</param>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Load parameters</param>
		/// <param name="priority">Load priority</param>
		/// <returns>Asynchronous result</returns>
		public AsyncLoadResult QueueLoad( AssetManager manager, ISource source, LoadParameters parameters, LoadPriority priority )
		{
			FullAsyncLoadResult result = new FullAsyncLoadResult( manager, source, parameters );
			lock ( m_Locations )
			{
				//	TODO: AP: Handle load priority
				m_Locations.Add( result );

				//	Signal the thread that there's stuff to process
				m_QueueNotEmpty.Set( );
			}
			return result;
		}

		#region Private stuff

		private readonly ManualResetEvent				m_QueueNotEmpty	= new ManualResetEvent( false );
		private readonly ManualResetEvent				m_Exit			= new ManualResetEvent( false );
		private readonly List< FullAsyncLoadResult >	m_Locations		= new List< FullAsyncLoadResult >( );
		private Thread									m_LoadThread;

		/// <summary>
		/// Loading thread
		/// </summary>
		private void LoadThread( )
		{
			WaitHandle[] handles = new WaitHandle[] { m_QueueNotEmpty, m_Exit };

			while ( true )
			{
				//	Wait until there's something useful to process
				int index = WaitHandle.WaitAny( handles );
				if ( handles[ index ] == m_Exit )
				{
					return;
				}

				//	The queue is not empty
				FullAsyncLoadResult currentResult = null;

				//	Retrieve the first load result from the queue
				lock ( m_Locations )
				{
					if ( m_Locations.Count > 0 )
					{
						currentResult = m_Locations[ 0 ];
						m_Locations.RemoveAt( 0 );
					}
					if ( m_Locations.Count == 0 )
					{
						//	Reset the queueNotEmpty wait handle, so the next loop iteration will block until there's stuff
						//	in the queue to load
						m_QueueNotEmpty.Reset( );
					}
				}

				//	Load the current result
				if ( currentResult != null )
				{
					currentResult.Load( );
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
			/// <param name="manager">Asset manager</param>
			/// <param name="source">Asset source</param>
			/// <param name="parameters">Asset load parameters</param>
			public FullAsyncLoadResult( AssetManager manager, ISource source, LoadParameters parameters )
			{
				m_LoadState = manager.CreateLoadState( source, parameters );
			}

			/// <summary>
			/// Loads the asset
			/// </summary>
			public void Load( )
			{
				try
				{
					m_LoadState.Load( );
					OnSuccess( m_LoadState.Asset );
				}
				catch ( Exception ex )
				{
					AssetsLog.Exception( ex, "Failed to async load asset {0}", m_LoadState.Source );
					OnFailure( );
				}
			}

			private readonly LoadState m_LoadState;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Kills the load thread
		/// </summary>
		public void Dispose( )
		{
			if ( m_LoadThread != null )
			{
				//	Signal load thread to exit
				m_Exit.Set( );

				//	Wait for it to finish
				m_LoadThread.Join( );
				m_LoadThread = null;
			}
		}

		#endregion
	}
}
