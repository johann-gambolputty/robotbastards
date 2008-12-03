using System;
using Poc1.Fast.Terrain;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Flat;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers.Patches;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Flat.Models
{
	/// <summary>
	/// Procedural terrain model for flat planets
	/// </summary>
	public class FlatPlanetProcTerrainModel : IPlanetProcTerrainModel, ITerrainPatchGenerator
	{
		#region IPlanetProcTerrainModel Members

		/// <summary>
		/// Sets up the terrain functions
		/// </summary>
		/// <param name="heightFunction">The terrain height function</param>
		/// <param name="groundFunction">The terrain ground offset function</param>
		public void SetupTerrain( TerrainFunction heightFunction, TerrainFunction groundFunction )
		{
			if ( Planet == null )
			{
				throw new InvalidOperationException( "Terrain model does not have an associated planet. Cannot setup terrain" );
			}

			float height = MaximumHeight.ToRenderUnits;

			m_Gen = new TerrainGenerator( TerrainGeometry.Plane, heightFunction, groundFunction );
			m_Gen.Setup( 1024, 0, height );
			m_Gen.SetSmallestStepSize( 0.001f, 0.001f );

			OnModelChanged( );
		}

		#endregion

		#region IPlanetTerrainModel Members

		/// <summary>
		/// Gets/sets the terrain types texture
		/// </summary>
		public ITexture2d TerrainTypesTexture
		{
			get { return m_TerrainTypesTexture; }
			set
			{
				if ( m_TerrainTypesTexture != value )
				{
					m_TerrainTypesTexture = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the terrain pack texture
		/// </summary>
		public ITexture2d TerrainPackTexture
		{
			get { return m_TerrainPackTexture; }
			set
			{
				if ( m_TerrainPackTexture != value )
				{
					m_TerrainPackTexture = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the maximum height of terrain generated by this model
		/// </summary>
		/// <remarks>
		/// On set, the ModelChanged event is invoked.
		/// </remarks>
		public Units.Metres MaximumHeight
		{
			get { return m_MaximumHeight; }
			set
			{
				if ( m_MaximumHeight != value )
				{
					m_MaximumHeight = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Returns true if the model is ready to use
		/// </summary>
		/// <remarks>
		/// Can return false if the model has not been set up yet
		/// </remarks>
		public bool ReadyToUse
		{
			get { return m_Gen != null; }
		}

		#endregion

		#region IPlanetEnvironmentModel Members

		/// <summary>
		/// Event raised when the model changes
		/// </summary>
		public event EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the associated planet
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = ( IFlatPlanet )value; }
		}

		#endregion

		#region ITerrainPatchGenerator Members

		/// <summary>
		/// Generates vertices for a terrain patch
		/// </summary>
		/// <param name="patch">Terrain patch</param>
		/// <param name="res">Terrain patch resolution</param>
		/// <param name="firstVertex">Pointer to terrain vertices</param>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainVertex* firstVertex )
		{
			m_Gen.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, patch.Uv, patch.UvResolution, firstVertex );
		}

		/// <summary>
		/// Generates vertices for a terrain patch
		/// </summary>
		/// <param name="patch">Terrain patch</param>
		/// <param name="res">Terrain patch resolution</param>
		/// <param name="firstVertex">Pointer to terrain vertices</param>
		/// <param name="error">Terrain patch error output here</param>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainVertex* firstVertex, out float error )
		{
			m_Gen.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, patch.Uv, patch.UvResolution, firstVertex, out error );
		}

		#endregion

		#region Private Members

		private IFlatPlanet m_Planet;
		private TerrainGenerator m_Gen;
		private Units.Metres m_MaximumHeight;
		private ITexture2d m_TerrainTypesTexture;
		private ITexture2d m_TerrainPackTexture;

		/// <summary>
		/// Raises the ModelChanged event
		/// </summary>
		private void OnModelChanged( )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, EventArgs.Empty );
			}
		}

		#endregion

	}
}
