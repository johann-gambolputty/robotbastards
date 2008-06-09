using Poc1.Particles.Interfaces;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Renders particles as point sprites
	/// </summary>
	public class SimpleSpriteRenderer : IParticleRenderer
	{
		SimpleSpriteRenderer( )
		{
		}

		private void BuildBuffers( int maxParticles )
		{
			m_Vb = Graphics.Factory.CreateVertexBuffer( );

			VertexBufferFormat vbFormat = new VertexBufferFormat( );
			vbFormat.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			vbFormat.Static = false;
			m_Vb.Create( vbFormat, maxParticles * 4 );

			m_Ib = Graphics.Factory.CreateIndexBuffer( );

			ushort[] indices = new ushort[ maxParticles * 6 ];
			for ( int particleCount = 0; particleCount < maxParticles; ++particleCount )
			{
				
			}
			m_Ib.Create( indices, false );
		}

		/// <summary>
		/// Gets/sets the texture used to render the particles
		/// </summary>
		public ITexture2d Texture
		{
			get { return m_Texture; }
			set { m_Texture = value; }
		}

		#region Private Members

		private ITexture2d m_Texture;
		private IVertexBuffer m_Vb;
		private IIndexBuffer m_Ib;

		#endregion

		#region IParticleRenderer Members

		/// <summary>
		/// Renders a particle system
		/// </summary>
		public void RenderParticles( IRenderContext context, IParticleSystem particleSystem )
		{

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
	}
}
