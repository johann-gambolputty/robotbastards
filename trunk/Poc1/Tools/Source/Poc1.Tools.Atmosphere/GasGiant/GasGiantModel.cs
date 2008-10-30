
using System.Drawing;
using System.Xml.Serialization;
using Rb.Core.Maths;

namespace Poc1.Tools.Atmosphere.GasGiant
{
	/// <summary>
	/// The gas giant model used by <see cref="GasGiantMarbleTextureBuilder"/> to build marble textures
	/// </summary>
	public class GasGiantModel
	{
		/// <summary>
		/// XML serializable colour structure
		/// </summary>
		public struct XmlColour
		{
			[XmlAttribute( "R" )]
			public byte R;
			[XmlAttribute( "G" )]
			public byte G;
			[XmlAttribute( "B" )]
			public byte B;
		}

		/// <summary>
		/// Sets band colours. For serialization only.
		/// </summary>
		[XmlArray( "BandColours" )]
		public XmlColour[] BandColoursIo
		{
			get
			{
				XmlColour[] colours = new XmlColour[ BandColours.Length ];
				for ( int i = 0; i < BandColours.Length; ++i )
				{
					colours[ i ].R = BandColours[ i ].R;
					colours[ i ].G = BandColours[ i ].G;
					colours[ i ].B = BandColours[ i ].B;
				}
				return colours;
			}
			set
			{
				m_BandColours = new Color[ value.Length ];
				for ( int i = 0; i < value.Length; ++i )
				{
					m_BandColours[ i ] = Color.FromArgb( value[ i ].R, value[ i ].G, value[ i ].B );
				}
			}
		}

		/// <summary>
		/// Gets/sets the gas band colours
		/// </summary>
		[XmlIgnore]
		public Color[] BandColours
		{
			get { return m_BandColours; }
			set { m_BandColours = value; }
		}

		/// <summary>
		/// Gets/sets the width of the generated texture
		/// </summary>
		public int Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		/// <summary>
		/// Gets/sets the height of the generated texture
		/// </summary>
		public int Height
		{
			get { return m_Height; }
			set { m_Height = value; }
		}

		public float Pass0XNoiseScale
		{
			get { return m_Pass0XNoiseScale; }
			set { m_Pass0XNoiseScale = value; }
		}

		public float Pass0YNoiseScale
		{
			get { return m_Pass0YNoiseScale; }
			set { m_Pass0YNoiseScale = value; }
		}

		public float Pass1XNoiseScale
		{
			get { return m_Pass1XNoiseScale; }
			set { m_Pass1XNoiseScale = value; }
		}

		public float Pass1YNoiseScale
		{
			get { return m_Pass1YNoiseScale; }
			set { m_Pass1YNoiseScale = value; }
		}

		public float Pass1XNoiseMultiplier
		{
			get { return m_Pass1XNoiseMultiplier; }
			set { m_Pass1XNoiseMultiplier = value; }
		}

		public float Pass1YNoiseMultiplier
		{
			get { return m_Pass1YNoiseMultiplier; }
			set { m_Pass1YNoiseMultiplier = value; }
		}

		#region Private Members

		private int m_Width = 1024;
		private int m_Height = 512;
		private float m_Pass0XNoiseScale = 15.917894f;
		private float m_Pass0YNoiseScale = 15.917894f;
		private float m_Pass1XNoiseScale = 0.31234f;
		private float m_Pass1YNoiseScale = 10.109175f;
		private float m_Pass1XNoiseMultiplier = 0.3f;
		private float m_Pass1YNoiseMultiplier = 0.5f;

		private Color[] m_BandColours = new Color[]
			{
				Color.WhiteSmoke,
				Color.Lavender,
				Color.LightYellow,
				Color.Goldenrod,
				Color.OrangeRed
			};

		private static Color ScaleColour( Color src, float scale )
		{
			float r = Utils.Clamp( src.R * scale, 0, 255 );
			float g = Utils.Clamp( src.G * scale, 0, 255 );
			float b = Utils.Clamp( src.B * scale, 0, 255 );
			return Color.FromArgb( ( int )r, ( int )g, ( int )b );
		}

		#endregion
	}
}
