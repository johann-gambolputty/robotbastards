using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rb.Log.Controls
{
	public partial class LogListView : ListView
	{
		public LogListView( )
		{
			InitializeComponent( );

			DoubleBuffered = true;

			OwnerDraw = true;
			DrawColumnHeader += new DrawListViewColumnHeaderEventHandler( LogListView_DrawColumnHeader );
			DrawItem += new DrawListViewItemEventHandler( LogListView_DrawItem );

			Rb.Log.Source.OnNewLogEntry += new OnNewLogEntryDelegate( OnNewLogEntry );
		}

		~LogListView( )
		{
			Rb.Log.Source.OnNewLogEntry -= new OnNewLogEntryDelegate( OnNewLogEntry );
		}

		void LogListView_DrawItem( object sender, DrawListViewItemEventArgs e )
        {
            Entry entry = ( Entry )e.Item.Tag;
            System.Drawing.Color backCol = System.Drawing.Color.White;

            switch ( entry.Severity )
            {
                case Severity.Info		: backCol = Color.BlanchedAlmond; break;
                case Severity.Warning	: backCol = Color.Orange; break;
                case Severity.Error		: backCol = Color.Red; break;
            }

            if ( e.Item.Selected )
            {
                backCol = ControlPaint.Dark( backCol );
            }
            e.Graphics.FillRectangle( new SolidBrush( backCol ), e.Bounds );

            foreach ( ListViewItem.ListViewSubItem subItem in e.Item.SubItems )
            {
                e.Graphics.DrawString( subItem.Text, Font, Brushes.Black, subItem.Bounds );
            }
        }

		static void LogListView_DrawColumnHeader( object sender, DrawListViewColumnHeaderEventArgs e )
        {
            e.DrawDefault = true;
        }

        private bool EntryPassesFilter( Entry entry )
        {
            return IsSourceVisible( entry.Source );
        }

        private void AddListViewItem( ListViewItem item )
        {
            if ( IsHandleCreated )
            {
                Items.Add( item );

                if ( m_TrackingLastItem )
                {
                    item.Selected = true;
                    item.EnsureVisible( );
                }
            }
        }

        private delegate void AddListViewItemDelegate( ListViewItem item );

        private void OnNewLogEntry( Entry entry )
        {
            if ( !EntryPassesFilter( entry ) )
            {
                return;
            }

			if ( IsHandleCreated )
            {
                string[] lines = entry.Message.Split( new char[] { '\n' } );
                lock ( m_Lock )
                {
                    foreach ( string line in lines )
                    {
                        ListViewItem newItem = new ListViewItem( entry.Id.ToString( ) );

                        string[] subItems = new string[ Columns.Count ];
                        
                        subItems[ m_FileColumn.DisplayIndex ]       = entry.File;
                        subItems[ m_LineColumn.DisplayIndex ]       = entry.Line.ToString( );
                        subItems[ m_ColumnColumn.DisplayIndex ]     = entry.Column.ToString( );
                        subItems[ m_MethodColumn.DisplayIndex ]     = entry.Method;
                        subItems[ m_SourceColumn.DisplayIndex ]     = entry.Source.ToString( );
                        subItems[ m_TimeColumn.DisplayIndex ]       = entry.Time.ToString( );
                        subItems[ m_ThreadColumn.DisplayIndex ]     = entry.Thread;
                        subItems[ m_MessageColumn.DisplayIndex ]    = line;

                        newItem.SubItems.AddRange( subItems );

                        newItem.Tag = entry;

						//	TODO: AP: If the list view is destroyed at the point, this blocks
                        Invoke( new AddListViewItemDelegate( AddListViewItem ), newItem );   
                    }
                }
            }
        }

        private static void AddEntryToCache( Entry entry )
        {
            ms_Cache[ ms_CacheIndex ] = entry;
            ms_CacheIndex = ( ms_CacheIndex + 1 ) % ms_Cache.Length;
            if ( ms_CacheIndex == ms_CacheStart )
            {
                ms_CacheStart = ( ms_CacheStart + 1 ) % ms_Cache.Length;
            }
        }

        static LogListView( )
        {
            Rb.Log.Source.OnNewLogEntry += new OnNewLogEntryDelegate( AddEntryToCache );
        }

        private Dictionary< Rb.Log.Source, bool >   m_SourceFilter = new Dictionary< Rb.Log.Source, bool >( );
        private Dictionary< Rb.Log.Tag, bool >      m_TagFilter = new Dictionary< Rb.Log.Tag, bool >( );

        private bool IsSourceVisible( Rb.Log.Source source )
        {
            if ( !m_SourceFilter.ContainsKey( source ) )
            {
                m_SourceFilter[ source ] = !source.Suppress;
                return !source.Suppress;
            }

            return m_SourceFilter[ source ];
        }

        private void SetSourceVisibility(Rb.Log.Source source, bool visible)
        {
            if (!m_SourceFilter.ContainsKey(source))
            {
                m_SourceFilter.Add(source, visible);
            }
            else
            {
                m_SourceFilter[source] = visible;
            }
        }

        private bool IsTagVisible( Rb.Log.Tag tag )
        {
            if ( !m_TagFilter.ContainsKey( tag ) )
            {
                m_TagFilter[ tag ] = !tag.Suppress;
                return !tag.Suppress;
            }

            return m_TagFilter[ tag ];
        }

        private void SetTagVisibility( Rb.Log.Tag tag, bool visible )
        {
            if (!m_TagFilter.ContainsKey(tag))
            {
                m_TagFilter.Add( tag, visible );
            }
            else
            {
                m_TagFilter[ tag ] = visible;
            }

            foreach ( Rb.Log.Source source in tag.Sources )
            {
                SetSourceVisibility( source, visible );
            }

            foreach ( Rb.Log.Tag childTag in tag.ChildTags )
            {
                SetTagVisibility( childTag, visible );
            }
        }

        private void ToggleSourceVisibility(object sender, EventArgs args)
        {
            MenuItem item = (MenuItem)sender;
            item.Checked = !item.Checked;
            SetSourceVisibility((Rb.Log.Source)item.Tag, item.Checked);
            RefreshView();
        }

        private void ToggleTagVisibility(object sender, EventArgs args)
        {
            MenuItem item = (MenuItem)sender;
            item.Checked = !item.Checked;
            SetTagVisibility((Rb.Log.Tag)item.Tag, item.Checked);
            RefreshView();
        }

        private static Entry[]  ms_Cache            = new Entry[ 1000 ];
        private static int      ms_CacheStart       = 0;
        private static int      ms_CacheIndex       = 0;

        private object          m_Lock              = new object( );

        private bool            m_TrackingLastItem  = true;

		private void LogListView_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( SelectedIndices.Count == 0 )
			{
				return;
			}
			m_TrackingLastItem = ( SelectedIndices[ 0 ] == ( Items.Count - 1 ) );
			if ( !m_TrackingLastItem )
			{
				SelectedItems[ 0 ].Selected = true;
				SelectedItems[ 0 ].EnsureVisible( );
			}
		}
        private void RefreshView( )
        {
            Items.Clear( );

            for ( int cacheIndex = ms_CacheStart; cacheIndex != ms_CacheIndex; cacheIndex = ( cacheIndex + 1 ) % ms_Cache.Length )
            {
                OnNewLogEntry( ms_Cache[ cacheIndex ] );
            }
        }

        private void BuildSourceFilterContextMenu(ContextMenu menu, Rb.Log.Source source, string prefix)
        {
            MenuItem item = new MenuItem(prefix + source.Name);
            item.Checked = IsSourceVisible(source);
            item.Tag = source;
            menu.MenuItems.Add(item);

            item.Click += new EventHandler(ToggleSourceVisibility);
        }

	    private void BuildTagFilterContextMenu( ContextMenu menu, Rb.Log.Tag tag, string prefix )
        {
            string childPrefix = prefix + "    ";

            if ( tag != Rb.Log.Tag.Root )
            {
                MenuItem item = new MenuItem( prefix + tag.Name );
                item.Tag = tag;
                item.BarBreak = ((menu.MenuItems.Count > 0) && (tag.Parent.IsRootTag));
                item.Checked = IsTagVisible( tag );
                menu.MenuItems.Add( item );

                item.Click += new EventHandler( ToggleTagVisibility );

                foreach ( Source source in tag.Sources )
                {
                    if (source != null)
                    {
                        BuildSourceFilterContextMenu( menu, source,childPrefix );
                    }
                }
            }

            foreach ( Tag childTag in tag.ChildTags )
            {
                BuildTagFilterContextMenu( menu, childTag, childPrefix );
            }
        }

        private void LogListView_MouseDown(object sender, MouseEventArgs e)
        {
            if ( e.Button == MouseButtons.Right )
            {
                //  Create a context menu
                ContextMenu contextMenu = new ContextMenu( );

                BuildTagFilterContextMenu( contextMenu, Rb.Log.Tag.Root, "" );

                contextMenu.Show( this, e.Location );
            }
        }
	}
}
