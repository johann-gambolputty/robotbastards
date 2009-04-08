
using Poc1.Core.Interfaces;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Abstract base class for sphere planet marble renderers
	/// </summary>
	public abstract class AbstractSpherePlanetMarbleRenderer : SpherePlanetEnvironmentRenderer
	{
		#region Protected Members

		/// <summary>
		/// Gets the current renderable marble geometry
		/// </summary>
		/// <remarks>
		/// Geometry is rebuilt if the planet radius has changed
		/// </remarks>
		protected IRenderable Geometry
		{
			get
			{
				Units.Metres expectedGeometryRadius = Planet.Model.Radius;
				if ( ( m_Geometry == null ) || ( m_GeometryRadius != expectedGeometryRadius ) )
				{
					BuildGeometry( expectedGeometryRadius );
				}
				return m_Geometry;
			}
		}
		 
		#endregion

		#region Private Members

		private Units.Metres m_GeometryRadius;
		private IRenderable m_Geometry;

		/// <summary>
		/// Destroys marble geometry
		/// </summary>
		private void DestroyGeometry( )
		{
			DisposableHelper.Dispose( m_Geometry );
			m_Geometry = null;
		}

		/// <summary>
		/// Builds marble geometry
		/// </summary>
		private void BuildGeometry( Units.Metres radius )
		{
			DestroyGeometry( );

			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, ( float )radius.ToAstroRenderUnits, 20, 20 );
			m_Geometry = Graphics.Draw.StopCache( );
			m_GeometryRadius = radius;
		}

		#endregion
	}
}
