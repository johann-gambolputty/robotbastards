using System;
using System.Drawing;

namespace Rb.Rendering.Lights
{
	/// <summary>
	/// Base class for lights
	/// </summary>
	[Serializable]
	public class Light : ILight
	{
		/// <summary>
		/// Sets whether this light is a shadow caster or not
		/// </summary>
		public bool	CastsShadows
		{
			get { return m_ShadowCaster; }
			set { m_ShadowCaster = value; }
		}

		/// <summary>
		/// Light colour
		/// </summary>
		public Color Colour
		{
			get { return m_Colour; }
			set { m_Colour = value; }
		}

		private bool	m_ShadowCaster = true;
		private Color	m_Colour;
	}
}
