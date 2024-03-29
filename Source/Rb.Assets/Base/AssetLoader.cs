using System;
using Rb.Assets.Interfaces;
using System.IO;

namespace Rb.Assets.Base
{
	/// <summary>
	/// Simple implementation of the IAssetLoader interface
	/// </summary>
	public abstract class AssetLoader : IAssetLoader
	{
		#region IAssetLoader Members

		/// <summary>
		/// Gets the asset name
		/// </summary>
		public abstract string Name
		{
			get;
		}

		/// <summary>
		/// Gets the asset extensions
		/// </summary>
		public abstract string[] Extensions
		{
			get;
		}

		/// <summary>
		/// Gets the cache for this loader
		/// </summary>
		public IAssetCache Cache
		{
			get { return m_Cache; }
		}

		/// <summary>
		/// Creates default loading parameters
		/// </summary>
		public virtual LoadParameters CreateDefaultParameters( bool addAllProperties )
		{
			return new LoadParameters( );
		}

		/// <summary>
		/// Loads an asset
		/// </summary>
		/// <param name="source">Source of the asset</param>
		/// <param name="parameters">Load parameters</param>
		/// <returns>Loaded asset</returns>
		public abstract object Load( ISource source, LoadParameters parameters );

		/// <summary>
		/// Returns true if this loader can load the asset at the specified location
		/// </summary>
		/// <param name="source">Source of the asset</param>
		/// <returns>Returns true if this loader can process the specified source</returns>
		public virtual bool CanLoad( ISource source )
		{
			string sourceStr = source.ToString( );
			foreach ( string ext in Extensions )
			{
				if ( sourceStr.EndsWith( ext, StringComparison.CurrentCultureIgnoreCase ) )
				{
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Protected members

		protected static Stream OpenStream( ISource source )
		{
			IStreamSource streamSource = source as IStreamSource;
			if ( streamSource == null )
			{
				throw new ArgumentException( string.Format( "Cannot open a stream from \"{0}\"", source.Name ), "source" );
			}
			return streamSource.Open( );
		}

		#endregion

		#region Private members

		private readonly IAssetCache m_Cache = new AssetCache( );

		#endregion
	}
}
