using System;
using System.Windows.Forms;
using Rb.Core.Assets;
using Rb.Core.Assets.Windows;

namespace Poc0.LevelEditor
{
	public partial class ObjectUITypeEditorForm : Form
	{
		public ObjectUITypeEditorForm()
		{
			InitializeComponent();

			foreach ( ILocationManager locationManager in LocationManagers.Instance )
			{
				if ( locationManager is ILocationBrowserProvider )
				{
					locationManagerComboBox.Items.Add( locationManager );
				}
			}

			if ( locationManagerComboBox.Items.Count == 0 )
			{
				sourceTabs.TabPages.Remove( assetTabPage );
				MessageBox.Show( Properties.Resources.NoLocationManagersWithUI, "bad thing", MessageBoxButtons.OK, MessageBoxIcon.Error );
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
		}
	}
}