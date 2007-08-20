using System;
using System.Collections.Generic;
using System.Text;
using Rb.World;
using System.Windows.Forms;

namespace Poc0.LevelEditor.Core.EditModes
{

	/// <summary>
	/// Singleton class that stores the context for editing code
	/// </summary>
	public class EditModeContext
	{
		#region Singleton

		/// <summary>
		/// Gets the singleton instance of this class
		/// </summary>
		public static EditModeContext Instance
		{
			get { return ms_Singleton; }
		}

		/// <summary>
		/// Creates the new EditModeContext singleton
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="grid"></param>
		/// <param name="selection"></param>
		public static EditModeContext CreateNewContext( Scene scene, TileGrid grid, SelectedObjects selection )
		{
			ms_Singleton = new EditModeContext( scene, grid, selection );
			return Instance;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets an array of edit controls
		/// </summary>
		public Control[] EditControls
		{
			get { return m_Controls.ToArray( ); }
		}

		/// <summary>
		/// Gets and sets the current edit mode
		/// </summary>
		/// <remarks>
		/// On set, the current edit mode is stopped (<see cref="IEditMode.Stop"/>) and
		/// the new edit mode is started (<see cref="IEditMode.Start"/>).
		/// </remarks>
		public IEditMode EditMode
		{
			get { return m_EditMode; }
			set
			{
				if ( m_EditMode != null )
				{
					m_EditMode.Stop( );
				}
				m_EditMode = value;
				if ( m_EditMode != null )
				{
					m_EditMode.Start( );
				}
			}
		}

		/// <summary>
		/// Sets and gets the current tile under the mouse cursor
		/// </summary>
		public Tile TileUnderCursor
		{
			get { return m_TileUnderCursor; }
			set { m_TileUnderCursor = value; }
		}

		/// <summary>
		/// Gets the selected object set
		/// </summary>
		public SelectedObjects Selection
		{
			get { return m_Selection; }
		}

		/// <summary>
		/// Gets the tile grid
		/// </summary>
		public TileGrid Grid
		{
			get { return m_Grid; }
		}

		/// <summary>
		/// Gets the scene
		/// </summary>
		public Scene Scene
		{
			get { return m_Scene; }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Adds an edit control
		/// </summary>
		public void AddEditControl( Control control )
		{
			m_Controls.Add( control );
		}

		/// <summary>
		/// Removes an edit control
		/// </summary>
		public void RemoveEditControl( Control control )
		{
			m_Controls.Remove( control );
		}

		#endregion

		#region Private members

		private static	EditModeContext		ms_Singleton;

		public readonly List< Control >		m_Controls = new List< Control >( );
		private IEditMode					m_EditMode;
		private Tile						m_TileUnderCursor;
		private readonly SelectedObjects	m_Selection;
		private readonly TileGrid			m_Grid;
		private readonly Scene				m_Scene;

		/// <summary>
		/// Sets up the context
		/// </summary>
		/// <param name="scene">Scene</param>
		/// <param name="grid">Tile grid</param>
		/// <param name="selection">Selected objects</param>
		private EditModeContext( Scene scene, TileGrid grid, SelectedObjects selection )
		{
			m_Scene = scene;
			m_Grid = grid;
			m_Selection = selection;
		}

		#endregion

	}
}
