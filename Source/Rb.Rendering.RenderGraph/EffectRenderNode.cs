using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.RenderGraph
{
	/// <summary>
	/// Implementation of <see cref="IRenderNode"/> that uses an <see cref="IEffect"/> for rendering
	/// </summary>
	public class EffectRenderNode : RenderNode
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Node name</param>
		/// <param name="id">Node unique, zero-based identifier</param>
		public EffectRenderNode( string name, int id ) :
			base( name, id )
		{
		}

		/// <summary>
		/// Gets the effect technique selector
		/// </summary>
		public TechniqueSelector Selector
		{
			get { return m_Selector; }
		}

		/// <summary>
		/// Gets the render target
		/// </summary>
		public IRenderTarget Target
		{
			get { return m_Target; }
			set
			{
				if ( m_TargetColourDataSource != null )
				{
					Graphics.EffectDataSources.UnbindDataSource( m_TargetColourDataSource );
				}
				if ( m_TargetDepthDataSource != null )
				{
					Graphics.EffectDataSources.UnbindDataSource( m_TargetDepthDataSource );	
				}
				m_Target = value;
				if ( m_Target != null )
				{
					string targetColour = Name + ".Target.Colour";
					string targetDepth = Name + ".Target.Depth";

					GraphicsLog.Info( "Adding annotation data source for render node target colour buffer ({0})", targetColour );
					m_TargetColourDataSource = Graphics.EffectDataSources.CreateValueDataSourceForSemantic<ITexture>( targetColour );
					
					GraphicsLog.Info( "Adding annotation data source for render node target depth buffer ({0})", targetDepth );
					m_TargetDepthDataSource = Graphics.EffectDataSources.CreateValueDataSourceForNamedParameter<ITexture>( targetDepth );
				}
			}
		}

		/// <summary>
		/// Runs this node
		/// </summary>
		public override void Run( IRenderContext context, IRenderable renderable )
		{
			if ( m_Target != null )
			{
				m_Target.Begin( );
			}
			try
			{
				Selector.Apply( context, renderable );
			}
			finally
			{
				if ( m_Target != null )
				{
					m_Target.End( );
					m_TargetColourDataSource.Value = m_Target.Texture;
					m_TargetDepthDataSource.Value = m_Target.DepthTexture;
				}
			}
		}

		#region Private Members

		private IRenderTarget m_Target;
		private readonly TechniqueSelector m_Selector = new TechniqueSelector( );
		private IEffectValueDataSource<ITexture> m_TargetColourDataSource;
		private IEffectValueDataSource<ITexture> m_TargetDepthDataSource;

		#endregion
	}
}
