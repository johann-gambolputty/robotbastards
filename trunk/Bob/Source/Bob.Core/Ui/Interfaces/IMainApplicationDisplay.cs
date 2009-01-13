
namespace Bob.Core.Ui.Interfaces
{
	/// <summary>
	/// Main application display interface
	/// </summary>
	public interface IMainApplicationDisplay
	{
		/// <summary>
		/// Gets the command UI manager
		/// </summary>
		ICommandUiManager CommandUi
		{
			get;
		}

		/// <summary>
		/// Gets the message UI manager
		/// </summary>
		IMessageUiProvider MessageUi
		{
			get;
		}

		/// <summary>
		/// Closes the application
		/// </summary>
		void Close( );
	}
}
