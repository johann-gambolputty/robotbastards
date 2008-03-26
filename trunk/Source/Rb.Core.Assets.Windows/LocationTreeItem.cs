using Rb.Assets.Interfaces;

namespace Rb.Core.Assets.Windows
{
	public class LocationTreeItem : LocationTreeNode
	{
		public LocationTreeItem( LocationTreeFolder parent, ISource source, int image, int selectedImage ) :
			base( parent, source, image, selectedImage )
		{
		}

	}
}
