using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Render appliance, used for clearing the render target colour
	/// </summary>
	/// <seealso cref="Renderer.ClearColour"/>
	public class ClearTargetColour : IAppliance
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
		/// Sets the clear colour to black
		/// </summary>
		public ClearTargetColour( )
		{
		}


		/// <summary>
		/// Sets the clear colour
		/// </summary>
		public ClearTargetColour( System.Drawing.Color colour )
		{
			m_Colour = colour;
		}

		#region IAppliance Members

		/// <summary>
		/// Clears the current target, using Renderer.Clear()
		/// </summary>
		public void Begin( )
		{
			Renderer.Inst.ClearColour( m_Colour );
		}

		/// <summary>
		/// Does nothing (clearing doesn't really have an 'end')
		/// </summary>
		public void End( )
		{
		}

		#endregion

		private System.Drawing.Color m_Colour;

	}
}
