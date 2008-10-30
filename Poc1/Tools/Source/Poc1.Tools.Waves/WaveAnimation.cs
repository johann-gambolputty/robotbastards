using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.Tools.Waves
{
	/// <summary>
	/// Wave animation
	/// </summary>
	[Serializable]
	public class WaveAnimation : IDisposable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="frames">Animation frames</param>
		public WaveAnimation( Bitmap[] frames )
		{
			m_Frames = frames;
		}

		/// <summary>
		/// Gets the wave animation frames
		/// </summary>
		public Bitmap[] Frames
		{
			get { return m_Frames; }
		}

		/// <summary>
		/// Helper method that creates textures for each frame in the animation
		/// </summary>
		public ITexture2d[] ToTextures( bool createMipMaps )
		{
			ITexture2d[] textures = new ITexture2d[ Frames.Length ];
			for ( int textureIndex = 0; textureIndex < textures.Length; ++textureIndex )
			{
				textures[ textureIndex ] = RbGraphics.Factory.CreateTexture2d( );
				textures[ textureIndex ].Create( Frames[ textureIndex ], createMipMaps );
			}
			return textures;
		}

		#region Serialization Helpers

		/// <summary>
		/// Saves this object to a stream
		/// </summary>
		public void Save( Stream stream )
		{
			BinaryFormatter formatter = new BinaryFormatter( );
			formatter.Serialize( stream, this );
		}

		/// <summary>
		/// Saves this object to a file
		/// </summary>
		public void Save( string path )
		{
			using ( FileStream stream = new FileStream( path, FileMode.Create, FileAccess.Write ) )
			{
				Save( stream );
			}
		}

		/// <summary>
		/// Loads a WaveAnimation object from a stream
		/// </summary>
		public static WaveAnimation Load( Stream stream )
		{
			BinaryFormatter formatter = new BinaryFormatter( );
			return ( WaveAnimation )formatter.Deserialize( stream );
		}

		#endregion

		#region Private Members

		private Bitmap[] m_Frames;

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes of this object
		/// </summary>
		public void Dispose( )
		{
			if ( m_Frames == null )
			{
				return;
			}
			foreach ( Bitmap bitmap in m_Frames )
			{
				bitmap.Dispose( );
			}
			m_Frames = null;
		}

		#endregion
	}
}
