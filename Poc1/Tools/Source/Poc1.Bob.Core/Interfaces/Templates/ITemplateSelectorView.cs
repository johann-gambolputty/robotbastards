using System;

namespace Poc1.Bob.Core.Interfaces.Templates
{
	/// <summary>
	/// Template selection viewer
	/// </summary>
	public interface ITemplateSelectorView
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
		/// Gets the currently selected template base. Returns null if no template group or template is selected
		/// </summary>
		TemplateBase SelectedTemplateBase
		{
			get;
		}

		/// <summary>
		/// Gets the currently selected template. Returns null if no template is selected
		/// </summary>
		Template SelectedTemplate
		{
			get;
		}

		/// <summary>
		/// Gets/sets the root template group
		/// </summary>
		TemplateGroupContainer RootGroup
		{
			get; set;
		}

		/// <summary>
		/// Refreshes the view (TODO: AP: Lazy... should watch for updates on the model)
		/// </summary>
		void Refresh( );
	}
}
