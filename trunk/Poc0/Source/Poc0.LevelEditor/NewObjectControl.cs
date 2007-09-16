using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace Poc0.LevelEditor
{
	public partial class NewObjectControl : UserControl
	{
		public event Action< Type > SelectionMade;

		public NewObjectControl( )
		{
			InitializeComponent( );

			typeView.DrawMode = DrawMode.OwnerDrawFixed;
		}

		public Type BaseType
		{
			get { return m_BaseType; }
			set { m_BaseType = value; }
		}

		private Type m_BaseType;
		private Font m_GroupFont;

		private void NewObjectControl_Load( object sender, EventArgs e )
		{
			if ( !DesignMode )
			{
				foreach ( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies( ) )
				{
					AddAssembly( assembly );
				}

				m_GroupFont = new Font( Font.FontFamily, Font.Size, Font.Style | FontStyle.Bold, Font.Unit );
			}
		}

		private void AddAssembly( Assembly assembly )
		{
			List< Type > types = new List< Type >( );

			if ( m_BaseType.IsInterface )
			{
				foreach ( Type type in assembly.GetTypes( ) )
				{
					if ( type.GetInterface( m_BaseType.Name ) != null )
					{
						types.Add( type );
					}
				}
			}
			else
			{
				foreach ( Type type in assembly.GetTypes( ) )
				{
					if ( ( type == m_BaseType ) || ( type.IsSubclassOf( m_BaseType ) ) )
					{
						types.Add( type );
					}
				}
			}

			if ( types.Count > 0 )
			{
				typeView.Items.Add( new TypeGroup( assembly.GetName( ).Name, types.ToArray( ) ) );
			}
		}

		private class TypeItem
		{
			public TypeItem( Type type )
			{
				m_Type = type;
			}

			public Type Type
			{
				get { return m_Type; }
			}

			public override string ToString( )
			{
				return m_Type.Name;
			}

			private readonly Type m_Type;
		}

		private class TypeGroup
		{
			public TypeGroup( string groupName, Type[] types )
			{
				m_Name = groupName;
				m_TypeItems = new TypeItem[ types.Length ];

				for ( int typeIndex = 0; typeIndex < types.Length; ++typeIndex )
				{
					m_TypeItems[ typeIndex ] = new TypeItem( types[ typeIndex ] );
				}
			}

			public bool IsExpanded
			{
				get { return m_Expanded; }
			}

			public void Expand( ListBox view )
			{
				if ( !m_Expanded )
				{
					for ( int typeIndex = 0; typeIndex < m_TypeItems.Length; ++typeIndex )
					{
						view.Items.Insert( view.Items.IndexOf( this ) + 1, m_TypeItems[ typeIndex ] );
					}
				}
				else
				{
					foreach ( TypeItem typeItem in m_TypeItems )
					{
						view.Items.Remove( typeItem );
					}
				}
				m_Expanded = !m_Expanded;
			}

			public override string ToString( )
			{
				return m_Name;
			}

			private bool m_Expanded;
			private readonly string m_Name;
			private readonly TypeItem[] m_TypeItems;
		}

		public Type NewObjectType
		{
			get
			{
				if ( typeView.SelectedItems.Count == 0 )
				{
					return null;
				}
				TypeItem item = typeView.SelectedItems[ 0 ] as TypeItem;
				return ( item == null ) ? null : item.Type;
			}
		}

		private void typeView_DoubleClick( object sender, EventArgs e )
		{
			TypeGroup hdr = typeView.SelectedItems[ 0 ] as TypeGroup;
			if ( hdr != null )
			{
				hdr.Expand( typeView );
				return;
			}
			Type type = NewObjectType;
			if ( ( type != null ) && ( SelectionMade != null ) )
			{
				SelectionMade( type );
			}
		}

		private Brush m_BackBrush;

		private void typeView_DrawItem( object sender, DrawItemEventArgs e )
		{
			e.DrawFocusRectangle( );
			e.DrawBackground( );

			Rectangle bounds = e.Bounds;

			object item = typeView.Items[ e.Index ];
			string text = item.ToString( );

			TypeGroup asmHdr = item as TypeGroup;
			if ( asmHdr != null )
			{
				if ( m_BackBrush == null )
				{
					m_BackBrush = new LinearGradientBrush( bounds, Color.LightGray, Color.LightSlateGray, LinearGradientMode.Vertical );
				}

				e.Graphics.FillRectangle( m_BackBrush, bounds );
				e.Graphics.DrawRectangle( Pens.Gray, bounds );

				const int expandBoxWidth = 8;
				const int expandBoxHeight = 8;
				Rectangle expandBox = new Rectangle( bounds.X + 2, bounds.Y + 2, expandBoxWidth, expandBoxHeight );
				int midX = expandBox.X + ( expandBoxWidth / 2 );
				int midY = expandBox.Y + ( expandBoxHeight / 2 );

				if ( !asmHdr.IsExpanded )
				{
					e.Graphics.DrawRectangle( Pens.Black, expandBox );
					e.Graphics.DrawLine( Pens.Black, midX, expandBox.Top + 2, midX, expandBox.Bottom - 2 );
					e.Graphics.DrawLine( Pens.Black, expandBox.Left + 2, midY, expandBox.Right - 2, midY );
				}
				else
				{
					e.Graphics.DrawRectangle( Pens.Black, expandBox );
					e.Graphics.DrawLine( Pens.Black, expandBox.Left + 2, midY, expandBox.Right - 2, midY );
				}
				e.Graphics.DrawString( text, m_GroupFont, Brushes.Black, bounds.X + expandBoxWidth + 8, bounds.Y );
			}
			else
			{
				e.DrawBackground( );
				e.Graphics.DrawString( text, Font, Brushes.Black, bounds.X + 16, bounds.Y );
			}
		}
	}
}
