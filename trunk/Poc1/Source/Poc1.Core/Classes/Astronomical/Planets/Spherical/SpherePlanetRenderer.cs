
namespace Poc1.Core.Classes.Astronomical.Planets.Spherical
{
	/// <summary>
	/// Spherical planet renderer
	/// </summary>
	public class SpherePlanetRenderer : AbstractPlanetRenderer, ISpherePlanetRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Spherical planet that this renderer is attached to</param>
		public SpherePlanetRenderer( ISpherePlanet planet )
			:
			base( planet )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Spherical planet that this renderer is attached to</param>
		/// <param name="nearSorter">Render order sorter used to determine the order that near objects are rendered in</param>
		/// <param name="farSorter">Render order sorter used to determine the order that far objects are rendered in</param>
		public SpherePlanetRenderer( ISpherePlanet planet, IRenderOrderSorter nearSorter, IRenderOrderSorter farSorter )
			:
			base( planet, nearSorter, farSorter )
		{
		}

		#region Protected Members

		/// <summary>
		/// Creates the default near sorter
		/// </summary>
		protected override IRenderOrderSorter DefaultNearSorter( )
		{
			TypeOrderedRenderOrderSorter sorter = new TypeOrderedRenderOrderSorter( true );
			sorter.AddFirstTypes( typeof( SpherePlanetRingRenderer ), typeof( SpherePlanetAtmosphereScatteringRenderer ) );

			return sorter;
		}

		/// <summary>
		/// Creates the default far sorter
		/// </summary>
		protected override IRenderOrderSorter DefaultFarSorter( )
		{
			TypeOrderedRenderOrderSorter sorter = new TypeOrderedRenderOrderSorter( true );
			sorter.AddFirstTypes( typeof( SpherePlanetOceanRenderer ) );
			return sorter;
		}

		#endregion

		#region ISpherePlanetRenderer Members

		/// <summary>
		/// Gets the spherical planet that this renderer is attached to
		/// </summary>
		public new ISpherePlanet Planet
		{
			get { return ( ISpherePlanet )base.Planet; }
		}

		/// <summary>
		/// Shortcut to Planet.PlanetModel
		/// </summary>
		public new ISpherePlanetModel PlanetModel
		{
			get { return ( ISpherePlanetModel )base.PlanetModel; }
		}

		#endregion

	}

}
