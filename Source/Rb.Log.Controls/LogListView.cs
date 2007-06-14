using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Rb.Log.Controls
{
	public partial class LogListView : ListView
    {
        #region Construction and destruction

        public LogListView( )
		{
            m_SourceColours[ ( int )Severity.Verbose ]  = Color.White;
            m_SourceColours[ ( int )Severity.Info ]     = Color.BlanchedAlmond;
            m_SourceColours[ ( int )Severity.Warning ]  = Color.Orange;
            m_SourceColours[ ( int )Severity.Error ]    = Color.Red;

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
        

        static LogListView( )
        {
            Rb.Log.Source.OnNewLogEntry += new OnNewLogEntryDelegate( AddEntryToCache );
        }

        #endregion

        #region Source colour properties

        public Color VerboseColour
        {
            get { return GetSourceColour(Severity.Verbose); }
            set { SetSourceColour(Severity.Verbose, value); }
        }
        public Color InfoColour
        {
            get { return GetSourceColour(Severity.Info); }
            set { SetSourceColour(Severity.Info, value); }
        }
        public Color WarningColour
        {
            get { return GetSourceColour(Severity.Warning); }
            set { SetSourceColour(Severity.Warning, value); }
        }
        public Color ErrorColour
        {
            get { return GetSourceColour(Severity.Error); }
            set { SetSourceColour(Severity.Error, value); }
        }

        #endregion

        #region Private source colours

        private Color[] m_SourceColours = new Color[ ( int )Severity.Count ];

        private Color GetSourceColour( Severity s )
        {
            return m_SourceColours[ ( int )s ];
        }

        private void SetSourceColour( Severity s, Color col )
        {
            m_SourceColours[ ( int )s ] = col;
        }

        #endregion

        #region View rendering
        
        private void RefreshView( )
        {
            Items.Clear( );

            for ( int cacheIndex = ms_CacheStart; cacheIndex != ms_CacheIndex; cacheIndex = ( cacheIndex + 1 ) % ms_Cache.Length )
            {
                OnNewLogEntry( ms_Cache[ cacheIndex ] );
            }
        }

        void LogListView_DrawItem( object sender, DrawListViewItemEventArgs e )
        {
            Entry entry = ( Entry )e.Item.Tag;
            bool oddItem = (e.ItemIndex % 2) != 0;
            System.Drawing.Color backCol = m_SourceColours[ ( int )entry.Severity ];

            float factor = 1.0f;
            if ( e.Item.Selected )
            {
                factor = oddItem ? 0.6f : 0.75f;
            }
            else
            {
                factor = oddItem ? 0.9f : 1.0f;
            }
            
            backCol = Color.FromArgb( ( int )( backCol.R * factor ), ( int )( backCol.G * factor ), ( int )( backCol.B * factor ) );

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

        #endregion

        #region Entry handling

        private bool    m_TrackingLastItem  = true;
        private object  m_Lock              = new object();

        private delegate void AddListViewItemDelegate( ListViewItem item );

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

        #endregion

        #region Entry caching

        private static Entry[]  ms_Cache        = new Entry[ 1000 ];
        private static int      ms_CacheStart   = 0;
        private static int      ms_CacheIndex   = 0;

        private static void AddEntryToCache( Entry entry )
        {
            ms_Cache[ ms_CacheIndex ] = entry;
            ms_CacheIndex = ( ms_CacheIndex + 1 ) % ms_Cache.Length;
            if ( ms_CacheIndex == ms_CacheStart )
            {
                ms_CacheStart = ( ms_CacheStart + 1 ) % ms_Cache.Length;
            }
        }

        #endregion

        #region Entry filtering
        
        private Dictionary< Rb.Log.Source, bool >   m_SourceFilter = new Dictionary< Rb.Log.Source, bool >( );
        private Dictionary< Rb.Log.Tag, bool >      m_TagFilter = new Dictionary< Rb.Log.Tag, bool >( );

        private bool EntryPassesFilter(Entry entry)
        {
            return IsSourceVisible(entry.Source);
        }

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

        #endregion

        #region Interaction

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

        #endregion

        private void LogListView_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = ( string[] )e.Data.GetData( "FileName" );
            Entry[] entries = Entry.CreateEntriesFromLogFile( filenames[ 0 ] );

            foreach ( Entry entry in entries )
            {
                OnNewLogEntry( entry );
            }
        }

        private void LogListView_DragEnter(object sender, DragEventArgs e)
        {
            if ( e.Data.GetDataPresent( "FileName" ) )
            {
                e.Effect = DragDropEffects.Link;
            }
        }
    }
}
