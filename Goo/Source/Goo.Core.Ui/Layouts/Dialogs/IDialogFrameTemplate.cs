namespace Goo.Common.Layouts.Dialogs
{
	/// <summary>
	/// Defines a template that can be used to create dialog frames
	/// </summary>
	public interface IDialogFrameTemplate : IDialogFrameFactory
	{
		/// <summary>
		/// Gets/sets the dialog title
		/// </summary>
		string Title
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the resizable flag
		/// </summary>
		bool Resizable
		{
			get; set;
		}

		/// <summary>
		/// Adds a button to the bottom right of the frame, that causes the dialog to exit when pressed
		/// </summary>
		IDialogFrameTemplate AddExitButton( string text, ResultOfDialog result );
	}
}
