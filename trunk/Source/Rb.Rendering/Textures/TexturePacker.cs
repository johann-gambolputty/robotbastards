using System;
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Rendering.Interfaces.Objects;
using Rectangle=Rb.Core.Maths.Rectangle;

namespace Rb.Rendering.Textures
{
	/// <summary>
	/// Packs a list of 2d textures into a single 2d texture
	/// </summary>
	public class TexturePacker
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="width">Width of the pack texture</param>
		/// <param name="height">Height of the pack texture</param>
		public TexturePacker( int width, int height )
		{
			m_Root = new Node( 0, 0, width, height );

			m_TextureImage = new Bitmap( width, height, PixelFormat.Format32bppArgb );
			m_Graphics = System.Drawing.Graphics.FromImage( m_TextureImage );
		}

		/// <summary>
		/// Gets the underlying texture
		/// </summary>
		/// <remarks>
		/// Until <see cref="Finish"/> has been called, this property will return null.
		/// </remarks>
		public ITexture2d Texture
		{
			get { return m_Texture; }
		}

		/// <summary>
		/// Adds a bitmap to the pack texture
		/// </summary>
		/// <param name="bmp">Bitmap to add</param>
		/// <returns>Returns the rectangle in the pack texture where bitmap is placed. Returns null if no space could be found for texture</returns>
		public Rectangle Add( Bitmap bmp )
		{
			if ( m_Texture != null )
			{
				throw new InvalidOperationException( "Cannot add to this packed texture after Finish() was called" );
			}

			Node node = Node.Add( m_Root, bmp.Width, bmp.Height );
			if ( node == null )
			{
				return null;
			}

			m_Graphics.DrawImage( bmp, node.Rect.X, node.Rect.Y );

			float invWidth = 1.0f / m_TextureImage.Width;
			float invHeight = 1.0f / m_TextureImage.Height;
			return new Rectangle( node.Rect.X * invWidth, node.Rect.Y * invHeight, node.Rect.Width * invWidth, node.Rect.Height * invHeight );
		}
		
		/// <summary>
		/// Adds a texture to the pack texture
		/// </summary>
		/// <param name="texture">Texture to add</param>
		/// <returns>Returns the rectangle in the pack texture where texture is placed. Returns null if no space could be found for texture</returns>
		public Rectangle Add( ITexture2d texture )
		{
			return Add( texture.ToBitmap( ) );
		}

		/// <summary>
		/// Creates the underlying texture, accessible via <see cref="Texture"/>
		/// </summary>
		/// <param name="generateMipMaps">If true, then the texture gets mip-mapped</param>
		public void Finish( bool generateMipMaps )
		{
			m_Texture = Graphics.Factory.CreateTexture2d( );
			m_Texture.Load( m_TextureImage, generateMipMaps );
		}

		#region Private stuff

		private readonly Bitmap m_TextureImage;
		private readonly System.Drawing.Graphics m_Graphics;

		private ITexture2d m_Texture;
		private readonly Node m_Root;

		/// <summary>
		/// Texture storage node
		/// </summary>
		private class Node
		{
			/// <summary>
			/// Node setup constructor
			/// </summary>
			public Node( int x, int y, int width, int height )
			{
				m_X = x;
				m_Y = y;
				m_Width = width;
				m_Height = height;
			}

			/// <summary>
			/// Gets the UV rectangle of the node
			/// </summary>
			public Rectangle Rect
			{
				get { return new Rectangle( m_X, m_Y, m_Width, m_Height );}
			}

			/// <summary>
			/// Adds a texture to the specified node
			/// </summary>
			public static Node Add( Node node, int width, int height )
			{
				if ( node == null )
				{
					return null;
				}
				if ( !node.IsLeaf )
				{
					return Add( node.m_Left, width, height ) ?? Add( node.m_Right, width, height );
				}

				if ( ( node.m_Occupied ) || ( !node.CanFit( width, height ) ) )
				{
					return null;
				}
				if ( node.ExactFit( width, height ) )
				{
					node.m_Occupied = true;
					return node;
				}
				int widthDiff = node.m_Width - width;
				int heightDiff = node.m_Height - height;
				if ( widthDiff > heightDiff )
				{
					node.m_Left = new Node( node.m_X, node.m_Y, width, node.m_Height );
					node.m_Right = new Node( node.m_X + width + 1, node.m_Y, widthDiff, node.m_Height );
				}
				else
				{
					node.m_Left = new Node( node.m_X, node.m_Y, node.m_Width, height );
					node.m_Right = new Node( node.m_X, node.m_Y + width + 1, node.m_Width, heightDiff );
				}
				return Add( node.m_Left, width, height );
			}

			#region Private stuff

			/// <summary>
			/// Returns true if this is a leaf node
			/// </summary>
			private bool IsLeaf
			{
				get { return (m_Left == null) && (m_Right == null); }
			}

			/// <summary>
			/// Returns true if the specified dimensions are an exact fit in this node
			/// </summary>
			private bool ExactFit( int width, int height )
			{
				return ( width == m_Width ) && ( height == m_Height );
			}

			/// <summary>
			/// Returns true if the specified dimensions can fit in this node
			/// </summary>
			private bool CanFit( int width, int height )
			{
				return width <= m_Width && height <= m_Height;
			}

			private bool m_Occupied;
			private readonly int m_X;
			private readonly int m_Y;
			private readonly int m_Width;
			private readonly int m_Height;
			private Node m_Left;
			private Node m_Right;

			#endregion
		}


		#endregion
	}
}
