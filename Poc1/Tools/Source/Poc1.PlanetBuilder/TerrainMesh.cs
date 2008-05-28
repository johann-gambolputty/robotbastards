using System;
using Poc1.Universe;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Rendering;
using Poc1.Universe.Classes.Rendering;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using Poc1.Universe.Classes.Cameras;

namespace Poc1.PlanetBuilder
{
	public class TerrainMesh : IPlanetTerrain, IRenderable
	{
		/// <summary>
		/// Terrain mesh setup constructor
		/// </summary>
		/// <param name="width">Mesh width</param>
		/// <param name="maxHeight">Maximum mesh height</param>
		/// <param name="depth">Mesh depth</param>
		public TerrainMesh( float width, float maxHeight, float depth )
		{
			m_MaxHeight = maxHeight;

			Point3 origin = new Point3( -width / 2, 0, -depth / 2 );
			Vector3 xAxis = new Vector3( width, 0, 0 );
			Vector3 zAxis = new Vector3( 0, 0, depth );

			m_Vertices = new TerrainQuadPatchVertices( );
			m_RootPatch = new TerrainQuadPatch( m_Vertices, origin, xAxis, zAxis, 256.0f );
		}

		#region IPlanetTerrain Members

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, float uvRes, TerrainVertex* firstVertex )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, float uvRes, TerrainVertex* firstVertex, out float error )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the terrain
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			IUniCamera cam = UniCamera.Current;
			Point3 pt = UniUnits.RenderUnits.MakeRelativePoint( m_Centre, cam.Position );
			m_RootPatch.UpdateLod( pt, this, cam );
			m_RootPatch.Update( UniCamera.Current, this );
			m_RootPatch.Render( );
		}

		#endregion

		#region Private Members

		private readonly UniPoint3 m_Centre = new UniPoint3( );
		private readonly float m_MaxHeight;
		private readonly TerrainQuadPatchVertices m_Vertices;
		private readonly TerrainQuadPatch m_RootPatch;
	//	private readonly FlatTerrainGenerator m_Gen = new FlatTerrainGenerator( TerrainGeneratorType.Ridged, true, TimeSeed );

		/// <summary>
		/// Gets the current time in ticks. Used to seed the PNG in the terrain generator
		/// </summary>
		private static uint TimeSeed
		{
			get
			{
				return ( uint )DateTime.Now.Ticks;
			}
		}

		#endregion

	}
}
