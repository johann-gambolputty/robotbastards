using System.Runtime.InteropServices;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Base class for ocean rendering
	/// </summary>
	public abstract class OceanRenderer : IRenderable
	{
		#region Vertex Struct

		/// <summary>
		/// The ocean shader assumes vertices with this layout
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		public struct Vertex
		{

			public Point3 Position
			{
				get
				{
					return new Point3( m_X, m_Y, m_Z );
				}
				set
				{
					m_X = value.X;
					m_Y = value.Y;
					m_Z = value.Z;
				}
			}

			public Vector3 Normal
			{
				get
				{
					return new Vector3( m_Nx, m_Ny, m_Nz );
				}
				set
				{
					m_Nx = value.X;
					m_Ny = value.Y;
					m_Nz = value.Z;
				}
			}

			private float m_X;
			private float m_Y;
			private float m_Z;
			private float m_Nx;
			private float m_Ny;
			private float m_Nz;
		}

		#endregion
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		public OceanRenderer( string oceanEffectPath )
		{
			m_Texture = ( ITexture )AssetManager.Instance.Load( "Ocean/ocean0.jpg", new TextureLoader.TextureLoadParameters( true ) );

			//	Load in sea effect
			IEffect oceanEffect = ( IEffect )AssetManager.Instance.Load( oceanEffectPath );
			m_OceanTechniques = new TechniqueSelector( oceanEffect, "DefaultTechnique" );
		}

		#region IRenderable Members

		/// <summary>
		/// Renders the ocean
		/// </summary>
		/// <param name="context">Rendering context</param>
		public virtual void Render( IRenderContext context )
		{
			m_OceanTechniques.Effect.Parameters[ "OceanTexture" ].Set( m_Texture );
			context.ApplyTechnique( m_OceanTechniques, RenderOcean );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Creates the vertex format implied by the Vertex struct
		/// </summary>
		protected static VertexBufferFormat CreateOceanVertexBufferFormat( )
		{
			VertexBufferFormat vbFormat = new VertexBufferFormat( );
			vbFormat.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			vbFormat.Add( VertexFieldSemantic.Normal, VertexFieldElementTypeId.Float32, 3 );

			return vbFormat;
		}

		/// <summary>
		/// Renders the ocean geometry
		/// </summary>
		protected abstract void RenderOcean( IRenderContext context );

		#endregion

		#region Private Members

		private readonly ITexture m_Texture;
		private readonly TechniqueSelector m_OceanTechniques;

		#endregion


	}
}
