using System.Collections.Generic;
using System.Drawing;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Renderers
{
	public class Graph2dRendererList : IGraph2dRenderer
	{
		public Graph2dRendererList( )
		{
		}

		public Graph2dRendererList( params IGraph2dRenderer[] renderers )
		{
			m_Renderers.AddRange( renderers );
			Colour = Color.Red;
		}

		#region IGraph2dRenderer Members

		public Color Colour
		{
			get { return m_Colour; }
			set
			{
				m_Colour = value;
				foreach ( IGraph2dRenderer renderer in m_Renderers )
				{
					renderer.Colour = value;
				}
			}
		}

		public void Render( IGraphCanvas graphics, GraphTransform transform, IGraph2dSource data, PointF cursorDataPt, bool enabled )
		{
			foreach ( IGraph2dRenderer renderer in m_Renderers )
			{
				renderer.Render( graphics, transform, data, cursorDataPt, enabled );
			}
		}

		#endregion

		#region Private Members

		private Color m_Colour = Color.Red;
		private readonly List<IGraph2dRenderer> m_Renderers = new List<IGraph2dRenderer>( );

		#endregion
	}
}
