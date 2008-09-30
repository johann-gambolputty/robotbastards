using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Rb.Assets;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using Tao.OpenGl;
using Graphics=Rb.Rendering.Graphics;

namespace Rb.TextureViewer
{
	public partial class TextureForm : Form
	{
		public TextureForm( string filePath )
		{
			InitializeComponent( );

			m_RenderState = Graphics.Factory.CreateRenderState( );
			m_RenderState.Enable2dTextures = true;
			m_RenderState.Enable2dTextureUnit( 0, true );
			m_RenderState.Colour = Color.Red;
			m_FilePath = filePath;
		}

		/// <summary>
		/// Gets the texture being displayed in this form
		/// </summary>
		public ITexture2d Texture
		{
			get { return m_Sampler.Texture; }
		}

		#region Private Members

		private readonly string m_FilePath;
		private readonly IRenderState m_RenderState;
		private ITexture2dSampler m_Sampler;

		#endregion

		private void display1_OnRender( object sender, EventArgs e )
		{
			if ( m_Sampler == null )
			{
				return;
			}

			Graphics.Renderer.Push2d( );

			m_RenderState.Begin( );
			m_Sampler.Begin( );

			Gl.glBegin( Gl.GL_QUADS );

			Gl.glVertex2f( 0, 0 );
			Gl.glTexCoord2f( 0, 0 );
		//	Gl.glColor3f( 1, 0, 0 );
		//	Gl.glMultiTexCoord2f( Gl.GL_TEXTURE_2D, 0, 0 );

			Gl.glVertex2f( DisplayRectangle.Width, 0 );
			Gl.glTexCoord2f( 1, 0 );
		//	Gl.glColor3f( 0, 1, 0 );
		//	Gl.glMultiTexCoord2f( Gl.GL_TEXTURE_2D, 1, 0 );

			Gl.glVertex2f( DisplayRectangle.Width, DisplayRectangle.Height );
			Gl.glTexCoord2f( 1, 1 );
		//	Gl.glColor3f( 0, 0, 1 );
		//	Gl.glMultiTexCoord2f( Gl.GL_TEXTURE_2D, 1, 1 );

			Gl.glVertex2f( 0, DisplayRectangle.Height );
			Gl.glTexCoord2f( 0, 1 );
		//	Gl.glColor3f( 0, 1, 1 );
		//	Gl.glMultiTexCoord2f( Gl.GL_TEXTURE_2D, 0, 1 );

			Gl.glEnd( );

			m_Sampler.End( );
			m_RenderState.End( );

			Graphics.Renderer.Pop2d( );
		}

		private void TextureForm_Shown( object sender, System.EventArgs e )
		{
			ITexture2d texture;
			try
			{
				TextureLoadParameters parameters = new TextureLoadParameters( true );
				texture = ( ITexture2d )AssetManager.Instance.Load( m_FilePath, parameters );

				//texture.ToBitmap( false )[ 0 ].Save( "output.png" );
			}
			catch ( Exception ex )
			{
				string msg = string.Format( "Error occurred opening \"{0}\" - {1}", Path.GetFileName( m_FilePath ), ex.Message );
				MessageBox.Show( this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				Close( );
				return;
			}

			m_Sampler = Graphics.Factory.CreateTexture2dSampler( );
			m_Sampler.Texture = texture;
			m_Sampler.Mode = TextureMode.Modulate;
		}

		private void TextureForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			try
			{
				m_Sampler.Texture.Dispose( );
			}
			catch
			{
				return;
			}
		}

	}
}