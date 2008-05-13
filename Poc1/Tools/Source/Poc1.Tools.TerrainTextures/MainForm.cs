using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;
using Poc1.Tools.TerrainTextures.Properties;

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
			AddTypeControls( null );

			m_SampleUpdateWorker.DoWork +=
				delegate( object sender, DoWorkEventArgs e )
				{
					e.Result = m_SampleBuilder.Build( SampleWidth, SampleHeight, m_TerrainTypes );
				};
			m_SampleUpdateWorker.RunWorkerCompleted +=
				delegate( object sender, RunWorkerCompletedEventArgs e )
				{
					if ( !e.Cancelled )
					{
						Bitmap bmp = ( Bitmap )e.Result;
						samplePanel.BackgroundImage = bmp;
					}
					if ( m_SampleIsDirty )
					{
						m_SampleIsDirty = false;
						m_SampleUpdateWorker.RunWorkerAsync( );
					}
				};
		}

		public bool ApplyLighting
		{
			get { return m_SampleBuilder.ApplyLighting; }
			set { m_SampleBuilder.ApplyLighting = value; }
		}

		private readonly TerrainBitmapBuilder m_SampleBuilder = new TerrainBitmapBuilder( );
		private TerrainTypeSet m_TerrainTypes = new TerrainTypeSet( );
		private readonly BackgroundWorker m_SampleUpdateWorker = new BackgroundWorker( );
		private const int SampleWidth = 256;
		private const int SampleHeight = 256;
		private bool m_SampleIsDirty;
		
		#region Exports and saves

		private string m_SavePath;
		private string m_ExportDirectory;

		private void SaveAs( )
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filter = "Terrain Type Set (*.tts)|*.TTS|All Files (*.*)|*.*";
			dialog.DefaultExt = "tts";
			if ( dialog.ShowDialog( this ) != DialogResult.OK )
			{
				return;
			}
			Save( dialog.FileName );
		}

		private void Save( )
		{
			if ( m_SavePath == null )
			{
				SaveAs( );
			}
			else
			{
				Save( m_SavePath );
			}
		}

		private void Save( string path )
		{
			try
			{
				m_TerrainTypes.Save( path );
			}
			catch
			{
				MessageBox.Show( "Failed to save terrain types to file" );
			}
			m_SavePath = path;
		}

		private void ExportTo( )
		{
			FolderBrowserDialog folderDialog = new FolderBrowserDialog( );
			folderDialog.Description = Resources.ChooseExportFolder;
			folderDialog.SelectedPath = string.IsNullOrEmpty( m_ExportDirectory ) ? Directory.GetCurrentDirectory( ) : m_ExportDirectory;
			if ( folderDialog.ShowDialog( this ) != DialogResult.OK )
			{
				return;
			}
			m_ExportDirectory = folderDialog.SelectedPath;
			Export( m_ExportDirectory );
		}

		private void Export( )
		{
			if ( string.IsNullOrEmpty( m_ExportDirectory ) )
			{
				ExportTo( );
			}
			else
			{
				Export( m_ExportDirectory );
			}
		}

		private void Export( string directory )
		{
			m_TerrainTypes.Export( directory );
		}


		#endregion

		private void UpdateSample( )
		{
			if ( m_SampleUpdateWorker.IsBusy )
			{
				m_SampleIsDirty = true;
			}
			else
			{
				m_SampleUpdateWorker.RunWorkerAsync( );
			}
		}

		private void AddTypeControls( TerrainType terrainType )
		{
			TerrainTypeEditorControl newControl = new TerrainTypeEditorControl( );
			newControl.Enabled = terrainType != null;
			if ( terrainType != null )
			{
				newControl.TerrainType = terrainType;
			}
			newControl.RemoveControl += TerrainTypeEditorControl_RemoveControl;
			newControl.TerrainTypeChanged += TerrainTypeEditorControl_TerrainTypeChanged;
			terrainTypeControlsLayoutPanel.Controls.Add( newControl );
		}

		private void TerrainTypeEditorControl_TerrainTypeChanged( TerrainType terrainType )
		{
			UpdateSample( );
		}

		private void TerrainTypeEditorControl_RemoveControl( TerrainTypeEditorControl control )
		{
			m_TerrainTypes.TerrainTypes.Remove( control.TerrainType );
			terrainTypeControlsLayoutPanel.Controls.Remove( control );
			UpdateSample( );
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
				control.TerrainType = new TerrainType( );
				m_TerrainTypes.TerrainTypes.Add( control.TerrainType );
				UpdateSample( );

				AddTypeControls( null );
			}
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			if ( DesignMode )
			{
				return;
			}

			UpdateSample( );
		}

		private void applyLightingCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			m_SampleBuilder.ApplyLighting = applyLightingCheckBox.Checked;
			UpdateSample( );
		}

		private void newHeightMapButton_Click( object sender, EventArgs e )
		{
			m_SampleBuilder.UpdateHeights( );
			UpdateSample( );
		}

		private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveAs( );
		}


		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Save( );
		}

		private void openToolStripMenuItem_Click( object sender, EventArgs e )
		{
			OpenFileDialog dialog = new OpenFileDialog( );
			dialog.Filter = "Terrain Type Set (*.tts)|*.TTS|All Files (*.*)|*.*";
			if ( dialog.ShowDialog( this ) != DialogResult.OK )
			{
				return;
			}
			string path = dialog.FileName;

			try
			{
				m_TerrainTypes = TerrainTypeSet.Load( path );
			}
			catch
			{
				MessageBox.Show( "Failed to open terrain types file" );
				return;
			}
			m_SavePath = path;

			terrainTypeControlsLayoutPanel.Controls.Clear( );

			foreach ( TerrainType type in m_TerrainTypes.TerrainTypes )
			{
				AddTypeControls( type );
			}

			AddTypeControls( null );
			UpdateSample( );
		}

		private void newToolStripMenuItem_Click( object sender, EventArgs e )
		{
			terrainTypeControlsLayoutPanel.Controls.Clear( );
			AddTypeControls( null );
			m_TerrainTypes.TerrainTypes.Clear( );
			UpdateSample( );
		}

		private void exportToToolStripMenuItem_Click( object sender, EventArgs e )
		{
			ExportTo( );
		}

		private void exportToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Export( );
		}
	}
}