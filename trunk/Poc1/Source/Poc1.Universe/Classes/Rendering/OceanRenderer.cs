using Rb.Assets;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Base class for ocean rendering
	/// </summary>
	public abstract class OceanRenderer
	{
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		public OceanRenderer( )
		{
			m_Texture = ( ITexture )AssetManager.Instance.Load( "Ocean/ocean0.jpg", new TextureLoader.TextureLoadParameters( true ) );

			//	Load in sea effect
			IEffect seaEffect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/Ocean.cgfx" );
			m_OceanTechniques = new TechniqueSelector( seaEffect, "DefaultTechnique" );
		}

		/// <summary>
		/// Renders the ocean
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			m_OceanTechniques.Effect.Parameters[ "OceanTexture" ].Set( m_Texture );
			context.ApplyTechnique( m_OceanTechniques, m_OceanGeometry );
		}

		#region Protected Members

		/// <summary>
		/// Gets/sets the object containing the renderable ocean geometry
		/// </summary>
		protected IRenderable OceanGeometry
		{
			get { return m_OceanGeometry; }
			set { m_OceanGeometry = value; }
		}

		#endregion

		#region Private Members

		private readonly ITexture m_Texture;
		private IRenderable m_OceanGeometry;
		private readonly TechniqueSelector m_OceanTechniques;

		#endregion


	}
}
