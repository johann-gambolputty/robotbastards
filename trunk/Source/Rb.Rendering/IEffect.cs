namespace Rb.Rendering
{
	/// <summary>
	/// A shader is a group of techniques (<see cref="ITechnique"/>)
	/// </summary>
	public interface IEffect : IPass
	{
		/// <summary>
		/// Gets a shader parameter by its name
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <returns>Returns the named shader parameter, or null if it doesn't exist</returns>
		ShaderParameter GetParameter( string name );

        /// <summary>
        /// Gets a technique from its name
        /// </summary>
        ITechnique GetTechnique( string name );

		/// <summary>
		/// Finds a technique in this effect that can substitute the specified technique
		/// </summary>
		/// <param name="technique">Technique to substitute</param>
		/// <returns>
		/// Returns an ITechnique from this effect that can substitute technique. If none
		/// can be found, technique is returned.
		/// </returns>
		ITechnique SubstituteTechnique( ITechnique technique );
	}
}
