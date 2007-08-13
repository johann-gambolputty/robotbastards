using System;
using System.Collections.Generic;
using System.Text;

namespace Poc0.LevelEditor.Core
{
	public class TileObject
	{
		public float X
		{
			get { return m_X; }
		}
		public float Y
		{
			get { return m_Y; }
		}

		public int TileX
		{
			get { return m_TileX; }
		}

		public int TileY
		{
			get { return m_TileY; }
		}

		public object Object
		{
			get { return m_Object; }
			set { m_Object = value; }
		}

		public void SetPosition( TileGrid grid, float x, float y )
		{
			m_X = x;
			m_Y = y;
		}

		private float	m_X;
		private float	m_Y;
		private int		m_TileX;
		private int		m_TileY;
		private object	m_Object;
	}
}
