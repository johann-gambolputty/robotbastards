
using System.Drawing;

namespace Poc1.Tools.Atmosphere.GasGiant
{
	/// <summary>
	/// The gas giant model used by <see cref="GasGiantMarbleTextureBuilder"/> to build marble textures
	/// </summary>
	public class GasGiantModel
	{
		/// <summary>
		/// Gets/sets the gas band colours
		/// </summary>
		public Color[] BandColours
		{
			get { return m_BandColours; }
			set { m_BandColours = value; }
		}

		#region Private Members

		private Color[] m_BandColours = new Color[]
			{
				Color.WhiteSmoke,
				Color.DarkSalmon,
				Color.OrangeRed
			};

		#endregion
	}
}
