using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace Poc1.Tools.TerrainTextures.Core
{
	[Serializable]
	public class TerrainType
	{
		public IDistribution AltitudeDistribution
		{
			get { return m_AltitudeDistribution; }
			set { m_AltitudeDistribution = value; }
		}

		public IDistribution SlopeDistribution
		{
			get { return m_SlopeDistribution; }
			set { m_SlopeDistribution = value; }
		}

		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		public void LoadBitmap( string path )
		{
			Bitmap bmp = new Bitmap( path );
			m_TexturePath = path;
			m_Texture = bmp;
			m_AverageColour = AverageBitmapColour( m_Texture );
		}

		public Bitmap Texture
		{
			get { return m_Texture; }
		}

		public Color AverageColour
		{
			get { return m_AverageColour; }
		}

		public TerrainType Clone( )
		{
			TerrainType clone = new TerrainType( );
			clone.Name = m_Name;
			clone.m_Texture = m_Texture;
			clone.m_AverageColour = m_AverageColour;
			return clone;
		}

		public float GetScore( float altitude, float slope )
		{
			return m_AltitudeDistribution.Sample( altitude ) * m_SlopeDistribution.Sample( slope );
		}

		[OnDeserialized]
		public void OnDeserialized( StreamingContext context )
		{
			if ( !string.IsNullOrEmpty( m_TexturePath ) )
			{
				LoadBitmap( m_TexturePath );
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
