
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Base class decorating a render context
	/// </summary>
	public class RenderContextDecorator : IRenderContext
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="innerContext">Inner context</param>
		public RenderContextDecorator( IRenderContext innerContext )
		{
			Arguments.CheckNotNull( innerContext, "innerContext" );
			m_InnerContext = innerContext;
		}

		#region IRenderContext Members

		#region Public properties

		/// <summary>
		/// Gets the number of frames rendered so far
		/// </summary>
		public ulong RenderFrameCounter
		{
			get { return m_InnerContext.RenderFrameCounter; }
		}

		/// <summary>
		/// Time that the render occurred in TinyTime clock ticks
		/// </summary>
		public long RenderTime
		{
			get { return m_InnerContext.RenderTime; }
		}

		#endregion

		#region Global technique

		/// <summary>
		/// Gets the current global technique
		/// </summary>
		/// <remarks>
		/// The global technique overrides locally applied techniques (passed to ApplyTechnique), unless
		/// the local technique is a reasonable substitute (<see cref="ITechnique.IsSubstituteFor"/>).
		/// </remarks>
		public ITechnique GlobalTechnique
		{
			get { return m_InnerContext.GlobalTechnique; }
		}

		/// <summary>
		/// Adds a technique to the global technique stack
		/// </summary>
		/// <seealso cref="GlobalTechnique"/>
		public void PushGlobalTechnique( ITechnique technique )
		{
			Arguments.CheckNotNull( technique, "technique" );
			m_InnerContext.PushGlobalTechnique( technique );
		}

		/// <summary>
		/// Pops a technique from the global technique stack
		/// </summary>
		/// <seealso cref="GlobalTechnique"/>
		public void PopGlobalTechnique( )
		{
			m_InnerContext.PopGlobalTechnique( );
		}

		#endregion

		#region Applying techniques

		/// <summary>
		/// Renders a renderable object using a given technique
		/// </summary>
		/// <param name="technique">Technique to render with</param>
		/// <param name="renderable">Object to render</param>
		/// <remarks>
		/// Equivalent to <see cref="ITechnique.Apply(IRenderContext,IRenderable)"/>. Differs in that if technique is a valid substitute to the 
		/// global technique (<see cref="GlobalTechnique"/>), technique is used to render the object, otherwise, the
		///  global technique is used.
		/// </remarks>
		public void ApplyTechnique( ITechnique technique, IRenderable renderable )
		{
			m_InnerContext.ApplyTechnique( technique, renderable );
		}

		/// <summary>
		/// Calls a rendering method using a given technique
		/// </summary>
		/// <param name="technique">Technique to render with</param>
		/// <param name="render">Render method delegate</param>
		/// <remarks>
		/// Equivalent to <see cref="ITechnique.Apply(IRenderContext,RenderDelegate)"/>. Differs in that if technique is a valid substitute to the 
		/// global technique (<see cref="GlobalTechnique"/>), technique is used to render the object, otherwise, the
		///  global technique is used.
		/// </remarks>
		public void ApplyTechnique( ITechnique technique, RenderDelegate render )
		{
			m_InnerContext.ApplyTechnique( technique, render );
		}

		#endregion

		#endregion

		#region Private Members

		private readonly IRenderContext m_InnerContext;

		#endregion

	}
}
