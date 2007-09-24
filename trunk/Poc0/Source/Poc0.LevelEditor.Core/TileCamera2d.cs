using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Cameras;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// 2D camera for displaying tiles
	/// </summary>
	public class TileCamera2d : CameraBase, ITilePicker
	{
		/// <summary>
		/// Width of an on screen tile 
		/// </summary>
		public const int TileScreenWidth = 64;

		/// <summary>
		/// Height of an on screen tile 
		/// </summary>
		public const int TileScreenHeight = 64;

		/// <summary>
		/// The camera origin
		/// </summary>
		public Point2 Origin
		{
			set { m_Origin = value; }
			get { return m_Origin; }
		}

		/// <summary>
		/// The scale factor
		/// </summary>
		public float Scale
		{
			set { m_Scale = value; }
			get { return m_Scale; }
		}
		
		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public override void Begin( )
		{
			Graphics.Renderer.Push2d( );
			Graphics.Renderer.PushTransform( Transform.LocalToWorld, Matrix44.Identity );
			Graphics.Renderer.Translate( Transform.LocalToWorld, m_Origin.X, m_Origin.Y, 0 );
			Graphics.Renderer.Scale( Transform.LocalToWorld, m_Scale, m_Scale, 1 );
			base.Begin( );
		}

		/// <summary>
		/// Should remove camera transforms from the current renderer
		/// </summary>
		public override void End( )
		{
			base.End( );
			Graphics.Renderer.PopTransform( Transform.LocalToWorld );
			Graphics.Renderer.Pop2d( );
		}

		#region Private stuff

		private Point2	m_Origin	= Point2.Origin;
		private float	m_Scale		= 1.0f;

		#endregion

		#region ITilePicker Members

		/// Returns the tile under the mouse cursor
		/// </summary>
		/// <param name="grid">Grid to select tiles from</param>
		/// <param name="cursorX">Mouse x position</param>
		/// <param name="cursorY">Mouse y position</param>
		/// <returns>Returns the tile in the grid under mouse cursor. Returns null if no tile is under the cursor</returns>
		public Tile PickTile( TileGrid grid, int cursorX, int cursorY )
		{
			int x = ( int )( ( cursorX - m_Origin.X ) / ( TileScreenWidth * m_Scale ) );
			int y = ( int )( ( cursorY - m_Origin.Y ) / ( TileScreenHeight * m_Scale ) );

			if ( ( x < 0 ) || ( x >= grid.Width ) || ( y < 0 ) || ( y >= grid.Height ) )
			{
				return null;
			}

			return grid[ x, y ];
		}
		
		/// <summary>
		/// Returns the world point from a cursor position
		/// </summary>
		/// <param name="cursorX">Cursor x position</param>
		/// <param name="cursorY">Cursor y position</param>
		/// <returns>World point</returns>
		public Point2 CursorToWorld( int cursorX, int cursorY )
		{
			float x = ( ( cursorX - m_Origin.X ) / m_Scale );
			float y = ( ( cursorY - m_Origin.Y ) / m_Scale );

			return new Point2( x, y );
			
		}

		#endregion
	}
}
