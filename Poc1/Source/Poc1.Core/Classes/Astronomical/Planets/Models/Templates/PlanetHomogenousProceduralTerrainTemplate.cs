using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Fast.Terrain;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Template for homogenous procedural terrain models
	/// </summary>
	public class PlanetHomogenousProceduralTerrainTemplate : PlanetTerrainTemplate, IPlanetHomogenousProceduralTerrainTemplate
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public PlanetHomogenousProceduralTerrainTemplate( )
		{
		//	TerrainFunction heightFunction = new TerrainFunction( TerrainFunctionType.RidgedFractal );
		//	FractalTerrainParameters parameters = ( FractalTerrainParameters )heightFunction.Parameters;
			TerrainFunction heightFunction = new TerrainFunction( TerrainFunctionType.Flat );
			m_HeightFunction = heightFunction;
		}

		/// <summary>
		/// Sets up a model instance from this template
		/// </summary>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			base.SetupInstance( model, context );
			IPlanetHomogenousProceduralTerrainModel terrainModel = ( IPlanetHomogenousProceduralTerrainModel )model;
			terrainModel.HeightFunction = HeightFunction;
			terrainModel.GroundOffsetFunction = GroundOffsetFunction;
		}

		#region IPlanetHomogenousProceduralTerrainTemplate Members

		/// <summary>
		/// Gets/sets the height function
		/// </summary>
		public TerrainFunction HeightFunction
		{
			get { return m_HeightFunction; }
			set
			{
				if ( m_HeightFunction != value )
				{
					m_HeightFunction = value;
					OnTemplateChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the ground offset function
		/// </summary>
		public TerrainFunction GroundOffsetFunction
		{
			get { return m_GroundOffsetFunction; }
			set
			{
				if ( m_GroundOffsetFunction != value )
				{
					m_GroundOffsetFunction = value;
					OnTemplateChanged( );
				}
			}
		}

		#endregion

		#region Private Members

		private TerrainFunction m_HeightFunction;
		private TerrainFunction m_GroundOffsetFunction;

		#endregion

	}
}
