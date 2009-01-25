using Poc1.Bob.Core.Interfaces.Rendering;
using Poc1.Universe.Interfaces.Planets;

namespace Poc1.Bob.Core.Classes.Rendering
{
	/// <summary>
	/// Controls a <see cref="IUniCameraView"/>, that displays an instance of a planet template
	/// </summary>
	public class PlanetViewController : UniCameraViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">Camera view</param>
		/// <param name="planet">Planet instance to view</param>
		/// <exception cref="System.ArgumentNullException">Thrown if view or planet are null</exception>
		public PlanetViewController( IUniCameraView view, IPlanet planet ) :
			base( view )
		{
			view.Renderable = planet;
		}
	}
}
