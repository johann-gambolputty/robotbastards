namespace Poc0.Core.Objects
{
	/// <summary>
	/// Interface for items that can be used
	/// </summary>
	public interface IItem
	{
		/// <summary>
		/// Called when this item is unequipped
		/// </summary>
		void Unequip( );

		/// <summary>
		/// Called when this item is equipped
		/// </summary>
		void Equip( );

		/// <summary>
		/// Uses the item
		/// </summary>
		void Use( );
	}
}
