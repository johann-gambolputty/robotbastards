using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Rb.Common.Controls.Graphs.Classes;

namespace Rb.Common.Controls.Forms.Graphs
{
	public partial class GraphLegendControl : UserControl
	{
		public GraphLegendControl( )
		{
			InitializeComponent( );
		}

		/// <summary>
		/// Gets/sets the associated graph control
		/// </summary>
		public GraphControl AssociatedGraphControl
		{
			get { return m_GraphControl; }
			set
			{
				m_GraphControl = value;
				foreach ( Control control in tableLayoutPanel1.Controls )
				{
					GraphComponentControl graphControl = control as GraphComponentControl;
					if ( graphControl != null )
					{
						graphControl.AssociatedGraphControl = value;
					}
				}
			}
		}
		/// <summary>
		/// Adds a graph component to the control
		/// </summary>
		public void AddGraphComponent( GraphComponent component )
		{
			m_GraphComponents.Add( component );
			GraphComponentControl componentControl = new GraphComponentControl( component );
			componentControl.AssociatedGraphControl = m_GraphControl;
			componentControl.Anchor |= AnchorStyles.Right;
			tableLayoutPanel1.Controls.Add( componentControl );
		}

		/// <summary>
		/// Removes a graph component from the control
		/// </summary>
		public bool RemoveGraphComponent( GraphComponent component )
		{
			if ( !m_GraphComponents.Remove( component ) )
			{
				return false;
			}

			foreach ( Control control in tableLayoutPanel1.Controls )
			{
				GraphComponentControl graphControl = control as GraphComponentControl;
				if ( graphControl == null )
				{
					continue;
				}
				if ( graphControl.GraphComponent == component )
				{
					tableLayoutPanel1.Controls.Remove( control );
					return true;
				}
			}
			
			return false;
		}

		#region Private Members

		private GraphControl m_GraphControl;
		private readonly List<GraphComponent> m_GraphComponents = new List<GraphComponent>( );

		#endregion

		#region Event Handlers

		private void GraphComponentsControl_Load( object sender, EventArgs e )
		{
			DoubleBuffered = true;
		}
 
		#endregion

	}
}
