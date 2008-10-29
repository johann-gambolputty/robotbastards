using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Base class for planetary cloud models
	/// </summary>
	public class PlanetCloudModel : IPlanetCloudModel
	{
		#region IPlanetEnvironmentModel Members

		/// <summary>
		/// Event, invoked when the model changes
		/// </summary>
		public event System.EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the associated planet
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

		/// <summary>
		/// Gets/sets the minimum height of the cloud layer
		/// </summary>
		public Units.Metres CloudLayerMinHeight
		{
			get { return m_CloudLayerMinHeight; }
			set
			{
				bool changed = m_CloudLayerMinHeight != value;
				m_CloudLayerMinHeight = value;
				RaiseModelChanged( changed );
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Raises the <see cref="ModelChanged"/> event
		/// </summary>
		/// <param name="changed">
		/// Changed flag. If false, the ModelChanged event is not raised. A convenience to keep property
		/// setters simpler.
		/// </param>
		protected void RaiseModelChanged( bool changed )
		{
			if ( changed && ModelChanged != null )
			{
				ModelChanged( this, null );
			}
		}

		#endregion

		#region Private Members

		private Units.Metres m_CloudLayerMinHeight = new Units.Metres( 7000 );
		private IPlanet m_Planet;

		#endregion
	}
}
