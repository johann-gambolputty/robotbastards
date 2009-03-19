
using Rb.Rendering;

namespace Poc1.Universe.Interfaces
{
	public interface IUniRenderContext
	{
		/// <summary>
		/// Returns true if geometry is being rendered for reflection
		/// </summary>
		bool Reflections
		{
			get;
		}

		/// <summary>
		/// Returns true if geometry is being rendered for shadows
		/// </summary>
		bool Shadows
		{
			get;
		}
	}

	/// <summary>
	/// Rendering context for universe objects
	/// </summary>
	public class UniRenderContext : RenderContext, IUniRenderContext
	{
		#region IUniRenderContext Members

		/// <summary>
		/// Returns true if geometry is being rendered for reflection
		/// </summary>
		public bool Reflections
		{
			get { return m_Reflections; }
			set { m_Reflections = value; }
		}

		/// <summary>
		/// Returns true if geometry is being rendered for shadows
		/// </summary>
		public bool Shadows
		{
			get { return m_Shadows; }
			set { m_Shadows = value; }
		}

		#endregion

		#region Private Members

		private bool m_Shadows;
		private bool m_Reflections;

		#endregion
	}
}
