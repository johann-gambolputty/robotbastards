
namespace Rb.Rendering
{
	/// <summary>
	/// Base class for lights
	/// </summary>
	public class Light
	{
		/// <summary>
		/// Sets whether this light is a shadow caster or not
		/// </summary>
		public bool	ShadowCaster
		{
			get { return m_ShadowCaster; }
			set { m_ShadowCaster = value; }
		}

		/// <summary>
		/// Light colour
		/// </summary>
		public System.Drawing.Color Colour
		{
			get { return m_Colour; }
			set { m_Colour = value; }
		}

		private bool					m_ShadowCaster = true;
		private System.Drawing.Color	m_Colour;
	}
}
