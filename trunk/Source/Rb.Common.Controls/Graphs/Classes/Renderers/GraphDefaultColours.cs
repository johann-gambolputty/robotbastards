
using System.Drawing;

namespace Rb.Common.Controls.Graphs.Classes.Renderers
{
	/// <summary>
	/// Graph default colour map
	/// </summary>
	public class GraphDefaultColours
	{
		/// <summary>
		/// Gets the next colour
		/// </summary>
		public Color NextColour( )
		{
			return m_Colours[ m_CurColour++ ];
		}

		/// <summary>
		/// Gets the default colour array
		/// </summary>
		public Color[] Colours
		{
			get { return m_Colours; }
			set { m_Colours = value; }
		}

		#region Private Members

		private int m_CurColour = 0;
		private Color[] m_Colours = new Color[]
			{
				Color.Red,
				Color.Green,
				Color.Blue,
				Color.DarkSalmon,
				Color.Goldenrod,
				Color.Purple
			};

		#endregion
	}
}
