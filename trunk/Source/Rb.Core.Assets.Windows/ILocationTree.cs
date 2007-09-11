
namespace Rb.Core.Assets.Windows
{
	interface ILocationTree
	{
		LocationTreeFolder[] Roots
		{
			get;
		}

		LocationTreeFolder DefaultFolder
		{
			get;
		}
	}
}
