using System;
using System.IO;
using System.Runtime.InteropServices;
using Rb.Assets;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;

namespace Rb.TextureAssets
{
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
		/// Loads an asset supported by this loader
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Asset load parameters</param>
		/// <returns>Returns the loaded object (ITexture)</returns>
		public override unsafe object Load( ISource source, LoadParameters parameters )
		{
			using ( Stream stream = ( ( IStreamSource )source ).Open( ) )
			{
				Header header = Read<Header>( stream );

				return null;	
			}
		}

		/// <summary>
		/// Reads a type directly from a stream
		/// </summary>
		private unsafe static T Read<T>( Stream stream ) where T : struct
		{
			byte[] buffer = new byte[ Marshal.SizeOf( typeof( T ) ) ];
			stream.Read( buffer, 0, buffer.Length );
			fixed ( byte* bufferPtr = buffer )
			{
				return ( T )Marshal.PtrToStructure( new IntPtr( bufferPtr ), typeof( T ) );
			}
		}
	}
}
