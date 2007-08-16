using System;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Utils;

namespace Rb.Core.Assets
{
	public delegate void LoadCompleteDelegate(object asset);

	/// <summary>
	/// Asynchronous load result. Handles notifications when the loading of an asset is completed
	/// </summary>
	public class AsyncLoadResult
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public AsyncLoadResult( )
		{
			m_InvokeQueue = InvokeQueue.Instance;
		}

		/// <summary>
		/// Adds a callback to be invoked when the asset is loaded
		/// </summary>
		/// <param name="callback">Delegate to call</param>
		/// <param name="addToInvokeQueue">
		/// If true, then the callback is invoked using the <see cref="InvokeQueue"/> that created the result object</param>
		public void AddLoadCompleteCallback( LoadCompleteDelegate callback, bool addToInvokeQueue )
		{
			lock ( this )
			{
				if ( m_Complete )
				{
					callback( m_Asset );
				}
				else
				{
					if ( addToInvokeQueue )
					{
						InvokedLoadComplete += callback;
					}
					else
					{
						LoadComplete += callback;
					}
				}
			}
		}

		/// <summary>
		/// Called by an <see cref="IAsyncAssetLoader"/> when an asset is loaded
		/// </summary>
		/// <param name="asset">Loaded asset</param>
		public void OnComplete( object asset )
		{
			lock ( this )
			{
				m_Asset = asset;
				m_Complete = true;

				if ( LoadComplete != null )
				{
					LoadComplete( m_Asset );
				}

				if ( InvokedLoadComplete != null )
				{
					m_InvokeQueue.Add( new InvokeCallbackDelegate( OnInvokedLoadComplete ) );
				}
			}
		}

		private event LoadCompleteDelegate LoadComplete;
		private event LoadCompleteDelegate InvokedLoadComplete;

		private readonly InvokeQueue	m_InvokeQueue;
		private bool					m_Complete;
		private object					m_Asset;

		private delegate void InvokeCallbackDelegate( );

		private void OnInvokedLoadComplete( )
		{
			InvokedLoadComplete( m_Asset );
		}
	}

}
