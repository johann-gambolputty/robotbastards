using System;
using System.Windows.Forms;
using Rb.Core.Assets;
using Rb.Core.Assets.Windows;

namespace Poc0.LevelEditor
{
	public partial class ObjectUITypeEditorForm : Form
	{
		public ObjectUITypeEditorForm( Type objectType )
		{
			InitializeComponent();

			//	Initialize asset loader tab
			if ( !CanLoadAsset( objectType ) )
			{
				NoAssetLoadingAllowed( string.Format( Properties.Resources.CantLoadAssetIntoType, objectType ) );
				return;
			}

			foreach ( ILocationManager locationManager in LocationManagers.Instance )
			{
				if ( locationManager is ILocationBrowserProvider )
				{
					locationManagerComboBox.Items.Add( locationManager );
				}
			}

			if ( locationManagerComboBox.Items.Count == 0 )
			{
				NoAssetLoadingAllowed( Properties.Resources.NoLocationManagersWithUI );
			}
			else
			{
				locationManagerComboBox.SelectedIndex = 0;
			}
		}

		public object NewObject
		{
			get { return m_NewObject; }
		}

		private void NoAssetLoadingAllowed( string reason )
		{
			Label reasonLabel = new Label( );
			reasonLabel.Text = reason;
			reasonLabel.AutoSize = true;

			locationManagerControlPanel.Controls.Add( reasonLabel );

			locationManagerComboBox.Enabled = false;
		}

		private static bool CanLoadAsset( Type objectType )
		{
			return objectType.IsInterface;
		}

		private object m_NewObject;
		private ILocationBrowser m_AssetBrowserUi;

		private void locationManagerComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			ILocationBrowserProvider uiProvider = ( ILocationBrowserProvider )locationManagerComboBox.SelectedItem;

			m_AssetBrowserUi = uiProvider.CreateControl();
			Control uiControl = ( Control )m_AssetBrowserUi;
			locationManagerControlPanel.Controls.Add( uiControl );
			uiControl.Dock = DockStyle.Fill;

			m_AssetBrowserUi.SelectionChosen += ui_SelectionChosen;
		}

		void ui_SelectionChosen( object sender, EventArgs e )
		{
			m_NewObject = AssetManager.Instance.Load( m_AssetBrowserUi.Sources[ 0 ] );
			DialogResult = DialogResult.OK;
			Close( );
		}

		private void okButton_Click( object sender, EventArgs e )
		{
			if ( m_AssetBrowserUi.Sources.Length > 0 )
			{
				m_NewObject = AssetManager.Instance.Load( m_AssetBrowserUi.Sources[ 0 ] );
				DialogResult = DialogResult.OK;
				Close( );
			}
		}
	}
}