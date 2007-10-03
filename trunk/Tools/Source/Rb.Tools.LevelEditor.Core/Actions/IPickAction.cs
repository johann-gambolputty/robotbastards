using System.Collections;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Rb.Tools.LevelEditor.Core.Actions
{
	/// <summary>
	/// An action that relies on changing pick data
	/// </summary>
	public interface IPickAction : IAction
	{
		/// <summary>
		/// Adds objects to this action
		/// </summary>
		/// <param name="objects">Objects to add</param>
		void AddObjects( IEnumerable objects );

		/// <summary>
		/// Called when the pick info changes
		/// </summary>
		/// <param name="lastPick">Last pick information</param>
		/// <param name="curPick">Current pick information</param>
		void PickChanged( PickInfoCursor lastPick, PickInfoCursor curPick );

		/// <summary>
		/// Returns true if PickChanged() was called with arguments that altered the states of the attached objects
		/// </summary>
		bool HasModifiedObjects
		{
			get;
		}
	}
}
