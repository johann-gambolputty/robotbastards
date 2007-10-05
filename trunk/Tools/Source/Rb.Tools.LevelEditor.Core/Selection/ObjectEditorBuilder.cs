
using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Simple implementation of <see cref="IObjectEditorBuilder"/>
	/// </summary>
	public class ObjectEditorBuilder : IObjectEditorBuilder
	{
		#region ISelectableBuilder Members

		/// <summary>
		/// Creates an ISelectable object from a runtime game object
		/// </summary>
		/// <param name="pick">Position to place the object at</param>
		/// <param name="instance">Runtime game object</param>
		/// <returns>Selectable instance</returns>
		public virtual IObjectEditor Create( ILineIntersection pick, object instance )
		{
			return new ObjectEditor( instance );
		}

		#endregion
	}
}
