using System;
using System.IO;
using System.Xml;
using Rb.Core.Assets;
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
	/// In this instance, an asset ("badger.model") is loaded. Because it's the only child of the
    /// <rb/> element, it's returned as the result of the Load() call. If a valid target had been
	/// passed into Load() then the asset would have been added as a child to the target.
    /// 
    /// </remarks>
    public class Loader : AssetLoader
	{
		/// <summary>
		/// Gets the asset name
		/// </summary>
		public override string Name
		{
			get { return "Component XML"; }
		}

		/// <summary>
		/// Gets the asset extension
		/// </summary>
		public override string[] Extensions
		{
			get { return new string[] { "components.xml" }; }
		}

		#region	Stream loading

		/// <summary>
		/// Loads component XML from a stream
		/// </summary>
		public object Load( Stream stream, string sourceName, LoadParameters parameters )
		{
			if ( !( parameters is ComponentLoadParameters ) )
			{
				ComponentLoadParameters newParameters = new ComponentLoadParameters( parameters.Target );

				foreach ( IDynamicProperty property in parameters.Properties )
				{
					newParameters.Properties.Add( property );
				}

				parameters = newParameters;
			}

			parameters.CanCache = false;

			ErrorCollection errors = new ErrorCollection( string.Copy( sourceName ) );

			sourceName = Path.GetFileName( sourceName );

			XmlTextReader reader = new XmlTextReader( stream );
			reader.WhitespaceHandling = WhitespaceHandling.Significant;
			try
			{
				if ( reader.MoveToContent( ) == XmlNodeType.None )
				{
					AssetsLog.Warning( "XML component asset \"{0}\" was empty - returning null", sourceName );
					return null;
				}
			}
			catch ( XmlException ex )
			{
				AssetsLog.Error( "Moving to XML component asset \"{0}\" content threw an exception", sourceName );

				Entry entry = new Entry( AssetsLog.GetSource( Severity.Error ), ex.Message );
				Source.HandleEntry( entry.Locate( sourceName, ex.LineNumber, ex.LinePosition, "" ) );

			    throw new ApplicationException( string.Format( "Failed to load component XML asset \"{0}\" (see log for details)", sourceName ) );
			}

            string cacheable = reader.GetAttribute( "cacheable" );
            parameters.CanCache = ( cacheable != null ) && ( ( cacheable == "yes" ) || ( cacheable == "true" ) );

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
                throw new ApplicationException( string.Format( "Failed to load component XML asset \"{0}\" (see log for details)", sourceName ) );
            }

			//	TODO: AP: bit dubious... if there's more than one object, return a list
			if ( builder.Children.Count == 0 )
			{
				throw new ApplicationException( string.Format( "Failed to load component XML asset \"{0}\" - did not contain any components", sourceName ) );
			}
			if ( builder.Children.Count == 1 )
			{
				return builder.Children[ 0 ];
			}
			return builder.Children;
		}

		/// <summary>
		/// Loads an asset
		/// </summary>
		/// <param name="source">Data source</param>
		/// <param name="parameters">Loading parameters</param>
		/// <returns>Returns loaded object</returns>
		public override object Load( ISource source, LoadParameters parameters )
		{
			using ( Stream stream = source.Open( ) )
			{
				return Load( stream, source.Path, parameters );
			}
		}

        /// <summary>
        /// Creates a new empty ComponentLoadParameters object
        /// </summary>
        public override LoadParameters CreateDefaultParameters( )
        {
            return new ComponentLoadParameters( );
        }

		#endregion
    }
}
