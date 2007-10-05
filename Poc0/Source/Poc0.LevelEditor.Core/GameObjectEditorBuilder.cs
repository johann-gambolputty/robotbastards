using Poc0.Core;
using Rb.Core.Components;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Creates editable shells around game objects
	/// </summary>
	public class GameObjectEditorBuilder : ObjectEditorBuilder
	{
		/// <summary>
		/// Creates an ISelectable object from a runtime game object
		/// </summary>
		/// <param name="pick">Position to place the object at</param>
		/// <param name="instance">Runtime game object</param>
		/// <returns>Selectable instance</returns>
		public override IObjectEditor Create( PickInfoCursor pick, object instance )
		{
			ObjectEditor result = new ObjectEditor( instance );

			IHasPosition hasPosition = Parent.GetType< IHasPosition >( instance );
			if ( hasPosition != null )
			{
				result.Add( new PositionEditor( hasPosition, pick ) );
			}

			return result;
		}

	}
}
