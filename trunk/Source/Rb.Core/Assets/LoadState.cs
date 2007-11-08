
namespace Rb.Core.Assets
{
	/// <summary>
	/// Manages asset loading states
	/// </summary>
	public class LoadState
	{
		#region Public properties

		/// <summary>
		/// The loader that will load the asset
		/// </summary>
		public IAssetLoader Loader
		{
			get { return m_Loader; }
		}

		/// <summary>
		/// The location of the asset
		/// </summary>
		public ISource Source
		{
			get { return m_Source; }
		}

		/// <summary>
		/// Access to the loading parameters
		/// </summary>
		public LoadParameters Parameters
		{
			get { return m_Parameters; }
			set { m_Parameters = value; }
		}

		/// <summary>
		/// Gets the loaded asset
		/// </summary>
		public object Asset
		{
			get { return m_Asset; }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="loader">Asset loader</param>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Loading parameters</param>
		public LoadState( IAssetLoader loader, ISource source, LoadParameters parameters )
		{
			m_Loader		= loader;
			m_Source		= source;
			m_Parameters	= parameters ?? loader.CreateDefaultParameters( false );
		}

		/// <summary>
		/// Load step. Loads the object
		/// </summary>
		public virtual object Load( )
		{
			m_Asset = m_Loader.Cache.Find( m_Source.GetHashCode( ) );
			if ( m_Asset != null )
			{
				AssetsLog.Verbose( "Retrieved cached asset {0}", m_Source );
			}
			else
			{
				AssetsLog.Info( "Loading asset {0}", m_Source );
				m_Asset = m_Loader.Load( m_Source, m_Parameters );
				if ( ( m_Asset != null ) && ( m_Parameters.CanCache ) )
				{
					AssetsLog.Verbose( "Caching asset {0}", m_Source );
					m_Loader.Cache.Add( m_Source.GetHashCode( ), m_Asset );
				}
			}
			return m_Asset;
		}

		#endregion

		#region Private members

		private readonly IAssetLoader	m_Loader;
		private readonly ISource		m_Source;
		private object					m_Asset;
		private LoadParameters			m_Parameters;

		#endregion
	}
}
