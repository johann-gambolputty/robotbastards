using System;

namespace RbEngine.Rendering
{

	/// <summary>
	/// Texture filtering options
	/// </summary>
	public enum TextureFilter
	{
		/// <summary>
		/// Chooses the nearest texel from the source texture
		/// </summary>
		kNearestTexel,

		/// <summary>
		/// Weighted average of the four nearest texels from the source texture
		/// </summary>
		kLinearTexel,

		/// <summary>
		/// Chooses the nearest texel from the nearest mipmap
		/// </summary>
		kNearestTexelNearestMipMap,

		/// <summary>
		/// Takes the weighted average of the four nearest texels from the nearest mipmap
		/// </summary>
		kLinearTexelNearestMipMap,

		/// <summary>
		/// Takes the nearest texel from the two nearest mipmaps, and returns their weighted average
		/// </summary>
		kNearestTexelLinearMipMap,

		/// <summary>
		/// Takes the weighted average of the 4 nearest texels from the two nearest mipmaps, and returns the weighted average of these two values
		/// </summary>
		kLinearTexelLinearMipMap

	};

	/// <summary>
	/// Texture wrap types
	/// </summary>
	public enum TextureWrap
	{
		/// <summary>
		/// Wraps texture coordinates back to the origin
		/// </summary>
		kRepeat,

		/// <summary>
		/// Clamps texture coordinates
		/// </summary>
		kClamp
	};

	/// <summary>
	/// Texture mode types
	/// </summary>
	public enum TextureMode
	{
		/// <summary>
		/// Replaces fragment with texel
		/// </summary>
		kReplace,

		/// <summary>
		/// Combines fragment and texel
		/// </summary>
		kModulate,
		
		/// <summary>
		/// Blends fragment and texel
		/// </summary>
		kDecal,

		/// <summary>
		/// Blends fragment and colour
		/// </summary>
		kBlend
	};


	/// <summary>
	/// Summary description for ApplyTexture2d.
	/// </summary>
	public class ApplyTexture2d : IApplicable
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ApplyTexture2d( )
		{
		}

		/// <summary>
		/// Sets the texture to apply
		/// </summary>
		/// <param name="unit"> Texture unit </param>
		/// <param name="texture"> Texture to apply</param>
		public ApplyTexture2d( int unit, Texture2d texture )
		{
			m_Texture = texture;
		}

		/// <summary>
		/// Access to the bound texture
		/// </summary>
		public Texture2d		Texture
		{
			get
			{
				return m_Texture;
			}
			set
			{
				m_Texture = value;
			}
		}

		/// <summary>
		/// The filter used when the area covered by a fragment is greater than the area of a texel
		/// </summary>
		public TextureFilter	MinFilter
		{
			get
			{
				return m_MinFilter;
			}
			set
			{
				m_MinFilter = value;
			}
		}

		/// <summary>
		/// The filter used when the area covered by a fragment is less than the area of a texel. Can be either kNearest or kLinear
		/// </summary>
		public TextureFilter	MagFilter
		{
			get
			{
				return m_MagFilter;
			}
			set
			{
				m_MagFilter = value;
			}
		}

		/// <summary>
		/// Access to the way the texture sampling changes when the texture S coordinate reaches the edge of the texture
		/// </summary>
		public TextureWrap		WrapS
		{
			get
			{
				return m_WrapS;
			}
			set
			{
				m_WrapS = value;
			}
		}

		/// <summary>
		/// Access to the way the texture sampling changes when the texture T coordinate reaches the edge of the texture
		/// </summary>
		public TextureWrap		WrapT
		{
			get
			{
				return m_WrapT;
			}
			set
			{
				m_WrapT = value;
			}
		}

		/// <summary>
		/// Sets the way that the texture is interpreted when texturing a fragment
		/// </summary>
		public TextureMode		Mode
		{
			get
			{
				return m_Mode;
			}
			set
			{
				m_Mode = value;
			}
		}

		#region	Private stuff

	//	private int				m_Unit;
		private Texture2d		m_Texture;
		private TextureFilter	m_MinFilter	= TextureFilter.kNearestTexel;
		private TextureFilter	m_MagFilter = TextureFilter.kNearestTexel;
		private TextureWrap		m_WrapS		= TextureWrap.kClamp;
		private TextureWrap		m_WrapT		= TextureWrap.kClamp;
		private TextureMode		m_Mode		= TextureMode.kReplace;

		#endregion
	}
}
