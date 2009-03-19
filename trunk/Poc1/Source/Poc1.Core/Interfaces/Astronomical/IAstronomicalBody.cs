
namespace Poc1.Core.Interfaces.Astronomical
{
	/// <summary>
	/// Astronomical body interface
	/// </summary>
	public interface IAstronomicalBody : IUniObject
	{
		/// <summary>
		/// Gets/sets the orbit of this body
		/// </summary>
		IOrbit Orbit
		{
			get; set;
		}
	}

}
