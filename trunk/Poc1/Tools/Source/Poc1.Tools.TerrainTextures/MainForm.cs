using System;
using System.Windows.Forms;
using System.Drawing;
using Poc1.Tools.TerrainTextures.Core;

namespace Poc1.Tools.TerrainTextures
{
	public partial class MainForm : Form
	{
		//	Sample terrain textures:
		//	http://www.gamedev.net/community/forums/mod/journal/journal.asp?jn=263350&cmonth=2&cyear=2007
		
		//	Adding rivers to procedurally generated terrain:
		//	http://www.gamedev.net/reference/articles/article2065.asp
		
		//	Shader-tiled terrain texture splatting:
		//	http://www.gamedev.net/community/forums/mod/journal/journal.asp?jn=263350&cmonth=3&cyear=2008
		
		//	Atmosphere shaders:
		//	http://www.gamedev.net/community/forums/topic.asp?topic_id=335023
		
		
		/*
		
			Terrain types:
				Each latitude has a set of terrain types, e.g.:
					Arctic: Tundra, snow, rock
					Temperate: Grass, plain, rock, 
					Desert: Sand, rock, ...
					
				As well as latitude, should be able to define longitudinal ranges?
					Volcanic: Rock (basalt), rock, lava, dust
				
				Each terrain type can have an angle and altitude distribution
				For a given terrain zone, there's a function:
					TerrainType GetTerrainType( float x, float y, float z, float slope );
				
				This can be encoded in a texture 
				
				Detail maps:
					Required to break up tiling patterns
		*/
	
		public MainForm( )
		{
			InitializeComponent( );
			AddTypeControls( );
		}

		private void AddTypeControls( )
		{
			TerrainTypeEditorControl newControl = new TerrainTypeEditorControl( );
			newControl.Enabled = false;
			newControl.RemoveControl += TerrainTypeEditorControl_RemoveControl;
			terrainTypeControlsLayoutPanel.Controls.Add( newControl );
		}

		private void TerrainTypeEditorControl_RemoveControl( TerrainTypeEditorControl control )
		{
			terrainTypeControlsLayoutPanel.Controls.Remove( control );
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}

		private void terrainTypeControlsLayoutPanel_MouseClick( object sender, MouseEventArgs e )
		{
			TerrainTypeEditorControl control = terrainTypeControlsLayoutPanel.GetChildAtPoint( e.Location ) as TerrainTypeEditorControl;
			if ( control == null )
			{
				return;
			}
			if ( !control.Enabled )
			{
				control.Enabled = true;

				AddTypeControls( );
			}
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			if ( DesignMode )
			{
				return;
			}

			samplePanel.BackgroundImage = TerrainBitmapBuilder.Build( samplePanel.Width, samplePanel.Height );
		}
	}
}