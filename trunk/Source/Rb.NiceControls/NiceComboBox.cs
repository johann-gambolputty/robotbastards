using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Rb.NiceControls
{
	public partial class NiceComboBox : ComboBox
	{
		public NiceComboBox( )
		{
			InitializeComponent( );
			DropDownStyle = ComboBoxStyle.DropDownList;
			DrawMode = DrawMode.OwnerDrawFixed;
		}

		/// <summary>
		/// Adds a separator item to the item list
		/// </summary>
		public void AddSeparator( )
		{
			Items.Add( new Item( ) );
		}

		/// <summary>
		/// Gets the object stored in the tag of one of the items
		/// </summary>
		public object GetTag( int index )
		{
			return ( ( Item )Items[ index ] ).Tag;
		}

		/// <summary>
		/// Gets the typed object stored in the tag of one of the items
		/// </summary>
		public T GetTag< T >( int index )
		{
			return ( T )( ( Item )Items[ index ] ).Tag;
		}

		/// <summary>
		/// Nice combo box standard item (add these to the Items collection)
		/// </summary>
		public class Item
		{
			/// <summary>
			/// Creates a separator item
			/// </summary>
			public Item( )
			{
				m_Separator = true;
			}

			/// <summary>
			/// Creates a standard item
			/// </summary>
			/// <param name="depth">Item 'depth' (determines prefix spacing)</param>
			/// <param name="text">Item text</param>
			/// <param name="style">Item font rendering style</param>
			/// <param name="image">Item image, drawn before text</param>
			/// <param name="selectedImage">Item image, drawn before text, if item is selected</param>
			/// <param name="tag">User cookie</param>
			public Item( int depth, string text, FontStyle style, Image image, Image selectedImage, object tag )
			{
				m_Depth			= depth;
				m_Separator		= false;
				m_Image			= image;
				m_SelectedIamge	= selectedImage;
				m_Text			= text;
				m_Style			= style;
				m_Tag			= tag;
			}

			/// <summary>
			/// True if this is a separator
			/// </summary>
			public bool Separator
			{
				get { return m_Separator; }
			}

			/// <summary>
			/// Item image
			/// </summary>
			public Image Image
			{
				get { return m_Image; }
			}

			/// <summary>
			/// Item selected image
			/// </summary>
			public Image SelectedImage
			{
				get { return m_SelectedIamge; }
			}

			/// <summary>
			/// Item text
			/// </summary>
			public string Text
			{
				get { return m_Text; }
			}

			/// <summary>
			/// Item font style
			/// </summary>
			public FontStyle Style
			{
				get { return m_Style; }
			}

			/// <summary>
			/// Item user cookie
			/// </summary>
			public object Tag
			{
				get { return m_Tag; }
			}

			/// <summary>
			/// Item depth
			/// </summary>
			public int Depth
			{
				get { return m_Depth; }
			}

			private readonly int		m_Depth;
			private readonly bool		m_Separator;
			private readonly Image		m_Image;
			private readonly Image		m_SelectedIamge;
			private readonly string		m_Text;
			private readonly FontStyle	m_Style;
			private readonly object		m_Tag;
		}

		
		/// <summary>
		/// Draws an item
		/// </summary>
		protected override void OnDrawItem( DrawItemEventArgs args )
		{
			args.DrawBackground( );
			args.DrawFocusRectangle( );

			if ( args.Index == -1 )
			{
				return;
			}

			Rectangle bounds = args.Bounds;

			int x = bounds.Left;
			int y = bounds.Top;
			bool editItem = ( args.State & DrawItemState.ComboBoxEdit ) != 0;

			Item item = Items[ args.Index ] as Item;
			if ( item == null )
			{
				//	Not an Item (oooh... bad...). Draw it as a bog standard string
				string str = Items[ args.Index ].ToString( );
				args.Graphics.DrawString( str, args.Font, new SolidBrush( args.ForeColor ), x, y );
				return;
			}

			if ( item.Separator )
			{
				//	Separator item - draw it as a dashed line
				y = bounds.Top + ( bounds.Height / 2 );
				Pen separatorPen = new Pen( new HatchBrush( HatchStyle.DarkVertical, args.ForeColor, args.BackColor ), 1 );

				args.Graphics.DrawLine( separatorPen, bounds.Left, y, bounds.Right, y );
				return;
			}

			Image img = ( args.Index == SelectedIndex ) ? ( item.SelectedImage ?? item.Image ) : item.Image;

			if ( !editItem )
			{
				x += item.Depth * ( img == null ? 16 : img.Width );
			}

			if ( img != null )
			{
				args.Graphics.DrawImage( img, x, y );
				x += img.Width;
			}

			Font font = args.Font;
			if ( ( font.Style & item.Style ) != item.Style )
			{
				font = new Font( font.FontFamily, font.Size, font.Style | item.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont );
			}
			args.Graphics.DrawString( item.Text + args.State + SelectedIndex, font, new SolidBrush( args.ForeColor ), x, y );

			base.OnDrawItem( args );
		}

	}
}
