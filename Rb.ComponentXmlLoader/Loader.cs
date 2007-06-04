using System;
using System.Collections.Generic;
using System.Text;

using Rb.Core.Resources;
using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Loads a .component.xml file
    /// </summary>
    public class Loader : ResourceStreamLoader
    {
		#region	Stream loading

		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <returns> The loaded resource </returns>
		public override Object Load( System.IO.Stream input, string inputSource )
		{
		    return Load( input, inputSource, new ComponentLoadParameters( ) );
		}

		/// <summary>
		/// Loads into a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <param name="parameters">Loading parameters</param>
		/// <returns>Returns resource</returns>
		public override Object Load( System.IO.Stream input, string inputSource, LoadParameters parameters )
		{
		    BaseBuilder builder;
            if ( parameters.Target != null )
            {
                builder = new ReferenceBuilder( ( ComponentLoadParameters )parameters, parameters.Target );
            }
            else
            {
                builder = new ReferenceBuilder( ( ComponentLoadParameters )parameters, parameters.Target );
            }

            builder.Resolve( );

		    return builder.BuildObject;
		}

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
		public override bool CanLoadStream( string path, System.IO.Stream input )
		{
		    return path.EndsWith( ".components.xml", StringComparison.CurrentCultureIgnoreCase );
		}

		#endregion

    }
}
