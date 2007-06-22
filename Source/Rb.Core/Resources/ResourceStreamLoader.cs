using System;
using System.IO;

namespace Rb.Core.Resources
{
	/// <summary>
	/// Loads resources from streams
	/// </summary>
    public abstract class ResourceStreamLoader : ResourceLoader
    {
        #region Resource caching

        /// <summary>
        /// Gets the resource cache for this loader. By default, this is a global cache shared by all stream loader objects
        /// </summary>
        public override IResourceCache Cache
        {
            get { return m_Cache; }
        }

        #endregion

		#region	Stream loading

		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
        /// <param name="canCacheResult">Set by the method. If true, then the return value can be cached in association with the source</param>
		/// <returns> The loaded resource </returns>
		public abstract Object Load( Stream input, string inputSource, out bool canCacheResult );

		/// <summary>
		/// Loads into a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
        /// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
        /// <param name="canCacheResult">Set by the method. If true, then the return value can be cached in association with the source</param>
        /// <param name="parameters">Loading parameters</param>
		/// <returns>Returns resource</returns>
		public abstract Object Load( Stream input, string inputSource, out bool canCacheResult, LoadParameters parameters );

		/// <summary>
		/// Returns true if this loader can load the specified stream
		/// </summary>
		/// <param name="path"> Stream path (contains extension that the loader can check)</param>
		/// <param name="input"> Input stream (file types can be identified by peeking at header bytes) </param>
		/// <returns> Returns true if the stream can </returns>
		/// <remarks>
		/// path can be null, in which case, the loader must be able to identify the resource type by checking the content in input (e.g. by peeking
		/// at the header bytes).
		/// </remarks>
		public abstract bool CanLoadStream( string path, Stream input );

		#endregion

        #region PreLoadState

        /// <summary>
        /// PreLoadState for the stream loader type
        /// </summary>
        public class StreamPreLoadState : ResourcePreLoadState
        {
            /// <summary>
            /// Sets up the preload state
            /// </summary>
            /// <param name="loader">Associated loader</param>
            /// <param name="parameters">Parameters to send to <see cref="ResourceStreamLoader.Load"/></param>
            /// <param name="location">Resource location (stream path)</param>
            /// <param name="stream">Resource stream</param>
            public StreamPreLoadState( ResourceLoader loader, LoadParameters parameters, string location, Stream stream ) :
                base( loader, parameters, location )
            {
                m_Stream = stream;
            }

            /// <summary>
            /// Loads and returns the resource
            /// </summary>
            public override object Load()
            {
                object result = base.Load( );
                if ( result != null )
                {
                    return result;
                }

                bool canCache;
                try
                {
                    if ( Parameters == null )
                    {
                        result = ( ( ResourceStreamLoader )Loader ).Load( m_Stream, Location, out canCache );
                    }
                    else
                    {
                        result = ( ( ResourceStreamLoader )Loader ).Load( m_Stream, Location, out canCache, Parameters );
                    }
                }
                finally
                {
                    m_Stream.Close( );
                }

                return PostLoad( result, canCache );
            }

            private Stream m_Stream;
        }

        #endregion

        #region Private stuff

        private ResourceCache m_Cache = new ResourceCache( );

        #endregion
	}
}
