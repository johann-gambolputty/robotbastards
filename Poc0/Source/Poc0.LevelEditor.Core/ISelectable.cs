
namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Editor selection interface
	/// </summary>
	public interface ISelectable
	{
		/// <summary>
		/// Selected flag
		/// </summary>
		bool Selected
		{
			get; set;
		}

		/// <summary>
		/// Highlight flag
		/// </summary>
		bool Highlight
		{
			get; set;
		}
	}
}
