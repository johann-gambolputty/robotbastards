using System;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Base class for planet environment models
	/// </summary>
	public class PlanetEnvironmentModel : IPlanetEnvironmentModel
	{
		#region IPlanetEnvironmentModel Members

		/// <summary>
		/// Event raised when the model changes
		/// </summary>
		public event EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the planet associated with this model
		/// </summary>
		public virtual IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Raises the <see cref="ModelChanged"/> event with empty event args
		/// </summary>
		protected virtual void OnModelChanged( )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, EventArgs.Empty );
			}
		}

		/// <summary>
		/// Raises the <see cref="ModelChanged"/> event with specified event args
		/// </summary>
		protected virtual void OnModelChanged( EventArgs args )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, args );
			}
		}

		#endregion

		#region Private Members

		private IPlanet m_Planet;

		#endregion
	}
}
