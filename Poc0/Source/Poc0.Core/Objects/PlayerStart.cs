using System;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Player start point
	/// </summary>
	[Serializable]
	public class PlayerStart : Signpost
	{
		/// <summary>
		/// Gets/sets the player index
		/// </summary>
		public int PlayerIndex
		{
			get { return m_PlayerIndex; }
			set { m_PlayerIndex = value; }
		}

		private int m_PlayerIndex;
	}
}
