
using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Creates editable representations of in-game objects
	/// </summary>
	public interface IObjectEditorBuilder
	{
		/// <summary>
		/// Creates an IObjectEditor object from a runtime game object
		/// </summary>
		/// <param name="pick">Position to place the object at</param>
		/// <param name="instance">Runtime game object</param>
		/// <returns>Selectable instance</returns>
		IObjectEditor Create( ILineIntersection pick, object instance );
	}
}
