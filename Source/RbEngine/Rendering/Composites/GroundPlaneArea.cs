using System;

namespace RbEngine.Rendering.Composites
{
	/// <summary>
	/// Renders an area of the ground plane
	/// </summary>
	public abstract class GroundPlaneArea : Composite
	{
		/// <summary>
		/// Returns the width (x dimension)
		/// </summary>
		public virtual float	Width
		{
			get
			{
				return m_Width;
			}
			set
			{
				m_Width = value;
			}
		}

		/// <summary>
		/// Returns the height (z dimension)
		/// </summary>
		public virtual float	Height
		{
			get
			{
				return m_Height;
			}
			set
			{
				m_Height = value;
			}
		}

		private float			m_Width		= 128;
		private float			m_Height	= 128;
	}
}
