using System;
using System.Collections;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
using Poc0.LevelEditor.Core.Geometry;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.World;

namespace Poc0.LevelEditor
{
	public partial class EditorControls : UserControl
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public EditorControls( )
		{
			InitializeComponent( );


			//	TODO: AP: Make directory traversal part of the location manager
			ILocationManager locations = Rb.Core.Assets.Location.DetermineLocationManager( "Editor/Templates" );
			foreach ( ISource source in locations.GetSourcesInDirectory( "Editor/Templates" ) )
			{
				object loadResult = AssetManager.Instance.Load( source );
				if ( loadResult is ObjectTemplate )
				{
					m_Templates.Add( loadResult );
				}
				else if ( loadResult is IList )
				{
					foreach ( ObjectTemplate template in ( IList )loadResult )
					{
						m_Templates.Add( template );
					}
				}
				else
				{
					throw new ApplicationException( string.Format( "Editor/Templates directory contained file that did not contain a template ({0})", source.Name ) );
				}
			}

			PopulateObjectTemplates( m_Templates );
		}

		private EditMode m_CurrentEditMode;
		private readonly ArrayList m_Templates = new ArrayList( );
		private static readonly RayCastOptions ms_PickOptions = new RayCastOptions( RayCastLayers.Grid | RayCastLayers.StaticGeometry );


		private void PopulateObjectTemplates( IEnumerable templates )
		{
			objectsTreeView.Nodes.Clear( );

			//	Populate the object types tree
			TreeNode allObjects = objectsTreeView.Nodes.Add( "All Objects" );

			foreach ( object template in templates )
			{
				TreeNode node = new TreeNode( template.ToString( ) );
				node.Tag = template;

				allObjects.Nodes.Add( node );
			}
		}

		private void objectsTreeView_AfterSelect( object sender, TreeViewEventArgs e )
		{
			object template = objectsTreeView.SelectedNode.Tag;
			if ( template != null )
			{
				EditorState.Instance.AddEditMode( new AddObjectEditMode( MouseButtons.Right, template, ms_PickOptions ) );
			}
		}

		private static LevelGeometry LevelGeometry
		{
			get
			{	
				LevelGeometry level = LevelGeometry.FromScene( EditorState.Instance.CurrentScene );
				if ( level == null )
				{
					throw new InvalidOperationException( "Expected a LevelGeometry object to be present in the scene" );
				}
				return level;
			}
		}

		private void userBrushRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			m_CurrentEditMode = new PolygonBrushEditMode( LevelGeometry );
			EditorState.Instance.AddEditMode( m_CurrentEditMode );
		}

		private void brushPage_Enter( object sender, EventArgs e )
		{
			m_CurrentEditMode = new PolygonBrushEditMode( LevelGeometry );
			EditorState.Instance.AddEditMode( m_CurrentEditMode );
		}

		private void circleBrushRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			m_CurrentEditMode = new CircleBrushEditMode( LevelGeometry, ( int )circleEdgeCountUpDown.Value );
			EditorState.Instance.AddEditMode( m_CurrentEditMode );
		}

		private void circleEdgeCountUpDown_ValueChanged( object sender, EventArgs e )
		{
			CircleBrushEditMode circleEditMode = m_CurrentEditMode as CircleBrushEditMode;
			if ( circleEditMode == null )
			{
				return;
			}
			circleEditMode.EdgeCount = ( int )circleEdgeCountUpDown.Value; 
		}
	}
}
