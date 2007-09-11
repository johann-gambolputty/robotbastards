using System.Collections.Generic;

namespace Rb.Core.Assets.Windows
{
	public abstract class LocationTreeFolder : LocationTreeNode
	{
		public LocationTreeFolder( LocationTreeFolder parent, ISource source ) :
			base( parent, source )
		{
		}
		
		public LocationTreeFolder( LocationTreeFolder parent, ISource source, string name ) :
			base( parent, source, name )
		{
		}

		public abstract IEnumerable< LocationTreeNode > Children
		{
			get;
		}

		public IEnumerable< LocationTreeItem > Items
		{
			get
			{
				foreach ( LocationTreeNode node in Children )
				{
					LocationTreeItem item = node as LocationTreeItem;
					if ( item != null )
					{
						yield return item;
					}
				}
			}
		}
		
		public IEnumerable< LocationTreeFolder > Folders
		{
			get
			{
				foreach ( LocationTreeNode node in Children )
				{
					LocationTreeFolder folder = node as LocationTreeFolder;
					if ( folder != null )
					{
						yield return folder;
					}
				}
			}
		}
	}
}
