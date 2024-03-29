using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Actions;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Interface for objects that can be picked
	/// </summary>
	public interface IPickable
	{
		/// <summary>
		/// Creates an action in response being picked
		/// </summary>
		/// <param name="pick">Pick information</param>
		/// <returns>Returns the pick action</returns>
		IPickAction CreatePickAction( ILineIntersection pick );

		/// <summary>
		/// Returns true if the specified pick action is supported
		/// </summary>
		/// <param name="action">Action to check</param>
		/// <returns>true if action is supported by this object</returns>
		bool SupportsPickAction( IPickAction action );
	}
}
