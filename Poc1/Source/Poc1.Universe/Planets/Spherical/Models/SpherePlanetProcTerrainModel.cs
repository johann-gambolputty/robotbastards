using System;
using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Fast.Terrain;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers.Patches;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Interfaces.Rendering;
using Poc1.Universe.Planets.Models;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using ITerrainPatch=Poc1.Universe.Interfaces.Planets.Renderers.Patches.ITerrainPatch;
using IPlanet = Poc1.Universe.Interfaces.Planets.IPlanet;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Rectangle=System.Drawing.Rectangle;

namespace Poc1.Universe.Planets.Spherical.Models
{
	/// <summary>
	/// Terrain model implementation for spherical planets
	/// </summary>
	public class SpherePlanetProcTerrainModel : ISpherePlanetTerrainModel, IPlanetProcTerrainModel, ITerrainPatchGenerator
	{
		/// <summary>
		/// Default constructor. Loads default terrain textures
		/// </summary>
		public SpherePlanetProcTerrainModel( )
		{
			//	Load in default textures
		//	TextureLoadParameters loadParameters = new TextureLoadParameters( true );
		//	m_TerrainPackTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Pack.jpg", loadParameters );
		//	m_TerrainTypesTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Distribution.bmp" );
		}

		/// <summary>
		/// Gets/sets the associated planet
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = ( ISpherePlanet )value; }
		}


		#region ISpherePlanetTerrainModel Members

		/// <summary>
		/// Creates a face for the marble texture cube map
		/// </summary>
		public unsafe Bitmap CreateMarbleTextureFace( CubeMapFace face, int width, int height )
		{
			if ( !ReadyToUse )
			{
				throw new InvalidOperationException( "Planet terrain model has not yet been set up - can't generate marble cube map" );
			}
			Bitmap bmp = new Bitmap( width, height, PixelFormat.Format24bppRgb );
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, width, height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* pixels = ( byte* )bmpData.Scan0;
			m_Gen.GenerateTerrainPropertyCubeMapFace( face, width, height, bmpData.Stride, pixels );
			bmp.UnlockBits( bmpData );
			return bmp;
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
				m_TerrainTypesTexture = value;
				OnModelChanged( true, false );
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
				m_TerrainPackTexture = value;
				OnModelChanged( true, false );
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

		#region ITerrainPatchGenerator Members

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstVertex">Patch vertices</param>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainVertex* firstVertex )
		{
			SetPatchPlanetParameters( patch );
			SafeTerrainGenerator.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, patch.Uv, patch.UvResolution, firstVertex );
		}

		/// <summary>
		/// Generates vertices for a patch. Calculates maximum error between this patch and next higher detail patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstVertex">Patch vertices</param>
		/// <param name="error">Maximum error value between this patch and higher level patch</param>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainVertex* firstVertex, out float error )
		{
			SetPatchPlanetParameters( patch );
			SafeTerrainGenerator.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, patch.Uv, patch.UvResolution, firstVertex, out error );
		}

		#endregion

		#region IPlanetProcTerrainModel Members

		/// <summary>
		/// Sets up the terrain functions
		/// </summary>
		/// <param name="heightFunction">The terrain height function</param>
		/// <param name="groundFunction">The terrain ground offset function</param>
		public void SetupTerrain( TerrainFunction heightFunction, TerrainFunction groundFunction )
		{
			if ( m_Planet == null )
			{
				throw new InvalidOperationException( "Terrain model does not have an associated planet. Cannot setup terrain" );
			}
			float radius = m_Planet.Radius.ToRenderUnits;
			float height = MaximumHeight.ToRenderUnits;

			// NOTE: AP: Patch scale is irrelevant, because vertices are projected onto the function sphere anyway
			m_Gen = new TerrainGenerator( TerrainGeometry.Sphere, heightFunction, groundFunction );
			m_Gen.Setup( 1024, radius, radius + height );
			m_Gen.SetSmallestStepSize( MinimumStepSize, MinimumStepSize );

			OnModelChanged( false, true );
		}

		#endregion

		#region IPlanetTerrainModel Members

		/// <summary>
		/// Event, invoked when the terrain model changes
		/// </summary>
		public event EventHandler ModelChanged;

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
				m_MaximumHeight = value;
				OnModelChanged( false, false );
			}
		}

		#endregion

		#region Private Members

		private ITexture2d m_TerrainTypesTexture;
		private ITexture2d m_TerrainPackTexture;
		private TerrainGenerator m_Gen;
		private ISpherePlanet m_Planet;
		private Units.Metres m_MaximumHeight = new Units.Metres( 3000 );

		/// <summary>
		/// Terrain generator minimum step size. 4 samples are taken from the terrain geometry model, using
		/// this step size, around a central sample, to calculate the terrain normal
		/// </summary>
		private const float MinimumStepSize = 0.01f;

		/// <summary>
		/// Gets the current terrain generator. If there isn't one, a flat terrain generator is created.
		/// </summary>
		private TerrainGenerator SafeTerrainGenerator
		{
			get
			{
				if ( m_Gen == null )
				{
					SetupTerrain( new TerrainFunction( TerrainFunctionType.Flat ), null );
				}
				return m_Gen;
			}
		}

		/// <summary>
		/// Called when the model is changed
		/// </summary>
		private void OnModelChanged( bool texturesChanged, bool geometryChanged)
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, new PlanetTerrainModelChangedEventArgs( texturesChanged, geometryChanged ) );
			}
		}

		/// <summary>
		/// Patches are defined in a local space. This determines the planet-space parameters of a patch
		/// </summary>
		public void SetPatchPlanetParameters( ITerrainPatch patch )
		{
			float radius = m_Planet.Radius.ToRenderUnits;

			Point3 edge = patch.LocalOrigin + ( patch.LocalUAxis / 2 );
			Point3 centre = edge + ( patch.LocalVAxis / 2 );

			Point3 plEdge = ( edge.ToVector3( ).MakeNormal( ) * radius ).ToPoint3( );
			Point3 plCentre = ( centre.ToVector3( ).MakeNormal( ) * radius ).ToPoint3( );

			patch.SetPlanetParameters( plCentre, plCentre.DistanceTo( plEdge ) );
		}

		#endregion

	}
}
