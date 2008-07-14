using System;
using Poc1.Fast.Terrain;
using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// Planet terrain model
	/// </summary>
	public unsafe interface IPlanetTerrainModel
	{
		/// <summary>
		/// Event, invoked if the model changes
		/// </summary>
		event EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the maximum height of terrain that can be generated
		/// </summary>
		float MaximumHeight
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the function used for generating terrain heights
		/// </summary>
		TerrainFunction HeightFunction
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the function used for displacing terrain ground positions (if null, no ground displacement is applied)
		/// </summary>
		TerrainFunction GroundFunction
		{
			get; set;
		}

		/// <summary>
		/// Generates vertices for a terrain patch
		/// </summary>
		/// <param name="centre">Patch centre point</param>
		/// <param name="xAxis">Patch x axis (from tangent frame at centre point)</param>
		/// <param name="zAxis">Patch z axis (from tangent frame at centre point)</param>
		/// <param name="scale">Patch scale (dimensions of patch in metres)</param>
		/// <param name="res">Terrain patch resolution</param>
		/// <param name="uvRes">Patch texture resolution</param>
		/// <param name="vertices">Pointer into the patch vertex memory</param>
		void GeneratePatchVertices( Point3 centre, Vector3 xAxis, Vector3 zAxis, float scale, int res, float uvRes, void* vertices );

		/// <summary>
		/// Generates vertices for a terrain patch. Calculates an error value for the patch
		/// </summary>
		/// <param name="centre">Patch centre point</param>
		/// <param name="xAxis">Patch x axis (from tangent frame at centre point)</param>
		/// <param name="zAxis">Patch z axis (from tangent frame at centre point)</param>
		/// <param name="scale">Patch scale (dimensions of patch in metres)</param>
		/// <param name="res">Terrain patch resolution</param>
		/// <param name="uvRes">Patch texture resolution</param>
		/// <param name="vertices">Pointer into the patch vertex memory</param>
		/// <returns>Returns the patch error</returns>
		float GeneratePatchVerticesAndError( Point3 centre, Vector3 xAxis, Vector3 zAxis, float scale, int res, float uvRes, void* vertices );
	}

	/// <summary>
	/// Planet cloud model
	/// </summary>
	public interface IPlanetCloudModel
	{
		/// <summary>
		/// Event, invoked if the model is changed
		/// </summary>
		event EventHandler ModelChanged;

		/// <summary>
		/// Height of the cloud layer
		/// </summary>
		float CloudLayerHeight
		{
			get; set;
		}
	}

	/// <summary>
	/// Planetary atmosphere model
	/// </summary>
	public interface IPlanetAtmosphereModel
	{
		/// <summary>
		/// Event, invoked if the model is changed
		/// </summary>
		event EventHandler ModelChanged;

		/// <summary>
		/// Outer atmosphere height
		/// </summary>
		float OuterAtmosphereHeight
		{
			get; set;
		}
	}

	/// <summary>
	/// PLan
	/// </summary>
	public interface IPlanetOceanModel
	{
		event EventHandler ModelChanged;

		float OceanHeight
		{
			get; set;
		}
	}

	public interface IPlanetModel
	{
		/// <summary>
		/// Event, invoked if any component model of the planet changes
		/// </summary>
		event EventHandler ModelChanged;

		/// <summary>
		/// Gets the terrain model for the planet
		/// </summary>
		IPlanetTerrainModel TerrainModel
		{
			get;
		}

		/// <summary>
		/// Gets the cloud model for the planet
		/// </summary>
		IPlanetCloudModel CloudModel
		{
			get;
		}

		/// <summary>
		/// Gets the atmosphere model for the planet
		/// </summary>
		IPlanetAtmosphereModel AtmosphereModel
		{
			get;
		}

		/// <summary>
		/// Gets the ocean model for the planet
		/// </summary>
		IPlanetOceanModel OceanModel
		{
			get;
		}
	}

	/// <summary>
	/// Extended interface for spherical planets
	/// </summary>
	public interface ISpherePlanetModel : IPlanetModel
	{
		/// <summary>
		/// Radius of the planet (in metres)
		/// </summary>
		double Radius
		{
			get; set;
		}
	}
}
