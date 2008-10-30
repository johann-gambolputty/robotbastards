using System.IO;
using Rb.Assets;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;

namespace Poc1.Tools.Waves
{
	/// <summary>
	/// Loads serialized WaveAnimation objects through the asset framework
	/// </summary>
	public class WaveAnimationLoader : AssetLoader
	{
		/// <summary>
		/// Gets the name of this loader
		/// </summary>
		public override string Name
		{
			get { return "WaveAnimation"; }
		}

		/// <summary>
		/// Gets the extensions that this loader supports
		/// </summary>
		public override string[] Extensions
		{
			get { return new string[] { "waves.bin" }; }
		}

		/// <summary>
		/// Loads a waves.bin file
		/// </summary>
		public override object Load( ISource source, LoadParameters parameters )
		{
			using ( Stream stream = ( ( IStreamSource )source ).Open( ) )
			{
				return WaveAnimation.Load( stream );
			}
		}
	}
}
