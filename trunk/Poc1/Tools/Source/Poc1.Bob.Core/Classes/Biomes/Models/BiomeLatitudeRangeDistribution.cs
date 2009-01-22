using System;
using Poc1.Bob.Core.Interfaces.Biomes.Models;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Biomes.Models
{
	/// <summary>
	/// Defines the distribution of a biome as a range of latitudes
	/// </summary>
	public class BiomeLatitudeRangeDistribution : IBiomeDistribution
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public BiomeLatitudeRangeDistribution( BiomeModel biome, float minLatitude, float maxLatitude )
		{
			Arguments.CheckNotNull( biome, "biome" );
			if ( minLatitude > maxLatitude )
			{
				throw new ArgumentException( "Minimum latitude cannot be greater than the maximum latitude" );
			}
			m_Biome = biome;
			m_MinLatitude = minLatitude;
			m_MaxLatitude = maxLatitude;
		}

		/// <summary>
		/// Gets/sets the minimum latitude that this biome appears at (0=pole, 1=equator)
		/// </summary>
		public float MinLatitude
		{
			get { return m_MinLatitude; }
			set
			{
				if ( value > m_MaxLatitude )
				{
					throw new ArgumentException( "Tried to set the minimum latitude to a value higher than the maximum altitude" );
				}
				if ( m_MinLatitude != Math.Max( value, 0 ) )
				{
					m_MinLatitude = Math.Max( value, 0 );
					OnDistributionChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the maximum latitude that this biome appears at (0=pole, 1=equator)
		/// </summary>
		public float MaxLatitude
		{
			get { return m_MaxLatitude; }
			set
			{
				if ( value < m_MinLatitude )
				{
					throw new ArgumentException( "Tried to set the maximum latitude to a value lower than the minimum altitude" );
				}
				if ( m_MaxLatitude != Math.Min( value, 1 ) )
				{
					m_MaxLatitude = Math.Min( value, 1 );
					OnDistributionChanged( );
				}
			}
		}

		#region IBiomeDistribution Members

		/// <summary>
		/// Event, raised when the distribution changes
		/// </summary>
		public event ActionDelegates.Action<BiomeLatitudeRangeDistribution> DistributionChanged;

		/// <summary>
		/// Gets the biome that this distribution is associated with
		/// </summary>
		public BiomeModel Biome
		{
			get { return m_Biome; }
		}

		/// <summary>
		/// Splits this distribution in two, changing this distribution in place and returning
		/// a new distribution representing the other area
		/// </summary>
		public IBiomeDistribution Split( BiomeModel model )
		{
			float midLatitude = ( m_MinLatitude + m_MaxLatitude ) / 2;

			BiomeLatitudeRangeDistribution distribution = new BiomeLatitudeRangeDistribution( model, midLatitude, m_MaxLatitude );
			m_MaxLatitude = midLatitude;

			OnDistributionChanged( );

			return distribution;
		}

		#endregion

		#region Private Members

		private readonly BiomeModel m_Biome;
		private float m_MinLatitude;
		private float m_MaxLatitude;

		/// <summary>
		/// Raises the DistributionChanged event
		/// </summary>
		private void OnDistributionChanged( )
		{
			if ( DistributionChanged != null )
			{
				DistributionChanged( this );
			}
		}

		#endregion

	}
}
