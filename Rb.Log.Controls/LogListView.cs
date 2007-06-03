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
            return true;
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
                        newItem.SubItems.Add( entry.File );
                        newItem.SubItems.Add( entry.Line.ToString( ) );
                        newItem.SubItems.Add( entry.Column.ToString( ) );
                        newItem.SubItems.Add( entry.Method );
                        newItem.SubItems.Add( entry.Source.ToString( ) );
                        newItem.SubItems.Add( entry.Time.ToString( ) );
                        newItem.SubItems.Add( entry.Thread );
                        newItem.SubItems.Add( line );
                        newItem.Tag = entry;

						//	TODO: AP: If the list view is destroy at the point, this blocks
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

        private static Entry[]  ms_Cache        = new Entry[ 1000 ];
        private static int      ms_CacheStart   = 0;
        private static int      ms_CacheIndex   = 0;

        private object          m_Lock = new object( );

        private bool            m_TrackingLastItem = true;

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
	}
}
