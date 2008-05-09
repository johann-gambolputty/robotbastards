using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Universe.Interfaces.Rendering;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Manages terrain types for a planet
	/// </summary>
	public class TerrainTypeManager : ITerrainTypeManager
	{

		/// <summary>
		/// Creates a bitmap, with 
		/// </summary>
		public unsafe Bitmap ToBitmap()
		{
			Bitmap bmp = new Bitmap( 256, 256, PixelFormat.Format24bppRgb );

			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );

			byte* curPixel = ( byte* )bmpData.Scan0;
			for ( int elevation = 0; elevation < bmp.Height; ++elevation )
			{
				for ( int slope = 0; slope < bmp.Width; ++slope )
				{
					byte result = ( byte )GetTerrainType( elevation, slope, 0 );

					curPixel[ 0 ] = result;
					curPixel[ 1 ] = result;
					curPixel[ 2 ] = result;

					curPixel += 3;
				}
			}

			bmp.UnlockBits( bmpData );
			
			return bmp;
		}

		private int GetTerrainType( int elevation, int slope, int latitude )
		{
			int result = 0;
			float best = TerrainTypes[ result ].Distribution.GetSample( elevation, slope, latitude );
			for ( int typeIndex = 1; typeIndex < TerrainTypes.Length; ++typeIndex )
			{
				TerrainDistribution distro = TerrainTypes[typeIndex].Distribution;
				float value = distro.GetSample( elevation, slope, latitude );
				if ( value > best )
				{
					best = value;
					result = typeIndex;
				}
			}
			return result;
		}


		/// <summary>
		/// Creates a simple default terrain type manager
		/// </summary>
		public static TerrainTypeManager CreateDefault()
		{
			TerrainTypeManager manager = new TerrainTypeManager( );

			List<ITerrainType> terrainTypes = new List<ITerrainType>( );

			TerrainDistribution.Sample[] unitySamples = new TerrainDistribution.Sample[]
				{
					new TerrainDistribution.Sample( 0, 1 ),
				};
			TerrainDistribution.Sample[] grassSamples = new TerrainDistribution.Sample[]
				{
					new TerrainDistribution.Sample( 0, 0 ),
					new TerrainDistribution.Sample( 0.2f, 0 ),
					new TerrainDistribution.Sample( 0.3f, 1 ),
					new TerrainDistribution.Sample( 0.8f, 0.1f )
				};
			TerrainDistribution.Sample[] grassLatitudeSamples = new TerrainDistribution.Sample[]
				{
					new TerrainDistribution.Sample( 0, 0 ),
					new TerrainDistribution.Sample( 0.2f, 1 ),
					new TerrainDistribution.Sample( 0.8f, 1 ),
					new TerrainDistribution.Sample( 1, 0 ),
				};
			TerrainDistribution.Sample[] rockSamples = new TerrainDistribution.Sample[]
				{
					new TerrainDistribution.Sample( 0, 0.5f ),
					new TerrainDistribution.Sample( 1, 0.2f ),
				};
			TerrainDistribution.Sample[] snowSamples = new TerrainDistribution.Sample[]
				{
					new TerrainDistribution.Sample( 0, 0 ),
					new TerrainDistribution.Sample( 0.6f, 0.3f ),
					new TerrainDistribution.Sample( 1.0f, 1.0f )
				};
			TerrainDistribution.Sample[] snowLatitudeSamples = new TerrainDistribution.Sample[]
				{
					new TerrainDistribution.Sample( 0, 1 ),
					new TerrainDistribution.Sample( 0.2f, 0.8f ),
					new TerrainDistribution.Sample( 0.8f, 0.8f ),
					new TerrainDistribution.Sample( 1, 1 ),
				};
			TerrainDistribution.Sample[] sandSamples = new TerrainDistribution.Sample[]
				{
					new TerrainDistribution.Sample( 0, 1 ),
					new TerrainDistribution.Sample( 0.1f, 1 ),
					new TerrainDistribution.Sample( 0.2f, 0 )
				};

			terrainTypes.Add( new TerrainType( "grass", new TerrainDistribution( grassSamples, grassLatitudeSamples, unitySamples ), Color.Green ) );
			terrainTypes.Add( new TerrainType( "grass", new TerrainDistribution( grassSamples, grassLatitudeSamples, unitySamples ), Color.DarkGreen ) );
			terrainTypes.Add( new TerrainType( "rock", new TerrainDistribution( rockSamples, unitySamples, unitySamples ), Color.SandyBrown ) );
			terrainTypes.Add( new TerrainType( "rock", new TerrainDistribution( rockSamples, unitySamples, unitySamples ), Color.Gray ) );
			terrainTypes.Add( new TerrainType( "sand", new TerrainDistribution( sandSamples, unitySamples, unitySamples ), Color.BlanchedAlmond ) );
			terrainTypes.Add( new TerrainType( "sand", new TerrainDistribution( sandSamples, unitySamples, unitySamples ), Color.Yellow ) );
			terrainTypes.Add( new TerrainType( "snow", new TerrainDistribution( snowSamples, snowLatitudeSamples, unitySamples ), Color.White ) );
			terrainTypes.Add( new TerrainType( "snow", new TerrainDistribution( snowSamples, snowLatitudeSamples, unitySamples ), Color.WhiteSmoke ) );

			manager.TerrainTypes = terrainTypes.ToArray( );

			return manager;
		}


		#region Public Members

		public ITerrainType[] TerrainTypes
		{
			get { return m_TerrainTypes; }
			set
			{
				m_TerrainTypes = new List<ITerrainType>( value ).ToArray( );
				m_Distributions = new TerrainDistribution[ m_TerrainTypes.Length ];

				for ( int index = 0; index < m_TerrainTypes.Length; ++index )
				{
					m_Distributions[ index ] = m_TerrainTypes[ index ].Distribution;
				}
			}
		}

		#endregion


		#region ITerrainTypeManager Members

		#endregion

		#region Private Members

		private ITerrainType[] m_TerrainTypes;
		private TerrainDistribution[] m_Distributions;

		#endregion
	}
}
