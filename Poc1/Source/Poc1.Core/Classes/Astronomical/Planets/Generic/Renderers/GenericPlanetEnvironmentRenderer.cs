using System;
using Poc1.Core.Classes.Astronomical.Planets.Renderers;
using Poc1.Core.Interfaces.Astronomical.Planets;

namespace Poc1.Core.Classes.Astronomical.Planets.Generic.Renderers
{
	/// <summary>
	/// Generic planet environment renderer
	/// </summary>
	/// <typeparam name="TPlanet">Planet type</typeparam>
	/// <typeparam name="TPlanetRenderer">Planet renderer type</typeparam>
	public abstract class GenericPlanetEnvironmentRenderer<TPlanet, TPlanetRenderer> : AbstractPlanetEnvironmentRenderer
		where TPlanet : IPlanet
		where TPlanetRenderer : IPlanetRenderer
	{
		/// <summary>
		/// Gets the planet that this renderer is attached to (via the planet renderer)
		/// </summary>
		public new TPlanet Planet
		{
			get { return ( TPlanet )base.Planet; }
		}

		/// <summary>
		/// Gets/sets the planet renderer that this renderer is a part of
		/// </summary>
		public new TPlanetRenderer PlanetRenderer
		{
			get { return ( TPlanetRenderer )base.PlanetRenderer; }
			set { base.PlanetRenderer = value; }
		}

		#region Protected Members

		/// <summary>
		/// Called after this environment renderer has been added to the specified planet renderer
		/// </summary>
		/// <param name="renderer">Planet renderer that this environment renderer was added to</param>
		protected override void OnAddedToPlanetRenderer( IPlanetRenderer renderer )
		{
			if ( !( renderer is TPlanetRenderer ) )
			{
				throw new ArgumentException( "Expected planet model to be of type " + typeof( TPlanetRenderer ) );
			}
			if ( !( renderer.Planet is TPlanet ) )
			{
				throw new ArgumentException( "Expected planet to be of type " + typeof( TPlanet ) );
			}
			base.OnAddedToPlanetRenderer( renderer );
		}

		#endregion
	}
}
