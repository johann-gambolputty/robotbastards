using System;
using System.Collections.Generic;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Core.Utils;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Planet model base class
	/// </summary>
	public class PlanetModel : IPlanetModel
	{
		#region IPlanetModel Members

		/// <summary>
		/// Raised when the planet model is changed
		/// </summary>
		public event EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the planet associated with this model
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set
			{
				if ( m_Planet == value )
				{
					return;
				}
				m_Planet = value;
				if ( m_Planet != null && m_Planet.PlanetModel != this )
				{
					m_Planet.PlanetModel = this;
				}
				foreach ( IPlanetEnvironmentModel model in m_Models )
				{
					model.Planet = m_Planet;
				}
			}
		}

		/// <summary>
		/// Gets/sets the planet's ring model
		/// </summary>
		public IPlanetRingModel RingModel
		{
			get { return m_Rings; }
			set
			{
				if ( m_Rings != value )
				{
					UnlinkModel( m_Rings );
					m_Rings = value;
					LinkModel( m_Rings );
				}
			}
		}

		/// <summary>
		/// Gets/sets the planet's atmosphere model
		/// </summary>
		public IPlanetAtmosphereModel AtmosphereModel
		{
			get { return m_Atmosphere; }
			set
			{
				if ( m_Atmosphere != value )
				{
					UnlinkModel( m_Atmosphere );
					m_Atmosphere = value;
					LinkModel( m_Atmosphere );
				}
			}
		}

		/// <summary>
		/// Gets/sets the planet's cloud model
		/// </summary>
		public IPlanetCloudModel CloudModel
		{
			get { return m_Clouds; }
			set
			{
				if ( m_Clouds != value )
				{
					UnlinkModel( m_Clouds );
					m_Clouds = value;
					LinkModel( m_Clouds );
				}
			}
		}

		/// <summary>
		/// Gets/sets the planet ocean model
		/// </summary>
		public IPlanetOceanModel OceanModel
		{
			get { return m_Ocean; }
			set
			{
				if ( m_Ocean != value )
				{
					UnlinkModel( m_Ocean );
					m_Ocean = value;
					LinkModel( m_Ocean );
				}
			}
		}

		/// <summary>
		/// Gets/sets the planet terrain model
		/// </summary>
		public IPlanetTerrainModel TerrainModel
		{
			get { return m_Terrain; }
			set
			{
				if ( m_Terrain != value )
				{
					UnlinkModel( m_Terrain );
					m_Terrain = value;
					LinkModel( m_Terrain );
				}
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes of this model
		/// </summary>
		public virtual void Dispose( )
		{
			foreach ( IPlanetEnvironmentModel model in m_Models )
			{
				DisposableHelper.Dispose( model );
			}
			m_Models.Clear( );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Raises the <see cref="ModelChanged"/> event with empty event args
		/// </summary>
		protected void OnModelChanged( )
		{
			OnModelChanged( EventArgs.Empty );
		}

		/// <summary>
		/// Raises the <see cref="ModelChanged"/> event with specified event args
		/// </summary>
		protected void OnModelChanged( EventArgs args )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, args );
			}
		}

		#endregion

		#region Private Members

		private IPlanet					m_Planet;
		private IPlanetRingModel		m_Rings;
		private IPlanetAtmosphereModel	m_Atmosphere;
		private IPlanetCloudModel		m_Clouds;
		private IPlanetOceanModel		m_Ocean;
		private IPlanetTerrainModel		m_Terrain;
		private readonly List<IPlanetEnvironmentModel> m_Models = new List<IPlanetEnvironmentModel>( );

		/// <summary>
		/// Unlinks a model
		/// </summary>
		private void UnlinkModel( IPlanetEnvironmentModel model )
		{
			if ( model != null )
			{
				m_Models.Remove( model );
			}
		}

		/// <summary>
		/// Links a model
		/// </summary>
		private void LinkModel( IPlanetEnvironmentModel model )
		{
			if ( model != null )
			{
				m_Models.Add( model );
				model.Planet = m_Planet;
			}
		}

		#endregion

	}
}
