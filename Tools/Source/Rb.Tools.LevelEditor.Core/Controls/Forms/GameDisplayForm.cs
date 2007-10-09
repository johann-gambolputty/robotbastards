using System.Windows.Forms;
using Rb.Core.Assets;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public class GameSetup
	{
		public GameSetup( Scene scene )
		{
			m_Scene = scene;
		}

		public Scene Scene
		{
			get { return m_Scene; }
		}

		public ISource UserCommandsLocation
		{
			get { return m_UserCommandsSource; }
			set { m_UserCommandsSource = value; }
		}

		public ISource ViewerCommandsLocation
		{
			get { return m_ViewerSource; }
			set { m_ViewerSource = value; }
		}

		private ISource m_ViewerSource;
		private ISource m_UserCommandsSource;
		private readonly Scene m_Scene;
	}

	public partial class GameDisplayForm : Form
	{
		public GameDisplayForm( GameSetup setup )
		{
			InitializeComponent( );
			m_Setup = setup;
		}

		private readonly GameSetup m_Setup;

		private void GameDisplayForm_Load( object sender, System.EventArgs e )
		{
			AssetManager.Instance.Load( m_Setup.UserCommandsLocation );
			AssetManager.Instance.Load( m_Setup.ViewerCommandsLocation );
		}
	}
}