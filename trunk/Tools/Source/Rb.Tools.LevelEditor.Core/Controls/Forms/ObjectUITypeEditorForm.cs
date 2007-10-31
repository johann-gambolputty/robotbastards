using System;
using System.Windows.Forms;
using Rb.Core.Assets;
using Rb.Core.Assets.Windows;
using Rb.Core.Components;
using Rb.Log;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public partial class ObjectUITypeEditorForm : Form
	{
		public ObjectUITypeEditorForm( Type objectType )
		{
			m_ObjectType = objectType;

			InitializeComponent( );

			CloseAdvancedSection( );

			newObjectControl.BaseType = objectType;
			newObjectControl.SelectionMade += newObjectControl_SelectionMade;

			//	Initialize asset loader tab
			if ( !CanLoadAsset( objectType ) )
			{
				NoAssetLoadingAllowed( string.Format( Properties.Resources.CantLoadAssetIntoType, objectType ) );
				return;
			}

			//	Run through all available location managers
			foreach ( ILocationManager locationManager in LocationManagers.Instance )
			{
				//	Add the current location manager to the manager combo box, if it can provide a browser UI
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
			SetNewObjectAndClose( Activator.CreateInstance( type ) );
		}

		/// <summary>
		/// Gets the new object
		/// </summary>
		public object NewObject
		{
			get { return m_NewObject; }
		}

		/// <summary>
		/// Sets the new object and closes this dialog with an OK result
		/// </summary>
		private void SetNewObjectAndClose( object obj )
		{
			m_NewObject = obj;
			Close( );
			DialogResult = DialogResult.OK;
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
			toggleAdvancedButton.Enabled = false;
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
			if ( m_AssetBrowserUi != null )
			{
				m_AssetBrowserUi.SelectionChosen -= ui_SelectionChosen;
				m_AssetBrowserUi.SelectionChanged -= ui_SelectionChanged;
			}

			ILocationBrowserProvider uiProvider = ( ILocationBrowserProvider )locationManagerComboBox.SelectedItem;

			m_AssetBrowserUi = uiProvider.CreateControl();
			Control uiControl = ( Control )m_AssetBrowserUi;
			locationManagerControlPanel.Controls.Add( uiControl );
			uiControl.Dock = DockStyle.Fill;

			m_AssetBrowserUi.SelectionChosen += ui_SelectionChosen;
			m_AssetBrowserUi.SelectionChanged += ui_SelectionChanged;
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
			//throw new NotImplementedException( "I am lazy ha ha" );
			//	TODO: AP: This is a nasty workaround...
			AppLog.Warning( "Loading asset directly into memory, instead of creating asset handle proxy" );
			return AssetManager.Instance.Load( source );
		}

		private void ui_SelectionChanged( object sender, EventArgs e )
		{
			ISource[] sources = m_AssetBrowserUi.Sources;
			if ( sources.Length == 0 )
			{
				loadParametersGrid.SelectedObject = null;
				return;
			}
			IAssetLoader loader = AssetManager.Instance.FindLoaderForAsset( sources[ 0 ] );
			if ( loader == null )
			{
				return;
			}
			LoadParameters parameters = loader.CreateDefaultParameters( true );

			PropertyBag bag = new PropertyBag( );
			foreach ( IDynamicProperty dynProperty in parameters.Properties )
			{
				bag.Properties.Add( new DynPropertySpec( dynProperty ) );
			}
			if ( bag.Properties.Count == 0 )
			{
				return;
			}
			bag.SetValue += DynPropertySpec.SetValue;
			bag.GetValue += DynPropertySpec.GetValue;

			loadParametersGrid.SelectedObject = bag;
		}

		private class DynPropertySpec : PropertySpec
		{
			#region Access

			/// <summary>
			/// Sets the value of an extended property
			/// </summary>
			public static void SetValue( object sender, PropertySpecEventArgs e )
			{
				( ( DynPropertySpec )e.Property ).m_BaseProperty.Value = e.Value;
			}

			/// <summary>
			/// Gets the value of an extended property
			/// </summary>
			public static void GetValue( object sender, PropertySpecEventArgs e )
			{
				e.Value = ( ( DynPropertySpec )e.Property ).m_BaseProperty.Value;
			}

			#endregion

			public DynPropertySpec( IDynamicProperty baseProperty ) :
				base( baseProperty.Name, baseProperty.Value.GetType( ), "Properties", "", baseProperty.Value )
			{
				m_BaseProperty = baseProperty;
			}

			private readonly IDynamicProperty m_BaseProperty;
		}


		private void ui_SelectionChosen( object sender, EventArgs e )
		{
			SetNewObjectAndClose( SourceToObject( m_AssetBrowserUi.Sources[ 0 ] ) );
		}

		private void okButton_Click( object sender, EventArgs e )
		{
			if ( createObjectTabPage.Visible )
			{
				if ( newObjectControl.NewObjectType != null )
				{
					SetNewObjectAndClose( Activator.CreateInstance( newObjectControl.NewObjectType ) );
				}
			}
			else
			{
				if ( m_AssetBrowserUi.Sources.Length > 0 )
				{
					SetNewObjectAndClose( SourceToObject( m_AssetBrowserUi.Sources[ 0 ] ) );
				}
			}
		}

		private void toggleAdvancedButton_Click( object sender, EventArgs e )
		{
			if ( m_IsAdvancedSectionExpanded )
			{
				CloseAdvancedSection( );
				toggleAdvancedButton.Text = toggleAdvancedButton.Text.Replace( "<<", ">>" );
			}
			else
			{
				OpenAdvancedSection( );
				toggleAdvancedButton.Text = toggleAdvancedButton.Text.Replace( ">>", "<<" );	
			}
		}

		private void OpenAdvancedSection( )
		{
			loadParametersGrid.Show( );
			loadLayoutSplitter.SplitPosition = m_UnexpandedSplitPosition;
			loadLayoutSplitter.Show( );
			m_IsAdvancedSectionExpanded = true;
		}

		private void CloseAdvancedSection( )
		{
			m_UnexpandedSplitPosition = loadLayoutSplitter.SplitPosition;
			loadParametersGrid.Hide( );
			loadLayoutSplitter.SplitPosition = Width;
			loadLayoutSplitter.Hide( );

			m_IsAdvancedSectionExpanded = false;
		}

		private int m_UnexpandedSplitPosition;
		private bool m_IsAdvancedSectionExpanded;
	}
}