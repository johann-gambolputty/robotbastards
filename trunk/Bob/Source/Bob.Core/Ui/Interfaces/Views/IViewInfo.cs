
using Rb.Interaction.Classes;

namespace Bob.Core.Ui.Interfaces.Views
{
	/// <summary>
	/// View information
	/// </summary>
	public interface IViewInfo
	{
		/// <summary>
		/// Gets the view name
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the command used to show this view
		/// </summary>
		Command ShowCommand
		{
			get;
		}
	}
}
