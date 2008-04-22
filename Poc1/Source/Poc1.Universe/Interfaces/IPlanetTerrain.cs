
using System.Collections.Generic;
using Poc1.Universe.Classes.Rendering;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces
{
	/*
	/// <summary>
	/// Interface for generating a planet's terrain
	/// </summary>
	public interface IPlanetTerrain
	{
		//CollisionInfo GetCollision( IBoundingVolume cRep, Point3 start, Point3 end );

		/// <summary>
		/// Creates the 'marble' renderable representation of the planet
		/// </summary>
		/// <remarks>
		/// http://en.wikipedia.org/wiki/The_Blue_Marble
		/// 
		/// Also includes cloud rendering.
		/// </remarks>
		IRenderable CreateMarbleGraphics( );

		/// <summary>
		/// Creates terrain graphics
		/// </summary>
		IRenderable CreateTerrainGraphics( );
	}

	/// <summary>
	/// Manages planet terrain for spherical planets
	/// </summary>
	public abstract class SpherePlanetTerrain : IPlanetTerrain
	{
		/// <summary>
		/// Generates vertices for a terrain patch
		/// </summary>
		/// <param name="res">Patch resolution</param>
		/// <param name="origin">Position of the top left corner of the patch</param>
		/// <param name="xAxis">Patch X-axis</param>
		/// <param name="zAxis">Patch Z-axis</param>
		/// <param name="vertices">Pointer to the first terrain vertex in the patch</param>
		public abstract void GenerateTerrainPatchVertices( int res, Point3 origin, Vector3 xAxis, Vector3 zAxis, TerrainVertex* vertices );

		#region IPlanetTerrain Members

		/// <summary>
		/// Creates the 'marble' renderable representation of the planet
		/// </summary>
		/// <remarks>
		/// http://en.wikipedia.org/wiki/The_Blue_Marble
		/// 
		/// Also includes cloud rendering.
		/// </remarks>
		public abstract IRenderable CreateMarbleGraphics( );

		/// <summary>
		/// Creates terrain graphics
		/// </summary>
		public abstract IRenderable CreateTerrainGraphics( );

		#endregion


		#region Marble Private Class

		private class Marble : IRenderable
		{
			
		}

		#endregion

		#region Private Members

		private readonly List< TerrainPatch > m_Patches = new List< TerrainPatch >( );

		#endregion
	}
	*/
}