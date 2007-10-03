
namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Interface for selectable objects that are displayed in the editor
	/// </summary>
	public interface ISelectable
	{
		/// <summary>
		/// Highlight flag
		/// </summary>
		bool Highlighted
		{
			get; set;
		}

		/// <summary>
		/// Selection flag
		/// </summary>
		bool Selected
		{
			get; set;
		}
	}
}
