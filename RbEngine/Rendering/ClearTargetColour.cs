using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Render appliance, used for clearing colours
	/// </summary>
	public class ClearTargetColour : IApplicable
	{
		/// <summary>
		/// Clear colour
		/// </summary>
		public System.Drawing.Color		Colour
		{
			get { return m_Colour;	}
			set { m_Colour = value;	}
		}

		/// <summary>
		/// Sets the clear depth to black
		/// </summary>
		public ClearTargetColour( )
		{
		}

		public void EnableColourClear( System.Drawing.Color colour )
		{
			m_Colour = colour;
		}

		public void EnableDepthClear( float depth )
		{
			m_Depth = depth;
		}

		#region IApplicable Members

		/// <summary>
		/// Clears the current target, using Renderer.Clear()
		/// </summary>
		public void Apply()
		{

		}

		#endregion

		private System.Drawing.Color	m_Colour;
		private float					m_Depth;

	}
}
