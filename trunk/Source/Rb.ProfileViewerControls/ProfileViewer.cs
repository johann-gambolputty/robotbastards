using System.Windows.Forms;
using Rb.Core.Utils;

namespace Rb.ProfileViewerControls
{
	public partial class ProfileViewer : UserControl
	{
		public ProfileViewer( )
		{
			InitializeComponent( );
			profileSectionTreeView.SectionSelectionChanged += profileSectionTreeView_SectionSelectionChanged;
		}

		public ProfileSection RootSection
		{
			set { profileSectionTreeView.RootSection = value; }
		}

		private void profileSectionTreeView_SectionSelectionChanged( object sender, System.EventArgs e )
		{
			profileGraph1.Sections = profileSectionTreeView.SectionSelection;
		}

	}
}
