using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Tools.TerrainTextures.Core;
using Rb.Core.Utils;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class TerrainTypeListControl : UserControl, ITerrainTypeListView
	{
		public TerrainTypeListControl( )
		{
			InitializeComponent( );
		}

		#region ITerrainTypeListView Members

		/// <summary>
		/// Event raised when the user requests a new terrain type be added to the list
		/// </summary>
		public event ActionDelegates.Action<TerrainType> AddTerrainType;

		/// <summary>
		/// Event raised when the user requests a terrain type be removed from the list
		/// </summary>
		public event ActionDelegates.Action<TerrainType> RemoveTerrainType;

		/// <summary>
		/// Gets/sets the terrain types displayed by this view
		/// </summary>
		public TerrainTypeList TerrainTypes
		{
			get { return m_TerrainTypes; }
			set
			{
				UnbindFromTerrainTypeList( );
				m_TerrainTypes = value;
				BindToTerrainTypeList( );
			}
		}

		/// <summary>
		/// Unbinds this control from the current terrain type list
		/// </summary>
		private void UnbindFromTerrainTypeList( )
		{
			terrainTypeLayoutPanel.Controls.Clear( );
			if ( m_TerrainTypes == null )
			{
				return;
			}
			m_TerrainTypes.TerrainTypeAdded -= OnAddedTerrainType;
			m_TerrainTypes.TerrainTypeRemoved -= OnRemovedTerrainType;
		}

		/// <summary>
		/// Binds this control to the current terrain type list
		/// </summary>
		private void BindToTerrainTypeList( )
		{
			if ( m_TerrainTypes == null )
			{
				return;
			}
			//	Create a control for each terrain type in the list
			foreach ( TerrainType terrainType in m_TerrainTypes.TerrainTypes )
			{
				Control control = CreateControlForTerrainType( terrainType );
				if ( !( control is ITerrainTypeView ) )
				{
					throw new InvalidOperationException( "Controls created for terrain types must implement ITerrainTypeView" );
				}
				control.Enabled = true;
				control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
				terrainTypeLayoutPanel.Controls.Add( control );
			}
			AddNewTerrainTypeControl( new TerrainType( ), false );
			m_TerrainTypes.TerrainTypeAdded += OnAddedTerrainType;
			m_TerrainTypes.TerrainTypeRemoved += OnRemovedTerrainType;
		}

		/// <summary>
		/// Called when a terrain type is added to the terrain type list
		/// </summary>
		private void OnAddedTerrainType( TerrainType terrainType )
		{
			Arguments.CheckNotNull( terrainType, "terrainType" );

			int controlIndex = GetTerrainTypeControlIndex( terrainType );
			if ( controlIndex == -1 )
			{
				AddNewTerrainTypeControl( terrainType, true );
			}
			Control control = terrainTypeLayoutPanel.Controls[ controlIndex ];
			if ( control.Enabled )
			{
				//	Already have an enabled control for the terrain type
				return;
			}
			control.Enabled = true;
			AddNewTerrainTypeControl( new TerrainType( ), false );
		}

		/// <summary>
		/// Returns the index of the control in the terrain type layout panel, that matches the specified terrain type
		/// </summary>
		private int GetTerrainTypeControlIndex( TerrainType terrainType )
		{
			//	Find the control that matches the terrain type
			for ( int controlIndex = 0; controlIndex < terrainTypeLayoutPanel.Controls.Count; ++controlIndex )
			{
				ITerrainTypeView view = ( ITerrainTypeView )terrainTypeLayoutPanel.Controls[ controlIndex ];
				if ( view.TerrainType == terrainType )
				{
					return controlIndex;
				}
			}
			return -1;
		}

		/// <summary>
		/// Called when a terrain type is removed from the terrain type list
		/// </summary>
		private void OnRemovedTerrainType( TerrainType terrainType )
		{
			Arguments.CheckNotNull( terrainType, "terrainType" );

			//	Find the control that matches the terrain type
			int controlIndex = GetTerrainTypeControlIndex( terrainType );
			if ( controlIndex == -1 )
			{
				return;
			}

			ITerrainTypeView view = ( ITerrainTypeView )terrainTypeLayoutPanel.Controls[ controlIndex ];
			if ( view.TerrainType == terrainType )
			{
				terrainTypeLayoutPanel.Controls.RemoveAt( controlIndex );
				terrainTypeLayoutPanel.RowCount = terrainTypeLayoutPanel.Controls.Count;
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Creates a new control for a terrain type instance
		/// </summary>
		protected virtual Control CreateControlForTerrainType( TerrainType terrainType )
		{
			TerrainTypeTextureItemControl control = new TerrainTypeTextureItemControl( );
			control.TerrainType = terrainType;
			return control;
		}

		#endregion

		#region Private Members

		private TerrainTypeList m_TerrainTypes;

		/// <summary>
		/// Add new terrain type control
		/// </summary>
		private void AddNewTerrainTypeControl( TerrainType terrainType, bool enable )
		{
			Control control = CreateControlForTerrainType( terrainType );
			if ( !( control is ITerrainTypeView ) )
			{
				throw new InvalidOperationException( "Controls created for terrain types must implement ITerrainTypeView" );
			}
			control.Enabled = enable;
			control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

			//	A bit of hacky logic here - we know that if the control is enabled,
			//	then we want to insert it before the end
			if ( enable )
			{
				int row = terrainTypeLayoutPanel.Controls.Count - 1;
				terrainTypeLayoutPanel.Controls.Add( control, 0, row );
			}
			else
			{
				terrainTypeLayoutPanel.Controls.Add( control );
			}
			( ( ITerrainTypeView )control ).RemoveTerrainType += OnRemoveTerrainType;
		}

		#region Event Handlers

		private void OnRemoveTerrainType( ITerrainTypeView view )
		{
			terrainTypeLayoutPanel.Controls.Remove( ( Control )view );	
		}

		private void terrainTypeLayoutPanel_MouseClick( object sender, MouseEventArgs e )
		{
			ITerrainTypeView control = terrainTypeLayoutPanel.GetChildAtPoint( e.Location ) as ITerrainTypeView;
			if ( control == null )
			{
				return;
			}
			if ( !control.Enabled )
			{
				if ( AddTerrainType != null )
				{
					AddTerrainType( control.TerrainType );
				}
			}
		}

		private void TerrainTypeListControl_Load( object sender, EventArgs e )
		{
			terrainTypeLayoutPanel.RowCount = 0;
		}

		#endregion

		#endregion

	}
}
