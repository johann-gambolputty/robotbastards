using System.Drawing;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// A handy abstract base class that does most of the legwork implementing <see cref="IMaterial"/>
	/// </summary>
	/// <remarks>
	/// Create IMaterial objects using <see cref="Rb.Rendering.Interfaces.IGraphicsFactory.CreateMaterial"/>
	/// </remarks>
	public abstract class MaterialBase : IMaterial
	{
		/// <summary>
		/// The ambient response of the material
		/// </summary>
		public Color Ambient
		{
			get
			{
				return Color.FromArgb( ( int )( m_Ambient[ 0 ] * 255.0f ), ( int )( m_Ambient[ 1 ] * 255.0f ), ( int )( m_Ambient[ 2 ] * 255.0f ) );
			}
			set
			{
				m_Ambient[ 0 ] = value.R / 255.0f;
				m_Ambient[ 1 ] = value.G / 255.0f;
				m_Ambient[ 2 ] = value.B / 255.0f;
			}
		}

		/// <summary>
		/// The diffuse response of the material
		/// </summary>
		public Color Diffuse
		{
			get
			{
				return Color.FromArgb( ( int )( m_Diffuse[ 0 ] * 255.0f ), ( int )( m_Diffuse[ 1 ] * 255.0f ), ( int )( m_Diffuse[ 2 ] * 255.0f ) );
			}
			set
			{
				m_Diffuse[ 0 ] = value.R / 255.0f;
				m_Diffuse[ 1 ] = value.G / 255.0f;
				m_Diffuse[ 2 ] = value.B / 255.0f;
			}
		}

		/// <summary>
		/// The specular response of the material
		/// </summary>
		public Color Specular
		{
			get
			{
				return Color.FromArgb( ( int )( m_Specular[ 0 ] * 255.0f ), ( int )( m_Specular[ 1 ] * 255.0f ), ( int )( m_Specular[ 2 ] * 255.0f ) );
			}
			set
			{
				m_Specular[ 0 ] = value.R / 255.0f;
				m_Specular[ 1 ] = value.G / 255.0f;
				m_Specular[ 2 ] = value.B / 255.0f;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public MaterialBase( )
		{
		}

		/// <summary>
		/// Sets the ambient and diffuse material colours
		/// </summary>
		/// <param name="ambient"> Material ambient colour </param>
		/// <param name="diffuse"> Material diffuse colour </param>
		public MaterialBase( Color ambient, Color diffuse )
		{
			Ambient = ambient;
			Diffuse = diffuse;
		}

		/// <summary>
		/// Sets the ambient and diffuse material colours
		/// </summary>
		/// <param name="ambient"> Material ambient colour </param>
		/// <param name="diffuse"> Material diffuse colour </param>
		/// <returns> Returns this material </returns>
		public MaterialBase Setup( Color ambient, Color diffuse )
		{
			Ambient = ambient;
			Diffuse = diffuse;
			return this;
		}


		/// <summary>
		/// Applies this material
		/// </summary>
		public abstract void Begin( );

		/// <summary>
		/// Unapplies this material
		/// </summary>
		public abstract void End( );

		#region	Protected stuff

		protected float[]	m_Ambient	= new float[ 4 ] { 0.2f, 0.2f, 0.2f, 1.0f };
		protected float[]	m_Diffuse	= new float[ 4 ] { 0.8f, 0.8f, 0.8f, 1.0f };
		protected float[]	m_Specular	= new float[ 4 ] { 0.0f, 0.0f, 0.0f, 1.0f };

		#endregion

	}
}
