using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core.EditModes
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
			m_Controls = EditModeContext.Instance.EditControls;
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

		#region Private members

		private readonly Control[] m_Controls;

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
		}

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		public virtual void Stop( )
		{
		}

		#endregion
	}
}
