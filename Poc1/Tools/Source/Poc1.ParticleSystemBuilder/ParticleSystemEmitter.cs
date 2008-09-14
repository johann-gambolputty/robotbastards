
using System.Drawing;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.ParticleSystemBuilder
{
	/// <summary>
	/// Wrapper around a particle system. Renders the particle system frame
	/// </summary>
	class ParticleSystemEmitter : IRenderable
	{
		/// <summary>
		/// Sets the wrapped particle system
		/// </summary>
		public ParticleSystemEmitter( IParticleSystem ps )
		{
			m_Ps = ps;
		}

		/// <summary>
		/// Gets the wrapped particle system's frame
		/// </summary>
		public Matrix44 Frame
		{
			get { return m_Ps.Frame; }
		}

		#region IRenderable Members

		/// <summary>
		/// Renders the emitter shape
		/// </summary>
		public void Render( IRenderContext context )
		{
			Graphics.Draw.Sphere( s_PsSurface, m_Ps.Frame.Translation, 0.1f );
			m_Ps.Render( context );
		}

		#endregion

		#region Private Members

		private readonly IParticleSystem m_Ps;
		private readonly static DrawBase.ISurface s_PsSurface;

		static ParticleSystemEmitter( )
		{
			s_PsSurface = Graphics.Draw.NewSurface( Color.White );
			s_PsSurface.State.DepthTest = true;
			s_PsSurface.State.DepthWrite = true;
		}
		
		#endregion
	}
}
