using System.Windows.Forms;

namespace Rb.Tools.LevelEditor.Core.EditModes
{
	/// <summary>
	/// Base class implementation of <see cref="IEditMode"/>
	/// </summary>
	public class EditMode : IEditMode
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public EditMode( )
		{
			m_Controls = EditorState.Instance.CurrentSceneEditState.EditModeControls.ToArray( );
		}

		#region Public properties

		/// <summary>
		/// Gets the associated controls
		/// </summary>
		public Control[] Controls
		{
			get { return m_Controls; }
		}

		#endregion

		#region IEditMode Members

		/// <summary>
		/// Returns <see cref="Keys.None"/> (no hotkey activates this edit mode)
		/// </summary>
		public virtual Keys HotKey
		{
			get { return Keys.None; }
		}

		/// <summary>
		/// Returns true
		/// </summary>
		public virtual bool Exclusive
		{
			get { return true; }
		}

		/// <summary>
		/// Called when the edit mode is activated
		/// </summary>
		public virtual void Start( )
		{
			foreach ( Control control in m_Controls )
			{
				BindToControl( control );
			}
		}

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		public virtual void Stop( )
		{
			foreach ( Control control in m_Controls )
			{
				UnbindFromControl( control );
			}
		}

		#region Protected members

		/// <summary>
		/// Binds this edit mode to a given control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		protected virtual void BindToControl( Control control )
		{
		}

		/// <summary>
		/// Unbinds this edit mode from a given control
		/// </summary>
		/// <param name="control">Control to unbind from</param>
		protected virtual void UnbindFromControl( Control control )
		{
		}

		#endregion

		#endregion

		#region Private members

		private readonly Control[] m_Controls;

		#endregion

	}
}
