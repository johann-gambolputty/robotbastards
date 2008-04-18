using System.Drawing;
using Poc1.Universe.Interfaces.Rendering;
using Poc1.Universe.Classes.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rectangle=Rb.Core.Maths.Rectangle;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// A type of terrain found on a planet
	/// </summary>
	public class TerrainType : ITerrainType
	{
		#region Construction
		
		/// <summary>
		/// Terrain type setup constructor
		/// </summary>
		/// <param name="name">Terrain type name (e.g. "snow")</param>
		/// <param name="distribution">Distribution function for terrain type</param>
		/// <param name="colour">Terrain type colour representation</param>
		public TerrainType( string name, TerrainDistribution distribution, Color colour )
		{
			m_Name = name;
			m_Distribution = distribution;
			m_Colour = colour;
		}

		#endregion

		#region ITerrainType Members

		/// <summary>
		/// Gets the name of this terrain type
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets/sets the colour representation of this terrain type
		/// </summary>
		public Color Colour
		{
			get { return m_Colour; }
			set { m_Colour = value; }
		}

		/// <summary>
		/// Gets the distribution of this terrain type
		/// </summary>
		public TerrainDistribution Distribution
		{
			get { return m_Distribution; }
		}

		/// <summary>
		/// Gets the UV rectangle for this terrain type, in the specified terrain set texture
		/// </summary>
		public Rectangle GetUvRectangle( ITexture terrainSetTexture )
		{
			return null;
		}

		#endregion

		#region Private Members

		private Color m_Colour;
		private readonly string m_Name;
		private readonly TerrainDistribution m_Distribution;

		#endregion
	}

}