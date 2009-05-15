using System.IO;
using Rb.Assets;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;
using Rb.Core.Components;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;

namespace Rb.TextureAssets
{
	/// <summary>
	/// Texture asset loader
	/// </summary>
	public class Loader : AssetLoader
	{
		/// <summary>
		/// Gets the name of this loader
		/// </summary>
		public override string Name
		{
			get { return "Texture"; }
		}

		/// <summary>
		/// Gets the extensions that this loader supports
		/// </summary>
		public override string[] Extensions
		{
			get { return new string[] { "texture" }; }
		}

		/// <summary>
		/// Creates default texture load parameters
		/// </summary>
		/// <param name="addAllProperties">If true, adds all dynamic properties with default values to the parameters</param>
		/// <returns>Returns default load parameters</returns>
		public override LoadParameters CreateDefaultParameters( bool addAllProperties )
		{
			TextureLoadParameters parameters = new TextureLoadParameters( );
			if ( addAllProperties )
			{
				parameters.GenerateMipMaps = false;
				parameters.ReturnTextureDataOnly = false;
			}
			return parameters;
		}

		/// <summary>
		/// Loads an asset supported by this loader
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Asset load parameters</param>
		/// <returns>
		/// If the parameters specify "returnTextureDataOnly" as true  (<see cref="TextureLoadParameters.ReturnTextureDataOnly"/>),
		/// the method returns an array of <see cref="Texture2dData"/> or <see cref="Texture3dData"/>. If the "returnTextureDataOnly" 
		/// is false, or does not exist, this method returns an <see cref="ITexture"/> object.
		/// </returns>
		public override unsafe object Load( ISource source, LoadParameters parameters )
		{
			bool generateMipMaps = DynamicProperties.GetProperty( parameters.Properties, TextureLoadParameters.GenerateMipMapsPropertyName, false );
			bool returnTextureData = DynamicProperties.GetProperty( parameters.Properties, TextureLoadParameters.ReturnTextureDataOnlyName, false );
			using ( Stream stream = ( ( IStreamSource )source ).Open( ) )
			{
				return TextureReader.ReadTextureFromStream( source.ToString( ), stream, returnTextureData, generateMipMaps );
			}
		}
	}
}
