using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Summary description for Material.
	/// </summary>
	public abstract class Material : IApplicable
	{
		/// <summary>
		/// The ambient response of the material
		/// </summary>
		public System.Drawing.Color		Ambient
		{
			get
			{
				return System.Drawing.Color.FromArgb( ( int )( m_Ambient[ 0 ] * 255.0f ), ( int )( m_Ambient[ 1 ] * 255.0f ), ( int )( m_Ambient[ 2 ] * 255.0f ) );
			}
			set
			{
				m_Ambient[ 0 ] = ( float)value.R / 255.0f;
				m_Ambient[ 1 ] = ( float)value.G / 255.0f;
				m_Ambient[ 2 ] = ( float)value.B / 255.0f;
			}
		}

		/// <summary>
		/// The diffuse response of the material
		/// </summary>
		public System.Drawing.Color		Diffuse
		{
			get
			{
				return System.Drawing.Color.FromArgb( ( int )( m_Diffuse[ 0 ] * 255.0f ), ( int )( m_Diffuse[ 1 ] * 255.0f ), ( int )( m_Diffuse[ 2 ] * 255.0f ) );
			}
			set
			{
				m_Diffuse[ 0 ] = ( float)value.R / 255.0f;
				m_Diffuse[ 1 ] = ( float)value.G / 255.0f;
				m_Diffuse[ 2 ] = ( float)value.B / 255.0f;
			}
		}

		/// <summary>
		/// The specular response of the material
		/// </summary>
		public System.Drawing.Color		Specular
		{
			get
			{
				return System.Drawing.Color.FromArgb( ( int )( m_Specular[ 0 ] * 255.0f ), ( int )( m_Specular[ 1 ] * 255.0f ), ( int )( m_Specular[ 2 ] * 255.0f ) );
			}
			set
			{
				m_Specular[ 0 ] = ( float)value.R / 255.0f;
				m_Specular[ 1 ] = ( float)value.G / 255.0f;
				m_Specular[ 2 ] = ( float)value.B / 255.0f;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public							Material( )
		{
		}

		/// <summary>
		/// Sets the ambient and diffuse material colours
		/// </summary>
		/// <param name="ambient"> Material ambient colour </param>
		/// <param name="diffuse"> Material diffuse colour </param>
		public							Material( System.Drawing.Color ambient, System.Drawing.Color diffuse )
		{
			Ambient = ambient;
			Diffuse = diffuse;
		}

		/// <summary>
		/// Applies this material
		/// </summary>
		public abstract void			Apply( );

		#region	Protected stuff

		protected float[]	m_Ambient	= new float[ 4 ] { 0.2f, 0.2f, 0.2f, 1.0f };
		protected float[]	m_Diffuse	= new float[ 4 ] { 0.8f, 0.8f, 0.8f, 1.0f };
		protected float[]	m_Specular	= new float[ 4 ] { 0.0f, 0.0f, 0.0f, 1.0f };

		#endregion

	}
}
