
using System.Drawing;
using Rb.Rendering;

namespace Poc0.LevelEditor
{
	class TileType
	{

		public void SetDisplayTexture( Bitmap bmp )
		{
			if ( m_DisplayTexture != null )
			{
				m_DisplayTexture.Dispose( );
			}

			if ( RenderFactory.Instance != null )
			{
				m_DisplayTexture = RenderFactory.Instance.NewTexture2d( );
				m_DisplayTexture.Load( bmp );
			}
		}

		public static TileType Default
		{
			get { return ms_Default; }
			set { ms_Default = value; }
		}

		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		public Texture2d DisplayTexture
		{
			get { return m_DisplayTexture; }
			set { m_DisplayTexture = value; }
		}

		public Image Thumbnail
		{
			get { return m_Thumbnail; }
			set { m_Thumbnail = value; }
		}

		private Texture2d		m_DisplayTexture;
		private Image			m_Thumbnail;
		private string			m_Name;
		private static TileType	ms_Default = CreateDefaultTileType( );

		private static TileType CreateDefaultTileType( )
		{
			TileType result = new TileType( );
			result.Name = "Default";
			result.SetDisplayTexture( Properties.Resources.DefaultTileType );

			return result;
		}
	}
}
