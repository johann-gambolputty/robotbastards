using System;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Sources
{
	/// <summary>
	/// Simple implementation of the <see cref="IGraph2dSource"/> interface
	/// </summary>
	public abstract class Graph2dSourceAbstract : IGraph2dSource
	{
		#region IGraph2d Members

		/// <summary>
		/// Graph changed event
		/// </summary>
		public event EventHandler GraphChanged;

		/// <summary>
		/// Gets/sets the graph disabled flag
		/// </summary>
		public bool Disabled
		{
			get { return m_Disabled; }
			set
			{
				bool changed = ( m_Disabled != value );
				m_Disabled = value;
				OnGraphChanged( changed );
			}
		}

		/// <summary>
		/// Gets/sets the highlight state of the graph
		/// </summary>
		public bool Highlighted
		{
			get { return m_Highlight; }
			set
			{
				bool changed = ( m_Highlight != value );
				m_Highlight = value;
				OnGraphChanged( changed );
			}
		}

		/// <summary>
		/// Gets/sets the selected state of the graph
		/// </summary>
		public bool Selected
		{
			get { return m_Selected; }
			set
			{
				bool changed = ( m_Selected != value );
				m_Selected = value;
				OnGraphChanged( changed );
			}
		}

		/// <summary>
		/// Minimum X value
		/// </summary>
		public float MinimumX
		{
			get { return m_MinimumX; }
			set
			{
				bool changed = ( m_MinimumX != value );
				m_MinimumX = value;
				OnGraphChanged( changed );
			}
		}

		/// <summary>
		/// Minimum Y value
		/// </summary>
		public float MinimumY
		{
			get { return m_MinimumY; }
			set
			{
				bool changed = ( m_MinimumY != value );
				m_MinimumY = value;
				OnGraphChanged( changed );
			}
		}

		/// <summary>
		/// Maximum X value
		/// </summary>
		public float MaximumX
		{
			get { return m_MaximumX; }
			set
			{
				bool changed = ( m_MaximumX != value );
				m_MaximumX = value;
				OnGraphChanged( changed );
			}
		}

		/// <summary>
		/// Maximum Y value
		/// </summary>
		public float MaximumY
		{
			get { return m_MaximumY; }
			set
			{
				bool changed = ( m_MaximumY != value );
				m_MaximumY = value;
				OnGraphChanged( changed );
			}
		}

		/// <summary>
		/// Checks if a point in data space hits the graph
		/// </summary>
		public virtual bool IsHit( float x, float y, float tolerance )
		{
			return false;
		}

		/// <summary>
		/// Raises the GraphChanged event
		/// </summary>
		public void OnGraphChanged( )
		{
			if ( GraphChanged != null )
			{
				GraphChanged( this, EventArgs.Empty );
			}
		}

		/// <summary>
		/// Creates a controller for this graph.
		/// </summary>
		public virtual IGraph2dController CreateController( )
		{
			return null;
		}

		/// <summary>
		/// Gets the display value of the graph, when the data cursor is at (x,y)
		/// </summary>
		public abstract string GetDisplayValueAt( float x, float y );

		/// <summary>
		/// Creates a renderer for this graph
		/// </summary>
		public abstract IGraph2dRenderer CreateRenderer( );

		#endregion

		#region Protected Members

		/// <summary>
		/// Invokes the GraphChanged event
		/// </summary>
		protected void OnGraphChanged( bool changed )
		{
			if ( changed && GraphChanged != null )
			{
				GraphChanged( this, null );
			}
		}
		
		#endregion

		#region Private Members

		private bool m_Disabled;
		private bool m_Highlight;
		private bool m_Selected;
		private float m_MinimumX = 0;
		private float m_MinimumY = 0;
		private float m_MaximumX = 1;
		private float m_MaximumY = 1;
		
		#endregion
	}
}
