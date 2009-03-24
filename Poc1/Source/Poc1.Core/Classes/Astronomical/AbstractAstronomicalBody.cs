
using Poc1.Core.Interfaces.Astronomical;

namespace Poc1.Core.Classes.Astronomical
{
	/// <summary>
	/// Abstract base class for astronomical bodies
	/// </summary>
	public class AbstractAstronomicalBody : AbstractUniObject, IAstronomicalBody
	{
		#region IAstronomicalBody Members

		/// <summary>
		/// Gets/sets the orbit of this object
		/// </summary>
		public IOrbit Orbit
		{
			get { return m_Orbit; }
			set { m_Orbit = value; }
		}

		#endregion

		#region Private Members

		private IOrbit m_Orbit;

		#endregion
	}
}
