
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Techniques modify the rendering of an object in serial (unlike IPass, which modifies in parallel)
	/// </summary>
	public interface ITechnique
	{
		/// <summary>
		/// Gets the name of this technique
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the effect that this technique belongs to (can be null if no effect owns this technique)
		/// </summary>
		IEffect Effect
		{
			get; set;
		}

		/// <summary>
		/// Applies this technique when rendering the specified object
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="renderable">Object to render</param>
		void Apply( IRenderContext context, IRenderable renderable );

		/// <summary>
		/// Applies this technique to a render delegate
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="render">Render delegate</param>
		void Apply( IRenderContext context, RenderDelegate render );


		//	TODO: AP: Refactor this out to another class

		/// <summary>
		/// Returns true if this technique is a reasonable substitute for the specified technique
		/// </summary>
		/// <param name="technique">Technique to substitute</param>
		/// <returns>true if this technique can substitute the specified technique</returns>
		bool IsSubstituteFor( ITechnique technique );
	}
}
