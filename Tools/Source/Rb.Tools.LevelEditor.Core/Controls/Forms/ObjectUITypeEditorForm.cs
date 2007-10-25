using System;
using System.Windows.Forms;
using Rb.Core.Assets;
using Rb.Core.Assets.Windows;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public partial class ObjectUITypeEditorForm : Form
	{
		public ObjectUITypeEditorForm( Type objectType )
		{
			m_ObjectType = objectType;

			InitializeComponent( );

			newObjectControl.BaseType = objectType;
			newObjectControl.SelectionMade += newObjectControl_SelectionMade;

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

		void newObjectControl_SelectionMade( Type type )
		{
			NewObject = Activator.CreateInstance( type );
		}

		public object NewObject
		{
			set
			{
				m_NewObject = value;
				Close( );
				DialogResult = DialogResult.OK;
			}
			get { return m_NewObject; }
		}

		/// <summary>
		/// Disables asset loading
		/// </summary>
		/// <param name="reason">Reason that asset loading was disabled</param>
		private void NoAssetLoadingAllowed( string reason )
		{
			Label reasonLabel = new Label( );
			reasonLabel.Text = reason;
			reasonLabel.AutoSize = true;

			locationManagerControlPanel.Controls.Add( reasonLabel );

			locationManagerComboBox.Enabled = false;
		}

		/// <summary>
		/// Returns true if an asset can be loaded into the specified object type
		/// </summary>
		private static bool CanLoadAsset( Type objectType )
		{
			return	( objectType.IsInterface ) || // (Interfaces can use the automatic asset proxy generator)
					( objectType == typeof( ISource ) ) ||
					( objectType.GetInterface( typeof( ISource ).FullName ) != null ) ||
					( objectType.IsSubclassOf( typeof( AssetHandle ) ) );
		}

		private object m_NewObject;
		private ILocationBrowser m_AssetBrowserUi;
		private readonly Type m_ObjectType;

		private void locationManagerComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			ILocationBrowserProvider uiProvider = ( ILocationBrowserProvider )locationManagerComboBox.SelectedItem;

			m_AssetBrowserUi = uiProvider.CreateControl();
			Control uiControl = ( Control )m_AssetBrowserUi;
			locationManagerControlPanel.Controls.Add( uiControl );
			uiControl.Dock = DockStyle.Fill;

			m_AssetBrowserUi.SelectionChosen += ui_SelectionChosen;
		}

		private object SourceToObject( ISource source )
		{
			if ( m_ObjectType is ISource )
			{
				return source;
			}
			if ( m_ObjectType.IsSubclassOf( typeof( AssetHandle ) ) )
			{
				return Activator.CreateInstance( m_ObjectType, source );
			}
			//return AssetProxy.CreateProxy( m_ObjectType, )
			//	Asset handle proxies are incorrect - the need to access the AssetHandle.Asset property rather than
			//	their own m_Base field
			throw new NotImplementedException( "I am lazy ha ha" );
		}

		void ui_SelectionChosen( object sender, EventArgs e )
		{
			NewObject = SourceToObject( m_AssetBrowserUi.Sources[ 0 ] );
		}

		private void okButton_Click( object sender, EventArgs e )
		{
			if ( createObjectTabPage.Visible )
			{
				if ( newObjectControl.NewObjectType != null )
				{
					NewObject = Activator.CreateInstance( newObjectControl.NewObjectType );
				}
			}
			else
			{
				if ( m_AssetBrowserUi.Sources.Length > 0 )
				{
					NewObject = SourceToObject( m_AssetBrowserUi.Sources[ 0 ] );
				}
			}
		}
	}
}