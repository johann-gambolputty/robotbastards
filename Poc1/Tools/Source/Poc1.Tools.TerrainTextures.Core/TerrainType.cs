using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using Rb.Core.Utils;

namespace Poc1.Tools.TerrainTextures.Core
{
	/// <summary>
	/// Terrain type
	/// </summary>
	[Serializable]
	public class TerrainType
	{
		/// <summary>
		/// Distribution of terrain type over altitude
		/// </summary>
		public IDistribution AltitudeDistribution
		{
			get { return m_AltitudeDistribution; }
			set { m_AltitudeDistribution = value; }
		}

		/// <summary>
		/// Distribution of terrain type over slope
		/// </summary>
		public IDistribution SlopeDistribution
		{
			get { return m_SlopeDistribution; }
			set { m_SlopeDistribution = value; }
		}

		/// <summary>
		/// Terrain type name
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// Loads a bitmap into the terrain type texture
		/// </summary>
		public void LoadBitmap( string path )
		{
			Bitmap bmp = new Bitmap( path );
			m_TexturePath = path;
			m_Texture = bmp;
			m_AverageColour = AverageBitmapColour( m_Texture );
		}

		/// <summary>
		/// Gets the terrain type texture
		/// </summary>
		/// <seealso cref="LoadBitmap"/>
		public Bitmap Texture
		{
			get { return m_Texture; }
		}

		/// <summary>
		/// Average colour of the 
		/// </summary>
		public Color AverageColour
		{
			get { return m_AverageColour; }
		}

		/// <summary>
		/// Clones this terrain type
		/// </summary>
		public TerrainType Clone( )
		{
			TerrainType clone = new TerrainType( );
			clone.Name						= m_Name;
			clone.m_Texture					= m_Texture;
			clone.m_TexturePath				= m_TexturePath;
			clone.m_AverageColour			= m_AverageColour;
			clone.m_AltitudeDistribution	= m_AltitudeDistribution;
			clone.m_SlopeDistribution		= m_SlopeDistribution;
			return clone;
		}

		/// <summary>
		/// Returns the multiple of the values of the altitude and slope distributions
		/// </summary>
		public float GetScore( float altitude, float slope )
		{
			return m_AltitudeDistribution.Sample( altitude ) * m_SlopeDistribution.Sample( slope );
		}

		/// <summary>
		/// Called before this type is serialized. Ensures that the texture path is relative to the export
		/// path (which must be stored in the <see cref="StreamingContext.Context"/> property)
		/// </summary>
		[OnSerializing]
		public void OnSerializing( StreamingContext context )
		{
			if ( context.Context != null )
			{
				string saveDir = ( string )context.Context;
				if ( Path.IsPathRooted( m_TexturePath ) )
				{
					m_TexturePath = PathHelpers.MakeRelativePath( saveDir, m_TexturePath );
				}
			}
		}

		/// <summary>
		/// Called when this type is deserialized. Loads the type texture bitmap
		/// </summary>
		[OnDeserialized]
		public void OnDeserialized( StreamingContext context )
		{
			if ( !string.IsNullOrEmpty( m_TexturePath ) )
			{
				if ( !File.Exists( m_TexturePath ) )
				{
					if ( !Path.IsPathRooted( m_TexturePath ) )
					{
						m_TexturePath = Path.Combine( ( string )context.Context, m_TexturePath );
					}
					else
					{
						//	Erk... Try to repair earlier broken paths
						m_TexturePath = m_TexturePath.Replace( "Projects", "Downloads" );
						m_TexturePath = m_TexturePath.Replace( "Terrain Types\\Images", "Terrain\\Textures" );
					}
				}
				try
				{
					LoadBitmap( m_TexturePath );
				}
				catch ( Exception ex )
				{
					throw new ApplicationException( string.Format( "Failed to load texture from path \"{0}\"", m_TexturePath ), ex );
				}
			}
		}

		#region Private Members

		private IDistribution m_AltitudeDistribution = CreateDefaultDistribution( );
		private IDistribution m_SlopeDistribution = CreateDefaultDistribution( );
		private string m_Name;
		private string m_TexturePath;

		[NonSerialized]
		private Bitmap m_Texture;

		[NonSerialized]
		private Color m_AverageColour;

		private static IDistribution CreateDefaultDistribution( )
		{
			IDistribution distribution = new LinearDistribution( );
			distribution.ControlPoints.Add( new ControlPoint( 0, 1 ) );
			distribution.ControlPoints.Add( new ControlPoint( 1, 1 ) );
			return distribution;
		}

		private static Color AverageBitmapColour( Bitmap bmp )
		{
			//	A very rubbish way of doing things...
			double avgR = 0;
			double avgG = 0;
			double avgB = 0;
			double avgMul = 1.0f / ( double )( bmp.Width * bmp.Height );

			for ( int row = 0; row < bmp.Height; ++row )
			{
				for ( int col = 0; col < bmp.Width; ++col )
				{
					Color c = bmp.GetPixel( col, row );
					avgR += ( c.R * avgMul );
					avgG += ( c.G * avgMul );
					avgB += ( c.B * avgMul );
				}
			}

			return Color.FromArgb( ( byte )avgR, ( byte )avgG, ( byte )avgB );
		}

		#endregion
	}
}
