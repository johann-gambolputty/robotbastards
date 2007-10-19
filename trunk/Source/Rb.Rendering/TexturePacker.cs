using Rb.Core.Maths;

namespace Rb.Rendering
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
		/// <param name="format">Format of the pack texture</param>
		public TexturePacker( int width, int height, TextureFormat format )
		{
			m_Texture = Graphics.Factory.NewTexture2d( );
			m_Texture.Create( width, height, format );

			m_Root = new Node( 0, 0, width, height );
		}
		
		/// <summary>
		/// Adds a texture to the pack texture
		/// </summary>
		/// <param name="texture">Texture to add</param>
		/// <returns>Returns the rectangle in the pack texture where texture is placed. Returns null if no space could be found for texture</returns>
		public Rectangle Add( Texture2d texture )
		{
			Node node = Node.Add( m_Root, texture );
			return node == null ? null : node.Rect;
		}

		#region Private stuff

		private readonly Texture2d m_Texture;
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
			public static Node Add( Node node, Texture2d texture )
			{
				if ( node == null )
				{
					return null;
				}
				if ( !node.IsLeaf )
				{
					return Add( node.m_Left, texture ) ?? Add( node.m_Right, texture );
				}

				int width = texture.Width;
				int height = texture.Height;

				if ( ( node.m_Texture != null ) || ( !node.CanFit( width, height ) ) )
				{
					return null;
				}
				if ( node.ExactFit( width, height ) )
				{
					node.m_Texture = texture;
					return node;
				}
				int widthDiff = node.m_Width - texture.Width;
				int heightDiff = node.m_Height - texture.Height;
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
				return Add( node.m_Left, texture );
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

			private Texture2d m_Texture;
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
