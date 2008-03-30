using System;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Textures
{
	/// <summary>
	/// A handy abstract base class that does most of the legwork implementing <see cref="ITexture2dSampler"/>
	/// </summary>
	[Serializable]
	public abstract class Texture2dSamplerBase : ITexture2dSampler
	{
		#region Construction

		/// <summary>
		/// Default constructor
		/// </summary>
		public Texture2dSamplerBase( )
		{
		}

		/// <summary>
		/// Sets the texture to apply
		/// </summary>
		/// <param name="texture">Texture to apply</param>
		public Texture2dSamplerBase( ITexture2d texture )
		{
			m_Texture = texture;
		}

		#endregion

		#region IPass methods

		/// <summary>
		/// Starts applying this texture
		/// </summary>
		public abstract void Begin( );
		
		/// <summary>
		/// Stops applying this texture
		/// </summary>
		public abstract void End( );

		#endregion

		#region ITexture2dSampler Members

		/// <summary>
		/// Access to the bound texture
		/// </summary>
		public ITexture2d Texture
		{
			get { return m_Texture; }
			set { m_Texture = value; }
		}

		/// <summary>
		/// The filter used when the area covered by a fragment is greater than the area of a texel
		/// </summary>
		public TextureFilter MinFilter
		{
			get { return m_MinFilter; }
			set { m_MinFilter = value; }
		}

		/// <summary>
		/// The filter used when the area covered by a fragment is less than the area of a texel. Can be either kNearest or kLinear
		/// </summary>
		public TextureFilter MagFilter
		{
			get { return m_MagFilter; }
			set { m_MagFilter = value; }
		}

		/// <summary>
		/// Access to the way the texture sampling changes when the texture S coordinate reaches the edge of the texture
		/// </summary>
		public TextureWrap WrapS
		{
			get { return m_WrapS; }
			set { m_WrapS = value; }
		}

		/// <summary>
		/// Access to the way the texture sampling changes when the texture T coordinate reaches the edge of the texture
		/// </summary>
		public TextureWrap WrapT
		{
			get { return m_WrapT; }
			set { m_WrapT = value; }
		}

		/// <summary>
		/// Sets the way that the texture is interpreted when texturing a fragment
		/// </summary>
		public TextureMode Mode
		{
			get { return m_Mode; }
			set { m_Mode = value; }
		}

		#endregion

		#region	Private stuff

		private ITexture2d		m_Texture;
		private TextureFilter	m_MinFilter	= TextureFilter.NearestTexel;
		private TextureFilter	m_MagFilter = TextureFilter.NearestTexel;
		private TextureWrap		m_WrapS		= TextureWrap.Clamp;
		private TextureWrap		m_WrapT		= TextureWrap.Clamp;
		private TextureMode		m_Mode		= TextureMode.Replace;

		#endregion

	}
}
