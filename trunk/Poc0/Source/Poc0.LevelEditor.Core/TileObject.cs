using Rb.Core.Components;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Wrapper around an object, attached to a given tile
	/// </summary>
	public class TileObject : Component
	{
		public float X
		{
			get { return m_X; }
		}

		public float Y
		{
			get { return m_Y; }
		}

		public Tile Tile
		{
			get { return m_Tile; }
			set { m_Tile = value; }
		}

		public object Object
		{
			get { return m_Object; }
			set { m_Object = value; }
		}

		public TileObject( Tile tile, float x, float y, object obj )
		{
			m_X = x;
			m_Y = y;
			m_Tile = tile;
			m_Object = obj;
		}

		public void SetPosition( TileGrid grid, float x, float y )
		{
			m_X = x;
			m_Y = y;
			m_Tile = grid[ ( int )x, ( int )y ];
		}

		private float	m_X;
		private float	m_Y;
		private Tile	m_Tile;
		private object	m_Object;
	}
}
