
namespace Rb.Rendering.Interfaces.Objects
{
    /// <summary>
    /// Provides the context in which objects can be rendered
    /// </summary>
    public interface IRenderContext
	{
		#region Public properties

		/// <summary>
		/// Time that the render occurred in TinyTime clock ticks
		/// </summary>
		long RenderTime
		{
			get; set;
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
        ITechnique GlobalTechnique
        {
            get;
        }

        /// <summary>
        /// Adds a technique to the global technique stack
        /// </summary>
        /// <seealso cref="GlobalTechnique"/>
        void PushGlobalTechnique( ITechnique technique );

        /// <summary>
        /// Pops a technique from the global technique stack
		/// </summary>
		/// <seealso cref="GlobalTechnique"/>
        void PopGlobalTechnique( );

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
        void ApplyTechnique( ITechnique technique, IRenderable renderable );

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
		void ApplyTechnique( ITechnique technique, RenderDelegate render );

		#endregion
	}
}
