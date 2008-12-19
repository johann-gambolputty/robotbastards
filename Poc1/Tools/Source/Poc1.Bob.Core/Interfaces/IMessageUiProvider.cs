using System;

namespace Poc1.Bob.Core.Interfaces
{
	/// <summary>
	/// Provides functionality to show UI for simple messages and errors
	/// </summary>
	public interface IMessageUiProvider
	{
		/// <summary>
		/// Shows an error message dialog with a formatted string message
		/// </summary>
		void ShowError( string errorMessageFormat, params object[] args );

		/// <summary>
		/// Shows an error message dialog with a formatted string message, and exception details
		/// </summary>
		void ShowError( Exception ex, string errorMessageFormat, params object[] args );
	}
}
