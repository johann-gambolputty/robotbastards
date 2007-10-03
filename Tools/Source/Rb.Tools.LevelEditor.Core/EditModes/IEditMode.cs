
using System.Windows.Forms;

namespace Rb.Tools.LevelEditor.Core.EditModes
{
	/// <summary>
	/// An edit mode determines how user actions get applied
	/// </summary>
	public interface IEditMode
	{
		/// <summary>
		/// Called when the edit mode is activated
		/// </summary>
		void Start( );

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		void Stop( );

		/// <summary>
		/// Gets the key that switches to this edit mode
		/// </summary>
		Keys HotKey
		{
			get;
		}

		/// <summary>
		/// There can be only one exclusive edit mode active at any one time
		/// </summary>
		bool Exclusive
		{
			get;
		}
	}
}
