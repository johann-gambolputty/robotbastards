using System;
using System.Xml;
using Rb.Core.Resources;
using Rb.Core.Components;
using Rb.Core;
using Rb.Log;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Loads a .components.xml file
    /// </summary>
    /// <remarks>
    /// Format:
    /// All component xml files should have the prefix ".components.xml".
    /// They should have a root element <rb></rb>
    /// The root element represents the returned object - therefore, it is bound to the input target
    /// (LoadParameters.Target). If there is no target, then <rb/> can have only one child element, 
    /// and that is the returned value.
    /// Example:
    /// <rb>
    ///     <resource path="badger.model"/>
    /// </rb>
    /// In this instance, a resource ("badger.model") is loaded. Because it's the only child of the
    /// <rb/> element, it's returned as the result of the Load() call. If a valid target had been
    /// passed into Load() then the resource would have been added as a child to the target.
    /// 
    /// </remarks>
    public class Loader : ResourceStreamLoader
    {
		#region	Stream loading

		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <returns> The loaded resource </returns>
		public override Object Load( System.IO.Stream input, string inputSource, out bool canCache )
		{
		    return Load( input, inputSource, out canCache, new ComponentLoadParameters( ) );
		}

		/// <summary>
		/// Loads into a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <param name="parameters">Loading parameters</param>
		/// <returns>Returns resource</returns>
		public override Object Load( System.IO.Stream input, string inputSource, out bool canCache, LoadParameters parameters )
		{
		    canCache = false;

			ErrorCollection errors = new ErrorCollection( inputSource );

			XmlTextReader reader = new XmlTextReader( input );
			reader.WhitespaceHandling = WhitespaceHandling.Significant;
			try
			{
				if ( reader.MoveToContent( ) == XmlNodeType.None )
				{
					ResourcesLog.Warning( "XML component resource \"{0}\" was empty - returning null", inputSource );
					return null;
				}
			}
			catch ( XmlException ex )
			{
				ResourcesLog.Error( "Moving to XML component resource \"{0}\" content threw an exception", inputSource );

				Entry entry = new Entry( ResourcesLog.GetSource( Severity.Error ), ex.Message );
				Source.HandleEntry( entry.Locate( inputSource, ex.LineNumber, ex.LinePosition, "" ) );

			    throw new ApplicationException( string.Format( "Failed to load component XML resource \"{0}\" (see log for details)", inputSource ) );
			}

            string cacheable = reader.GetAttribute( "cacheable" );
            canCache = ( cacheable != null ) && ( ( cacheable == "yes" ) || ( cacheable == "true" ) );

			RootBuilder builder = ( RootBuilder )BaseBuilder.CreateBuilderFromReader( null, ( ComponentLoadParameters )parameters, errors, reader );

            if ( errors.Count == 0 )
            {
                BaseBuilder.SafePostCreate( builder );
                if ( errors.Count == 0 )
                {
                    BaseBuilder.SafeResolve( builder, true );
                }
            }

            if ( ( builder.BuildObject == null ) && ( errors.Count == 0 ) )
            {
                errors.Add( builder, "Empty components file" );
            }

            if ( errors.Count > 0 )
            {
                foreach ( Entry error in errors )
                {
                    Source.HandleEntry( error );
                }
                throw new ApplicationException( string.Format( "Failed to load component XML resource \"{0}\" (see log for details)", inputSource ) );
            }

			//	TODO: AP: bit dubious... if there's more than one object, return a list
			if ( builder.Children.Count == 0 )
			{
				return null;
			}
			if ( builder.Children.Count == 1 )
			{
				return builder.Children[ 0 ];
			}
			return builder.Children;
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

        /// <summary>
        /// Creates a new empty ComponentLoadParameters object
        /// </summary>
        public override LoadParameters CreateDefaultLoadParameters( )
        {
            return new ComponentLoadParameters( );
        }

		#endregion
    }
}
