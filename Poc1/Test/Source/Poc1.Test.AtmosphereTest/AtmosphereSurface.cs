
using System.Collections.Generic;
using System.Drawing;
using Poc1.Test.AtmosphereTest;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.AtmosphereTest
{
	/// <summary>
	/// Renderable atmosphere
	/// </summary>
	public class AtmosphereSurface : IRenderable
	{
		/// <summary>
		/// Atmosphere vertex
		/// </summary>
		public struct Vertex
		{
			[VertexField( VertexFieldSemantic.Position )]
			public Point3	Position;

			[VertexField( VertexFieldSemantic.Diffuse )]
			public Color	Colour;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="radius">Initial atmosphere radius</param>
		public AtmosphereSurface( float radius )
		{
			m_Radius = radius;
			m_Technique = new TechniqueSelector( "Shared/diffuseLit.cgfx", true, "DefaultTechnique" );
			m_CalculatorWorker.VertexBatchComplete += OnVertexBatchComplete;
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			DebugText.Write( "Last calculation duration: {0:G4}", m_CalculatorWorker.DurationOfLastBatchInSeconds );

			FlightCamera camera = ( ( FlightCamera )Graphics.Renderer.Camera );
			m_CalculatorWorker.CameraPosition = camera.Position;
			if ( m_Vertices == null )
			{
				Build( );
			}
			context.ApplyTechnique( m_Technique, RenderGeometry );
		}

		#endregion

		#region Private Members

		private readonly TechniqueSelector m_Technique;
		private IIndexBuffer m_Indices;
		private IVertexBuffer m_Vertices;
		private float m_Radius;
		private readonly AtmosphereCalculatorWorker m_CalculatorWorker = new AtmosphereCalculatorWorker( );

		/// <summary>
		/// Called when the atmosphere calculator has finished with a batch of vertices
		/// </summary>
		private void OnVertexBatchComplete( Vertex[] vertices )
		{
			if ( m_Vertices != null )
			{
				m_Vertices.Dispose( );
				m_Vertices = null;
			}
			VertexBufferData vbData = VertexBufferData.FromVertexCollection( vertices );
			m_Vertices = Graphics.Factory.CreateVertexBuffer( );
			m_Vertices.Create( vbData );

			m_CalculatorWorker.Start( vertices );
		}

		/// <summary>
		/// Resets this object, forcing a rebuild of all data on the next render call
		/// </summary>
		private void Reset( )
		{
			if ( m_Vertices != null )
			{
				m_Vertices.Dispose( );
				m_Vertices = null;
			}
			if ( m_Indices != null )
			{
				m_Indices.Dispose( );
				m_Indices = null;
			}
		}

		/// <summary>
		/// Builds this object
		/// </summary>
		private void Build( )
		{
			Reset( );

			m_CalculatorWorker.Stop( );

			List<int> indices = new List<int>( );
			Vertex[] vertexArray = BuildVertices( m_Radius, 100, 100, indices );

			m_Indices = Graphics.Factory.CreateIndexBuffer( );
			m_Indices.Create( indices.ToArray( ), true );

			VertexBufferData vbData = VertexBufferData.FromVertexCollection( vertexArray );
			m_Vertices = Graphics.Factory.CreateVertexBuffer( );
			m_Vertices.Create( vbData );

			m_CalculatorWorker.Start( vertexArray );
		}

		/// <summary>
		/// Renders the current atmosphere shell
		/// </summary>
		private void RenderGeometry( IRenderContext context )
		{
			m_Vertices.Begin( );
			m_Indices.Draw( PrimitiveType.QuadList );
			m_Vertices.End( );
		}

		/// <summary>
		/// Builds an array of vertex and index values
		/// </summary>
		private static Vertex[] BuildVertices( float radius, int sSamples, int tSamples, List<int> indices )
		{
			float minT = 0;
			float maxT = Constants.Pi;
			float minS = 0;
			float maxS = Constants.TwoPi;

			//	Render the sphere as a series of strips
			float sIncrement = ( maxS - minS ) / ( sSamples );
			float tIncrement = ( maxT - minT ) / ( tSamples );

			Vertex[] vertices = new Vertex[ tSamples * sSamples ];

			float t = minT;

			int vertexIndex = 0;
			for ( int tCount = 0; tCount < tSamples; ++tCount )
			{
				float s = minS;
				for ( int sCount = 0; sCount < sSamples; ++sCount, ++vertexIndex )
				{
					float cosS = Functions.Cos( s );
					float cosT = Functions.Cos( t );

					float sinS = Functions.Sin( s );
					float sinT = Functions.Sin( t );

					float x = cosS * sinT;
					float y = cosT;
					float z = sinS * sinT;

					vertices[ vertexIndex ].Position = new Point3( x * radius, y * radius, z * radius );
					s += sIncrement;

					if ( tCount < ( tSamples - 1 ) )
					{
						AddQuadIndices( indices, sCount, tCount, sSamples );
					}

				}
				t += tIncrement;
			}
			return vertices;
		}

		/// <summary>
		/// Adds a quad to an index list
		/// </summary>
		private static void AddQuadIndices( List<int> indices, int sIndex, int tIndex, int sMax )
		{
			int curTOffset = tIndex * sMax;
			int nextTOffset = ( tIndex + 1 ) * sMax;

			int curTcurSIndex = curTOffset + sIndex;
			int curTnextSIndex = curTOffset + ( sIndex + 1 ) % sMax;
			int nextTcurSIndex = nextTOffset + sIndex;
			int nextTnextSIndex = nextTOffset + ( sIndex + 1 ) % sMax;

			indices.Add( curTcurSIndex );
			indices.Add( nextTcurSIndex );
			indices.Add( nextTnextSIndex );
			indices.Add( curTnextSIndex );

		}

		#endregion
	}
}
