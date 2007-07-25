
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Stores transitions between tile types
	/// </summary>
	public class TileTypeTransitions
	{
		/// <summary>
		/// Gets the default transitions texture
		/// </summary>
		public static Texture2d TransitionsTexture
		{
			get
			{
				if ( ms_TransitionsTexture == null )
				{
					ms_TransitionsTexture = RenderFactory.Instance.NewTexture2d( );
					ms_TransitionsTexture.Load( Properties.Resources.TransitionMasks );
				}
				return ms_TransitionsTexture;
			}
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="inner">Inner type</param>
		/// <param name="outer">Outer type</param>
		public TileTypeTransitions( TileType inner, TileType outer )
		{
			m_Inner = inner;
			m_Outer = outer;
		}

		/// <summary>
		/// Returns the inner tile type
		/// </summary>
		public TileType InnerType
		{
			get { return m_Inner;  }
		}

		/// <summary>
		/// Returns the outer tile type
		/// </summary>
		public TileType OuterType
		{
			get { return m_Outer;  }
		}

		/// <summary>
		/// Sets a transition for a given corner code
		/// </summary>
		/// <param name="cornerCode">Corner code</param>
		/// <param name="type">Transition type</param>
		public void SetTransition( byte cornerCode, TileType type )
		{
			m_CodeTiles[ cornerCode ] = type;
		}

		/// <summary>
		/// Gets a transition
		/// </summary>
		/// <param name="cornerCode">Corner code</param>
		/// <returns>Returns the tile type for the corner code</returns>
		public TileType GetTransition( byte cornerCode )
		{
			return m_CodeTiles[ cornerCode ];
		}

		private readonly TileType	m_Inner;
		private readonly TileType	m_Outer;
		private readonly TileType[] m_CodeTiles = new TileType[ 256 ];
		private static Texture2d	ms_TransitionsTexture;

	}
}
