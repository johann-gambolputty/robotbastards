
namespace Poc1.Bob.Core.Interfaces.Templates.Generic
{
	/// <summary>
	/// Extends <see cref="TemplateGroup"/>. Templates created by this group are of type T
	/// </summary>
	/// <typeparam name="T">Type of Project type created by this group</typeparam>
	public class TemplateGroup<T> : TemplateGroup
		where T : Template
	{
		/// <summary>
		/// Creates a new template of type T
		/// </summary>
		protected override Template CreateTemplate( )
		{
			Template template = ( Template )System.Activator.CreateInstance( typeof( T ), this );
			return template;
		}
	}
}
