using System.Xml.Serialization;

namespace Poc1.Tools.TerrainTextures.Core
{
	[XmlRoot]
	public class NoiseBitmapBuilderParameters
	{
		//	TODO: AP: Add NoiseGenerationType property

		/// <summary>
		/// Width of the generated bitmap
		/// </summary>
		public int BitmapWidth
		{
			get { return m_BitmapWidth; }
			set { m_BitmapWidth = value; }
		}

		/// <summary>
		/// Height of the generated bitmap
		/// </summary>
		public int BitmapHeight
		{
			get { return m_BitmapHeight; }
			set { m_BitmapHeight = value; }
		}

		/// <summary>
		/// Starting point X coordinate in noise basis domain
		/// </summary>
		public float NoiseX
		{
			get { return m_NoiseX; }
			set { m_NoiseX = value; }
		}

		/// <summary>
		/// Starting point Y coordinate in noise basis domain
		/// </summary>
		public float NoiseY
		{
			get { return m_NoiseY; }
			set { m_NoiseY = value; }
		}

		/// <summary>
		/// Width of rectangle in noise domain
		/// </summary>
		public float NoiseWidth
		{
			get { return m_NoiseWidth; }
			set { m_NoiseWidth = value; }
		}

		/// <summary>
		/// Height of rectangle in noise domain
		/// </summary>
		public float NoiseHeight
		{
			get { return m_NoiseHeight; }
			set { m_NoiseHeight = value; }
		}

		/// <summary>
		/// Optional filename. If not null or empty, the builder will create a sample 4x4 tiling of the noise texture in this file
		/// </summary>
		public string TestFilePath
		{
			get { return m_TestFilePath; }
			set { m_TestFilePath = value; }

		}

		/// <summary>
		/// Gets/sets the noise generation type
		/// </summary>
		public NoiseGenerationType GenerationType
		{
			get { return m_GenerationType; }
			set { m_GenerationType = value; }
		}

		#region Private Members

		private int					m_BitmapWidth;
		private int					m_BitmapHeight;
		private float 				m_NoiseX;
		private float 				m_NoiseY;
		private float 				m_NoiseWidth;
		private float 				m_NoiseHeight;
		private string				m_TestFilePath;
		private NoiseGenerationType m_GenerationType;
		
		#endregion

	}

}
