using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Poc1.Bob.Core.Interfaces.Biomes.Models;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Biomes.Models
{
	/// <summary>
	/// Keeps an ordered list of biomes from an underlying BiomeListModel
	/// </summary>
	public class BiomeListLatitudeDistributionModel : IBiomeListDistributionModel
	{
		/// <summary>
		/// Event raised when a distribution is added to the model
		/// </summary>
		public event Action<BiomeLatitudeRangeDistribution> DistributionAdded;

		/// <summary>
		/// Event raised when a distribution is removed from the model
		/// </summary>
		public event Action<BiomeLatitudeRangeDistribution> DistributionRemoved;

		/// <summary>
		/// Event raised when a distribution changes
		/// </summary>
		public event Action<BiomeLatitudeRangeDistribution> DistributionChanged;

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="biomes">Biome list</param>
		public BiomeListLatitudeDistributionModel( BiomeListModel biomes )
		{
			Arguments.CheckNotNull( biomes, "biomes" );
			m_Biomes = biomes;
			m_Biomes.Models.ListChanged += Models_ListChanged;

			float latitudeStep = 1.0f / biomes.Models.Count;
			for ( int biomeIndex = 0; biomeIndex < biomes.Models.Count; ++biomeIndex )
			{
				BiomeModel biome = biomes.Models[ biomeIndex ];
				float minLatitude = biomeIndex * latitudeStep;
				float maxLatitude = ( biomeIndex + 1 ) * latitudeStep;
				BiomeLatitudeRangeDistribution distribution = new BiomeLatitudeRangeDistribution( biome, minLatitude, maxLatitude );
				distribution.DistributionChanged += OnDistributionChanged;
				m_Distributions.Add( distribution );
			}
		}

		/// <summary>
		/// Gets a latitude range distribution from a biome model
		/// </summary>
		public BiomeLatitudeRangeDistribution this[ BiomeModel model ]
		{
			get { return GetBiomeModelDistribution( model ); }
		}

		/// <summary>
		/// Returns the number of distributions stored in this model
		/// </summary>
		public int NumberOfDistributions
		{
			get { return m_Distributions.Count; }
		}

		/// <summary>
		/// Gets the distribution list
		/// </summary>
		public ReadOnlyCollection<BiomeLatitudeRangeDistribution> Distributions
		{
			get { return m_Distributions.AsReadOnly( ); }
		}

		#region IBiomeListDistributionModel Members

		/// <summary>
		/// Gets the distribution used by a specified biome in this model
		/// </summary>
		IBiomeDistribution IBiomeListDistributionModel.this[ BiomeModel model ]
		{
			get { return GetBiomeModelDistribution( model ); }
		}

		#endregion

		#region Private Members

		private readonly BiomeListModel m_Biomes;
		private readonly List<BiomeLatitudeRangeDistribution> m_Distributions = new List<BiomeLatitudeRangeDistribution>( );

		/// <summary>
		/// Adds a distribution of the model
		/// </summary>
		private void AddDistribution( int insertAt, BiomeLatitudeRangeDistribution distribution )
		{
			m_Distributions.Insert( insertAt, distribution );
			if ( DistributionAdded != null )
			{
				DistributionAdded( distribution );
			}
			distribution.DistributionChanged += OnDistributionChanged;
		}

		/// <summary>
		/// Called when a distribution changes
		/// </summary>
		private void OnDistributionChanged( BiomeLatitudeRangeDistribution distribution )
		{
			if ( DistributionChanged != null )
			{
				DistributionChanged( distribution );
			}
		}

		/// <summary>
		/// Invoked when the biome list changes
		/// </summary>
		private void Models_ListChanged( object sender, ListChangedEventArgs e )
		{
			switch ( e.ListChangedType )
			{
				case ListChangedType.ItemAdded :
					{
						//	Get the added biome
						BiomeModel newBiome = m_Biomes.Models[ e.NewIndex ];
						if ( m_Distributions.Count == 0 )
						{
							//	Distribution list is empty - new distribution covers the entire range
							m_Distributions.Add( new BiomeLatitudeRangeDistribution( newBiome, 0, 1 ) );
							return;
						}

						//	Choose a neighbouring distribution and split it with the new biome
						int splitIndex = e.NewIndex == 0 ? 1 : e.NewIndex - 1;
						IBiomeDistribution newDistribution = this[ m_Biomes.Models[ splitIndex ] ].Split( newBiome );

						//	Add the new distribution
						AddDistribution( e.NewIndex, ( BiomeLatitudeRangeDistribution )newDistribution );

						break;
					}

				case ListChangedType.ItemDeleted :
					{
						break;
					}
			}
		}

		/// <summary>
		/// Gets a biome distribution from a biome
		/// </summary>
		private BiomeLatitudeRangeDistribution GetBiomeModelDistribution( BiomeModel model )
		{
			Arguments.CheckNotNull( model, "model" );
			foreach ( BiomeLatitudeRangeDistribution distribution in m_Distributions )
			{
				if ( distribution.Biome == model )
				{
					return distribution;
				}
			}
			throw new ArgumentException( string.Format( "No distribution is associated with biome \"{0}\"", model.Name ) );
		}

		#endregion
	}
}
