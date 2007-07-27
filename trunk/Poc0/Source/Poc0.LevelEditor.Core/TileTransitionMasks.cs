using System;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Transition masks
	/// </summary>
	public class TileTransitionMasks
	{
		public class TextureRect
		{
			public TextureRect( TextureRect rect )
			{
				m_TopLeft = new Point2( rect.m_TopLeft );
				m_TopRight = new Point2( rect.m_TopRight );
				m_BottomLeft = new Point2( rect.m_BottomLeft );
				m_BottomRight = new Point2( rect.m_BottomRight );
			}

			public TextureRect( float u, float v, float width, float height )
			{
				m_TopLeft = new Point2( u, v );
				m_TopRight = new Point2( u + width, v );
				m_BottomLeft = new Point2( u, v + height );
				m_BottomRight = new Point2( u + width, v + height );
			}

			public TextureRect( Point2 tl, Point2 tr, Point2 bl, Point2 br )
			{
				m_TopLeft = tl;
				m_TopRight = tr;
				m_BottomLeft = bl;
				m_BottomRight = br;
			}

			public void Mirror( )
			{
				float tmp = m_TopLeft.X;
				m_TopLeft.X = m_TopRight.X;
				m_TopRight.X = tmp;

				tmp = m_BottomLeft.X;
				m_BottomLeft.X = m_BottomRight.X;
				m_BottomRight.X = tmp;
			}

			public void Rotate( int rotations )
			{
				if ( ( rotations % 4 ) == 0 )
				{
					return;
				}

				for ( int rotation = 0; rotation < rotations; ++rotation )
				{
					float tmpX = m_BottomLeft.X;
					float tmpY = m_BottomLeft.Y;
					m_BottomLeft.Set( m_BottomRight.X, m_BottomRight.Y );
					m_BottomRight.Set( m_TopRight.X, m_TopRight.Y );
					m_TopRight.Set( m_TopLeft.X, m_TopLeft.Y );
					m_TopLeft.Set( tmpX, tmpY );
				}
			}


			public Point2 TopLeft
			{
				get { return m_TopLeft; }
			}

			public Point2 TopRight
			{
				get { return m_TopRight; }
			}

			public Point2 BottomLeft
			{
				get { return m_BottomLeft; }
			}

			public Point2 BottomRight
			{
				get { return m_BottomRight; }
			}

			private Point2 m_TopLeft;
			private Point2 m_TopRight;
			private Point2 m_BottomLeft;
			private Point2 m_BottomRight;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="texture">Transitions texture</param>
		/// <param name="textureRects">Texture rectangles for all tiles</param>
		public TileTransitionMasks( Texture2d texture, TextureRect[] textureRects )
		{
			m_TextureRects = textureRects;
			m_Texture = texture;
		}

		/// <summary>
		/// Gets the texture containing all transitions
		/// </summary>
		public Texture2d Texture
		{
			get { return m_Texture; }
		}

		/// <summary>
		/// Gets a texture coordinates for a given tile
		/// </summary>
		/// <param name="code">Tile code</param>
		/// <returns>Returns the texture rectangle on the texture for the given tile code</returns>
		public TextureRect GetTextureCoords( int code )
		{
			return m_TextureRects[ code ];
		}

		private readonly TextureRect[] m_TextureRects;
		private readonly Texture2d m_Texture;
	}
}
