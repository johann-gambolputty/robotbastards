using System;
using System.Collections.Generic;
using Rb.Assets.Interfaces;

namespace Rb.Core.Assets.Windows
{
	public abstract class LocationTreeFolder : LocationTreeNode
	{
		public LocationTreeFolder( LocationTreeFolder parent, ISource source, int image, int selectedImage ) :
			base( parent, source, image, selectedImage )
		{
		}

		public LocationTreeFolder( LocationTreeFolder parent, ISource source, string name, int image, int selectedImage ) :
			base( parent, source, name, image, selectedImage )
		{
		}

		public bool IsRootOf( LocationTreeFolder folder )
		{
			for ( LocationTreeFolder parent = folder; parent != null; parent = parent.Parent )
			{
				if ( ReferenceEquals( parent, this ) )
				{
					return true;
				}
			}
			return false;
		}

		public int GetDistanceTo( LocationTreeFolder folder )
		{
			int distance = 0;
			for ( LocationTreeFolder parent = folder; parent != null; parent = parent.Parent, ++distance )
			{
				if ( ReferenceEquals( parent, this ) )
				{
					return distance;
				}
			}
			return -1;
		}

		public LocationTreeFolder FindFolder( string name )
		{
			foreach ( LocationTreeFolder folder in Folders )
			{
				if ( string.Compare( folder.Name, name, StringComparison.CurrentCultureIgnoreCase ) == 0 )
				{
					return folder;
				}
			}
			return null;
		}

		public abstract IEnumerable< LocationTreeItem > Items
		{
			get;
		}

		public abstract IEnumerable< LocationTreeFolder > Folders
		{
			get;
		}
	}
}
