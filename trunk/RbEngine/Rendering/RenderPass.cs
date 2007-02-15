using System;
using System.Collections;

namespace RbEngine.Rendering
{
	/// <summary>
	/// A render pass sets up the state of the renderer for rendering
	/// </summary>
	public class RenderPass
	{

		/// <summary>
		/// Adds an object to this render pass (must implement the Rendering.IApplicable interface)
		/// </summary>
		public void						Add( Object obj )
		{
			m_Appliances.Add( ( IApplicable )obj );
		}

		/// <summary>
		/// Applies this render pass
		/// </summary>
		public virtual void				Begin( )
		{
			foreach ( IApplicable appliance in m_Appliances )
			{
				appliance.Apply( );
			}
		}

		/// <summary>
		/// Cleans up the application of this render pass
		/// </summary>
		public virtual void				End( )
		{
		}

		#region	Private stuff

		private ArrayList	m_Appliances = new ArrayList( );

		#endregion

	}
}
