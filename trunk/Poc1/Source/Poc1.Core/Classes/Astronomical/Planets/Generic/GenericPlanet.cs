
using Poc1.Core.Interfaces.Astronomical.Planets;

namespace Poc1.Core.Classes.Astronomical.Planets.Generic
{
	/// <summary>
	/// Specifically typed planet class
	/// </summary>
	/// <typeparam name="TPlanetModel">Planet model interface type</typeparam>
	/// <typeparam name="TPlanetModelClass">Concrete planet model class</typeparam>
	/// <typeparam name="TPlanetRenderer">Planet renderer interface type</typeparam>
	/// <typeparam name="TPlanetRendererClass">Concrete planet renderer class</typeparam>
	public class GenericPlanet<TPlanetModel, TPlanetModelClass, TPlanetRenderer, TPlanetRendererClass> : AbstractPlanet
		where TPlanetModel : IPlanetModel
		where TPlanetModelClass : TPlanetModel
		where TPlanetRenderer : IPlanetRenderer
		where TPlanetRendererClass : TPlanetRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public GenericPlanet( )
			:
			base( PlanetModelFactory<TPlanetModelClass>.Instance, PlanetRendererFactory<TPlanetRendererClass>.Instance )
		{
		}

		/// <summary>
		/// Gets the planet model used by this planet
		/// </summary>
		public new TPlanetModel Model
		{
			get { return ( TPlanetModel )base.Model; }
		}

		/// <summary>
		/// Gets the planet renderer used by this planet
		/// </summary>
		public new TPlanetRenderer Renderer
		{
			get { return ( TPlanetRenderer )base.Renderer; }
		}
	}
}
