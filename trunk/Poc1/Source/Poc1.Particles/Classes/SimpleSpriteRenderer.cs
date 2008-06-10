using System.ComponentModel;
using System.Runtime.InteropServices;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Rb.Rendering.Platform;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Renders particles as point sprites
	/// </summary>
	public class SimpleSpriteRenderer : IParticleRenderer
	{
		public SimpleSpriteRenderer( )
		{
			m_Texture = Graphics.Factory.CreateTexture2dSampler( );
			m_Texture.WrapS = TextureWrap.Repeat;
			m_Texture.WrapT = TextureWrap.Repeat;
		}

		/// <summary>
		/// Gets/sets the rendered size of the particles 
		/// </summary>
		[Description( "The size of the rendered particles" )]
		public float ParticleSize
		{
			get { return m_ParticleSize; }
			set { m_ParticleSize = value; }
		}

		/// <summary>
		/// Gets/sets the texture used to render the particles
		/// </summary>
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		[Editor( typeof( Texture2dUITypeEditor ), typeof( System.Drawing.Design.UITypeEditor ) )]
		[Description( "The texture used when rendering the particles" )]
		public ITexture2d Texture
		{
			get { return m_Texture.Texture; }
			set { m_Texture.Texture = value; }
		}

		/// <summary>
		/// Gets/sets alpha blending
		/// </summary>
		[Category( "Alpha Blending" )]
		public bool Blend
		{
			get { return m_RenderState.Blend; }
			set { m_RenderState.Blend = value; }
		}

		/// <summary>
		/// Gets/sets source blending
		/// </summary>
		[Category( "Alpha Blending" )]
		public BlendFactor SourceBlend
		{
			get { return m_RenderState.SourceBlend; }
			set { m_RenderState.SourceBlend = value; }
		}

		/// <summary>
		/// Gets/sets destination blending
		/// </summary>
		[Category( "Alpha Blending" )]
		public BlendFactor DestinationBlend
		{
			get { return m_RenderState.DestinationBlend; }
			set { m_RenderState.DestinationBlend = value; }
		}

		#region IParticleRenderer Members

		/// <summary>
		/// Renders a particle system
		/// </summary>
		public unsafe void RenderParticles( IRenderContext context, IParticleSystem particleSystem )
		{
			ISerialParticleBuffer sBuffer = ( ISerialParticleBuffer )particleSystem.Buffer;
			if ( m_MaxParticles != sBuffer.MaximumNumberOfParticles )
			{
				BuildBuffers( sBuffer.MaximumNumberOfParticles );
			}
			using ( IVertexBufferLock vbLock = m_Vb.Lock( 0, sBuffer.NumActiveParticles * 4, false, true ) )
			{
				Vertex* curVertex = ( Vertex* )vbLock.Bytes;
				SerialParticleFieldIterator posIter = new SerialParticleFieldIterator( sBuffer, ParticleBase.Position );

				ICamera3 camera = ( ICamera3 )Graphics.Renderer.Camera;
				Vector3 xAxis = camera.Frame.XAxis * ParticleSize / 2;
				Vector3 yAxis = camera.Frame.YAxis * ParticleSize / 2;

				const float maxU = 1.0f;
				const float maxV = 1.0f;

				for ( int particleIndex = 0; particleIndex < sBuffer.NumActiveParticles; ++particleIndex )
				{
					posIter.MoveNext( );
					curVertex++->Setup( posIter.Point3Value - xAxis - yAxis, 0, 0 );
					curVertex++->Setup( posIter.Point3Value + xAxis - yAxis, maxU, 0 );
					curVertex++->Setup( posIter.Point3Value + xAxis + yAxis, maxU, maxV );
					curVertex++->Setup( posIter.Point3Value - xAxis + yAxis, 0, maxV );
				}
			}

			m_RenderState.Begin( );
			if ( m_Texture.Texture != null )
			{
				Graphics.Renderer.PushTextures( );
				m_Texture.Begin( );
			}
			m_Vb.Begin( );
			m_Ib.Draw( PrimitiveType.TriList, 0, sBuffer.NumActiveParticles * 2 );
			m_Vb.End( );
			if ( m_Texture.Texture != null )
			{
				m_Texture.End( );
				Graphics.Renderer.PopTextures( );
			}
			m_RenderState.End( );
		}

		#endregion

		#region IParticleSystemComponent Members

		/// <summary>
		/// Called when this component is attached to a particle system
		/// </summary>
		public void Attach( IParticleSystem particles )
		{
			particles.Buffer.AddField( ParticleBase.Position, ParticleFieldType.Float32, 3, 0.0f );
		}

		#endregion
		
		#region Private Members

		#region Vertex Struct

		[StructLayout( LayoutKind.Sequential )]
		private struct Vertex
		{
			public float m_X;
			public float m_Y;
			public float m_Z;
			public float m_U;
			public float m_V;

			public void Setup( Point3 pos, float u, float v )
			{
				Position = pos;
				m_U = u;
				m_V = v;
			}

			public Point3 Position
			{
				set
				{
					m_X = value.X;
					m_Y = value.Y;
					m_Z = value.Z;
				}
			}
		}

		#endregion

		private float m_ParticleSize = 0.1f;
		private readonly ITexture2dSampler m_Texture;
		private IVertexBuffer m_Vb;
		private IIndexBuffer m_Ib;
		private int m_MaxParticles;
		private readonly IRenderState m_RenderState = CreateRenderState( );

		private static IRenderState CreateRenderState( )
		{
			IRenderState renderState = Graphics.Factory.CreateRenderState();
			renderState.DepthTest = true;
			renderState.DepthWrite = true;
			renderState.CullFaces = false;
			renderState.Enable2dTextures = true;
			renderState.Enable2dTextureUnit( 0, true );
			renderState.Blend = true;
			renderState.SourceBlend = BlendFactor.SrcAlpha;
			renderState.DestinationBlend = BlendFactor.One;
			return renderState;
		}
		
		private void BuildBuffers( int maxParticles )
		{
			m_MaxParticles = maxParticles;
			m_Vb = Graphics.Factory.CreateVertexBuffer( );

			VertexBufferFormat vbFormat = new VertexBufferFormat( );
			vbFormat.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			vbFormat.Add( VertexFieldSemantic.Texture0, VertexFieldElementTypeId.Float32, 2 );
			vbFormat.Static = false;
			m_Vb.Create( vbFormat, maxParticles * 4 );

			m_Ib = Graphics.Factory.CreateIndexBuffer( );

			ushort[] indices = new ushort[ maxParticles * 6 ];
			int curIndex = 0;
			ushort indexValue = 0;
			for ( int particleCount = 0; particleCount < maxParticles; ++particleCount )
			{
				indices[ curIndex++ ] = indexValue;
				indices[ curIndex++ ] = ( ushort )( indexValue + 1 );
				indices[ curIndex++ ] = ( ushort )( indexValue + 3 );
				
				indices[ curIndex++ ] = ( ushort )( indexValue + 1 );
				indices[ curIndex++ ] = ( ushort )( indexValue + 2 );
				indices[ curIndex++ ] = ( ushort )( indexValue + 3 );

				indexValue += 4;
			}
			m_Ib.Create( indices, true );
		}

		#endregion

	}
}
