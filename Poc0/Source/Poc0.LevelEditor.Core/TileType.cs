
using System.Drawing;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Defines the type of a tile
	/// </summary>
	public class TileType
	{
		/// <summary>
		/// Gets the <see cref="TileTypeSet"/> that this type belongs to
		/// </summary>
		public TileTypeSet Set
		{
			get { return m_Set; }
			set
			{
				//	TODO: AP: Should remove from existing set, check for value==null also
				m_Set = value;
				m_Set.Add( this );
			}
		}

		/// <summary>
		/// Texture rectangle (area on the tile type set display texture that this type uses)
		/// </summary>
		public Rectangle TextureRectangle
		{
			get { return m_TextureRect; }
			set { m_TextureRect = value; }
		}

		/// <summary>
		/// The name of the tile type
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		private string			m_Name;
		private Rectangle		m_TextureRect = new Rectangle( 0, 0, 32, 32 );
		private TileTypeSet		m_Set;
	}
}
