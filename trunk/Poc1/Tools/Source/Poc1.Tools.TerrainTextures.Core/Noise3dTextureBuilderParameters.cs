using System.IO;
using System.Xml.Serialization;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Tools.TerrainTextures.Core
{
	/// <summary>
	///	Stores parameters used by <see cref="Noise3dTextureBuilder"/>
	/// </summary>
	[XmlRoot]
	public class Noise3dTextureBuilderParameters
	{
		/// <summary>
		/// Validates parameters
		/// </summary>
		public void Validate( )
		{
			if ( Width <= 0 )
			{
				throw new InvalidDataException( "Width cannot be <= 0" );
			}
			if ( Height <= 0 )
			{
				throw new InvalidDataException( "Height cannot be <= 0" );
			}
			if ( Depth <= 0 )
			{
				throw new InvalidDataException( "Depth cannot be <= 0" );
			}
			if ( !TextureFormatInfo.IsTrueColourFormat( Format ) )
			{
				throw new InvalidDataException( string.Format( "Noise generation only supports true-colour texture formats, not {0}", Format ) );
			}
			if ( GenerationType == NoiseGenerationType.MultiChannel )
			{
				if ( Format != TextureFormat.R8G8B8A8 )
				{
					throw new InvalidDataException( string.Format( "Multi-channel noise only supports R8G8B8A8 texture format, not {0}", Format ) );
				}
			}
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

		/// <summary>
		/// Gets/sets the depth of the generated texture
		/// </summary>
		public int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}

		/// <summary>
		/// Gets/sets the format of the texture
		/// </summary>
		public TextureFormat Format
		{
			get { return m_Format; }
			set { m_Format = value; }
		}

		/// <summary>
		/// Gets/sets the noise generation starting X coordinate
		/// </summary>
		public float NoiseX
		{
			get { return m_NoiseX; }
			set { m_NoiseX = value; }
		}

		/// <summary>
		/// Gets/sets the noise generation starting Y coordinate
		/// </summary>
		public float NoiseY
		{
			get { return m_NoiseY; }
			set { m_NoiseY = value; }
		}

		/// <summary>
		/// Gets/sets the noise generation starting Z coordinate
		/// </summary>
		public float NoiseZ
		{
			get { return m_NoiseZ; }
			set { m_NoiseZ = value; }
		}

		/// <summary>
		/// Gets/sets the noise generation width
		/// </summary>
		public float NoiseWidth
		{
			get { return m_NoiseWidth; }
			set { m_NoiseWidth = value; }
		}

		/// <summary>
		/// Gets/sets the noise generation height
		/// </summary>
		public float NoiseHeight
		{
			get { return m_NoiseHeight; }
			set { m_NoiseHeight = value; }
		}

		/// <summary>
		/// Gets/sets the noise generation depth
		/// </summary>
		public float NoiseDepth
		{
			get { return m_NoiseDepth; }
			set { m_NoiseDepth = value; }
		}

		/// <summary>
		/// Gets/sets the method used to generate and store the noise
		/// </summary>
		public NoiseGenerationType GenerationType
		{
			get { return m_GenerationType; }
			set { m_GenerationType = value; }
		}

		#region Private Members

		private int					m_Width;
		private int					m_Height;
		private int					m_Depth;
		private TextureFormat		m_Format;
		private float 				m_NoiseX;
		private float 				m_NoiseY;
		private float				m_NoiseZ;
		private float 				m_NoiseWidth;
		private float 				m_NoiseHeight;
		private float				m_NoiseDepth;
		private NoiseGenerationType m_GenerationType;

		#endregion
	}
}
