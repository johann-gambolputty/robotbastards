using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
		}
	}
}