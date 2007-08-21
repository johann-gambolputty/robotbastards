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
		/// Adds an edit mode. If the mode is exclusive, then it replaces the current exclusive mode
		/// </summary>
		/// <param name="mode">Mode to add</param>
		public void AddEditMode( IEditMode mode )
		{
			if ( mode.Exclusive )
			{
				if ( m_ExclusiveMode != null )
				{
					m_ExclusiveMode.Stop( );
				}
				m_ExclusiveMode = mode;
				mode.Start( );
			}
			else if ( !m_SharedModes.Contains( mode ) )
			{
				m_SharedModes.Add(mode);
				mode.Start( );
			}
		}

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
		private Tile						m_TileUnderCursor;
		private readonly SelectedObjects	m_Selection;
		private readonly TileGrid			m_Grid;
		private readonly Scene				m_Scene;

		private IEditMode					m_ExclusiveMode;
		private readonly List< IEditMode >	m_SharedModes = new List< IEditMode >( );

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
