using System;
using System.Collections;

namespace RbEngine.Rendering
{

	//	TODO: This is just an ApplianceList

	/// <summary>
	/// A render pass sets up the state of the renderer for rendering
	/// </summary>
	public class RenderPass
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RenderPass( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="appliances">Appliances to add to the render pass</param>
		public RenderPass( params IAppliance[] appliances )
		{
			Add( appliances );
		}

		/// <summary>
		/// Adds an object to this render pass (must implement the Rendering.IAppliance interface)
		/// </summary>
		public void						Add( IAppliance obj )
		{
			m_Appliances.Add( obj );
		}

		/// <summary>
		/// Adds an object to this render pass (must implement the Rendering.IAppliance interface)
		/// </summary>
		public void						Add( params IAppliance[] appliances )
		{
			for ( int index = 0; index < appliances.Length; ++index )
			{
				m_Appliances.Add( appliances[ index ] );
			}
		}

		/// <summary>
		/// Applies this render pass
		/// </summary>
		public virtual void				Begin( )
		{
			foreach ( IAppliance appliance in m_Appliances )
			{
				appliance.Begin( );
			}
		}

		/// <summary>
		/// Cleans up the application of this render pass
		/// </summary>
		public virtual void				End( )
		{
			foreach ( IAppliance appliance in m_Appliances )
			{
				appliance.End( );
			}
		}

		#region	Private stuff

		private ArrayList	m_Appliances = new ArrayList( );

		#endregion

	}
}
