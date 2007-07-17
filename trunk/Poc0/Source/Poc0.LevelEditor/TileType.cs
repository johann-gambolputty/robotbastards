
namespace Poc0.LevelEditor
{
	class TileType
	{
		public static TileType Default
		{
			get { return ms_Default; }
			set { ms_Default = value; }
		}

		public string Name
		{
			get { return m_Name; }
		}

		private string			m_Name;
		private static TileType	ms_Default;
	}
}
