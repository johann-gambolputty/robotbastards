using Poc1.Universe.Interfaces.Rendering;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Atmosphere model for spherical planets
	/// </summary>
	public class SphereAtmosphereModel : IPlanetAtmosphereModel
	{
		/// <summary>
		/// The radius of the atmosphere (measured in uni-units)
		/// </summary>
		public long Radius
		{
			get { return m_Radius; }
			set { m_Radius = value; }
		}

		#region Private Members

		private long m_Radius = UniUnits.Metres.ToUniUnits( 6000 );

		#endregion
	}
}
