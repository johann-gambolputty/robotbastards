using Poc0.Core.Objects;
using Rb.World;

namespace Poc0.LevelEditor.Core.Objects
{
	/// <summary>
	/// Edits a player start position
	/// </summary>
	public class PlayerStartEditor : PlaceableObjectEditor
	{
		/// <summary>
		/// Builds the associated player start object
		/// </summary>
		public override void Build( Scene scene )
		{
			PlayerStart obj = new PlayerStart( );
			obj.Position = Position;

			scene.Objects.Add( obj );
		}
	}
}
