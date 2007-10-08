using System.Collections;
using Rb.Core.Maths;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core.Actions
{
	/// <summary>
	/// An action that relies on changing pick data
	/// </summary>
	public interface IPickAction : IAction
	{
		/// <summary>
		/// Gets pick ray cast options associated with this action
		/// </summary>
		RayCastOptions PickOptions
		{
			get;
		}

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
		void PickChanged( ILineIntersection lastPick, ILineIntersection curPick );

		/// <summary>
		/// Returns true if PickChanged() was called with arguments that altered the states of the attached objects
		/// </summary>
		bool HasModifiedObjects
		{
			get;
		}
	}
}
