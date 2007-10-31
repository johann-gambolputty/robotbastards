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

		#region DynPropertySpec private class
		
		/// <summary>
		/// Dynamic property PropertySpec
		/// </summary>
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

			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="category">Property setup</param>
			/// <param name="baseProperty">Dynamic property</param>
			public DynPropertySpec( string category, IDynamicProperty baseProperty ) :
				base( baseProperty.Name, baseProperty.Value.GetType( ), category, "", baseProperty.Value )
			{
				m_BaseProperty = baseProperty;
			}

			private readonly IDynamicProperty m_BaseProperty;
		}


		#endregion

		#region Private members

		private LoadParameters		m_CurrentLoadParameters;
		private object				m_NewObject;
		private ILocationBrowser	m_AssetBrowserUi;
		private readonly Type		m_ObjectType;
		private int					m_UnexpandedSplitPosition;
		private bool				m_IsAdvancedSectionExpanded;
		
		/// <summary>
		/// Opens the advanced section, used for specifying loading parameters
		/// </summary>
		private void OpenAdvancedSection( )
		{
			loadParametersGrid.Show( );
			loadLayoutSplitter.SplitPosition = m_UnexpandedSplitPosition;
			loadLayoutSplitter.Show( );
			m_IsAdvancedSectionExpanded = true;
		}

		/// <summary>
		/// Closes the advanced section
		/// </summary>
		private void CloseAdvancedSection( )
		{
			m_UnexpandedSplitPosition = loadLayoutSplitter.SplitPosition;
			loadParametersGrid.Hide( );
			loadLayoutSplitter.SplitPosition = Width;
			loadLayoutSplitter.Hide( );

			m_IsAdvancedSectionExpanded = false;
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

		/// <summary>
		/// Converts a source object to the expected object type
		/// </summary>
		private object SourceToObject( ISource source )
		{
			if ( m_ObjectType.GetInterface( typeof( ISource ).Name ) != null )
			{
				//	Expected object type is an ISource - no conversion necessary
				return source;
			}
			if ( m_ObjectType.IsSubclassOf( typeof( AssetHandle ) ) )
			{
				//	Expected object type is an asset handle - create an instance of using the source
				return Activator.CreateInstance( m_ObjectType, source );
			}
			
			//	TODO: AP: There should be an option to directly load the object (as this hack does), as well as create an asset handle proxy

			//return AssetProxy.CreateProxy( m_ObjectType, source )
			//	Asset handle proxies are incorrect - the need to access the AssetHandle.Asset property rather than
			//	their own m_Base field
			//throw new NotImplementedException( "I am lazy ha ha" );
			//	TODO: AP: This is a nasty workaround... just loads the object directly
			AppLog.Warning( "Loading asset directly into memory, instead of creating asset handle proxy" );
			return AssetManager.Instance.Load( source, m_CurrentLoadParameters );
		}

		#endregion

		#region Event handlers

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

		private void ui_SelectionChanged( object sender, EventArgs e )
		{
			//	The selection has changed - if the user has selected a loadable asset, then present the load parameters
			//	in a property grid
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
			m_CurrentLoadParameters = loader.CreateDefaultParameters( true );


			//	Create a property bag from the dynamic properties
			PropertyBag bag = new PropertyBag( );

			string category = string.Format( Properties.Resources.LoaderProperties, loader.Name );
			foreach ( IDynamicProperty dynProperty in m_CurrentLoadParameters.Properties )
			{
				bag.Properties.Add( new DynPropertySpec( category, dynProperty ) );
			}
			if ( bag.Properties.Count == 0 )
			{
				return;
			}
			bag.SetValue += DynPropertySpec.SetValue;
			bag.GetValue += DynPropertySpec.GetValue;
			
			//	Add the property bag to the load parameters property grid
			loadParametersGrid.SelectedObject = bag;
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

		#endregion

	}
}