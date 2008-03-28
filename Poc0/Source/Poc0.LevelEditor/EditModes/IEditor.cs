using System.Windows.Forms;
using Rb.Rendering.Interfaces.Objects;

namespace Poc0.LevelEditor.EditModes
{
	/// <summary>
	/// Interface for scene editors
	/// </summary>
	public interface IEditor : IRenderable
	{
		/// <summary>
		/// Gets a string describing the usage of this editor
		/// </summary>
		string Description
		{
			get;
		}

		/// <summary>
		/// Binds this editor to a control
		/// </summary>
		void BindToControl( Control control );

		/// <summary>
		/// Unbinds this editor from a control
		/// </summary>
		void UnbindFromControl( Control control );
	}
}
