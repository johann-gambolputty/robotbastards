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
		/// Constructor
		/// </summary>
		public RenderPass( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="appliances">Appliances to add to the render pass</param>
		public RenderPass( params IApplicable[] appliances )
		{
			Add( appliances );
		}

		/// <summary>
		/// Adds an object to this render pass (must implement the Rendering.IApplicable interface)
		/// </summary>
		public void						Add( IApplicable obj )
		{
			m_Appliances.Add( obj );
		}

		/// <summary>
		/// Adds an object to this render pass (must implement the Rendering.IApplicable interface)
		/// </summary>
		public void						Add( params IApplicable[] appliances )
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
