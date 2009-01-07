
using System;
using Poc1.Universe.Interfaces.Planets.Models;

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
		/// Gets/sets the planet's ring model
		/// </summary>
		public IPlanetRingModel Rings
		{
			get { return m_Rings; }
			set
			{
				if ( m_Rings != value )
				{
					m_Rings = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the planet's atmosphere model
		/// </summary>
		public IPlanetAtmosphereModel Atmosphere
		{
			get { return m_Atmosphere; }
			set
			{
				m_Atmosphere = value;
				OnModelChanged( );
			}
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
				ModelChanged( this, EventArgs.Empty );
			}	
		}

		#endregion

		#region Private Members

		private IPlanetRingModel m_Rings;
		private IPlanetAtmosphereModel m_Atmosphere;

		#endregion
	}
}
