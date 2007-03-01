using System;
using System.Collections;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Stores an array of IApplicable objects
	/// </summary>
	public class ApplianceList : IApplicable
	{
		/// <summary>
		/// Adds an object to this list
		/// </summary>
		public void			Add( Object obj )
		{
			if ( m_Appliances == null )
			{
				m_Appliances = new ArrayList( );
			}
			m_Appliances.Add( ( IApplicable )obj );
		}

		#region	Private stuff

		private ArrayList	m_Appliances;

		#endregion

		#region IApplicable Members

		/// <summary>
		/// Applies all the IApplicable objects in this list
		/// </summary>
		public void Apply()
		{
			if ( m_Appliances != null )
			{
				for ( int index = 0; index < m_Appliances.Count; ++index )
				{
					( ( IApplicable )m_Appliances[ index ] ).Apply( );
				}
			}
		}

		#endregion
	}
}
