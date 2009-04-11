using System;

namespace Poc1.Bob.Core.Interfaces.Projects
{
	/// <summary>
	/// ProjectType selection viewer
	/// </summary>
	public interface IProjectTypeSelectorView
	{
		/// <summary>
		/// Event raised when the user changes the current selection
		/// </summary>
		event EventHandler SelectionChanged;

		/// <summary>
		/// Gets/sets the currently displayed descriptive text
		/// </summary>
		string Description
		{
			get; set;
		}

		/// <summary>
		/// Gets the currently selected project node. Returns null if no template group or template is selected
		/// </summary>
		ProjectNode SelectedProjectNode
		{
			get;
		}

		/// <summary>
		/// Gets the currently selected template. Returns null if no template is selected
		/// </summary>
		ProjectType SelectedProjectType
		{
			get;
		}

		/// <summary>
		/// Gets/sets the root template group
		/// </summary>
		ProjectGroupContainer RootGroup
		{
			get; set;
		}

		/// <summary>
		/// Refreshes the view (TODO: AP: Lazy... should watch for updates on the model)
		/// </summary>
		void Refresh( );
	}
}
