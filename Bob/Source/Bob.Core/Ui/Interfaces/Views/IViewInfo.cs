
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
		/// Returns true if this view can be created from a command
		/// </summary>
		bool AvailableAsCommand
		{
			get;
		}
	}
}
