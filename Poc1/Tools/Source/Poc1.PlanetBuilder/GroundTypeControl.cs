using System;
using System.Drawing;
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;

namespace Poc1.PlanetBuilder
{
	public partial class GroundTypeControl : UserControl
	{
		#region Control Events

		/// <summary>
		/// Event, invoked when the underlying terrin type changes
		/// </summary>
		public event Action<TerrainType> TerrainTypeChanged;

		/// <summary>
		/// Event, invoked when the control is selected
		/// </summary>
		public event Action<GroundTypeControl> ControlSelected;

		/// <summary>
		/// Event, invoked when the control delete button is clicked
		/// </summary>
		public event Action<GroundTypeControl> RemoveControl;

		/// <summary>
		/// Event, invoked when the control move-up button is clicked
		/// </summary>
		public event Action<GroundTypeControl> MoveControlUp;

		/// <summary>
		/// Event, invoked when the control move-down button is clicked
		/// </summary>
		public event Action<GroundTypeControl> MoveControlDown;

		#endregion

		/// <summary>
		/// Initializes the control
		/// </summary>
		public GroundTypeControl( )
		{
			InitializeComponent( );

			deleteButton.Image = MakeTransparent( deleteButton.Image );
			moveUpButton.Image = MakeTransparent( moveUpButton.Image );
			moveDownButton.Image = MakeTransparent( moveDownButton.Image );

		}

		/// <summary>
		/// Gets/sets the control selected flag. Selecting the control darkens the control background
		/// </summary>
		public bool Selected
		{
			get
			{
				return m_Selected;
			}
			set
			{
				m_Selected = value;
				if ( m_Selected )
				{
					BackColor = m_SelectedColour;
					Invalidate( );
				}
				else
				{
					BackColor = m_UnselectedColour;
					Invalidate( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the terrain type edited by this control
		/// </summary>
		public TerrainType TerrainType
		{
			get { return m_TerrainType; }
			set
			{
				m_TerrainType = value;

				nameTextBox.Text = m_TerrainType.Name;
				texturePanel.BackgroundImage = m_TerrainType.Texture == null ? null : ( Bitmap )m_TerrainType.Texture.Clone( );
			}
		}

		#region Private Members

		private bool m_Selected;
		private TerrainType m_TerrainType = new TerrainType( );
		private Color m_UnselectedColour;
		private Color m_SelectedColour;


		private void OnTerrainTypeChanged( )
		{
			if ( TerrainTypeChanged != null )
			{
				TerrainTypeChanged( m_TerrainType );
			}
		}
		
		private static Bitmap MakeTransparent( Image img )
		{
			Bitmap bmp = ( Bitmap )img;
			bmp.MakeTransparent( Color.Magenta );
			return bmp;
		}

		#endregion

		#region Control Event Handlers

		private void texturePanel_Click( object sender, EventArgs e )
		{
			OpenFileDialog openDlg = new OpenFileDialog( );
			openDlg.Filter = "Image files (*.jpg, *.bmp)|*.JPG;*.BMP|All Files (*.*)|*.*";
			if ( openDlg.ShowDialog( ) != DialogResult.OK )
			{
				return;
			}

			m_TerrainType.LoadBitmap( openDlg.FileName );
			texturePanel.BackgroundImage = m_TerrainType.Texture == null ? null : ( Bitmap )m_TerrainType.Texture.Clone( );
			OnTerrainTypeChanged( );

			TerrainTypeTextureBuilder.Instance.Rebuild( false, true );
		}

		private void moveUpButton_Click( object sender, EventArgs e )
		{
			if ( MoveControlUp != null )
			{
				MoveControlUp( this );
			}
		}

		private void moveDownButton_Click( object sender, EventArgs e )
		{
			if ( MoveControlDown != null )
			{
				MoveControlDown( this );
			}
		}

		private void deleteButton_Click( object sender, EventArgs e )
		{
			if ( RemoveControl != null )
			{
				RemoveControl( this );
			}
		}
		
		private void GroundTypeControl_Load( object sender, EventArgs e )
		{
			m_UnselectedColour = BackColor;
			m_SelectedColour = Color.FromArgb( m_UnselectedColour.R / 2, m_UnselectedColour.G / 2, m_UnselectedColour.B / 2 );
		}

		private void GroundTypeControl_MouseClick( object sender, MouseEventArgs e )
		{
			if ( ControlSelected != null )
			{
				ControlSelected( this );
			}
		}

		#endregion

	}
}
