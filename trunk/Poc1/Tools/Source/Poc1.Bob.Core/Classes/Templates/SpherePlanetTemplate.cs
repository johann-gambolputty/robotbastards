using Poc1.Bob.Core.Interfaces.Templates;

namespace Poc1.Bob.Core.Classes.Templates
{
	/// <summary>
	/// Sphere planet template
	/// </summary>
	public class SpherePlanetTemplate : Template
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="group">Parent group</param>
		/// <exception cref="System.ArgumentNullException">Thrown if group is null</exception>
		public SpherePlanetTemplate( TemplateGroup group ) : base( group )
		{
		}
	}
}
