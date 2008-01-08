
namespace Rb.Rendering
{
	/// <summary>
	/// Interface for objects that can select a technique from an effect, to render with
	/// </summary>
	public interface ITechniqueSelector
	{
		/// <summary>
		/// Gets/sets the effect that techniques are selected from
		/// </summary>
		IEffect Effect
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the current technique
		/// </summary>
		/// <remarks>
		/// Setter will override <see cref="Effect"/>
		/// </remarks>
		ITechnique Technique
		{
			get; set;
		}

		/// <summary>
		/// Selects a given technique from the current effect, by name
		/// </summary>
		/// <param name="techniqueName">Name of the technique to select</param>
		void Select( string techniqueName );
	}
}
