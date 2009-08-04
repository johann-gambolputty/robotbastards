
using System.Drawing;
using Rb.Interaction.Classes;

namespace Goo.Core.Ui.Commands.Menus
{
	/// <summary>
	/// Menu item interface
	/// </summary>
	public interface IMenuItem : IMenuNode
	{
		/// <summary>
		/// Fluent interface for setting images
		/// </summary>
		IMenuItem SetImage( Image image);

		/// <summary>
		/// Fluent interface for setting text
		/// </summary>
		IMenuItem SetText( string text );

		/// <summary>
		/// Gets/sets this item's image
		/// </summary>
		Image Image
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets this item's text
		/// </summary>
		string Text
		{
			get; set;
		}

		/// <summary>
		/// Gets the command associated with this item (can be null)
		/// </summary>
		Command Command
		{
			get;
		}
	}
}
