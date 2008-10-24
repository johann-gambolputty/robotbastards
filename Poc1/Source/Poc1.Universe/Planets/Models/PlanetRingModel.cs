using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Base class for planetary ring models
	/// </summary>
	public class PlanetRingModel : IPlanetRingModel
	{
		#region IPlanetRingModel Members

		/// <summary>
		/// Gets/sets the width of the rings
		/// </summary>
		public Units.Metres Width
		{
			get { return m_RingWidth; }
			set
			{
				bool widthChanged = ( m_RingWidth.Value != value.Value );
				if ( widthChanged )
				{
					m_RingWidth = value;
					OnModelChanged( );
				}
			}
		}

		#endregion

		#region IPlanetEnvironmentModel Members

		/// <summary>
		/// Event, raised when the model changes
		/// </summary>
		public event System.EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the planet that these rings are associated with
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Raises the ModelChanged event
		/// </summary>
		protected void OnModelChanged( )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, null );
			}
		}

		#endregion

		#region Private Members

		private IPlanet m_Planet;
		private Units.Metres m_RingWidth = new Units.Metres( 8000 );

		#endregion
	}
}
